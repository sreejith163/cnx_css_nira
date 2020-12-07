using Css.Api.Setup.Business.Interfaces;
using Css.Api.Setup.Controllers;
using Css.Api.Setup.Models.DTO.Request.SkillTag;
using Css.Api.Setup.UnitTest.Mock;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using Xunit;

namespace Css.Api.Setup.UnitTest.Controllers
{
    public class SkillTagControllerShould
    {
        /// <summary>
        /// The mock skill tag service
        /// </summary>
        private readonly Mock<ISkillTagService> mockSkillTagService;

        /// <summary>
        /// The controller
        /// </summary>
        SkillTagsController controller;

        /// <summary>
        /// The mock skill tag data
        /// </summary>
        private MockSkillTagData mockSkillTagData;

        /// <summary>
        /// 
        /// </summary>
        public SkillTagControllerShould()
        {
            mockSkillTagService = new Mock<ISkillTagService>();
            mockSkillTagData = new MockSkillTagData();
            controller = new SkillTagsController(mockSkillTagService.Object);
        }

        #region GetSkillTags

        [Fact]
        public async void GetSkillTags()
        {
            SkillTagQueryParameter queryParameters = new SkillTagQueryParameter();

            mockSkillTagService.Setup(mr => mr.GetSkillTags(It.IsAny<SkillTagQueryParameter>())).ReturnsAsync(
                (SkillTagQueryParameter queryParameters) => mockSkillTagData.GetSkillTags(queryParameters));

            var value = await controller.GetSkillTags(queryParameters);

            Assert.Equal((int)HttpStatusCode.OK, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region GetSkillTag

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetSkillTag_ReturnsOKResult(int skillTagId)
        {
            mockSkillTagService.Setup(mr => mr.GetSkillTag(It.IsAny<SkillTagIdDetails>())).ReturnsAsync(
                (SkillTagIdDetails idDetails) => mockSkillTagData.GetSkillTag(new SkillTagIdDetails { SkillTagId = skillTagId }));

            var value = await controller.GetSkillTag(skillTagId);

            Assert.Equal((int)HttpStatusCode.OK, (value as ObjectResult).StatusCode);
        }

        [Theory]
        [InlineData(100)]
        [InlineData(101)]
        public async void GetSkillTag_ReturnsNotFoundResult(int skillTagId)
        {
            mockSkillTagService.Setup(mr => mr.GetSkillTag(It.IsAny<SkillTagIdDetails>())).ReturnsAsync(
                (SkillTagIdDetails idDetails) => mockSkillTagData.GetSkillTag(new SkillTagIdDetails { SkillTagId = skillTagId }));

            var value = await controller.GetSkillTag(skillTagId);

            Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region CreateSkillTag

        [Fact]
        public async void CreateSkillTag_ReturnsConflictResult()
        {
            CreateSkillTag skillTagDetails = new CreateSkillTag()
            {
                RefId = 4,
                CreatedBy = "admin",
                Name = "skillTag1",
                SkillGroupId = 1
            };

            mockSkillTagService.Setup(mr => mr.CreateSkillTag(It.IsAny<CreateSkillTag>())).ReturnsAsync(
                (CreateSkillTag skillTag) => mockSkillTagData.CreateSkillTag(skillTagDetails));

            var value = await controller.CreateSkillTag(skillTagDetails);

            Assert.Equal((int)HttpStatusCode.Conflict, (value as ObjectResult).StatusCode);
        }

        [Fact]
        public async void CreateSkillTag_ReturnsOkResult()
        {
            CreateSkillTag skillTagDetails = new CreateSkillTag()
            {
                RefId = 4,
                CreatedBy = "admin",
                Name = "skillTag",
                SkillGroupId = 1
            };

            mockSkillTagService.Setup(mr => mr.CreateSkillTag(It.IsAny<CreateSkillTag>())).ReturnsAsync(
                (CreateSkillTag skillTag) => mockSkillTagData.CreateSkillTag(skillTagDetails));

            var value = await controller.CreateSkillTag(skillTagDetails);

            Assert.Equal((int)HttpStatusCode.Created, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region UpdateSkillTag

        [Theory]
        [InlineData(100)]
        [InlineData(101)]
        public async void UpdateSkillTag_ReturnsNotFoundResult(int skillTagId)
        {
            UpdateSkillTag skillTag = new UpdateSkillTag()
            {
                Name = "skillTag",
                SkillGroupId = 1,                
                ModifiedBy = "admin"              
            };

            mockSkillTagService.Setup(mr => mr.UpdateSkillTag(It.IsAny<SkillTagIdDetails>(), It.IsAny<UpdateSkillTag>())).ReturnsAsync(
                (SkillTagIdDetails idDetails, UpdateSkillTag update) =>
                mockSkillTagData.UpdateSkillTag(new SkillTagIdDetails { SkillTagId = skillTagId }, skillTag));

            var value = await controller.UpdateSkillTag(skillTagId, skillTag);

            Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        }

        [Theory]
        [InlineData(1, "skillTag2")]
        [InlineData(2, "skillTag3")]
        public async void UpdateSkillTag_ReturnsConflictResult(int skillTagId, string name)
        {
            UpdateSkillTag skillTag = new UpdateSkillTag()
            {
                Name = name,
                SkillGroupId = 1,
                ModifiedBy = "admin"
            };

            mockSkillTagService.Setup(mr => mr.UpdateSkillTag(It.IsAny<SkillTagIdDetails>(), It.IsAny<UpdateSkillTag>())).ReturnsAsync(
                (SkillTagIdDetails idDetails, UpdateSkillTag update) =>
                mockSkillTagData.UpdateSkillTag(new SkillTagIdDetails { SkillTagId = skillTagId }, skillTag));

            var value = await controller.UpdateSkillTag(skillTagId, skillTag);

            Assert.Equal((int)HttpStatusCode.Conflict, (value as ObjectResult).StatusCode);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void UpdateSkillTag_ReturnsNoContentResult(int skillTagId)
        {
            UpdateSkillTag skillTag = new UpdateSkillTag()
            {
                Name = "skillTag",
                SkillGroupId = 1,
                ModifiedBy = "admin"
            };

            mockSkillTagService.Setup(mr => mr.UpdateSkillTag(It.IsAny<SkillTagIdDetails>(), It.IsAny<UpdateSkillTag>())).ReturnsAsync(
                (SkillTagIdDetails idDetails, UpdateSkillTag update) =>
                mockSkillTagData.UpdateSkillTag(new SkillTagIdDetails { SkillTagId = skillTagId }, skillTag));

            var value = await controller.UpdateSkillTag(skillTagId, skillTag);

            Assert.Equal((int)HttpStatusCode.NoContent, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region DeleteClient

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void DeleteSkillTag_ReturnsNoContentResult(int skillTagId)
        {
            mockSkillTagService.Setup(mr => mr.DeleteSkillTag(It.IsAny<SkillTagIdDetails>())).ReturnsAsync(
                (SkillTagIdDetails idDetails) => mockSkillTagData.DeleteSkillTags(new SkillTagIdDetails { SkillTagId = skillTagId }));

            var value = await controller.DeleteSkillTag(skillTagId);

            Assert.Equal((int)HttpStatusCode.NoContent, (value as ObjectResult).StatusCode);
        }

        [Theory]
        [InlineData(100)]
        [InlineData(101)]
        public async void DeleteSkillTag_ReturnsNotFoundResult(int skillTagId)
        {
            mockSkillTagService.Setup(mr => mr.DeleteSkillTag(It.IsAny<SkillTagIdDetails>())).ReturnsAsync(
                (SkillTagIdDetails idDetails) => mockSkillTagData.DeleteSkillTags(new SkillTagIdDetails { SkillTagId = skillTagId }));

            var value = await controller.DeleteSkillTag(skillTagId);

            Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        }

        #endregion
    }
}
