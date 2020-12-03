using AutoMapper;
using Css.Api.Admin.Business.Interfaces;
using Css.Api.Admin.Business.UnitTest.Mock;
using Css.Api.Admin.Models.Profiles.Language;
using Css.Api.Admin.Models.Profiles.Translation;
using Css.Api.Admin.Repository;
using Css.Api.Admin.Repository.Interfaces;
using Css.Api.Core.Models.DTO.Response;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Collections.Generic;
using System.Net;
using Xunit;

namespace Css.Api.Admin.Business.UnitTest.Services
{
    public class CssLanguageServiceShould
    {
        /// <summary>
        /// The language service service
        /// </summary>
        private readonly ICssLanguageService languageService;

        /// <summary>
        /// The repository wrapper
        /// </summary>
        private readonly IRepositoryWrapper repositoryWrapper;

        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="CssLanguageServiceShould"/> class.
        /// </summary>
        public CssLanguageServiceShould()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new LanguageProfile());
            });

            mapper = new Mapper(mapperConfig);

            var context = new DefaultHttpContext();
            Mock<IHttpContextAccessor> mockHttContext = new Mock<IHttpContextAccessor>();
            mockHttContext.Setup(_ => _.HttpContext).Returns(context);

            var mockSchedulingContext = new MockDataContext().IntializeMockData();

            repositoryWrapper = new MockRepositoryWrapper(mockSchedulingContext, mapper);

            languageService = new CssLanguageService(repositoryWrapper);
        }

        #region CssLanguage

        /// <summary>
        /// Gets the CSS languages.
        /// </summary>
        [Fact]
        public async void GetCssLanguages()
        {
            var result = await languageService.GetCssLanguages();

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<List<KeyValue>>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion
    }
}
