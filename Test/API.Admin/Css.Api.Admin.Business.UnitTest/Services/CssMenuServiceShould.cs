using AutoMapper;
using Css.Api.Admin.Business.Interfaces;
using Css.Api.Admin.Business.UnitTest.Mock;
using Css.Api.Admin.Models.DTO.Request.Menu;
using Css.Api.Admin.Models.Profiles.Menu;
using Css.Api.Admin.Models.Profiles.Translation;
using Css.Api.Admin.Models.Profiles.Variable;
using Css.Api.Admin.Repository;
using Css.Api.Admin.Repository.Interfaces;
using Css.Api.Core.Models.DTO.Response;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Xunit;

namespace Css.Api.Admin.Business.UnitTest.Services
{
    public class CssMenuServiceShould
    {
        /// <summary>
        /// The menu service service
        /// </summary>
        private readonly ICssMenuService menuService;

        /// <summary>
        /// The repository wrapper
        /// </summary>
        private readonly IRepositoryWrapper repositoryWrapper;

        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="CssMenuServiceShould"/> class.
        /// </summary>
        public CssMenuServiceShould()
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

            menuService = new CssMenuService(repositoryWrapper);
        }

        #region GetCssMenus

        /// <summary>
        /// Gets the CSS menus.
        /// </summary>
        [Fact]
        public async void GetCssMenus()
        {
            var result = await menuService.GetCssMenus();

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<List<KeyValue>>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region GetCssMenuVariables

        /// <summary>
        /// Gets the CSS menus.
        /// </summary>
        /// <param name="menuId">The menu identifier.</param>
        [Theory]
        [InlineData(100)]
        [InlineData(101)]
        public async void GetCssMenuVariablesWithNotFound(int menuId)
        {
            var result = await menuService.GetCssMenuVariables(new MenuIdDetails { MenuId = menuId });

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
        }

        /// <summary>
        /// Gets the CSS menus.
        /// </summary>
        /// <param name="menuId">The menu identifier.</param>
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetCssMenuVariables(int menuId)
        {
            var result = await menuService.GetCssMenuVariables(new MenuIdDetails { MenuId = menuId });

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<List<KeyValue>>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion
    }
}
