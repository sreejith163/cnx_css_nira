using Css.Api.Core.Utilities.Extensions;
using Css.Api.Reporting.Business.Interfaces;
using Css.Api.Reporting.Business.Strategies;
using Css.Api.Reporting.Business.UnitTest.Mocks;
using Css.Api.Reporting.Models.DTO.Processing;
using Css.Api.Reporting.Models.DTO.Response;
using Css.Api.Reporting.Models.Enums;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Css.Api.Reporting.Business.UnitTest.Strategies
{
    /// <summary>
    /// The unit testing class for import strategy
    /// </summary>
    public class ImportStrategyShould
    {
        #region Private Properties

        /// <summary>
        /// The mock service resolver
        /// </summary>
        private Mock<IServiceResolver> _mockResolver;

        /// <summary>
        /// The import strategy instance
        /// </summary>
        private IImportStrategy _importStrategy;

        /// <summary>
        /// The mock factory data
        /// </summary>
        private readonly MockFactoryData _data;
        #endregion 

        #region Constructor

        /// <summary>
        /// Constructor to initialize mock data
        /// </summary>
        public ImportStrategyShould()
        {
            _data = new MockFactoryData();
        }
        #endregion

        #region Process(string key)

        /// <summary>
        /// The method to test the successful processing
        /// </summary>
        [Fact]
        public async void CheckProcessSuccess()
        {
            var response = await CheckProcess((int)ProcessStatus.Success);
            Assert.NotNull(response);
            Assert.IsType<ImportStrategyResponse>(response);
            Assert.Single(response.Completed);
            Assert.Empty(response.Partial);
            Assert.Empty(response.Failed);
            Assert.Equal(1, response.TotalSources);
            Assert.Equal(ProcessStatus.Success.GetDescription(), response.Status);
        }

        /// <summary>
        /// The method to test the partial processing
        /// </summary>
        [Fact]
        public async void CheckProcessPartial()
        {
            var response = await CheckProcess((int)ProcessStatus.Partial);
            Assert.NotNull(response);
            Assert.IsType<ImportStrategyResponse>(response);
            Assert.Single(response.Partial);
            Assert.Empty(response.Completed);
            Assert.Empty(response.Failed);
            Assert.Equal(1, response.TotalSources);
            Assert.Equal(ProcessStatus.Partial.GetDescription(), response.Status);
        }

        /// <summary>
        /// The method to test the failed processing
        /// </summary>
        [Fact]
        public async void CheckProcessFailed()
        {
            var response = await CheckProcess((int)ProcessStatus.Failed);
            Assert.NotNull(response);
            Assert.IsType<ImportStrategyResponse>(response);
            Assert.Single(response.Failed);
            Assert.Empty(response.Partial);
            Assert.Empty(response.Completed);
            Assert.Equal(1, response.TotalSources);
            Assert.Equal(ProcessStatus.Failed.GetDescription(), response.Status);
        }

        #region Private Method
        
        /// <summary>
        /// The method to execute the import strategy process based on the enum value ProcessStatus
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        private async Task<dynamic> CheckProcess(int status)
        {
            _mockResolver = new Mock<IServiceResolver>();
            _mockResolver.SetReturnsDefault<IImporter>(_data.GetMockImporter(status).Object);
            _mockResolver.SetReturnsDefault<IExporter>(_data.GetMockExporter(status).Object);
            _mockResolver.SetReturnsDefault<Task<List<DataFeed>>>(Task.FromResult<List<DataFeed>>(_data.GetFeeds()));
            _mockResolver.SetReturnsDefault<Task>(Task.CompletedTask);

            _importStrategy = new ImportStrategy(_mockResolver.Object);

            return await _importStrategy.Process("UDW");
        }
        #endregion
        #endregion
    }
}
