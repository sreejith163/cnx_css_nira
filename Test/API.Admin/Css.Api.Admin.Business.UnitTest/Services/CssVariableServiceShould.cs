using AutoMapper;
using Css.Api.Admin.Business.Interfaces;
using Css.Api.Admin.Business.UnitTest.Mock;
using Css.Api.Admin.Models.DTO.Request.Variable;
using Css.Api.Admin.Models.DTO.Response.SchedulingCode;
using Css.Api.Admin.Models.Profiles.Menu;
using Css.Api.Admin.Models.Profiles.Variable;
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
    public class CssVariableServiceShould
    {
        /// <summary>
        /// The menu service service
        /// </summary>
        private readonly ICssVariableService variableService;

        /// <summary>
        /// The repository wrapper
        /// </summary>
        private readonly IRepositoryWrapper repositoryWrapper;

        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="CssVariableServiceShould"/> class.
        /// </summary>
        public CssVariableServiceShould()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MenuProfile());
                cfg.AddProfile(new VariableProfile());
            });

            mapper = new Mapper(mapperConfig);

            var context = new DefaultHttpContext();
            Mock<IHttpContextAccessor> mockHttContext = new Mock<IHttpContextAccessor>();
            mockHttContext.Setup(_ => _.HttpContext).Returns(context);

            var mockSchedulingContext = new MockDataContext().IntializeMockData();

            repositoryWrapper = new MockRepositoryWrapper(mockSchedulingContext, mapper);

            variableService = new CssVariableService(repositoryWrapper);
        }

        #region CssVariable

        /// <summary>
        /// Gets the CSS variables.
        /// </summary>
        [Fact]
        public async void GetCssVariables()
        {
            VariableQueryParams variableQueryParams = new VariableQueryParams();
            var result = await variableService.GetCssVariables(variableQueryParams);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<List<KeyValue>>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion
    }
}
