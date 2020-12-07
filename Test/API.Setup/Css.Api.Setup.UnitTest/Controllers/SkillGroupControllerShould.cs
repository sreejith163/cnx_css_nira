using Css.Api.Setup.Business.Interfaces;
using Css.Api.Setup.Controllers;
using Css.Api.Setup.Models.DTO.Request.SkillGroup;
using Css.Api.Setup.UnitTest.Mock;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using Xunit;

namespace Css.Api.Setup.UnitTest.Controllers
{
    public class SkillGroupControllerShould
    {
        /// <summary>
        /// The mock skill group service
        /// </summary>
        private readonly Mock<ISkillGroupService> mockSkillGroupService;

        /// <summary>
        /// The controller
        /// </summary>
        SkillGroupsController controller;

        /// <summary>
        /// The mock skill group data
        /// </summary>
        private MockSkillGroupData mockSkillGroupData;

        /// <summary>
        /// 
        /// </summary>
        public SkillGroupControllerShould()
        {
            mockSkillGroupService = new Mock<ISkillGroupService>();
            mockSkillGroupData = new MockSkillGroupData();
            controller = new SkillGroupsController(mockSkillGroupService.Object);
        }

        #region GetSkillGroups

        [Fact]
        public async void GetSkillGroups()
        {
            SkillGroupQueryParameter queryParameters = new SkillGroupQueryParameter();

            mockSkillGroupService.Setup(mr => mr.GetSkillGroups(It.IsAny<SkillGroupQueryParameter>())).ReturnsAsync(
                (SkillGroupQueryParameter queryParameters) => mockSkillGroupData.GetSkillGroups(queryParameters));

            var value = await controller.GetSkillGroups(queryParameters);

            Assert.Equal((int)HttpStatusCode.OK, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region GetSkillGroup

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetSkillGroup_ReturnsOKResult(int skillGroupId)
        {
            mockSkillGroupService.Setup(mr => mr.GetSkillGroup(It.IsAny<SkillGroupIdDetails>())).ReturnsAsync(
                (SkillGroupIdDetails idDetails) => mockSkillGroupData.GetSkillGroup(new SkillGroupIdDetails { SkillGroupId = skillGroupId }));

            var value = await controller.GetSkillGroup(skillGroupId);

            Assert.Equal((int)HttpStatusCode.OK, (value as ObjectResult).StatusCode);
        }

        [Theory]
        [InlineData(100)]
        [InlineData(101)]
        public async void GetSkillGroup_ReturnsNotFoundResult(int skillGroupId)
        {
            mockSkillGroupService.Setup(mr => mr.GetSkillGroup(It.IsAny<SkillGroupIdDetails>())).ReturnsAsync(
                (SkillGroupIdDetails idDetails) => mockSkillGroupData.GetSkillGroup(new SkillGroupIdDetails { SkillGroupId = skillGroupId }));

            var value = await controller.GetSkillGroup(skillGroupId);

            Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region CreateSkillGroup

        [Fact]
        public async void CreateSkillGroup_ReturnsConflictResult()
        {
            CreateSkillGroup skillGroupDetails = new CreateSkillGroup()
            {
                RefId = 4,
                CreatedBy = "admin",
                Name = "skillGroup1",
                ClientLobGroupId = 1,
                FirstDayOfWeek = 1,
                TimezoneId = 1
            };

            mockSkillGroupService.Setup(mr => mr.CreateSkillGroup(It.IsAny<CreateSkillGroup>())).ReturnsAsync(
                (CreateSkillGroup skillGroup) => mockSkillGroupData.CreateSkillGroup(skillGroupDetails));

            var value = await controller.CreateSkillGroup(skillGroupDetails);

            Assert.Equal((int)HttpStatusCode.Conflict, (value as ObjectResult).StatusCode);
        }

        [Fact]
        public async void CreateSkillGroup_ReturnsOkResult()
        {
            CreateSkillGroup skillGroupDetails = new CreateSkillGroup()
            {
                RefId = 4,
                CreatedBy = "admin",
                Name = "skillGroup",
                ClientLobGroupId = 1,
                FirstDayOfWeek = 1,
                TimezoneId = 1
            };

            mockSkillGroupService.Setup(mr => mr.CreateSkillGroup(It.IsAny<CreateSkillGroup>())).ReturnsAsync(
                (CreateSkillGroup skillGroup) => mockSkillGroupData.CreateSkillGroup(skillGroupDetails));

            var value = await controller.CreateSkillGroup(skillGroupDetails);

            Assert.Equal((int)HttpStatusCode.Created, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region UpdateSkillGroup

        [Theory]
        [InlineData(100)]
        [InlineData(101)]
        public async void UpdateSkillGroup_ReturnsNotFoundResult(int skillGroupId)
        {
            UpdateSkillGroup skillGroup = new UpdateSkillGroup()
            {
                Name = "skillGroup",
                ClientLobGroupId = 1,
                FirstDayOfWeek = 1,
                ModifiedBy = "admin",
                TimezoneId = 1
            };

            mockSkillGroupService.Setup(mr => mr.UpdateSkillGroup(It.IsAny<SkillGroupIdDetails>(), It.IsAny<UpdateSkillGroup>())).ReturnsAsync(
                (SkillGroupIdDetails idDetails, UpdateSkillGroup update) =>
                mockSkillGroupData.UpdateSkillGroup(new SkillGroupIdDetails { SkillGroupId = skillGroupId }, skillGroup));

            var value = await controller.UpdateSkillGroup(skillGroupId, skillGroup);

            Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        }

        [Theory]
        [InlineData(1, "skillGroup2")]
        [InlineData(2, "skillGroup3")]
        public async void UpdateSkillGroup_ReturnsConflictResult(int skillGroupId, string name)
        {
            UpdateSkillGroup skillGroup = new UpdateSkillGroup()
            {
                Name = name,
                ClientLobGroupId = 1,
                FirstDayOfWeek = 1,
                ModifiedBy = "admin",
                TimezoneId = 1
            };

            mockSkillGroupService.Setup(mr => mr.UpdateSkillGroup(It.IsAny<SkillGroupIdDetails>(), It.IsAny<UpdateSkillGroup>())).ReturnsAsync(
                (SkillGroupIdDetails idDetails, UpdateSkillGroup update) =>
                mockSkillGroupData.UpdateSkillGroup(new SkillGroupIdDetails { SkillGroupId = skillGroupId }, skillGroup));

            var value = await controller.UpdateSkillGroup(skillGroupId, skillGroup);

            Assert.Equal((int)HttpStatusCode.Conflict, (value as ObjectResult).StatusCode);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void UpdateSkillGroup_ReturnsNoContentResult(int skillGroupId)
        {
            UpdateSkillGroup skillGroup = new UpdateSkillGroup()
            {
                Name = "skillGroup",
                ClientLobGroupId = 1,
                FirstDayOfWeek = 1,
                ModifiedBy = "admin",
                TimezoneId = 1
            };

            mockSkillGroupService.Setup(mr => mr.UpdateSkillGroup(It.IsAny<SkillGroupIdDetails>(), It.IsAny<UpdateSkillGroup>())).ReturnsAsync(
                (SkillGroupIdDetails idDetails, UpdateSkillGroup update) =>
                mockSkillGroupData.UpdateSkillGroup(new SkillGroupIdDetails { SkillGroupId = skillGroupId }, skillGroup));

            var value = await controller.UpdateSkillGroup(skillGroupId, skillGroup);

            Assert.Equal((int)HttpStatusCode.NoContent, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region DeleteClient

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void DeleteSkillGroup_ReturnsNoContentResult(int skillGroupId)
        {
            mockSkillGroupService.Setup(mr => mr.DeleteSkillGroup(It.IsAny<SkillGroupIdDetails>())).ReturnsAsync(
                (SkillGroupIdDetails idDetails) => mockSkillGroupData.DeleteSkillGroups(new SkillGroupIdDetails { SkillGroupId = skillGroupId }));

            var value = await controller.DeleteSkillGroup(skillGroupId);

            Assert.Equal((int)HttpStatusCode.NoContent, (value as ObjectResult).StatusCode);
        }

        [Theory]
        [InlineData(100)]
        [InlineData(101)]
        public async void DeleteSkillGroup_ReturnsNotFoundResult(int skillGroupId)
        {
            mockSkillGroupService.Setup(mr => mr.DeleteSkillGroup(It.IsAny<SkillGroupIdDetails>())).ReturnsAsync(
                (SkillGroupIdDetails idDetails) => mockSkillGroupData.DeleteSkillGroups(new SkillGroupIdDetails { SkillGroupId = skillGroupId }));

            var value = await controller.DeleteSkillGroup(skillGroupId);

            Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        }

        #endregion
    }
}