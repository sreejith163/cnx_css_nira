using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Controllers;
using Css.Api.Scheduling.UnitTest.Mock;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Xunit;

namespace Css.Api.Scheduling.UnitTest.Controllers
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
        SchedulingCodeTypesController controller;

        /// <summary>
        /// The service
        /// </summary>
        ISchedulingCodeTypeService service;

        /// <summary>
        /// 
        /// </summary>
        public SchedulingCodeTypesControllerShould()
        {
            mockSchedulingCodeTypeService = new Mock<ISchedulingCodeTypeService>();
            controller = new SchedulingCodeTypesController(mockSchedulingCodeTypeService.Object);
        }

        #region GetSchedulingCode

        [Fact]
        public async void GetSchedulingCodes()
        {
            mockSchedulingCodeTypeService.Setup(mr => mr.GetSchedulingCodeTypes()).Returns =>
                MockSchedulingCodeTypeService.GetSchedulingCodeTypes());

            var value = await controller.GetSchedulingCodes();

            Assert.Equal((int)HttpStatusCode.OK, (value as ObjectResult).StatusCode);
        }

        #endregion
    }
}
