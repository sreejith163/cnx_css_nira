using Css.Api.Reporting.Business.Interfaces;
using Css.Api.Reporting.Business.Services;
using Css.Api.Reporting.Business.UnitTest.Mocks;
using Css.Api.Reporting.Models.DTO.Request;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Css.Api.Reporting.Business.UnitTest.Services
{
    /// <summary>
    /// The unit testing class for MapperService
    /// </summary>
    public class MapperServiceShould
    {
        #region Private Properties

        /// <summary>
        /// The mapper service
        /// </summary>
        private readonly Mock<IFTPService> _ftp;

        /// <summary>
        /// The list of mock mapping contexts
        /// </summary>
        private readonly List<MappingContext> _contexts;
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public MapperServiceShould()
        {
            var mockFactory = new MockFactoryData();
            _contexts = mockFactory.GetMappingContexts();
            _ftp = mockFactory.GetMockFTP();
            
        }
        #endregion

        #region Context

        /// <summary>
        /// The method to test the mapping context
        /// </summary>
        [Fact]
        public void CheckMappingContext()
        {
            IMapperService mapperService = GetMapperService();
            var result = mapperService.Context;
            Assert.NotNull(result);
            Assert.IsType<MappingContext>(result);
        }
        #endregion

        #region InitializeFTP<T>()

        /// <summary>
        /// The method to check the FTP initialization for ISource
        /// </summary>
        [Theory]
        [InlineData("UDW")]
        public void CheckInitializeFTPSource(string key)
        {
            IMapperService mapperService = GetMapperService(key);
            mapperService.InitializeFTP<ISource>();
            Assert.True(true);
        }

        /// <summary>
        /// The method to check the FTP initialization for ITarget
        /// </summary>
        [Theory]
        [InlineData("EStart")]
        public void CheckInitializeFTPTarget(string key)
        {
            IMapperService mapperService = GetMapperService(key);
            mapperService.InitializeFTP<ITarget>();
            Assert.True(true);
        }

        /// <summary>
        /// The method to check the failure of FTP initialization
        /// </summary>
        [Fact]
        public void CheckInitializeFailed()
        {
            IMapperService mapperService = GetMapperService();
            Action invocation = () => mapperService.InitializeFTP<IMapperService>();
            Assert.Throws<NotImplementedException>(invocation);
        }
        #endregion


        /// <summary>
        /// The method returns a IMapperService instance
        /// </summary>
        /// <returns></returns>
        private IMapperService GetMapperService()
        {
            var context = new DefaultHttpContext();
            var mockHttpContext = new Mock<IHttpContextAccessor>();
            context.Items.Add("Mappers", _contexts.First());
            mockHttpContext.Setup(_ => _.HttpContext).Returns(context);
            return new MapperService(mockHttpContext.Object, _ftp.Object);
        }

        /// <summary>
        /// The method returns a IMapperService instance using the key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private IMapperService GetMapperService(string key)
        {
            var context = new DefaultHttpContext();
            var mockHttpContext = new Mock<IHttpContextAccessor>();
            context.Items.Add("Mappers", _contexts.First(x => x.Key.Equals(key)));
            mockHttpContext.Setup(_ => _.HttpContext).Returns(context);
            return new MapperService(mockHttpContext.Object, _ftp.Object);
        }
    }
}
