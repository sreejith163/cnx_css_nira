using Css.Api.AdminOps.Business.Interfaces;
using Css.Api.AdminOps.Controllers;
using Css.Api.AdminOps.UnitTest.Mock;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using Xunit;

namespace Css.Api.AdminOps.UnitTest.Controllers
{
    public class SchedulingCodeTypesControllerShould
    {
        /// <summary>
        /// The mock scheduling code service
        /// </summary>
        private readonly Mock<ISchedulingCodeTypeService> mockSchedulingCodeTypeService;

        /// <summary>
        /// The controller
        /// </summary>
        private readonly SchedulingCodeTypesController controller;

        /// <summary>
        /// The mock scheduling code type data
        /// </summary>
        private readonly MockSchedulingCodeTypeData mockSchedulingCodeTypeData;

        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulingCodeTypesControllerShould"/> class.
        /// </summary>
        public SchedulingCodeTypesControllerShould()
        {
            mockSchedulingCodeTypeService = new Mock<ISchedulingCodeTypeService>();
            mockSchedulingCodeTypeData = new MockSchedulingCodeTypeData();
            controller = new SchedulingCodeTypesController(mockSchedulingCodeTypeService.Object);
        }

        #region GetSchedulingCode

        [Fact]
        public async void GetSchedulingCodes()
        {
            mockSchedulingCodeTypeService.Setup(mr => mr.GetSchedulingCodeTypes()).ReturnsAsync(() =>
              mockSchedulingCodeTypeData.GetSchedulingCodeTypes());

            var value = await controller.GetSchedulingCodes();

            Assert.Equal((int)HttpStatusCode.OK, (value as ObjectResult).StatusCode);
        }

        #endregion
    }
}
