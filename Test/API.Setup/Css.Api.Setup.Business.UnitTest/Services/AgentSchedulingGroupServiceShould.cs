using AutoMapper;
using Css.Api.Core.Models.Domain;
using Css.Api.Setup.Business.Interfaces;
using Css.Api.Setup.Business.UnitTest.Mock;
using Css.Api.Setup.Models.DTO.Request.OperationHour;
using Css.Api.Setup.Models.DTO.Request.AgentSchedulingGroup;
using Css.Api.Setup.Models.DTO.Response.AgentSchedulingGroup;
using Css.Api.Setup.Models.Profiles.AgentSchedulingGroup;
using Css.Api.Setup.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Collections.Generic;
using System.Net;
using Xunit;

namespace Css.Api.Setup.Business.UnitTest.Services
{
    public class AgentSchedulingGroupServiceShould
    {
        /// <summary>
        /// The agent scheduling group service
        /// </summary>
        private readonly IAgentSchedulingGroupService agentSchedulingGroupService;

        /// <summary>
        /// The repository wrapper
        /// </summary>
        private readonly IRepositoryWrapper repositoryWrapper;

        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentSchedulingGroupServiceShould"/> class.
        /// </summary>
        public AgentSchedulingGroupServiceShould()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AgentSchedulingGroupProfile());
            });

            mapper = new Mapper(mapperConfig);

            var context = new DefaultHttpContext();
            Mock<IHttpContextAccessor> mockHttContext = new Mock<IHttpContextAccessor>();
            mockHttContext.Setup(_ => _.HttpContext).Returns(context);

            var mockSchedulingContext = new MockDataContext().IntializeMockData();

            repositoryWrapper = new MockRepositoryWrapper(mockSchedulingContext, mapper);

            agentSchedulingGroupService = new AgentSchedulingGroupService(repositoryWrapper, mockHttContext.Object, mapper);
        }

        #region GetAgentSchedulingGroups

        /// <summary>
        /// Gets the agent scheduling groups.
        /// </summary>
        [Fact]
        public async void GetAgentSchedulingGroups()
        {
            AgentSchedulingGroupQueryParameter queryParameters = new AgentSchedulingGroupQueryParameter();
            var result = await agentSchedulingGroupService.GetAgentSchedulingGroups(queryParameters);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<PagedList<Entity>>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region GetAgentSchedulingGroup

        /// <summary>
        /// Gets the agent scheduling group with not found.
        /// </summary>
        /// <param name="agentSchedulingGroupId">The agent scheduling group identifier.</param>
        [Theory]
        [InlineData(100)]
        public async void GetAgentSchedulingGroupWithNotFound(int agentSchedulingGroupId)
        {
            var result = await agentSchedulingGroupService.GetAgentSchedulingGroup(new AgentSchedulingGroupIdDetails { AgentSchedulingGroupId = agentSchedulingGroupId });

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
        }

        /// <summary>
        /// Gets the agent scheduling group.
        /// </summary>
        /// <param name="agentSchedulingGroupId">The agent scheduling group identifier.</param>
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetAgentSchedulingGroup(int agentSchedulingGroupId)
        {
            var result = await agentSchedulingGroupService.GetAgentSchedulingGroup(new AgentSchedulingGroupIdDetails { AgentSchedulingGroupId = agentSchedulingGroupId });

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<AgentSchedulingGroupDetailsDTO>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region CreateAgentSchedulingGroup

        /// <summary>
        /// Creates the agent scheduling group with skill tag found.
        /// </summary>
        /// <param name="skillTagId">The skill tag identifier.</param>
        [Theory]
        [InlineData(100)]
        public async void CreateAgentSchedulingGroupWithSkillTagFound(int skillTagId)
        {
            CreateAgentSchedulingGroup agentSchedulingGroup = new CreateAgentSchedulingGroup()
            {
                RefId = 1,
                Name = "agentSchedulingGroup1",
                SkillTagId = skillTagId,
                FirstDayOfWeek = 1,
                TimezoneId = 1,
                OperationHour = new List<OperationHourAttribute>(),
                CreatedBy = "admin"
            };

            var result = await agentSchedulingGroupService.CreateAgentSchedulingGroup(agentSchedulingGroup);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
        }

        /// <summary>
        /// Creates the agent scheduling group with conflict found.
        /// </summary>
        [Fact]
        public async void CreateAgentSchedulingGroupWithConflictFound()
        {
            CreateAgentSchedulingGroup agentSchedulingGroup = new CreateAgentSchedulingGroup()
            {
                RefId = 1,
                Name = "agentSchedulingGroup1",
                SkillTagId = 1,
                FirstDayOfWeek = 1,
                TimezoneId = 1,
                OperationHour = new List<OperationHourAttribute>(),
                CreatedBy = "admin"
            };

            var result = await agentSchedulingGroupService.CreateAgentSchedulingGroup(agentSchedulingGroup);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.Conflict, result.Code);
        }

        /// <summary>
        /// Creates the agent scheduling group.
        /// </summary>
        /// <param name="agentSchedulingGroup">The agent scheduling group.</param>
        [Fact]
        public async void CreateAgentSchedulingGroup()
        {
            CreateAgentSchedulingGroup agentSchedulingGroup = new CreateAgentSchedulingGroup()
            {
                RefId = 1,
                Name = "agentSchedulingGroup",
                SkillTagId = 1,
                FirstDayOfWeek = 1,
                TimezoneId = 1,
                OperationHour = new List<OperationHourAttribute>(),
                CreatedBy = "admin"
            };
            var result = await agentSchedulingGroupService.CreateAgentSchedulingGroup(agentSchedulingGroup);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<AgentSchedulingGroupIdDetails>(result.Value);
            Assert.Equal(HttpStatusCode.Created, result.Code);
        }

        #endregion

        #region UpdateAgentSchedulingGroup

        /// <summary>
        /// Updates the agent scheduling group with not found.
        /// </summary>
        /// <param name="agentSchedulingGroupId">The agent scheduling group identifier.</param>
        /// <param name="agentSchedulingGroup">The agent scheduling group.</param>
        [Theory]
        [InlineData(100)]
        public async void UpdateAgentSchedulingGroupWithNotFound(int agentSchedulingGroupId)
        {
            UpdateAgentSchedulingGroup agentSchedulingGroup = new UpdateAgentSchedulingGroup()
            {
                Name = "agentSchedulingGroup",
                SkillTagId = 1,
                FirstDayOfWeek = 1,
                TimezoneId = 1,
                OperationHour = new List<OperationHourAttribute>(),
                ModifiedBy = "admin"
            };
            var result = await agentSchedulingGroupService.UpdateAgentSchedulingGroup(new AgentSchedulingGroupIdDetails { AgentSchedulingGroupId = agentSchedulingGroupId }, agentSchedulingGroup);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
        }

        /// <summary>
        /// Updates the agent scheduling group with skill tag not found.
        /// </summary>
        /// <param name="agentSchedulingGroupId">The agent scheduling group identifier.</param>
        /// <param name="skilltagId">The skilltag identifier.</param>
        [Theory]
        [InlineData(1, 100)]
        public async void UpdateAgentSchedulingGroupWithSkillTagNotFound(int agentSchedulingGroupId, int skilltagId)
        {
            UpdateAgentSchedulingGroup agentSchedulingGroup = new UpdateAgentSchedulingGroup()
            {
                Name = "agentSchedulingGroup",
                SkillTagId = skilltagId,
                FirstDayOfWeek = 1,
                TimezoneId = 1,
                OperationHour = new List<OperationHourAttribute>(),
                ModifiedBy = "admin"
            };
            var result = await agentSchedulingGroupService.UpdateAgentSchedulingGroup(new AgentSchedulingGroupIdDetails { AgentSchedulingGroupId = agentSchedulingGroupId }, agentSchedulingGroup);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
        }

        /// <summary>
        /// Updates the agent scheduling group with conflict.
        /// </summary>
        /// <param name="agentSchedulingGroupId">The agent scheduling group identifier.</param>
        [Theory]
        [InlineData(1)]
        public async void UpdateAgentSchedulingGroupWithConflict(int agentSchedulingGroupId)
        {
            UpdateAgentSchedulingGroup agentSchedulingGroup = new UpdateAgentSchedulingGroup()
            {
                Name = "agentSchedulingGroup3",
                SkillTagId = 1,
                FirstDayOfWeek = 1,
                TimezoneId = 1,
                OperationHour = new List<OperationHourAttribute>(),
                ModifiedBy = "admin"
            };
            var result = await agentSchedulingGroupService.UpdateAgentSchedulingGroup(new AgentSchedulingGroupIdDetails { AgentSchedulingGroupId = agentSchedulingGroupId }, agentSchedulingGroup);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.Conflict, result.Code);
        }

        /// <summary>
        /// Creates the agent scheduling group.
        /// </summary>
        /// <param name="agentSchedulingGroup">The agent scheduling group.</param>
        [Theory]
        [InlineData(1)]
        public async void UpdateAgentSchedulingGroup(int agentSchedulingGroupId)
        {
            UpdateAgentSchedulingGroup agentSchedulingGroup = new UpdateAgentSchedulingGroup()
            {
                Name = "agentSchedulingGroup",
                SkillTagId = 1,
                FirstDayOfWeek = 1,
                TimezoneId = 1,
                OperationHour = new List<OperationHourAttribute>(),
                ModifiedBy = "admin"
            };
            var result = await agentSchedulingGroupService.UpdateAgentSchedulingGroup(new AgentSchedulingGroupIdDetails { AgentSchedulingGroupId = agentSchedulingGroupId }, agentSchedulingGroup);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NoContent, result.Code);
        }

        #endregion

        #region DeleteAgentSchedulingGroup
       
        /// <summary>
        /// Deletes the agent scheduling group.
        /// </summary>
        /// <param name="agentSchedulingGroupId">The agent scheduling group identifier.</param>
        [Theory]
        [InlineData(2)]
        public async void DeleteAgentSchedulingGroup(int agentSchedulingGroupId)
        {
            var result = await agentSchedulingGroupService.DeleteAgentSchedulingGroup(new AgentSchedulingGroupIdDetails { AgentSchedulingGroupId = agentSchedulingGroupId });

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NoContent, result.Code);
        }

        /// <summary>
        /// Deletes the agent scheduling group.
        /// </summary>
        /// <param name="agentSchedulingGroupId">The agent scheduling group identifier.</param>
        [Theory]
        [InlineData(100)]
        public async void DeleteAgentSchedulingGroupWithNotFound(int agentSchedulingGroupId)
        {
            var result = await agentSchedulingGroupService.DeleteAgentSchedulingGroup(new AgentSchedulingGroupIdDetails { AgentSchedulingGroupId = agentSchedulingGroupId });

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
        }

        #endregion
    }
}

