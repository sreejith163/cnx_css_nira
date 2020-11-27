using AutoMapper;
using Css.Api.Core.Models.Domain;
using Css.Api.Setup.Business.Interfaces;
using Css.Api.Setup.Business.UnitTest.Mock;
using Css.Api.Setup.Models.DTO.Request.OperationHour;
using Css.Api.Setup.Models.DTO.Request.SkillTag;
using Css.Api.Setup.Models.DTO.Response.SkillTag;
using Css.Api.Setup.Models.Profiles.OperatingHours;
using Css.Api.Setup.Models.Profiles.SkillTag;
using Css.Api.Setup.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Collections.Generic;
using System.Net;
using Xunit;

namespace Css.Api.Setup.Business.UnitTest.Services
{
    public class SkillTagServiceShould
    {
        /// <summary>
        /// The skil tag service
        /// </summary>
        private readonly ISkillTagService skillTagService;

        /// <summary>
        /// The repository wrapper
        /// </summary>
        private readonly IRepositoryWrapper repositoryWrapper;

        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="SkillTagServiceShould"/> class.
        /// </summary>
        public SkillTagServiceShould()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new SkillTagProfile());
                cfg.AddProfile(new OperatingHourProfile());
            });

            mapper = new Mapper(mapperConfig);

            var context = new DefaultHttpContext();
            Mock<IHttpContextAccessor> mockHttContext = new Mock<IHttpContextAccessor>();
            mockHttContext.Setup(_ => _.HttpContext).Returns(context);

            var mockSchedulingContext = new MockDataContext().IntializeMockData();

            repositoryWrapper = new MockRepositoryWrapper(mockSchedulingContext, mapper);

            skillTagService = new SkillTagService(repositoryWrapper, mockHttContext.Object, mapper);
        }

        #region GetSkillTags

        /// <summary>
        /// Gets the skill tags.
        /// </summary>
        [Fact]
        public async void GetSkillTags()
        {
            SkillTagQueryParameter queryParameters = new SkillTagQueryParameter();
            var result = await skillTagService.GetSkillTags(queryParameters);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<PagedList<Entity>>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region GetSkillTag

        /// <summary>
        /// Gets the skill tag with not found.
        /// </summary>
        /// <param name="skillTagId">The skill tag identifier.</param>
        [Theory]
        [InlineData(100)]
        public async void GetSkillTagWithNotFound(int skillTagId)
        {
            var result = await skillTagService.GetSkillTag(new SkillTagIdDetails { SkillTagId = skillTagId });

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
        }

        /// <summary>
        /// Gets the skill tag.
        /// </summary>
        /// <param name="skillTagId">The skill tag identifier.</param>
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetSkillTag(int skillTagId)
        {
            var result = await skillTagService.GetSkillTag(new SkillTagIdDetails { SkillTagId = skillTagId });

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<SkillTagDetailsDTO>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region CreateSkillTag

        /// <summary>
        /// Creates the skill tag with skill group group not found.
        /// </summary>
        /// <param name="skillGroupId">The skill group identifier.</param>
        [Theory]
        [InlineData(100)]
        public async void CreateSkillTagWithSkillGroupGroupNotFound(int skillGroupId)
        {
            CreateSkillTag skillTag = new CreateSkillTag()
            {
                Name = "skillGroup",
                SkillGroupId = skillGroupId,
                OperationHour = new List<OperationHourAttribute>(),
                CreatedBy = "admin"
            };
            var result = await skillTagService.CreateSkillTag(skillTag);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
        }

        /// <summary>
        /// Creates the skill tag with conflict found.
        /// </summary>
        [Fact]
        public async void CreateSkillTagWithConflictFound()
        {
            CreateSkillTag skillTag = new CreateSkillTag()
            {
                RefId = 1,
                Name = "skillTag1",
                SkillGroupId = 1,
                OperationHour = new List<OperationHourAttribute>(),
                CreatedBy = "admin"
            };

            var result = await skillTagService.CreateSkillTag(skillTag);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.Conflict, result.Code);
        }

        /// <summary>
        /// Creates the skill tag.
        /// </summary>
        [Fact]
        public async void CreateSkillTag()
        {
            CreateSkillTag skillTag = new CreateSkillTag()
            {
                RefId = 1,
                Name = "skillTag",
                SkillGroupId = 1,
                OperationHour = new List<OperationHourAttribute>(),
                CreatedBy = "admin"
            };
            var result = await skillTagService.CreateSkillTag(skillTag);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<SkillTagIdDetails>(result.Value);
            Assert.Equal(HttpStatusCode.Created, result.Code);
        }

        #endregion

        #region UpdateSkillTag

        /// <summary>
        /// Updates the skill tag with not found.
        /// </summary>
        /// <param name="skillTagId">The skill tag identifier.</param>
        [Theory]
        [InlineData(100)]
        public async void UpdateSkillTagWithNotFound(int skillTagId)
        {
            UpdateSkillTag skillTag = new UpdateSkillTag()
            {
                Name = "skillTag",
                SkillGroupId = 1,              
                OperationHour = new List<OperationHourAttribute>(),
                ModifiedBy = "admin"
            };
            var result = await skillTagService.UpdateSkillTag(new SkillTagIdDetails { SkillTagId = skillTagId }, skillTag);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
        }

        /// <summary>
        /// Updates the skill tag with skill group not found.
        /// </summary>
        /// <param name="skillTagId">The skill tag identifier.</param>
        /// <param name="skillGroupId">The skill group identifier.</param>
        [Theory]
        [InlineData(1, 100)]
        public async void UpdateSkillTagWithSkillGroupNotFound(int skillTagId, int skillGroupId)
        {
            UpdateSkillTag skillTag = new UpdateSkillTag()
            {
                Name = "skillTag",
                SkillGroupId = skillGroupId,
                OperationHour = new List<OperationHourAttribute>(),
                ModifiedBy = "admin"
            };
            var result = await skillTagService.UpdateSkillTag(new SkillTagIdDetails { SkillTagId = skillTagId }, skillTag);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
        }

        /// <summary>
        /// Updates the skill tag with conflict.
        /// </summary>
        /// <param name="skillTagId">The skill tag identifier.</param>
        [Theory]
        [InlineData(1)]
        public async void UpdateSkillTagWithConflict(int skillTagId)
        {
            UpdateSkillTag skillTag = new UpdateSkillTag()
            {
                Name = "skillTag3",
                SkillGroupId = 1,
                OperationHour = new List<OperationHourAttribute>(),
                ModifiedBy = "admin"
            };
            var result = await skillTagService.UpdateSkillTag(new SkillTagIdDetails { SkillTagId = skillTagId }, skillTag);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.Conflict, result.Code);
        }

        /// <summary>
        /// Updates the skill tag.
        /// </summary>
        /// <param name="skillTagId">The skill tag identifier.</param>
        [Theory]
        [InlineData(1)]
        public async void UpdateSkillTag(int skillTagId)
        {
            UpdateSkillTag skillTag = new UpdateSkillTag()
            {
                Name = "skillTag",
                SkillGroupId = 1,
                OperationHour = new List<OperationHourAttribute>(),
                ModifiedBy = "admin"
            };
            var result = await skillTagService.UpdateSkillTag(new SkillTagIdDetails { SkillTagId = skillTagId }, skillTag);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NoContent, result.Code);
        }

        #endregion

        #region DeleteSkillTag

        /// <summary>
        /// Deletes the skill g tag with dependeny failed.
        /// </summary>
        /// <param name="skillTagId">The skill tag identifier.</param>
        [Theory]
        [InlineData(1)]
        public async void DeleteSkillGTagWithDependenyFailed(int skillTagId)
        {
            var result = await skillTagService.DeleteSkillTag(new SkillTagIdDetails { SkillTagId = skillTagId });

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.FailedDependency, result.Code);
        }

        /// <summary>
        /// Deletes the skill tag with not found.
        /// </summary>
        /// <param name="skillTagId">The skill tag identifier.</param>
        [Theory]
        [InlineData(100)]
        public async void DeleteSkillTagWithNotFound(int skillTagId)
        {
            var result = await skillTagService.DeleteSkillTag(new SkillTagIdDetails { SkillTagId = skillTagId });

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
        }

        /// <summary>
        /// Deletes the skill tag.
        /// </summary>
        /// <param name="skillTagId">The skill tag identifier.</param>
        [Theory]
        [InlineData(2)]
        public async void DeleteSkillTag(int skillTagId)
        {
            var result = await skillTagService.DeleteSkillTag(new SkillTagIdDetails { SkillTagId = skillTagId });

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NoContent, result.Code);
        }

        #endregion
    }
}


