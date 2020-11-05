using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Controllers;
using Css.Api.Scheduling.Models.DTO.Request.SchedulingCode;
using Css.Api.Scheduling.UnitTest.Mock;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Net;
using Xunit;

namespace Css.Api.Scheduling.UnitTest.Controllers
{
    public class SchedulingCodesControllerShould
    {
        /// <summary>
        /// The mock scheduling code service
        /// </summary>
        private readonly Mock<ISchedulingCodeService> mockSchedulingCodeService;

        /// <summary>
        /// The controller
        /// </summary>
        SchedulingCodesController controller;

        /// <summary>
        /// The mock scheduling code data
        /// </summary>
        private MockSchedulingCodeData mockSchedulingCodeData;

        /// <summary>
        /// 
        /// </summary>
        public SchedulingCodesControllerShould()
        {
            mockSchedulingCodeService = new Mock<ISchedulingCodeService>();
            mockSchedulingCodeData = new MockSchedulingCodeData();
            controller = new SchedulingCodesController(mockSchedulingCodeService.Object);
        }

        #region GetSchedulingCodes

        [Fact]
        public async void GetSchedulingCodes()
        {
            SchedulingCodeQueryParameters queryParameters = new SchedulingCodeQueryParameters();

            mockSchedulingCodeService.Setup(mr => mr.GetSchedulingCodes(It.IsAny<SchedulingCodeQueryParameters>())).ReturnsAsync(
                (SchedulingCodeQueryParameters queryParameters) => mockSchedulingCodeData.GetSchedulingCodes(queryParameters));

            var value = await controller.GetSchedulingCodes(queryParameters);

            Assert.Equal((int)HttpStatusCode.OK, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region GetSchedulingCode

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetSchedulingCode_ReturnsOKResult(int schedulingCodeId)
        {
            mockSchedulingCodeService.Setup(mr => mr.GetSchedulingCode(It.IsAny<SchedulingCodeIdDetails>())).ReturnsAsync(
                (SchedulingCodeIdDetails idDetails) => mockSchedulingCodeData.GetSchedulingCode(new SchedulingCodeIdDetails { SchedulingCodeId = schedulingCodeId }));

            var value = await controller.GetSchedulingCode(schedulingCodeId);

            Assert.Equal((int)HttpStatusCode.OK, (value as ObjectResult).StatusCode);
        }

        [Theory]
        [InlineData(100)]
        [InlineData(101)]
        public async void GetSchedulingCode_ReturnsNotFoundResult(int schedulingCodeId)
        {
            mockSchedulingCodeService.Setup(mr => mr.GetSchedulingCode(It.IsAny<SchedulingCodeIdDetails>())).ReturnsAsync(
                (SchedulingCodeIdDetails idDetails) => mockSchedulingCodeData.GetSchedulingCode(new SchedulingCodeIdDetails { SchedulingCodeId = schedulingCodeId }));

            var value = await controller.GetSchedulingCode(schedulingCodeId);

            Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region CreateSchedulingCode

        [Fact]
        public async void CreateSchedulingCode()
        {
            CreateSchedulingCode codeDetails = new CreateSchedulingCode()
            {
                RefId = 4,
                CodeTypes = new List<int>(),
                PriorityNumber = 4,
                CreatedBy = "admin",
                Description = "test",
                IconId = 1
            };

            mockSchedulingCodeService.Setup(mr => mr.CreateSchedulingCode(It.IsAny<CreateSchedulingCode>())).ReturnsAsync(
                (CreateSchedulingCode code) => mockSchedulingCodeData.CreateSchedulingCode(codeDetails));

            var value = await controller.CreateSchedulingCode(codeDetails);

            Assert.Equal((int)HttpStatusCode.Created, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region UpdateSchedulingCode

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void UpdateSchedulingCode_ReturnsNoContentResult(int schedulingCodeId)
        {
            UpdateSchedulingCode schedulingCode = new UpdateSchedulingCode()
            {
                CodeTypes = new List<int>(),
                Description = "test",
                IconId = 2,
                PriorityNumber = 4,
                ModifiedBy = "admin"
            };

            mockSchedulingCodeService.Setup(mr => mr.UpdateSchedulingCode(It.IsAny<SchedulingCodeIdDetails>(), It.IsAny<UpdateSchedulingCode>())).ReturnsAsync(
                (SchedulingCodeIdDetails idDetails, UpdateSchedulingCode update) =>
                mockSchedulingCodeData.UpdateSchedulingCode(new SchedulingCodeIdDetails { SchedulingCodeId = schedulingCodeId }, schedulingCode));

            var value = await controller.UpdateSchedulingCode(schedulingCodeId,schedulingCode);

            Assert.Equal((int)HttpStatusCode.NoContent, (value as ObjectResult).StatusCode);
        }

        [Theory]
        [InlineData(100)]
        [InlineData(101)]
        public async void UpdateSchedulingCode_ReturnsNotFoundResult(int schedulingCodeId)
        {
            UpdateSchedulingCode schedulingCode = new UpdateSchedulingCode()
            {
                CodeTypes = new List<int>(),
                Description = "test",
                IconId = 2,
                PriorityNumber = 4,
                ModifiedBy = "admin"
            };

            mockSchedulingCodeService.Setup(mr => mr.UpdateSchedulingCode(It.IsAny<SchedulingCodeIdDetails>(), It.IsAny<UpdateSchedulingCode>())).ReturnsAsync(
                (SchedulingCodeIdDetails idDetails, UpdateSchedulingCode update) =>
                mockSchedulingCodeData.UpdateSchedulingCode(new SchedulingCodeIdDetails { SchedulingCodeId = schedulingCodeId }, schedulingCode));

            var value = await controller.UpdateSchedulingCode(schedulingCodeId, schedulingCode);

            Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region DeleteClient

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void DeleteSchedulingCode_ReturnsNoContentResult(int schedulingCodeId)
        {
            mockSchedulingCodeService.Setup(mr => mr.DeleteSchedulingCode(It.IsAny<SchedulingCodeIdDetails>())).ReturnsAsync(
                (SchedulingCodeIdDetails idDetails) => mockSchedulingCodeData.DeleteSchedulingCodes(new SchedulingCodeIdDetails { SchedulingCodeId = schedulingCodeId }));

            var value = await controller.DeleteSchedulingCode(schedulingCodeId);

            Assert.Equal((int)HttpStatusCode.NoContent, (value as ObjectResult).StatusCode);
        }

        [Theory]
        [InlineData(100)]
        [InlineData(101)]
        public async void DeleteSchedulingCode_ReturnsNotFoundResult(int schedulingCodeId)
        {
            mockSchedulingCodeService.Setup(mr => mr.DeleteSchedulingCode(It.IsAny<SchedulingCodeIdDetails>())).ReturnsAsync(
                (SchedulingCodeIdDetails idDetails) => mockSchedulingCodeData.DeleteSchedulingCodes(new SchedulingCodeIdDetails { SchedulingCodeId = schedulingCodeId }));

            var value = await controller.DeleteSchedulingCode(schedulingCodeId);

            Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        }

        #endregion
    }
}
