using Css.Api.Admin.Business.Interfaces;
using Css.Api.Admin.Controllers;
using Css.Api.Admin.Models.DTO.Request.LanguageTranslation;
using Css.Api.Admin.UnitTest.Mock;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
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
        private readonly LanguageTranslationController controller;

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
            controller = new LanguageTranslationController(mockTranslationService.Object);
        }

        #region GetLanguageTranslations

        [Fact]
        public async void GetLanguageTranslations()
        {
            TranslationQueryParameters queryParameters = new TranslationQueryParameters();

            mockTranslationService.Setup(mr => mr.GetLanguageTranslations(It.IsAny<TranslationQueryParameters>())).ReturnsAsync(
                (TranslationQueryParameters queryParameters) => mockTranslationData.GetLanguageTranslations(queryParameters));

            var value = await controller.GetLanguageTranslations(queryParameters);

            Assert.Equal((int)HttpStatusCode.OK, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region GetLanguageTranslation

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetLanguageTranslation_ReturnsOKResult(int translationId)
        {
            mockTranslationService.Setup(mr => mr.GetLanguageTranslation(It.IsAny<TranslationIdDetails>())).ReturnsAsync(
                (TranslationIdDetails idDetails) => mockTranslationData.GetLanguageTranslation(new TranslationIdDetails { TranslationId = translationId }));

            var value = await controller.GetLanguageTranslation(translationId);

            Assert.Equal((int)HttpStatusCode.OK, (value as ObjectResult).StatusCode);
        }

        [Theory]
        [InlineData(100)]
        [InlineData(101)]
        public async void GetLanguageTranslation_ReturnsNotFoundResult(int translationId)
        {
            mockTranslationService.Setup(mr => mr.GetLanguageTranslation(It.IsAny<TranslationIdDetails>())).ReturnsAsync(
                (TranslationIdDetails idDetails) => mockTranslationData.GetLanguageTranslation(new TranslationIdDetails { TranslationId = translationId }));

            var value = await controller.GetLanguageTranslation(translationId);

            Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region CreateLanguageTranslation

        [Fact]
        public async void CreateLanguageTranslation_ReturnsConflictResult()
        {
            CreateLanguageTranslation languageTranslation = new CreateLanguageTranslation()
            {
                LanguageId = 1,
                LanguageName = "test",
                MenuId = 1,
                MenuName = "menu",
                VariableId = 1,
                VariableName = "variable",
                Description = "test",
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
                LanguageName = "test",
                MenuId = 4,
                MenuName = "menu",
                VariableId = 4,
                VariableName = "variable",
                Description = "test",
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
                LanguageName = "test",
                MenuId = 4,
                MenuName = "menu",
                VariableId = 4,
                VariableName = "variable",
                Description = "test",
                Translation = "testing",
                ModifiedBy = "admin"
            };

            mockTranslationService.Setup(mr => mr.UpdateLanguageTranslation(It.IsAny<TranslationIdDetails>(), It.IsAny<UpdateLanguageTranslation>())).ReturnsAsync(
                (TranslationIdDetails idDetails, UpdateLanguageTranslation update) =>
                mockTranslationData.UpdateLanguageTranslatione(new TranslationIdDetails { TranslationId = translationId }, languageTranslation));

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
                LanguageName = "test",
                MenuId = 2,
                MenuName = "menu",
                VariableId = 2,
                VariableName = "variable",
                Description = "test",
                Translation = "testing",
                ModifiedBy = "admin"
            };

            mockTranslationService.Setup(mr => mr.UpdateLanguageTranslation(It.IsAny<TranslationIdDetails>(), It.IsAny<UpdateLanguageTranslation>())).ReturnsAsync(
                (TranslationIdDetails idDetails, UpdateLanguageTranslation update) =>
                mockTranslationData.UpdateLanguageTranslatione(new TranslationIdDetails { TranslationId = translationId }, languageTranslation));

            var value = await controller.UpdateLanguageTranslation(translationId, languageTranslation);

            Assert.Equal((int)HttpStatusCode.Conflict, (value as ObjectResult).StatusCode);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void UpdateSchedulingCode_ReturnsNoContentResult(int translationId)
        {
            UpdateLanguageTranslation languageTranslation = new UpdateLanguageTranslation()
            {
                LanguageId = 2,
                LanguageName = "test",
                MenuId = 2,
                MenuName = "menu",
                VariableId = 2,
                VariableName = "variable",
                Description = "test",
                Translation = "testing",
                ModifiedBy = "admin"
            };

            mockTranslationService.Setup(mr => mr.UpdateLanguageTranslation(It.IsAny<TranslationIdDetails>(), It.IsAny<UpdateLanguageTranslation>())).ReturnsAsync(
                (TranslationIdDetails idDetails, UpdateLanguageTranslation update) =>
                mockTranslationData.UpdateLanguageTranslatione(new TranslationIdDetails { TranslationId = translationId }, languageTranslation));

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
            mockTranslationService.Setup(mr => mr.DeleteLanguageTranslation(It.IsAny<TranslationIdDetails>())).ReturnsAsync(
                (TranslationIdDetails idDetails) => mockTranslationData.DeleteLanguageTranslation(new TranslationIdDetails { TranslationId = translationId }));

            var value = await controller.DeleteLanguageTranslation(translationId);

            Assert.Equal((int)HttpStatusCode.NoContent, (value as ObjectResult).StatusCode);
        }

        [Theory]
        [InlineData(100)]
        [InlineData(101)]
        public async void DeleteLanguageTranslation_ReturnsNotFoundResult(int translationId)
        {
            mockTranslationService.Setup(mr => mr.DeleteLanguageTranslation(It.IsAny<TranslationIdDetails>())).ReturnsAsync(
                (TranslationIdDetails idDetails) => mockTranslationData.DeleteLanguageTranslation(new TranslationIdDetails { TranslationId = translationId }));

            var value = await controller.DeleteLanguageTranslation(translationId);

            Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        }

        #endregion
    }
}
