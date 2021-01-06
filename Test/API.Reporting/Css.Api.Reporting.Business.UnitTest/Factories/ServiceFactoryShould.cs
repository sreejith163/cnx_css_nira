using Css.Api.Reporting.Business.Exceptions;
using Css.Api.Reporting.Business.Factories;
using Css.Api.Reporting.Business.Interfaces;
using Css.Api.Reporting.Business.UnitTest.Mocks;
using Css.Api.Reporting.Models.DTO.Mappers;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
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
        /// The service factory
        /// </summary>
        private readonly IServiceFactory _serviceFactory;
        #endregion

        #region Constructor
        
        /// <summary>
        /// Constructor
        /// </summary>
        public ServiceFactoryShould()
        {
            _factoryData = new MockFactoryData();
            _serviceFactory = new ServiceFactory(_factoryData.GetMapperSettings()
                        , _factoryData.GetImporters()
                        , _factoryData.GetExporters());
        }
        #endregion

        #region Map<IImporter>(string key)
        /// <summary>
        /// The method to test the successful import mapping of the factory for the input key
        /// </summary>
        /// <param name="key"></param>
        [Theory]
        [InlineData("UDW")]
        [InlineData("eStart")]
        public void CheckFactoryImportSucess(string key)
        {
            var response = _serviceFactory.Map<IImporter>(key);

            Assert.NotNull(response);
            Assert.IsAssignableFrom<IImporter>(response);
        }

        /// <summary>
        /// The method to test the unmmapped import service for the key
        /// </summary>
        /// <param name="key"></param>
        [Theory]
        [InlineData("Test")]
        public void CheckFactoryImportServiceNotFound(string key)
        {
            var response = _serviceFactory.Map<IImporter>(key);
            Assert.Null(response);
        }

        /// <summary>
        /// The method to test an unmapped import key 
        /// </summary>
        /// <param name="key"></param>
        [Theory]
        [InlineData("Test1")]
        [InlineData("Test2")]
        public void CheckFactoryImportKeyNotFound(string key)
        {
            Action invocation = () => _serviceFactory.Map<IImporter>(key);
            Assert.Throws<MappingException>(invocation);
        }
        #endregion

        #region Map<IExporter>(string key)
        /// <summary>
        /// The method to test the successful export mapping of the factory for the input key
        /// </summary>
        /// <param name="key"></param>
        [Theory]
        [InlineData("eStart")]
        public void CheckFactoryExportSucess(string key)
        {
            var response = _serviceFactory.Map<IExporter>(key);

            Assert.NotNull(response);
            Assert.IsAssignableFrom<IExporter>(response);
        }

        /// <summary>
        /// The method to test the unmmapped export service for the key
        /// </summary>
        /// <param name="key"></param>
        [Theory]
        [InlineData("Test")]
        public void CheckFactoryExportServiceNotFound(string key)
        {
            var response = _serviceFactory.Map<IExporter>(key);
            Assert.Null(response);
        }

        /// <summary>
        /// The method to test an unmapped export key
        /// </summary>
        /// <param name="key"></param>
        [Theory]
        [InlineData("Test1")]
        [InlineData("Test2")]
        public void CheckFactoryExportKeyNotFound(string key)
        {
            Action invocation = () => _serviceFactory.Map<IExporter>(key);
            Assert.Throws<MappingException>(invocation);
        }
        #endregion
    }
}
