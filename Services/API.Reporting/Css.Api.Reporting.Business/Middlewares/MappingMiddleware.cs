using Css.Api.Reporting.Business.Data;
using Css.Api.Reporting.Business.Exceptions;
using Css.Api.Reporting.Models.DTO.Mappers;
using Css.Api.Reporting.Models.DTO.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public Task Invoke(HttpContext httpContext)
        {
            if(!httpContext.Request.Path.Equals("/api/v1/activity"))
            {
                return _next(httpContext);
            }

            if(httpContext.Items.ContainsKey("Mappers"))
            {
                httpContext.Items.Remove("Mappers");
            }
            
            var headers = httpContext.Request.Headers;
            
            if(!headers.ContainsKey("activity"))
            {
                throw new InvalidOperationException();
            }

            string key = headers["activity"];

            var settings = _mapper.Activities.FirstOrDefault(x => x.Key.Equals(key));
            if(settings == null)
            {
                throw new MappingException(string.Format(Messages.MappingNotFound, key));
            }

            if(string.IsNullOrWhiteSpace(settings.Source))
            {
                throw new MappingException(string.Format(Messages.InvalidSource, key));
            }

            if(string.IsNullOrWhiteSpace(settings.Target))
            {
                throw new MappingException(string.Format(Messages.InvalidTarget, key));
            }

            var sourceDataOption = _mapper.DataOptions.FirstOrDefault(x => x.Key.Equals(settings.SourceDataOption));
            if (sourceDataOption == null)
            {
                throw new MappingException(string.Format(Messages.InvalidDataSource, key));
            }

            var targetDataOption = _mapper.DataOptions.FirstOrDefault(x => x.Key.Equals(settings.TargetDataOption));
            if (targetDataOption == null)
            {
                throw new MappingException(string.Format(Messages.InvalidDataTarget, key));
            }

            httpContext.Items.Add("Mappers", new MappingContext()
            {
                Key = key,
                Source = settings.Source,
                SourceType = sourceDataOption.Type,
                SourceOptions = sourceDataOption.Options,
                Target = settings.Target,
                TargetType = targetDataOption.Type,
                TargetOptions = targetDataOption.Options
            });    
            return _next(httpContext);
        }
        #endregion
    }
}
