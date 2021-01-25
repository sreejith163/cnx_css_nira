using Css.Api.Reporting.Business.Exceptions;
using Css.Api.Reporting.Business.Middlewares;
using Css.Api.Reporting.Business.UnitTest.Mocks;
using Css.Api.Reporting.Models.DTO.Mappers;
using Css.Api.Reporting.Models.DTO.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Css.Api.Reporting.Business.UnitTest.Middlewares
{
    /// <summary>
    /// The unit test class for MappingMiddleware
    /// </summary>
    public class MappingMiddlewareShould
    {
        #region Private Properties

        /// <summary>
        /// The mock factory data
        /// </summary>
        private readonly MockFactoryData _factoryData;
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public MappingMiddlewareShould()
        {
            _factoryData = new MockFactoryData();
        }
        #endregion

        #region Invoke(HttpContext httpContext)

        /// <summary>
        /// The test cases of successful middleware execution
        /// </summary>
        /// <param name="requestPath">HTTP Request path</param>
        /// <param name="key">The mapping key</param>
        [Theory]
        [InlineData("/api/v1/activity", "UDW")]
        [InlineData("/api/v1/activity", "EStart")]
        public async void CheckMiddlewareSuccess(string requestPath, string key)
        {
            var mapper = _factoryData.GetMapperSettings();
            var response = JsonConvert.DeserializeObject<Dictionary<object, object>>(await ExecuteMiddleware(requestPath, mapper, key));
            Assert.NotEmpty(response);
            var mapperValue = response["Mappers"];
            Assert.NotNull(mapperValue);
        }

        /// <summary>
        /// The test case of an invalid request
        /// </summary>
        /// <param name="requestPath">HTTP Request path</param>
        [Theory]
        [InlineData("/test")]
        public async void CheckInvalidPath(string requestPath)
        {
            var mapper = _factoryData.GetMapperSettings();
            var response = JsonConvert.DeserializeObject<Dictionary<object,object>>(await ExecuteMiddleware(requestPath, mapper));
            Assert.Empty(response);
        }

        /// <summary>
        /// The test case for an invalid header
        /// </summary>
        [Fact]
        public async void CheckHeaderFailed()
        {
            var mapper = _factoryData.GetMapperSettings();
            await Assert.ThrowsAsync<InvalidOperationException>(() => ExecuteMiddleware("/api/v1/activity", mapper));
        }

        /// <summary>
        /// The test cases for invalid mapper keys
        /// </summary>
        /// <param name="key">The mapping key</param>
        [Theory]
        [InlineData("test1")]
        [InlineData("test2")]
        public async void CheckMappingKeyNotFound(string key)
        {
            var mapper = _factoryData.GetMapperSettings();
            await Assert.ThrowsAsync<MappingException>(() => ExecuteMiddleware("/api/v1/activity", mapper, key));
        }

        /// <summary>
        /// The test case for an invalid source
        /// </summary>
        /// <param name="key">The mapping key</param>
        [Theory]
        [InlineData("NoSource")]
        public async void CheckInvalidSource(string key)
        {
            var mapper = _factoryData.GetMapperSettings();
            await Assert.ThrowsAsync<MappingException>(() => ExecuteMiddleware("/api/v1/activity", mapper, key));
        }

        /// <summary>
        /// The test case for an invalid source option
        /// </summary>
        /// <param name="key">The mapping key</param>
        [Theory]
        [InlineData("NoSourceOption")]
        public async void CheckInvalidSourceOption(string key)
        {
            var mapper = _factoryData.GetMapperSettings();
            await Assert.ThrowsAsync<MappingException>(() => ExecuteMiddleware("/api/v1/activity", mapper, key));
        }

        /// <summary>
        /// The test case for an invalid target
        /// </summary>
        /// <param name="key">The mapping key</param>
        [Theory]
        [InlineData("NoTarget")]
        public async void CheckInvalidTarget(string key)
        {
            var mapper = _factoryData.GetMapperSettings();
            await Assert.ThrowsAsync<MappingException>(() => ExecuteMiddleware("/api/v1/activity", mapper, key));
        }

        /// <summary>
        /// The test for an invalid target option
        /// </summary>
        /// <param name="key">The mapping key</param>
        [Theory]
        [InlineData("NoTargetOption")]
        public async void CheckInvalidTargetOption(string key)
        {
            var mapper = _factoryData.GetMapperSettings();
            await Assert.ThrowsAsync<MappingException>(() => ExecuteMiddleware("/api/v1/activity", mapper, key));
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// A method to execute the middleware by mock configuring the HTTP context
        /// </summary>
        /// <param name="path">HTTP Request path</param>
        /// <param name="mapper">The mapper</param>
        /// <param name="key">The mapping key to be added to the request</param>
        /// <returns></returns>
        private async Task<string> ExecuteMiddleware(string path, IOptions<MapperSettings> mapper, string key = "")
        {
            var middleware = CreateMiddleware(mapper);
            var context = new DefaultHttpContext();
            context.Request.Path = path;
            context.Response.Body = new MemoryStream();

            if(!string.IsNullOrWhiteSpace(key))
            {
                context.Request.Headers["activity"] = key;
            }

            await middleware.Invoke(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            return new StreamReader(context.Response.Body).ReadToEnd();
        }

        /// <summary>
        /// The method to create the instance of MappingMiddleware
        /// </summary>
        /// <param name="mapper"></param>
        /// <returns></returns>
        private MappingMiddleware CreateMiddleware(IOptions<MapperSettings> mapper)
        {
            return new MappingMiddleware(
                next: (innerHttpContext) =>
                {
                    innerHttpContext.Response.WriteAsync(JsonConvert.SerializeObject(innerHttpContext.Items, Formatting.Indented));
                    return Task.CompletedTask;
                },
                mapper
            );
        }
        #endregion
    }
}
