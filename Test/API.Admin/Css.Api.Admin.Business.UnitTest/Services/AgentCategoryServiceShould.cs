using AutoMapper;
using Css.Api.Admin.Business.Interfaces;
using Css.Api.Admin.Business.UnitTest.Mock;
using Css.Api.Admin.Models.DTO.Request.AgentCategory;
using Css.Api.Admin.Models.DTO.Response.AgentCategory;
using Css.Api.Admin.Models.Profiles.AgentCategory;
using Css.Api.Admin.Repository;
using Css.Api.Admin.Repository.Interfaces;
using Css.Api.Core.EventBus.Services;
using Css.Api.Core.Models.Domain;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Net;
using Xunit;

namespace Css.Api.Admin.Business.UnitTest.Services
{
    public class AgentCategoryServiceShould
    {
        /// <summary>
        /// The agent category service
        /// </summary>
        private readonly IAgentCategoryService agentCategoryService;

        /// <summary>
        /// The repository wrapper
        /// </summary>
        private readonly IRepositoryWrapper repositoryWrapper;

        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentCategoryServiceShould"/> class.
        /// </summary>
        public AgentCategoryServiceShould()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AgentCategoryProfile());
            });

            mapper = new Mapper(mapperConfig);
            var busService = new Mock<IBusService>();

            var context = new DefaultHttpContext();
            Mock<IHttpContextAccessor> mockHttContext = new Mock<IHttpContextAccessor>();
            mockHttContext.Setup(_ => _.HttpContext).Returns(context);

            var mockAdminContext = new MockDataContext().IntializeMockData();

            repositoryWrapper = new MockRepositoryWrapper(mockAdminContext, mapper);

            agentCategoryService = new AgentCategoryService(repositoryWrapper, mockHttContext.Object, mapper, busService.Object);
        }

        #region GetAgentCategories

        /// <summary>
        /// Gets the agent Categories.
        /// </summary>
        [Fact]
        public async void GetAgentCategories()
        {
            AgentCategoryQueryParameter queryParameters = new AgentCategoryQueryParameter();
            var result = await agentCategoryService.GetAgentCategories(queryParameters);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<PagedList<Entity>>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region GetAgentCategory

        /// <summary>
        /// Gets the agent Category with no found.
        /// </summary>
        /// <param name="agentCategoryId">The agentCategory identifier.</param>
        [Theory]
        [InlineData(100)]
        public async void GetAgentCategoryWithNotFound(int agentCategoryId)
        {
            var result = await agentCategoryService.GetAgentCategory(new AgentCategoryIdDetails { AgentCategoryId = agentCategoryId });

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
        }

        /// <summary>
        /// Gets the agent Category.
        /// </summary>
        /// <param name="agentCategoryId">The agentCategory identifier.</param>
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetAgentCategory(int agentCategoryId)
        {
            var result = await agentCategoryService.GetAgentCategory(new AgentCategoryIdDetails { AgentCategoryId = agentCategoryId });

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<AgentCategoryDTO>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region CreateAgentCategory

        /// <summary>
        /// Gets the agent Category with no found.
        /// </summary>
        /// <param name="agentCategoryId">The agentCategory identifier.</param>
        [Fact]
        public async void CreateAgentCategoryWithConflictFound()
        {
            CreateAgentCategory agentCategoryDetails = new CreateAgentCategory()
            {
                CreatedBy = "admin",
                Name = "AgentCategory2"
            };
            var result = await agentCategoryService.CreateAgentCategory(agentCategoryDetails);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.Conflict, result.Code);
        }

        /// <summary>
        /// Creates the agent Category.
        /// </summary>
        [Fact]
        public async void CreateAgentCategory()
        {
            CreateAgentCategory agentCategoryDetails = new CreateAgentCategory()
            {
                CreatedBy = "admin",
                Name = "AgentCategory10"
            };
            var result = await agentCategoryService.CreateAgentCategory(agentCategoryDetails);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<AgentCategoryIdDetails>(result.Value);
            Assert.Equal(HttpStatusCode.Created, result.Code);
        }

        #endregion

        #region UpdateAgentCategory

        /// <summary>
        /// Updates the agent Category with not found.
        /// </summary>
        /// <param name="agentCategoryId">The agentCategory identifier.</param>
        [Theory]
        [InlineData(100)]
        public async void UpdateAgentCategoryWithNotFound(int agentCategoryId)
        {
            UpdateAgentCategory updateAgentCategory = new UpdateAgentCategory()
            {
                Name = "X",
                ModifiedBy = "admin1"
            };
            var result = await agentCategoryService.UpdateAgentCategory(new AgentCategoryIdDetails { AgentCategoryId = agentCategoryId }, updateAgentCategory);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
        }

        /// <summary>
        /// Updates the agent Category with not found.
        /// </summary>
        /// <param name="agentCategoryId">The agentCategory identifier.</param>
        [Theory]
        [InlineData(1)]
        public async void UpdateAgentCategoryWithConflict(int agentCategoryId)
        {
            UpdateAgentCategory updateAgentCategory = new UpdateAgentCategory()
            {
                Name = "AgentCategory2",
                ModifiedBy = "admin1"
            };
            var result = await agentCategoryService.UpdateAgentCategory(new AgentCategoryIdDetails { AgentCategoryId = agentCategoryId }, updateAgentCategory);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.Conflict, result.Code);
        }

        /// <summary>
        /// Updates the agent Category.
        /// </summary>
        /// <param name="agentCategoryId">The agentCategory identifier.</param>
        [Theory]
        [InlineData(1)]
        public async void UpdateAgentCategory(int agentCategoryId)
        {
            UpdateAgentCategory updateAgentCategory = new UpdateAgentCategory()
            {
                Name = "AgentCategory10",
                ModifiedBy = "admin1"
            };
            var result = await agentCategoryService.UpdateAgentCategory(new AgentCategoryIdDetails { AgentCategoryId = agentCategoryId }, updateAgentCategory);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NoContent, result.Code);
        }

        #endregion

        #region DeleteAgentCategory

        /// <summary>
        /// Deletes the agent Category with not found.
        /// </summary>
        /// <param name="agentCategoryId">The agentCategory identifier.</param>
        [Theory]
        [InlineData(100)]
        public async void DeleteAgentCategoryWithNotFound(int agentCategoryId)
        {
            var result = await agentCategoryService.DeleteAgentCategory(new AgentCategoryIdDetails { AgentCategoryId = agentCategoryId });

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
        }

        /// <summary>
        /// Deletes the agent Category.
        /// </summary>
        /// <param name="agentCategoryId">The agentCategory identifier.</param>
        [Theory]
        [InlineData(4)]
        [InlineData(5)]
        public async void DeleteAgentCategory(int agentCategoryId)
        {
            var result = await agentCategoryService.DeleteAgentCategory(new AgentCategoryIdDetails { AgentCategoryId = agentCategoryId });

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NoContent, result.Code);
        }

        #endregion
    }
}