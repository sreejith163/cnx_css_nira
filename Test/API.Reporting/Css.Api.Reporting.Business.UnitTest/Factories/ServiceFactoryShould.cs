using Css.Api.Reporting.Business.Exceptions;
using Css.Api.Reporting.Business.Factories;
using Css.Api.Reporting.Business.Interfaces;
using Css.Api.Reporting.Business.UnitTest.Mocks;
using Css.Api.Reporting.Models.DTO.Mappers;
using Css.Api.Reporting.Models.DTO.Request;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Css.Api.Reporting.Business.UnitTest.Factories
{
    /// <summary>
    /// The unit testing class for service factory
    /// </summary>
    public class ServiceFactoryShould
    {
        #region Private Properties
        
        /// <summary>
        /// The mock factory data
        /// </summary>
        private readonly MockFactoryData _factoryData;

        /// <summary>
        /// The list of mock Mapping contexts
        /// </summary>
        private readonly List<MappingContext> _contexts;
        #endregion

        #region Constructor
        
        /// <summary>
        /// Constructor
        /// </summary>
        public ServiceFactoryShould()
        {
            _factoryData = new MockFactoryData();
            _contexts = _factoryData.GetMappingContexts();
        }
        #endregion

        #region Map<ISource>()

        /// <summary>
        /// The method to test the successful source mapping of the factory
        /// </summary>
        /// <param name="key"></param>
        [Theory]
        [InlineData("UDW")]
        [InlineData("EStart")]
        public void CheckSourceMap(string key)
        {
            IServiceFactory serviceFactory = GetServiceFactory(key);
            var response = serviceFactory.Map<ISource>();

            Assert.NotNull(response);
            Assert.IsAssignableFrom<ISource>(response);
        }
        #endregion

        #region Map<ITarget>()

        /// <summary>
        /// The method to test the successful target mapping of the factory
        /// </summary>
        /// <param name="key"></param>
        [Theory]
        [InlineData("UDW")]
        [InlineData("EStart")]
        public void CheckTargetMap(string key)
        {
            IServiceFactory serviceFactory = GetServiceFactory(key);
            var response = serviceFactory.Map<ITarget>();

            Assert.NotNull(response);
            Assert.IsAssignableFrom<ITarget>(response);
        }
        #endregion

        #region Map<T>()
        /// <summary>
        /// The method to test the unmmapped export service for the key
        /// </summary>
        /// <param name="key"></param>
        [Theory]
        [InlineData("Test")]
        public void CheckInvalidMap(string key)
        {
            IServiceFactory serviceFactory = GetServiceFactory(key);

            Action invocation = () => serviceFactory.Map<IFTPService>();
            Assert.Throws<MappingException>(invocation);
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// The method to generate the IServiceFactory for the key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private IServiceFactory GetServiceFactory(string key)
        {
            var mapperService = new Mock<IMapperService>();
            mapperService.SetReturnsDefault<MappingContext>(_contexts.First(x => x.Key.Equals(key)));
            return new ServiceFactory(mapperService.Object, _factoryData.GetSources(), _factoryData.GetTargets());
        }
        #endregion
    }
}
