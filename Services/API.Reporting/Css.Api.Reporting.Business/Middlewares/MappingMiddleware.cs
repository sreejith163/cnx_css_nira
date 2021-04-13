using Css.Api.Reporting.Business.Data;
using Css.Api.Reporting.Business.Exceptions;
using Css.Api.Reporting.Models.DTO.Mappers;
using Css.Api.Reporting.Models.DTO.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Css.Api.Reporting.Business.Middlewares
{
    /// <summary>
    /// A middleware to set the mapper in the request context
    /// </summary>
    public class MappingMiddleware
    {
        #region Private Properties

        /// <summary>
        /// The request delegate
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// The mapper settings
        /// </summary>
        private readonly MapperSettings _mapper;
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor to initialize
        /// </summary>
        /// <param name="next"></param>
        /// <param name="options"></param>
        public MappingMiddleware(RequestDelegate next, IOptions<MapperSettings> options)
        {
            _next = next;
            _mapper = options.Value;
        }
        #endregion

        #region Invoke(HttpContext httpContext)

        /// <summary>
        /// The invoke method of the middleware
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                if (httpContext.Request.Path.Value.Contains("/api/v1/activity"))
                {
                    await PreprocessActivityRequests(httpContext);
                }
                else if(httpContext.Request.Path.Value.Contains("/api/v1/dispatch"))
                {
                    PreprocessDispatchRequests(httpContext);
                }
                await _next(httpContext);
            }
            catch(Exception ex)
            {
                var result = new JsonResult(new { Message = ex.Message });
                httpContext.Response.StatusCode = (int) HttpStatusCode.UnprocessableEntity;
                httpContext.Response.ContentType = "application/json";           
                await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(result.Value));
            }
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// A helper to pre-process all activity requests
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        private async Task PreprocessActivityRequests(HttpContext httpContext)
        {
            switch (httpContext.Request.Method.ToUpper())
            {
                case "POST":
                    await MapToActivity(httpContext);
                    break;
                case "GET":
                    await MapToCollector(httpContext);
                    break;
                case "PUT":
                    await MapToAssigner(httpContext);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// A helper to pre-process all dispatch requests
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        private void PreprocessDispatchRequests(HttpContext httpContext)
        {
            var key = GetRouteParam(@"\/api\/v1\/dispatch\/(?'key'\w*)", httpContext);
            var settings = GetDispatch(key);
            var dataOption = GetTargetDataOption(settings.TargetDataOption);
            AddMappingContext(httpContext, new MappingContext()
            {
                Key = settings.Key,
                Source = string.Empty,
                SourceType = string.Empty,
                TargetType = dataOption.Type,
                TargetOptions = dataOption.Options
            });
        }

        /// <summary>
        /// Helper method to map the activity POST request
        /// </summary>
        /// <param name="httpContext"></param>
        private async Task MapToActivity(HttpContext httpContext)
        {
            if (httpContext.Items.ContainsKey("Mappers"))
            {
                httpContext.Items.Remove("Mappers");
            }

            var headers = httpContext.Request.Headers;

            if (!headers.ContainsKey("activity"))
            {
                throw new InvalidOperationException();
            }

            string key = headers["activity"];

            var settings = GetActivity(key);
            var sourceDataOption = GetSourceDataOption(key, settings);
            var targetDataOption = GetTargetDataOption(key, settings);
            var content = await GetRequestContent(httpContext);
            
            AddMappingContext(httpContext, new MappingContext()
            {
                Key = key,
                Source = settings.Source,
                SourceType = sourceDataOption.Type,
                SourceOptions = sourceDataOption.Options,
                Target = settings.Target,
                TargetType = targetDataOption.Type,
                TargetOptions = targetDataOption.Options,
                RequestBody = content
            });
        }

        /// <summary>
        /// Helper method to map the activity GET request
        /// </summary>
        /// <param name="httpContext"></param>
        private async Task MapToCollector(HttpContext httpContext)
        {
            var routeParam = GetRouteParam(@"\/api\/v1\/activity\/(?'key'\w*)", httpContext);

            var key = string.Join("-", routeParam, "Export");

            var settings = GetActivity(key);
            var sourceDataOption = GetSourceDataOption(key, settings);
            var queryParams = await GetRequestHeaders(httpContext);

            AddMappingContext(httpContext, new MappingContext()
            {
                Key = key,
                Source = settings.Source,
                SourceType = sourceDataOption.Type,
                SourceOptions = sourceDataOption.Options,
                RequestQueryParams = queryParams
            });
        }

        /// <summary>
        /// Helper method to map the activity PUT request
        /// </summary>
        /// <param name="httpContext"></param>
        private async Task MapToAssigner(HttpContext httpContext)
        {
            var routeParam = GetRouteParam(@"\/api\/v1\/activity\/(?'key'\w*)", httpContext);

            var key = string.Join("-", routeParam, "Import");

            var settings = GetActivity(key);
            var targetDataOption = GetTargetDataOption(key, settings);
            var content = await GetRequestContent(httpContext);

            AddMappingContext(httpContext, new MappingContext()
            {
                Key = key,
                Target = settings.Target,
                TargetType = targetDataOption.Type,
                TargetOptions = targetDataOption.Options,
                RequestBody = content
            });
        }

        /// <summary>
        /// Helper method to fetch route param from the route
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        private string GetRouteParam(string routePattern, HttpContext httpContext)
        {
            var pattern = new Regex(routePattern);
            var match = pattern.Match(httpContext.Request.Path);

            if (!match.Success)
            {
                throw new MappingException(string.Format(Messages.RouteNotFound, httpContext.Request.Path));
            }

            var keyGroup = match.Groups.Values.FirstOrDefault(x => x.Name.Equals("key"));
            if (keyGroup == null)
            {
                throw new MappingException(string.Format(Messages.RouteNotFound, httpContext.Request.Path));
            }

            return keyGroup.Value.ToUpper();
        }

        /// <summary>
        /// A helper method get the activity from the config using the key
        /// </summary>
        /// <param name="key"></param>
        /// <returns>An instance of Activity</returns>
        private Activity GetActivity(string key)
        {
            var settings = _mapper.Activities.FirstOrDefault(x => x.Key.Equals(key));
            if (settings == null)
            {
                throw new MappingException(string.Format(Messages.MappingNotFound, key));
            }

            return settings;
        }

        /// <summary>
        /// The method to get the dispatcher details
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private Dispatch GetDispatch(string key)
        {
            var settings = _mapper.Dispatchers.FirstOrDefault(x => x.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase));
            if (settings == null)
            {
                throw new MappingException(string.Format(Messages.MappingNotFound, key));
            }

            return settings;
        }

        /// <summary>
        /// A helper method to fetch the source and its data option details
        /// </summary>
        /// <param name="key"></param>
        /// <param name="settings"></param>
        /// <returns>An instance of DataOption</returns>
        private DataOption GetSourceDataOption(string key, Activity settings)
        {
            if (string.IsNullOrWhiteSpace(settings.Source))
            {
                throw new MappingException(string.Format(Messages.InvalidSource, key));
            }

            var sourceDataOption = _mapper.DataOptions.FirstOrDefault(x => x.Key.Equals(settings.SourceDataOption));
            if (sourceDataOption == null)
            {
                throw new MappingException(string.Format(Messages.InvalidDataSource, key));
            }
            return sourceDataOption;
        }

        /// <summary>
        /// A helper method to fetch the target and its data option details
        /// </summary>
        /// <param name="key"></param>
        /// <param name="settings"></param>
        /// <returns>An instance of DataOption</returns>
        private DataOption GetTargetDataOption(string key, Activity settings)
        {
            if (string.IsNullOrWhiteSpace(settings.Target))
            {
                throw new MappingException(string.Format(Messages.InvalidTarget, key));
            }

            var targetDataOption = _mapper.DataOptions.FirstOrDefault(x => x.Key.Equals(settings.TargetDataOption));
            if (targetDataOption == null)
            {
                throw new MappingException(string.Format(Messages.InvalidDataTarget, key));
            }

            return targetDataOption;
        }

        /// <summary>
        /// The helper to map the target data option
        /// </summary>
        /// <param name="targetOption"></param>
        /// <returns></returns>
        private DataOption GetTargetDataOption(string targetOption)
        {
            var targetDataOption = _mapper.DataOptions.FirstOrDefault(x => x.Key.Equals(targetOption));
            if (targetDataOption == null)
            {
                throw new MappingException(string.Format(Messages.InvalidDataTarget, targetOption));
            }

            return targetDataOption;
        }

        /// <summary>
        /// A helper method to fetch the request body content
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns>The content string</returns>
        private async Task<string> GetRequestContent(HttpContext httpContext)
        {
            return await new StreamReader(httpContext.Request.Body).ReadToEndAsync();
        }

        /// <summary>
        /// A helper method to parse headers of the request
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        private async Task<string> GetRequestHeaders(HttpContext httpContext)
        {
            var query_dict = new Dictionary<string, object>();
            var headers = httpContext.Request.Headers;
            var agentIds = headers["AgentIds"];
            if (agentIds.Count > 0)
            {
                var values = agentIds.ToString().Split(",");
                var list_dict_value = new List<string>();
                foreach (var value in values)
                {
                    list_dict_value.Add(value);
                }
                query_dict.Add("agentIds", list_dict_value);
            }
            query_dict.Add("startDate", headers["startDate"].ToString());
            query_dict.Add("endDate", headers["endDate"].ToString());

            return await Task.FromResult(JsonConvert.SerializeObject(query_dict));
        }

        /// <summary>
        /// A helper to parse and return all query params in the request
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns>The jsonified string of all the query params</returns>
        private async Task<string> GetRequestQueryParams(HttpContext httpContext)
        {
            var query_dict = new Dictionary<string, object>();
            var query = httpContext.Request.Query;
            foreach(var key in query.Keys)
            {
                var values = query[key];
                object dict_value = values.ToString();
                if (values.Count > 1)
                {
                    var list_dict_value = new List<string>();
                    foreach(var value in values)
                    {
                        list_dict_value.Add(value);
                    }
                    dict_value = list_dict_value;
                }
                query_dict.Add(key, dict_value);
            }
            
            return await Task.FromResult(JsonConvert.SerializeObject(query_dict));
        }

        /// <summary>
        /// A helper method to add MappingContext
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="context"></param>
        private void AddMappingContext(HttpContext httpContext, MappingContext context)
        {
            httpContext.Items.Add("Mappers", context);
        }
        #endregion
    }
}
