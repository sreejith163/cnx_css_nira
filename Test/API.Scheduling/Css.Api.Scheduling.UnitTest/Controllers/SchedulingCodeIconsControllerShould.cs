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
    public class SchedulingCodeIconsControllerShould
    {
        /// <summary>
        /// The mock scheduling code service
        /// </summary>
        private readonly Mock<ISchedulingCodeIconService> mockSchedulingCodeIconService;

        /// <summary>
        /// The controller
        /// </summary>
        SchedulingCodeIconsController controller;

        /// <summary>
        /// The service
        /// </summary>
        ISchedulingCodeIconService service;

        /// <summary>
        /// 
        /// </summary>
        public SchedulingCodeIconsControllerShould()
        {
            mockSchedulingCodeIconService = new Mock<ISchedulingCodeIconService>();
            controller = new SchedulingCodeIconsController(mockSchedulingCodeIconService.Object);
        }

        #region GetSchedulingCode

        [Fact]
        public async void GetSchedulingCodes()
        {
            mockSchedulingCodeIconService.Setup(mr => mr.GetSchedulingCodeIcons  =>
                MockSchedulingCodeIconService.GetSchedulingCodeIcons());

            var value = await controller.GetSchedulingCodeTypes();

            Assert.Equal((int)HttpStatusCode.OK, (value as ObjectResult).StatusCode);
        }

        #endregion
    }
}
