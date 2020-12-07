using AutoMapper;
using Css.Api.Core.Models.Domain;
using Css.Api.Setup.Business.Interfaces;
using Css.Api.Setup.Business.UnitTest.Mock;
using Css.Api.Setup.Models.DTO.Request.OperationHour;
using Css.Api.Setup.Models.DTO.Request.SkillGroup;
using Css.Api.Setup.Models.DTO.Response.SkillGroup;
using Css.Api.Setup.Models.Profiles.OperatingHours;
using Css.Api.Setup.Models.Profiles.SkillGroup;
using Css.Api.Setup.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Collections.Generic;
using System.Net;
using Xunit;

namespace Css.Api.Setup.Business.UnitTest.Services
{
    public class SkillGroupServiceShould
    {
        /// <summary>
        /// The skill group service
        /// </summary>
        private readonly ISkillGroupService skillGroupService;

        /// <summary>
        /// The repository wrapper
        /// </summary>
        private readonly IRepositoryWrapper repositoryWrapper;

        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="SkillGroupServiceShould"/> class.
        /// </summary>
        public SkillGroupServiceShould()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new SkillGroupProfile());
                cfg.AddProfile(new OperatingHourProfile());
            });

            mapper = new Mapper(mapperConfig);

            var context = new DefaultHttpContext();
            Mock<IHttpContextAccessor> mockHttContext = new Mock<IHttpContextAccessor>();
            mockHttContext.Setup(_ => _.HttpContext).Returns(context);

            var mockSchedulingContext = new MockDataContext().IntializeMockData();

            repositoryWrapper = new MockRepositoryWrapper(mockSchedulingContext, mapper);

            skillGroupService = new SkillGroupService(repositoryWrapper, mockHttContext.Object, mapper);
        }

        #region GetSkillGroups

        /// <summary>
        /// Gets the skill groups.
        /// </summary>
        [Fact]
        public async void GetSkillGroups()
        {
            SkillGroupQueryParameter queryParameters = new SkillGroupQueryParameter();
            var result = await skillGroupService.GetSkillGroups(queryParameters);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<PagedList<Entity>>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region GetSkillGroup

        /// <summary>
        /// Gets the skill group with not found.
        /// </summary>
        /// <param name="skillGroupId">The skill group identifier.</param>
        [Theory]
        [InlineData(100)]
        public async void GetSkillGroupWithNotFound(int skillGroupId)
        {
            var result = await skillGroupService.GetSkillGroup(new SkillGroupIdDetails { SkillGroupId = skillGroupId });

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
        }

        /// <summary>
        /// Gets the skill group.
        /// </summary>
        /// <param name="skillGroupId">The skill group identifier.</param>
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetSkillGroup(int skillGroupId)
        {
            var result = await skillGroupService.GetSkillGroup(new SkillGroupIdDetails { SkillGroupId = skillGroupId });

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<SkillGroupDetailsDTO>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region CreateSkillGroup

        /// <summary>
        /// Updates the skill group with lob group not found.
        /// </summary>
        /// <param name="skillGroupId">The skill group identifier.</param>
        /// <param name="lobGroupId">The lob group identifier.</param>
        [Theory]
        [InlineData(100)]
        public async void CreateSkillGroupWithLOBGroupNotFound(int lobGroupId)
        {
            CreateSkillGroup skillGroup = new CreateSkillGroup()
            {
                Name = "skillGroup",
                ClientLobGroupId = lobGroupId,
                FirstDayOfWeek = 1,
                TimezoneId = 1,
                OperationHour = new List<OperationHourAttribute>(),
                CreatedBy = "admin"
            };
            var result = await skillGroupService.CreateSkillGroup(skillGroup);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
        }

        /// <summary>
        /// Creates the skill group with conflict found.
        /// </summary>
        [Fact]
        public async void CreateSkillGroupWithConflictFound()
        {
            CreateSkillGroup skillGroup = new CreateSkillGroup()
            {
                RefId = 1,
                Name = "skillGroup1",
                ClientLobGroupId = 1,
                FirstDayOfWeek = 1,
                TimezoneId = 1,
                OperationHour = new List<OperationHourAttribute>(),
                CreatedBy = "admin"
            };

            var result = await skillGroupService.CreateSkillGroup(skillGroup);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.Conflict, result.Code);
        }

        /// <summary>
        /// Creates the skill group.
        /// </summary>
        [Fact]
        public async void CreateSkillGroup()
        {
            CreateSkillGroup skillGroup = new CreateSkillGroup()
            {
                RefId = 1,
                Name = "skillGroup",
                ClientLobGroupId = 1,
                FirstDayOfWeek = 1,
                TimezoneId = 1,
                OperationHour = new List<OperationHourAttribute>(),
                CreatedBy = "admin"
            };
            var result = await skillGroupService.CreateSkillGroup(skillGroup);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<SkillGroupIdDetails>(result.Value);
            Assert.Equal(HttpStatusCode.Created, result.Code);
        }

        #endregion

        #region UpdateSkillGroup

        /// <summary>
        /// Updates the skill group with not found.
        /// </summary>
        /// <param name="skillGroupId">The skill group identifier.</param>
        [Theory]
        [InlineData(100)]
        public async void UpdateSkillGroupWithNotFound(int skillGroupId)
        {
            UpdateSkillGroup skillGroup = new UpdateSkillGroup()
            {
                Name = "skillGroup",
                ClientLobGroupId = 1,
                FirstDayOfWeek = 1,
                TimezoneId = 1,
                OperationHour = new List<OperationHourAttribute>(),
                ModifiedBy = "admin"
            };
            var result = await skillGroupService.UpdateSkillGroup(new SkillGroupIdDetails { SkillGroupId = skillGroupId }, skillGroup);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
        }

        /// <summary>
        /// Updates the skill group with lob group not found.
        /// </summary>
        /// <param name="skillGroupId">The skill group identifier.</param>
        /// <param name="lobGroupId">The lob group identifier.</param>
        [Theory]
        [InlineData(1, 100)]
        public async void UpdateSkillGroupWithLobGroupNotFound(int skillGroupId, int lobGroupId)
        {
            UpdateSkillGroup skillGroup = new UpdateSkillGroup()
            {
                Name = "skillGroup",
                ClientLobGroupId = lobGroupId,
                FirstDayOfWeek = 1,
                TimezoneId = 1,
                OperationHour = new List<OperationHourAttribute>(),
                ModifiedBy = "admin"
            };
            var result = await skillGroupService.UpdateSkillGroup(new SkillGroupIdDetails { SkillGroupId = skillGroupId }, skillGroup);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
        }

        /// <summary>
        /// Updates the skill group with conflict.
        /// </summary>
        /// <param name="skillGroupId">The skill group identifier.</param>
        [Theory]
        [InlineData(1)]
        public async void UpdateSkillGroupWithConflict(int skillGroupId)
        {
            UpdateSkillGroup skillGroup = new UpdateSkillGroup()
            {
                Name = "skillGroup3",
                ClientLobGroupId = 1,
                FirstDayOfWeek = 1,
                TimezoneId = 1,
                OperationHour = new List<OperationHourAttribute>(),
                ModifiedBy = "admin"
            };
            var result = await skillGroupService.UpdateSkillGroup(new SkillGroupIdDetails { SkillGroupId = skillGroupId }, skillGroup);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.Conflict, result.Code);
        }

        /// <summary>
        /// Creates the skill group.
        /// </summary>
        /// <param name="skillGroupId">The skill group identifier.</param>
        [Theory]
        [InlineData(1)]
        public async void UpdateSkillGroup(int skillGroupId)
        {
            UpdateSkillGroup skillGroup = new UpdateSkillGroup()
            {
                Name = "skillGroup",
                ClientLobGroupId = 1,
                FirstDayOfWeek = 1,
                TimezoneId = 1,
                OperationHour = new List<OperationHourAttribute>(),
                ModifiedBy = "admin"
            };
            var result = await skillGroupService.UpdateSkillGroup(new SkillGroupIdDetails { SkillGroupId = skillGroupId }, skillGroup);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NoContent, result.Code);
        }

        #endregion

        #region DeleteSkillGroup

        /// <summary>
        /// Deletes the skill group with dependeny failed.
        /// </summary>
        /// <param name="skillGroupId">The skill group identifier.</param>
        [Theory]
        [InlineData(1)]
        public async void DeleteSkillGroupWithDependenyFailed(int skillGroupId)
        {
            var result = await skillGroupService.DeleteSkillGroup(new SkillGroupIdDetails { SkillGroupId = skillGroupId });

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.FailedDependency, result.Code);
        }

        /// <summary>
        /// Deletes the skill group.
        /// </summary>
        /// <param name="skillGroupId">The skill group identifier.</param>
        [Theory]
        [InlineData(100)]
        public async void DeleteSkillGroupWithNotFound(int skillGroupId)
        {
            var result = await skillGroupService.DeleteSkillGroup(new SkillGroupIdDetails { SkillGroupId = skillGroupId });

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
        }

        /// <summary>
        /// Deletes the skill group.
        /// </summary>
        /// <param name="skillGroupId">The skill group identifier.</param>
        [Theory]
        [InlineData(2)]
        public async void DeleteSkillGroup(int skillGroupId)
        {
            var result = await skillGroupService.DeleteSkillGroup(new SkillGroupIdDetails { SkillGroupId = skillGroupId });

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NoContent, result.Code);
        }

        #endregion
    }
}

