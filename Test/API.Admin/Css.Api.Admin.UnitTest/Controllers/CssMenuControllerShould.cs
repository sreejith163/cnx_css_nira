using Css.Api.Admin.Business.Interfaces;
using Css.Api.Admin.Controllers;
using Css.Api.Admin.Models.DTO.Request.Menu;
using Css.Api.Admin.UnitTest.Mock;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using Xunit;

namespace Css.Api.Admin.UnitTest.Controllers
{
    public class CssMenuControllerShould
    {
        /// <summary>
        /// The mock menu service
        /// </summary>
        private readonly Mock<ICssMenuService> mockMenuService;

        /// <summary>
        /// The controller
        /// </summary>
        private readonly CssMenusController controller;

        /// <summary>
        /// The mock CSS menu data
        /// </summary>
        private readonly MockCssMenuData mockCssMenuData;

        /// <summary>
        /// The mock CSS variable data
        /// </summary>
        private readonly MockCssVariableData mockCssVariableData;

        /// <summary>
        /// Initializes a new instance of the <see cref="CssMenuControllerShould"/> class.
        /// </summary>
        public CssMenuControllerShould()
        {
            mockMenuService = new Mock<ICssMenuService>();
            mockCssMenuData = new MockCssMenuData();
            mockCssVariableData = new MockCssVariableData();
            controller = new CssMenusController(mockMenuService.Object);
        }

        #region GetMenus

        /// <summary>
        /// Gets the CSS menus.
        /// </summary>
        [Fact]
        public async void GetMenus()
        {
            mockMenuService.Setup(mr => mr.GetCssMenus()).ReturnsAsync(
                () => mockCssMenuData.GetCssMenus());

            var value = await controller.GetMenus();

            Assert.Equal((int)HttpStatusCode.OK, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region GetMenuVariables

        [Theory]
        [InlineData(100)]
        [InlineData(101)]
        public async void GetMenuVariablesWithNotFound(int menuId)
        {
            mockMenuService.Setup(mr => mr.GetCssMenuVariables(It.IsAny<MenuIdDetails>())).ReturnsAsync(
                (MenuIdDetails menuIdDetails) => mockCssVariableData.GetMenuVariables(new MenuIdDetails { MenuId = menuId }));

            var value = await controller.GetMenuVariables(menuId);

            Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        }

        /// <summary>
        /// Gets the CSS menus.
        /// </summary>
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetMenuVariables(int menuId)
        {
            mockMenuService.Setup(mr => mr.GetCssMenuVariables(It.IsAny<MenuIdDetails>())).ReturnsAsync(
                (MenuIdDetails menuIdDetails) => mockCssVariableData.GetMenuVariables(new MenuIdDetails { MenuId = menuId }));

            var value = await controller.GetMenuVariables(menuId);

            Assert.Equal((int)HttpStatusCode.OK, (value as ObjectResult).StatusCode);
        }

        #endregion
    }
}
