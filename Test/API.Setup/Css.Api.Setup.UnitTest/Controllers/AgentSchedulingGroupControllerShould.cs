using Css.Api.Setup.Business.Interfaces;
using Css.Api.Setup.Controllers;
using Css.Api.Setup.Models.DTO.Request.AgentSchedulingGroup;
using Css.Api.Setup.UnitTest.Mock;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using Xunit;

namespace Css.Api.Setup.UnitTest.Controllers
{
    public class AgentSchedulingGroupControllerShould
    {
        /// <summary>
        /// The mock agent scheduling group service
        /// </summary>
        private readonly Mock<IAgentSchedulingGroupService> mockAgentSchedulingGroupService;

        /// <summary>
        /// The controller
        /// </summary>
        private readonly AgentSchedulingGroupsController controller;

        /// <summary>
        /// The mock agent scheduling group data
        /// </summary>
        private readonly MockAgentSchedulingGroupData mockAgentSchedulingGroupData;

        /// <summary>
        /// 
        /// </summary>
        public AgentSchedulingGroupControllerShould()
        {
            mockAgentSchedulingGroupService = new Mock<IAgentSchedulingGroupService>();
            mockAgentSchedulingGroupData = new MockAgentSchedulingGroupData();
            controller = new AgentSchedulingGroupsController(mockAgentSchedulingGroupService.Object);
        }

        #region GetAgentSchedulingGroups

        [Fact]
        public async void GetAgentSchedulingGroups()
        {
            AgentSchedulingGroupQueryParameter queryParameters = new AgentSchedulingGroupQueryParameter();

            mockAgentSchedulingGroupService.Setup(mr => mr.GetAgentSchedulingGroups(It.IsAny<AgentSchedulingGroupQueryParameter>())).ReturnsAsync(
                (AgentSchedulingGroupQueryParameter queryParameters) => mockAgentSchedulingGroupData.GetAgentSchedulingGroups(queryParameters));

            var value = await controller.GetAgentSchedulingGroups(queryParameters);

            Assert.Equal((int)HttpStatusCode.OK, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region GetAgentSchedulingGroup

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetAgentSchedulingGroup_ReturnsOKResult(int agentSchedulingGroupId)
        {
            mockAgentSchedulingGroupService.Setup(mr => mr.GetAgentSchedulingGroup(It.IsAny<AgentSchedulingGroupIdDetails>())).ReturnsAsync(
                (AgentSchedulingGroupIdDetails idDetails) => mockAgentSchedulingGroupData.GetAgentSchedulingGroup(new AgentSchedulingGroupIdDetails { AgentSchedulingGroupId = agentSchedulingGroupId }));

            var value = await controller.GetAgentSchedulingGroup(agentSchedulingGroupId);

            Assert.Equal((int)HttpStatusCode.OK, (value as ObjectResult).StatusCode);
        }

        [Theory]
        [InlineData(100)]
        [InlineData(101)]
        public async void GetAgentSchedulingGroup_ReturnsNotFoundResult(int agentSchedulingGroupId)
        {
            mockAgentSchedulingGroupService.Setup(mr => mr.GetAgentSchedulingGroup(It.IsAny<AgentSchedulingGroupIdDetails>())).ReturnsAsync(
                (AgentSchedulingGroupIdDetails idDetails) => mockAgentSchedulingGroupData.GetAgentSchedulingGroup(new AgentSchedulingGroupIdDetails { AgentSchedulingGroupId = agentSchedulingGroupId }));

            var value = await controller.GetAgentSchedulingGroup(agentSchedulingGroupId);

            Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region CreateAgentSchedulingGroup

        [Fact]
        public async void CreateAgentSchedulingGroup_ReturnsConflictResult()
        {
            CreateAgentSchedulingGroup agentSchedulingGroupDetails = new CreateAgentSchedulingGroup()
            {
                RefId = 4,
                CreatedBy = "admin",
                Name = "agentSchedulingGroup1",
                SkillTagId = 1,
                FirstDayOfWeek = 1,
                TimezoneId = 1
            };

            mockAgentSchedulingGroupService.Setup(mr => mr.CreateAgentSchedulingGroup(It.IsAny<CreateAgentSchedulingGroup>())).ReturnsAsync(
                (CreateAgentSchedulingGroup agentSchedulingGroup) => mockAgentSchedulingGroupData.CreateAgentSchedulingGroup(agentSchedulingGroupDetails));

            var value = await controller.CreateAgentSchedulingGroup(agentSchedulingGroupDetails);

            Assert.Equal((int)HttpStatusCode.Conflict, (value as ObjectResult).StatusCode);
        }

        [Fact]
        public async void CreateAgentSchedulingGroup_ReturnsOkResult()
        {
            CreateAgentSchedulingGroup agentSchedulingGroupDetails = new CreateAgentSchedulingGroup()
            {
                RefId = 4,
                CreatedBy = "admin",
                Name = "agentSchedulingGroup",
                SkillTagId = 1,
                FirstDayOfWeek = 1,
                TimezoneId = 1
            };

            mockAgentSchedulingGroupService.Setup(mr => mr.CreateAgentSchedulingGroup(It.IsAny<CreateAgentSchedulingGroup>())).ReturnsAsync(
                (CreateAgentSchedulingGroup agentSchedulingGroup) => mockAgentSchedulingGroupData.CreateAgentSchedulingGroup(agentSchedulingGroupDetails));

            var value = await controller.CreateAgentSchedulingGroup(agentSchedulingGroupDetails);

            Assert.Equal((int)HttpStatusCode.Created, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region UpdateAgentSchedulingGroup

        [Theory]
        [InlineData(100)]
        [InlineData(101)]
        public async void UpdateAgentSchedulingGroup_ReturnsNotFoundResult(int agentSchedulingGroupId)
        {
            UpdateAgentSchedulingGroup agentSchedulingGroup = new UpdateAgentSchedulingGroup()
            {
                Name = "agentSchedulingGroup",
                SkillTagId = 1,
                FirstDayOfWeek = 1,
                ModifiedBy = "admin",
                TimezoneId = 1
            };

            mockAgentSchedulingGroupService.Setup(mr => mr.UpdateAgentSchedulingGroup(It.IsAny<AgentSchedulingGroupIdDetails>(), It.IsAny<UpdateAgentSchedulingGroup>())).ReturnsAsync(
                (AgentSchedulingGroupIdDetails idDetails, UpdateAgentSchedulingGroup update) =>
                mockAgentSchedulingGroupData.UpdateAgentSchedulingGroup(new AgentSchedulingGroupIdDetails { AgentSchedulingGroupId = agentSchedulingGroupId }, agentSchedulingGroup));

            var value = await controller.UpdateAgentSchedulingGroup(agentSchedulingGroupId, agentSchedulingGroup);

            Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        }

        [Theory]
        [InlineData(1, "agentSchedulingGroup2")]
        [InlineData(2, "agentSchedulingGroup3")]
        public async void UpdateAgentSchedulingGroup_ReturnsConflictResult(int agentSchedulingGroupId, string name)
        {
            UpdateAgentSchedulingGroup agentSchedulingGroup = new UpdateAgentSchedulingGroup()
            {
                Name = name,
                SkillTagId = 1,
                FirstDayOfWeek = 1,
                ModifiedBy = "admin",
                TimezoneId = 1
            };

            mockAgentSchedulingGroupService.Setup(mr => mr.UpdateAgentSchedulingGroup(It.IsAny<AgentSchedulingGroupIdDetails>(), It.IsAny<UpdateAgentSchedulingGroup>())).ReturnsAsync(
                (AgentSchedulingGroupIdDetails idDetails, UpdateAgentSchedulingGroup update) =>
                mockAgentSchedulingGroupData.UpdateAgentSchedulingGroup(new AgentSchedulingGroupIdDetails { AgentSchedulingGroupId = agentSchedulingGroupId }, agentSchedulingGroup));

            var value = await controller.UpdateAgentSchedulingGroup(agentSchedulingGroupId, agentSchedulingGroup);

            Assert.Equal((int)HttpStatusCode.Conflict, (value as ObjectResult).StatusCode);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void UpdateAgentSchedulingGroup_ReturnsNoContentResult(int agentSchedulingGroupId)
        {
            UpdateAgentSchedulingGroup agentSchedulingGroup = new UpdateAgentSchedulingGroup()
            {
                Name = "agentSchedulingGroup",
                SkillTagId = 1,
                FirstDayOfWeek = 1,
                ModifiedBy = "admin",
                TimezoneId = 1
            };

            mockAgentSchedulingGroupService.Setup(mr => mr.UpdateAgentSchedulingGroup(It.IsAny<AgentSchedulingGroupIdDetails>(), It.IsAny<UpdateAgentSchedulingGroup>())).ReturnsAsync(
                (AgentSchedulingGroupIdDetails idDetails, UpdateAgentSchedulingGroup update) =>
                mockAgentSchedulingGroupData.UpdateAgentSchedulingGroup(new AgentSchedulingGroupIdDetails { AgentSchedulingGroupId = agentSchedulingGroupId }, agentSchedulingGroup));

            var value = await controller.UpdateAgentSchedulingGroup(agentSchedulingGroupId, agentSchedulingGroup);

            Assert.Equal((int)HttpStatusCode.NoContent, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region DeleteClient

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void DeleteAgentSchedulingGroup_ReturnsNoContentResult(int agentSchedulingGroupId)
        {
            mockAgentSchedulingGroupService.Setup(mr => mr.DeleteAgentSchedulingGroup(It.IsAny<AgentSchedulingGroupIdDetails>())).ReturnsAsync(
                (AgentSchedulingGroupIdDetails idDetails) => mockAgentSchedulingGroupData.DeleteAgentSchedulingGroups(new AgentSchedulingGroupIdDetails { AgentSchedulingGroupId = agentSchedulingGroupId }));

            var value = await controller.DeleteAgentSchedulingGroup(agentSchedulingGroupId);

            Assert.Equal((int)HttpStatusCode.NoContent, (value as ObjectResult).StatusCode);
        }

        [Theory]
        [InlineData(100)]
        [InlineData(101)]
        public async void DeleteAgentSchedulingGroup_ReturnsNotFoundResult(int agentSchedulingGroupId)
        {
            mockAgentSchedulingGroupService.Setup(mr => mr.DeleteAgentSchedulingGroup(It.IsAny<AgentSchedulingGroupIdDetails>())).ReturnsAsync(
                (AgentSchedulingGroupIdDetails idDetails) => mockAgentSchedulingGroupData.DeleteAgentSchedulingGroups(new AgentSchedulingGroupIdDetails { AgentSchedulingGroupId = agentSchedulingGroupId }));

            var value = await controller.DeleteAgentSchedulingGroup(agentSchedulingGroupId);

            Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        }

        #endregion
    }
}
