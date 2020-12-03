using Moq;
using Xunit;
using System.Net;
using AutoMapper;
using Css.Api.Core.Models.Domain;
using Css.Api.Admin.Repository;
using Css.Api.Admin.Business.Interfaces;
using Css.Api.Admin.Repository.Interfaces;
using Css.Api.Admin.Business.UnitTest.Mock;
using Microsoft.AspNetCore.Http;
using Css.Api.Admin.Models.Profiles.Translation;
using Css.Api.Admin.Models.DTO.Request.LanguageTranslation;
using Css.Api.Admin.Models.DTO.Response.LanguageTranslation;
using Css.Api.Admin.Models.Profiles.Variable;
using Css.Api.Admin.Models.Profiles.Menu;
using Css.Api.Admin.Models.Profiles.Language;

namespace Css.Api.Admin.Business.UnitTest.Services
{
    public class LanguageTranslationServiceShould
    {
        /// <summary>
        /// The translation service
        /// </summary>
        private readonly ILanguageTranslationService translationService;

        /// <summary>
        /// The repository wrapper
        /// </summary>
        private readonly IRepositoryWrapper repositoryWrapper;

        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageTranslationServiceShould"/> class.
        /// </summary>
        public LanguageTranslationServiceShould()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new TranslationProfile());
                cfg.AddProfile(new MenuProfile());
                cfg.AddProfile(new VariableProfile());
                cfg.AddProfile(new LanguageProfile());
            });

            mapper = new Mapper(mapperConfig);

            var context = new DefaultHttpContext();
            Mock<IHttpContextAccessor> mockHttContext = new Mock<IHttpContextAccessor>();
            mockHttContext.Setup(_ => _.HttpContext).Returns(context);

            var mockSchedulingContext = new MockDataContext().IntializeMockData();

            repositoryWrapper = new MockRepositoryWrapper(mockSchedulingContext, mapper);

            translationService = new LanguageTranslationService(repositoryWrapper, mockHttContext.Object, mapper);
        }

        #region LanguageTranslations

        /// <summary>
        /// Gets the language translations.
        /// </summary>
        [Fact]
        public async void GetLanguageTranslations()
        {
            LanguageTranslationQueryParameters languageTranslationQueryParameters = new LanguageTranslationQueryParameters();
            var result = await translationService.GetLanguageTranslations(languageTranslationQueryParameters);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<PagedList<Entity>>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region GetLanguageTranslation

        /// <summary>
        /// Gets the language translation with not found.
        /// </summary>
        /// <param name="translationId">The translation identifier.</param>
        [Theory]
        [InlineData(100)]
        public async void GetLanguageTranslationWithNotFound(int translationId)
        {
            var result = await translationService.GetLanguageTranslation(new LanguageTranslationIdDetails { TranslationId = translationId });

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
        }

        /// <summary>
        /// Gets the language translation.
        /// </summary>
        /// <param name="translationId">The translation identifier.</param>
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetLanguageTranslation(int translationId)
        {
            var result = await translationService.GetLanguageTranslation(new LanguageTranslationIdDetails { TranslationId = translationId });

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<LanguageTranslationDTO>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region CreateLanguageTranslation

        /// <summary>
        /// Creates the language translation with conflict found.
        /// </summary>
        [Fact]
        public async void CreateLanguageTranslationWithConflictFound()
        {
            CreateLanguageTranslation languageTranslation = new CreateLanguageTranslation()
            {
                LanguageId=1,
                MenuId=1,
                VariableId=1,
                Description="test",
                Translation="test",
                CreatedBy="admin"
            };
            var result = await translationService.CreateLanguageTranslation(languageTranslation);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.Conflict, result.Code);
        }

        /// <summary>
        /// Creates the language translation.
        /// </summary>
        [Fact]
        public async void CreateLanguageTranslation()
        {
            CreateLanguageTranslation languageTranslation = new CreateLanguageTranslation()
            {
                LanguageId = 4,
                MenuId = 4,
                VariableId = 4,
                Description = "test",
                Translation = "test",
                CreatedBy = "admin"
            };
            var result = await translationService.CreateLanguageTranslation(languageTranslation);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<LanguageTranslationIdDetails>(result.Value);
            Assert.Equal(HttpStatusCode.Created, result.Code);
        }

        #endregion

        #region UpdateLanguageTranslation

        /// <summary>
        /// Updates the language translation with not found.
        /// </summary>
        /// <param name="translationId">The translation identifier.</param>
        [Theory]
        [InlineData(100)]
        public async void UpdateLanguageTranslationWithNotFound(int translationId)
        {
            UpdateLanguageTranslation languageTranslation = new UpdateLanguageTranslation()
            {
                LanguageId = 1,
                MenuId = 1,
                VariableId = 1,
                Description = "test",
                Translation = "test",
                ModifiedBy = "admin"
            };
            var result = await translationService.UpdateLanguageTranslation(new LanguageTranslationIdDetails { TranslationId = translationId }, languageTranslation);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
        }

        /// <summary>
        /// Updates the language translation with conflict.
        /// </summary>
        /// <param name="translationId">The translation identifier.</param>
        [Theory]
        [InlineData(1)]
        public async void UpdateLanguageTranslationWithConflict(int translationId)
        {
            UpdateLanguageTranslation languageTranslation = new UpdateLanguageTranslation()
            {
                LanguageId = 2,
                MenuId = 2,
                VariableId = 2,
                Description = "test",
                Translation = "test",
                ModifiedBy = "admin"
            };
            var result = await translationService.UpdateLanguageTranslation(new LanguageTranslationIdDetails { TranslationId = translationId }, languageTranslation);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.Conflict, result.Code);
        }

        /// <summary>
        /// Updates the language translation.
        /// </summary>
        /// <param name="translationId">The translation identifier.</param>
        [Theory]
        [InlineData(1)]
        public async void UpdateLanguageTranslation(int translationId)
        {
            UpdateLanguageTranslation languageTranslation = new UpdateLanguageTranslation()
            {
                LanguageId = 4,
                MenuId = 4,
                VariableId = 4,
                Description = "test",
                Translation = "testing",
                ModifiedBy = "admin"
            };
            var result = await translationService.UpdateLanguageTranslation(new LanguageTranslationIdDetails { TranslationId = translationId }, languageTranslation);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NoContent, result.Code);
        }

        #endregion

        #region DeleteLanguageTranslation

        /// <summary>
        /// Deletes the language translation.
        /// </summary>
        /// <param name="translationId">The translation identifier.</param>
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void DeleteLanguageTranslation(int translationId)
        {
            var result = await translationService.DeleteLanguageTranslation(new LanguageTranslationIdDetails { TranslationId = translationId });

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NoContent, result.Code);
        }

        /// <summary>
        /// Deletes the language translatione with not found.
        /// </summary>
        /// <param name="translationId">The translation identifier.</param>
        [Theory]
        [InlineData(100)]
        public async void DeleteLanguageTranslationeWithNotFound(int translationId)
        {
            var result = await translationService.DeleteLanguageTranslation(new LanguageTranslationIdDetails { TranslationId = translationId });

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
        }

        #endregion
    }
}
