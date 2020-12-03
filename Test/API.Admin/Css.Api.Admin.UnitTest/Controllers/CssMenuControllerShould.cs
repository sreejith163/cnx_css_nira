using Css.Api.Admin.Business.Interfaces;
using Css.Api.Admin.Controllers;
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
    public class CssMenuControllerShould
    {
        /// <summary>
        /// The mock menu service
        /// </summary>
        private readonly Mock<ICssMenuService> mockMenuService;

        /// <summary>
        /// The controller
        /// </summary>
        private readonly CssMenuController controller;

        /// <summary>
        /// The mock CSS menu data
        /// </summary>
        private readonly MockCssMenuData mockCssMenuData;

        /// <summary>
        /// Initializes a new instance of the <see cref="CssMenuControllerShould"/> class.
        /// </summary>
        public CssMenuControllerShould()
        {
            mockMenuService = new Mock<ICssMenuService>();
            mockCssMenuData = new MockCssMenuData();
            controller = new CssMenuController(mockMenuService.Object);
        }

        #region CssMenu

        /// <summary>
        /// Gets the CSS menus.
        /// </summary>
        [Fact]
        public async void GetCssMenus()
        {
            mockMenuService.Setup(mr => mr.GetCssMenus()).ReturnsAsync(
                () => mockCssMenuData.GetMenus());

            var value = await controller.GetMenus();

            Assert.Equal((int)HttpStatusCode.OK, (value as ObjectResult).StatusCode);
        }

        #endregion
    }
}
