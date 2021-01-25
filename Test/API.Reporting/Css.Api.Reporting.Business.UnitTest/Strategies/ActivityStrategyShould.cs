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
    /// The unit testing class for activity strategy
    /// </summary>
    public class ActivityStrategyShould
    {
        #region Private Properties

        /// <summary>
        /// The mock service resolver
        /// </summary>
        private Mock<IServiceFactory> _mockFactory;

        /// <summary>
        /// The import strategy instance
        /// </summary>
        private IActivityStrategy _activityStrategy;

        /// <summary>
        /// The mock factory data
        /// </summary>
        private readonly MockFactoryData _data;
        #endregion 

        #region Constructor

        /// <summary>
        /// Constructor to initialize mock data
        /// </summary>
        public ActivityStrategyShould()
        {
            _data = new MockFactoryData();
            _mockFactory = new Mock<IServiceFactory>();
            _mockFactory.SetReturnsDefault<ISource>(_data.GetMockSource().Object);
            _mockFactory.SetReturnsDefault<ITarget>(_data.GetMockTarget().Object);
            _mockFactory.SetReturnsDefault<Task<List<DataFeed>>>(Task.FromResult<List<DataFeed>>(_data.GetFeeds()));
            _mockFactory.SetReturnsDefault<Task>(Task.CompletedTask);

            _activityStrategy = new ActivityStrategy(_mockFactory.Object);
        }
        #endregion

        #region Process()

        /// <summary>
        /// The method to test the successful processing
        /// </summary>
        [Fact]
        public async void CheckStrategy()
        {
            var response = await _activityStrategy.Process();
            Assert.NotNull(response);
            Assert.IsType<ActivityResponse>(response);
        }
        #endregion
    }
}
