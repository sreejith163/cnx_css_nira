using Css.Api.Reporting.Business.Interfaces;
using Css.Api.Reporting.Business.Resolvers;
using Css.Api.Reporting.Business.UnitTest.Mocks;
using Css.Api.Reporting.Models.DTO.Processing;
using Css.Api.Reporting.Models.Enums;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Css.Api.Reporting.Business.UnitTest.Resolvers
{
    /// <summary>
    /// The unit testing class for service resolver
    /// </summary>
    public class ServiceResolverShould
    {
        #region Private Properties
        /// <summary>
        /// The mock service factory
        /// </summary>
        private readonly Mock<IServiceFactory> _factory;

        /// <summary>
        /// The mock ftp service
        /// </summary>
        private readonly Mock<IFTPService> _ftp;

        /// <summary>
        /// The mock data feed from ftp service
        /// </summary>
        private readonly DataFeed _mockFeed;

        /// <summary>
        /// The service resolver
        /// </summary>
        private readonly IServiceResolver _resolver;
        #endregion

        #region Constructor
        /// <summary>
        /// The construtor
        /// </summary>
        public ServiceResolverShould()
        {
            var mockFactoryData = new MockFactoryData();

            _factory = new Mock<IServiceFactory>();
            _factory.SetReturnsDefault<IImporter>(mockFactoryData.GetMockImporter().Object);
            _factory.SetReturnsDefault<IExporter>(mockFactoryData.GetMockExporter().Object);
            
            _ftp = new Mock<IFTPService>();
            _ftp.SetReturnsDefault<Task<List<DataFeed>>>(Task.FromResult<List<DataFeed>>(mockFactoryData.GetFeeds()));
            _ftp.SetReturnsDefault<Task>(Task.CompletedTask);
            
            _mockFeed = mockFactoryData.GetFeed();
            
            _resolver = new ServiceResolver(_factory.Object, _ftp.Object);
        }
        #endregion

        #region GetImporter(string key)
        /// <summary>
        /// The method to test the get importer functionality of the service resolver
        /// </summary>
        [Fact]
        public void CheckGetImporter()
        {
            var response = _resolver.GetImporter("UDW");
            Assert.NotNull(response);
            Assert.IsAssignableFrom<IImporter>(response);
        }
        #endregion

        #region GetExporter(string key)
        /// <summary>
        /// The method to test the get exporter functionality of the service resolver
        /// </summary>
        [Fact]
        public void CheckGetExporter()
        {
            var response = _resolver.GetExporter("UDW");
            Assert.NotNull(response);
            Assert.IsAssignableFrom<IExporter>(response);
        }
        #endregion

        #region GetDataFromSource(string key)
        /// <summary>
        /// The method to test the get data from source functionality of the service resolver
        /// </summary>
        [Fact]
        public async void CheckGetDataFromSource()
        {
            var response = await _resolver.GetDataFromSource("UDW");
            Assert.NotNull(response);
            Assert.IsType<List<DataFeed>>(response);
        }
        #endregion

        #region Finalize(int status, DataFeed feed)
        /// <summary>
        /// The method to test the finialize functionality of the service resolver based on the input status
        /// </summary>
        /// <param name="status">A value of enum ProcessStatus</param>
        [Theory]
        [InlineData((int)ProcessStatus.Success)]
        [InlineData((int)ProcessStatus.Partial)]
        [InlineData((int)ProcessStatus.Failed)]
        public async void CheckFinalize(int status)
        {
            try
            {
                await _resolver.Finalize(status, _mockFeed);
                Assert.True(true);
            }
            catch
            {
                Assert.True(false);
            }
        }
        #endregion
    }
}
