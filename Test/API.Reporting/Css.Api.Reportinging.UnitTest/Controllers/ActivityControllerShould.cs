using Css.Api.Reporting.Business.Interfaces;
using Css.Api.Reporting.Controllers;
using Css.Api.Reporting.Models.DTO.Response;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Css.Api.Reporting.UnitTest.Controllers
{
    /// <summary>
    /// The unit testing class for import controller
    /// </summary>
    public class ActivityControllerShould
    {
        #region Private Properties

        /// <summary>
        /// The mock activity strategy
        /// </summary>
        private readonly Mock<IActivityStrategy> _mockActivityStrategy;

        /// <summary>
        /// The import controller
        /// </summary>
        private readonly ActivityController _activityController;
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor to initialize mock data
        /// </summary>
        public ActivityControllerShould()
        {
            _mockActivityStrategy = new Mock<IActivityStrategy>();
            _mockActivityStrategy.SetReturnsDefault<Task<ActivityResponse>>(Task.FromResult<ActivityResponse>(new ActivityResponse()));
            _activityController = new ActivityController(_mockActivityStrategy.Object);
        }
        #endregion

        #region Post()
        
        /// <summary>
        /// The method to test the post call
        /// </summary>
        [Fact]
        public async void CheckPost()
        {
            var response = await _activityController.Post();
            Assert.NotNull(response);
            Assert.IsType<ActivityResponse>(response);
        }
        #endregion
    }
}
