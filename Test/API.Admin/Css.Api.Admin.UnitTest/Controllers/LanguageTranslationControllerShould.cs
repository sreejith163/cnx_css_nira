using Css.Api.Admin.Business.Interfaces;
using Css.Api.Admin.Controllers;
using Css.Api.Admin.Models.DTO.Request.Language;
using Css.Api.Admin.Models.DTO.Request.LanguageTranslation;
using Css.Api.Admin.Models.DTO.Request.Menu;
using Css.Api.Admin.UnitTest.Mock;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using Xunit;

namespace Css.Api.Admin.UnitTest.Controllers
{
    public class LanguageTranslationControllerShould
    {
        /// <summary>
        /// The mock translation service
        /// </summary>
        private readonly Mock<ILanguageTranslationService> mockTranslationService;

        /// <summary>
        /// The controller
        /// </summary>
        private readonly LanguageTranslationsController controller;

        /// <summary>
        /// The mock translation data
        /// </summary>
        private readonly MockLanguageTranslationData mockTranslationData;

        /// <summary>
        /// 
        /// </summary>
        public LanguageTranslationControllerShould()
        {
            mockTranslationService = new Mock<ILanguageTranslationService>();
            mockTranslationData = new MockLanguageTranslationData();
            controller = new LanguageTranslationsController(mockTranslationService.Object);
        }

        #region GetLanguageTranslations

        [Fact]
        public async void GetLanguageTranslations()
        {
            LanguageTranslationQueryParameters queryParameters = new LanguageTranslationQueryParameters();

            mockTranslationService.Setup(mr => mr.GetLanguageTranslations(It.IsAny<LanguageTranslationQueryParameters>())).ReturnsAsync(
                (LanguageTranslationQueryParameters queryParameters) => mockTranslationData.GetLanguageTranslations(queryParameters));

            var value = await controller.GetLanguageTranslations(queryParameters);

            Assert.Equal((int)HttpStatusCode.OK, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region GetLanguageTranslation

        [Theory]
        [InlineData(100)]
        [InlineData(101)]
        public async void GetLanguageTranslation_ReturnsNotFoundResult(int translationId)
        {
            mockTranslationService.Setup(mr => mr.GetLanguageTranslation(It.IsAny<LanguageTranslationIdDetails>())).ReturnsAsync(
                (LanguageTranslationIdDetails idDetails) => mockTranslationData.GetLanguageTranslation(new LanguageTranslationIdDetails { TranslationId = translationId }));

            var value = await controller.GetLanguageTranslation(translationId);

            Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetLanguageTranslation_ReturnsOKResult(int translationId)
        {
            mockTranslationService.Setup(mr => mr.GetLanguageTranslation(It.IsAny<LanguageTranslationIdDetails>())).ReturnsAsync(
                (LanguageTranslationIdDetails idDetails) => mockTranslationData.GetLanguageTranslation(new LanguageTranslationIdDetails { TranslationId = translationId }));

            var value = await controller.GetLanguageTranslation(translationId);

            Assert.Equal((int)HttpStatusCode.OK, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region GetLanguageTranslationsByMenuAndLanguage

        [Theory]
        [InlineData(100, 1)]
        [InlineData(101, 2)]
        public async void GetLanguageTranslationsByMenuAndLanguage_ReturnsNotFoundResultForLanguage(int languageId, int menuId)
        {
            mockTranslationService.Setup(mr => mr.GetLanguageTranslationsByMenuAndLanguage(It.IsAny<LanguageIdDetails>(), It.IsAny<MenuIdDetails>())).ReturnsAsync(
                (LanguageIdDetails languageIdDetails, MenuIdDetails menuIdDetails) => mockTranslationData.GetLanguageTranslationsByMenuAndLanguage(
                    new LanguageIdDetails { LanguageId = languageId }, new MenuIdDetails { MenuId = menuId }));

            var value = await controller.GetLanguageTranslationsByMenuAndLanguage(languageId, menuId);

            Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        }

        [Theory]
        [InlineData(1, 100)]
        [InlineData(2, 101)]
        public async void GetLanguageTranslationsByMenuAndLanguage_ReturnsNotFoundResultForMenu(int languageId, int menuId)
        {
            mockTranslationService.Setup(mr => mr.GetLanguageTranslationsByMenuAndLanguage(It.IsAny<LanguageIdDetails>(), It.IsAny<MenuIdDetails>())).ReturnsAsync(
                (LanguageIdDetails languageIdDetails, MenuIdDetails menuIdDetails) => mockTranslationData.GetLanguageTranslationsByMenuAndLanguage(
                    new LanguageIdDetails { LanguageId = languageId }, new MenuIdDetails { MenuId = menuId }));

            var value = await controller.GetLanguageTranslationsByMenuAndLanguage(languageId, menuId);

            Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        }


        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        public async void GetLanguageTranslationsByMenuAndLanguage_ReturnsOKResult(int languageId, int menuId)
        {
            mockTranslationService.Setup(mr => mr.GetLanguageTranslationsByMenuAndLanguage(It.IsAny<LanguageIdDetails>(), It.IsAny<MenuIdDetails>())).ReturnsAsync(
                (LanguageIdDetails languageIdDetails, MenuIdDetails menuIdDetails) => mockTranslationData.GetLanguageTranslationsByMenuAndLanguage(
                    new LanguageIdDetails { LanguageId = languageId }, new MenuIdDetails { MenuId = menuId }));

            var value = await controller.GetLanguageTranslationsByMenuAndLanguage(languageId, menuId);


            Assert.Equal((int)HttpStatusCode.OK, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region CreateLanguageTranslation

        [Fact]
        public async void CreateLanguageTranslation_ReturnsConflictResult()
        {
            CreateLanguageTranslation languageTranslation = new CreateLanguageTranslation()
            {
                LanguageId = 1,
                MenuId = 1,
                VariableId = 1,
                Translation = "test",
                CreatedBy = "admin"
            };

            mockTranslationService.Setup(mr => mr.CreateLanguageTranslation(It.IsAny<CreateLanguageTranslation>())).ReturnsAsync(
                (CreateLanguageTranslation language) => mockTranslationData.CreateLanguageTranslation(languageTranslation));

            var value = await controller.CreateLanguageTranslation(languageTranslation);

            Assert.Equal((int)HttpStatusCode.Conflict, (value as ObjectResult).StatusCode);
        }

        [Fact]
        public async void CreateLanguageTranslation_ReturnsOkResult()
        {
            CreateLanguageTranslation languageTranslation = new CreateLanguageTranslation()
            {
                LanguageId = 4,
                MenuId = 4,
                VariableId = 4,
                Translation = "test",
                CreatedBy = "admin"
            };

            mockTranslationService.Setup(mr => mr.CreateLanguageTranslation(It.IsAny<CreateLanguageTranslation>())).ReturnsAsync(
                (CreateLanguageTranslation language) => mockTranslationData.CreateLanguageTranslation(languageTranslation));

            var value = await controller.CreateLanguageTranslation(languageTranslation);

            Assert.Equal((int)HttpStatusCode.Created, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region UpdateLanguageTranslation

        [Theory]
        [InlineData(100)]
        [InlineData(101)]
        public async void UpdateSchedulingCode_ReturnsNotFoundResult(int translationId)
        {
            UpdateLanguageTranslation languageTranslation = new UpdateLanguageTranslation()
            {
                LanguageId = 4,
                MenuId = 4,
                VariableId = 4,
                Translation = "testing",
                ModifiedBy = "admin"
            };

            mockTranslationService.Setup(mr => mr.UpdateLanguageTranslation(It.IsAny<LanguageTranslationIdDetails>(), It.IsAny<UpdateLanguageTranslation>())).ReturnsAsync(
                (LanguageTranslationIdDetails idDetails, UpdateLanguageTranslation update) =>
                mockTranslationData.UpdateLanguageTranslatione(new LanguageTranslationIdDetails { TranslationId = translationId }, languageTranslation));

            var value = await controller.UpdateLanguageTranslation(translationId, languageTranslation);

            Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        }

        [Theory]
        [InlineData(1)]
        public async void UpdateLanguageTranslation_ReturnsConflictResult(int translationId)
        {
            UpdateLanguageTranslation languageTranslation = new UpdateLanguageTranslation()
            {
                LanguageId = 2,
                MenuId = 2,
                VariableId = 2,
                Translation = "testing",
                ModifiedBy = "admin"
            };

            mockTranslationService.Setup(mr => mr.UpdateLanguageTranslation(It.IsAny<LanguageTranslationIdDetails>(), It.IsAny<UpdateLanguageTranslation>())).ReturnsAsync(
                (LanguageTranslationIdDetails idDetails, UpdateLanguageTranslation update) =>
                mockTranslationData.UpdateLanguageTranslatione(new LanguageTranslationIdDetails { TranslationId = translationId }, languageTranslation));

            var value = await controller.UpdateLanguageTranslation(translationId, languageTranslation);

            Assert.Equal((int)HttpStatusCode.Conflict, (value as ObjectResult).StatusCode);
        }

        /// <summary>
        /// Updates the scheduling code returns no content result.
        /// </summary>
        /// <param name="translationId">The translation identifier.</param>
        /// <param name="languageId">The language identifier.</param>
        /// <param name="menuId">The menu identifier.</param>
        /// <param name="variableId">The variable identifier.</param>
        [Theory]
        [InlineData(1, 1, 1, 1)]
        [InlineData(2, 2, 2, 2)]
        public async void UpdateSchedulingCode_ReturnsNoContentResult(int translationId, int languageId, int menuId, int variableId)
        {
            UpdateLanguageTranslation languageTranslation = new UpdateLanguageTranslation()
            {
                LanguageId = languageId,
                MenuId = menuId,
                VariableId = variableId,
                Translation = "testing",
                ModifiedBy = "admin"
            };

            mockTranslationService.Setup(mr => mr.UpdateLanguageTranslation(It.IsAny<LanguageTranslationIdDetails>(), It.IsAny<UpdateLanguageTranslation>())).ReturnsAsync(
                (LanguageTranslationIdDetails idDetails, UpdateLanguageTranslation update) =>
                mockTranslationData.UpdateLanguageTranslatione(new LanguageTranslationIdDetails { TranslationId = translationId }, languageTranslation));

            var value = await controller.UpdateLanguageTranslation(translationId, languageTranslation);

            Assert.Equal((int)HttpStatusCode.NoContent, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region DeleteClient

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void DeleteLanguageTranslation_ReturnsNoContentResult(int translationId)
        {
            mockTranslationService.Setup(mr => mr.DeleteLanguageTranslation(It.IsAny<LanguageTranslationIdDetails>())).ReturnsAsync(
                (LanguageTranslationIdDetails idDetails) => mockTranslationData.DeleteLanguageTranslation(new LanguageTranslationIdDetails { TranslationId = translationId }));

            var value = await controller.DeleteLanguageTranslation(translationId);

            Assert.Equal((int)HttpStatusCode.NoContent, (value as ObjectResult).StatusCode);
        }

        [Theory]
        [InlineData(100)]
        [InlineData(101)]
        public async void DeleteLanguageTranslation_ReturnsNotFoundResult(int translationId)
        {
            mockTranslationService.Setup(mr => mr.DeleteLanguageTranslation(It.IsAny<LanguageTranslationIdDetails>())).ReturnsAsync(
                (LanguageTranslationIdDetails idDetails) => mockTranslationData.DeleteLanguageTranslation(new LanguageTranslationIdDetails { TranslationId = translationId }));

            var value = await controller.DeleteLanguageTranslation(translationId);

            Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        }

        #endregion
    }
}
