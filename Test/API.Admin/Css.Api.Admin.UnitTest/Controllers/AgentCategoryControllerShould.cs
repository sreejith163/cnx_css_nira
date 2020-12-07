using Css.Api.Admin.Business.Interfaces;
using Css.Api.Admin.Controllers;
using Css.Api.Admin.Models.DTO.Request.AgentCategory;
using Css.Api.Admin.UnitTest.Mock;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using Xunit;

namespace Css.Api.Admin.UnitTest.Controllers
{
    public class AgentCategoryControllerShould
    {
        /// <summary>
        /// The mock agentCategory service
        /// </summary>
        private readonly Mock<IAgentCategoryService> mockAgentCategoryService;

        /// <summary>
        /// The controller
        /// </summary>
        private AgentCategoriesController controller;

        /// <summary>
        /// The mock agentCategory data
        /// </summary>
        private MockAgentCategoryData mockAgentCategoryData;

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentCategoryControllerShould" /> class.
        /// </summary>
        public AgentCategoryControllerShould()
        {
            mockAgentCategoryService = new Mock<IAgentCategoryService>();
            mockAgentCategoryData = new MockAgentCategoryData();
            controller = new AgentCategoriesController(mockAgentCategoryService.Object);
        }

        #region GetAgentCategories

        /// <summary>
        /// Gets the agentCategories.
        /// </summary>
        [Fact]
        public async void GetAgentCategories()
        {
            AgentCategoryQueryParameter agentCategoryQueryParameters = new AgentCategoryQueryParameter();

            mockAgentCategoryService.Setup(mr => mr.GetAgentCategories(It.IsAny<AgentCategoryQueryParameter>())).ReturnsAsync((AgentCategoryQueryParameter agentCategory) =>
              mockAgentCategoryData.GetAgentCategories(agentCategoryQueryParameters));

            var value = await controller.GetAgentCategories(agentCategoryQueryParameters);

            Assert.Equal((int)HttpStatusCode.OK, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region GetAgentCategory

        [Theory]
        [InlineData(100)]
        public async void GetAgentCategory_ReturnsNotFoundResult(int agentCategoryId)
        {
            mockAgentCategoryService.Setup(mr => mr.GetAgentCategory(It.IsAny<AgentCategoryIdDetails>())).ReturnsAsync((AgentCategoryIdDetails agentCategory) =>
                mockAgentCategoryData.GetAgentCategory(new AgentCategoryIdDetails { AgentCategoryId = agentCategoryId }));

            var value = await controller.GetAgentCategory(agentCategoryId);

            Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetAgentCategory_ReturnsOKResult(int agentCategoryId)
        {
            mockAgentCategoryService.Setup(mr => mr.GetAgentCategory(It.IsAny<AgentCategoryIdDetails>())).ReturnsAsync((AgentCategoryIdDetails agentCategory) =>
                mockAgentCategoryData.GetAgentCategory(new AgentCategoryIdDetails { AgentCategoryId = agentCategoryId }));

            var value = await controller.GetAgentCategory(agentCategoryId);

            Assert.Equal((int)HttpStatusCode.OK, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region CreateAgentCategory

        [Fact]
        public async void CreateAgentCategory_ReturnsConflictResult()
        {
            CreateAgentCategory agentCategoryDetails = new CreateAgentCategory()
            {
                Name = "AgentCategory2",
                CreatedBy = "admin",
            };

            mockAgentCategoryService.Setup(mr => mr.CreateAgentCategory(It.IsAny<CreateAgentCategory>())).ReturnsAsync((CreateAgentCategory agentCategory) =>
                mockAgentCategoryData.CreateAgentCategory(agentCategoryDetails));

            var value = await controller.CreateAgentCategory(agentCategoryDetails);

            Assert.Equal((int)HttpStatusCode.Conflict, (value as ObjectResult).StatusCode);
        }

        [Fact]
        public async void CreateAgentCategory_ReturnsOkResult()
        {
            CreateAgentCategory agentCategoryDetails = new CreateAgentCategory()
            {
                Name = "AgentCategory10",
                CreatedBy = "admin",
            };
            mockAgentCategoryService.Setup(mr => mr.CreateAgentCategory(It.IsAny<CreateAgentCategory>())).ReturnsAsync((CreateAgentCategory agentCategory) =>
                mockAgentCategoryData.CreateAgentCategory(agentCategoryDetails));

            var value = await controller.CreateAgentCategory(agentCategoryDetails);

            Assert.Equal((int)HttpStatusCode.Created, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region UpdateAgentCategory

        [Theory]
        [InlineData(100)]
        [InlineData(101)]
        public async void UpdateAgentCategory_ReturnsNotFoundResult(int agentCategoryId)
        {
            UpdateAgentCategory updateAgentCategory = new UpdateAgentCategory()
            {
                Name = "X",
                ModifiedBy = "admin"
            };

            mockAgentCategoryService.Setup(mr => mr.UpdateAgentCategory(It.IsAny<AgentCategoryIdDetails>(), It.IsAny<UpdateAgentCategory>())).ReturnsAsync(
                (AgentCategoryIdDetails agentCategory, UpdateAgentCategory update) =>
                mockAgentCategoryData.UpdateAgentCategory(new AgentCategoryIdDetails { AgentCategoryId = agentCategoryId }, updateAgentCategory));

            var value = await controller.UpdateAgentCategory(agentCategoryId, updateAgentCategory);

            Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void UpdateAgentCategory_ReturnsConflictResult(int agentCategoryId)
        {
            UpdateAgentCategory updateAgentCategory = new UpdateAgentCategory()
            {
                Name = "AgentCategory4",
                ModifiedBy = "admin"
            };

            mockAgentCategoryService.Setup(mr => mr.UpdateAgentCategory(It.IsAny<AgentCategoryIdDetails>(), It.IsAny<UpdateAgentCategory>())).ReturnsAsync(
                (AgentCategoryIdDetails agentCategory, UpdateAgentCategory update) =>
                mockAgentCategoryData.UpdateAgentCategory(new AgentCategoryIdDetails { AgentCategoryId = agentCategoryId }, updateAgentCategory));

            var value = await controller.UpdateAgentCategory(agentCategoryId, updateAgentCategory);

            Assert.Equal((int)HttpStatusCode.Conflict, (value as ObjectResult).StatusCode);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void UpdateAgentCategory_ReturnsNoContentResult(int agentCategoryId)
        {
            UpdateAgentCategory updateAgentCategory = new UpdateAgentCategory()
            {
                Name = "AgentCategory10",
                ModifiedBy = "admin"
            };

            mockAgentCategoryService.Setup(mr => mr.UpdateAgentCategory(It.IsAny<AgentCategoryIdDetails>(), It.IsAny<UpdateAgentCategory>())).ReturnsAsync(
                (AgentCategoryIdDetails agentCategory, UpdateAgentCategory update) =>
                mockAgentCategoryData.UpdateAgentCategory(new AgentCategoryIdDetails { AgentCategoryId = agentCategoryId }, updateAgentCategory));

            var value = await controller.UpdateAgentCategory(agentCategoryId, updateAgentCategory);

            Assert.Equal((int)HttpStatusCode.NoContent, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region DeleteAgentCategory

        [Theory]
        [InlineData(100)]
        [InlineData(101)]
        public async void DeleteAgentCategory_ReturnsNotFoundResult(int agentCategoryId)
        {
            mockAgentCategoryService.Setup(mr => mr.DeleteAgentCategory(It.IsAny<AgentCategoryIdDetails>())).ReturnsAsync(
                (AgentCategoryIdDetails agentCategory) => mockAgentCategoryData.DeleteAgentCategory(new AgentCategoryIdDetails { AgentCategoryId = agentCategoryId }));

            var value = await controller.DeleteAgentCategory(agentCategoryId);

            Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        }

        [Theory]
        [InlineData(4)]
        [InlineData(5)]
        public async void DeleteAgentCategory_ReturnsNoContentResult(int agentCategoryId)
        {
            mockAgentCategoryService.Setup(mr => mr.DeleteAgentCategory(It.IsAny<AgentCategoryIdDetails>())).ReturnsAsync(
                (AgentCategoryIdDetails agentCategory) => mockAgentCategoryData.DeleteAgentCategory(new AgentCategoryIdDetails { AgentCategoryId = agentCategoryId }));

            var value = await controller.DeleteAgentCategory(agentCategoryId);

            Assert.Equal((int)HttpStatusCode.NoContent, (value as ObjectResult).StatusCode);
        }

        #endregion
    }
}
