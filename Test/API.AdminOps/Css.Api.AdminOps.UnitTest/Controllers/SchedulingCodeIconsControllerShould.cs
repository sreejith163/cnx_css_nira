using Css.Api.AdminOps.Business.Interfaces;
using Css.Api.AdminOps.Controllers;
using Css.Api.AdminOps.UnitTest.Mock;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using Xunit;

namespace Css.Api.AdminOps.UnitTest.Controllers
{
    public class SchedulingCodeIconsControllerShould
    {
        /// <summary>
        /// The mock scheduling code service
        /// </summary>
        private readonly Mock<ISchedulingCodeIconService> mockSchedulingCodeIconService;

        /// <summary>
        /// The controller
        /// </summary>
        private readonly SchedulingCodeIconsController controller;

        /// <summary>
        /// The mock scheduling code icon data
        /// </summary>
        private readonly MockSchedulingCodeIconData mockSchedulingCodeIconData;

        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulingCodeIconsControllerShould"/> class.
        /// </summary>
        public SchedulingCodeIconsControllerShould()
        {
            mockSchedulingCodeIconService = new Mock<ISchedulingCodeIconService>();
            mockSchedulingCodeIconData = new MockSchedulingCodeIconData();
            controller = new SchedulingCodeIconsController(mockSchedulingCodeIconService.Object);
        }

        #region GetSchedulingCode

        /// <summary>
        /// Gets the scheduling codes.
        /// </summary>
        [Fact]
        public async void GetSchedulingCodes()
        {
            mockSchedulingCodeIconService.Setup(mr => mr.GetSchedulingCodeIcons()).ReturnsAsync(
                () => mockSchedulingCodeIconData.GetSchedulingCodeIcons());

            var value = await controller.GetSchedulingCodeTypes();

            Assert.Equal((int)HttpStatusCode.OK, (value as ObjectResult).StatusCode);
        }

        #endregion
    }
}
