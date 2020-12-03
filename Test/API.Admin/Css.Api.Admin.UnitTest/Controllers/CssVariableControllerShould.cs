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
    public class CssVariableControllerShould
    {
        /// <summary>
        /// The mock variable service
        /// </summary>
        private readonly Mock<ICssVariableService> mockVariableService;

        /// <summary>
        /// The controller
        /// </summary>
        private readonly CssVariableController controller;

        /// <summary>
        /// The mock CSS variable data
        /// </summary>
        private readonly MockCssVariableData mockCssVariableData;

        /// <summary>
        /// Initializes a new instance of the <see cref="CssVariableControllerShould"/> class.
        /// </summary>
        public CssVariableControllerShould()
        {
            mockVariableService = new Mock<ICssVariableService>();
            mockCssVariableData = new MockCssVariableData();
            controller = new CssVariableController(mockVariableService.Object);
        }

        #region CssVariables

        /// <summary>
        /// Gets the CSS variables.
        /// </summary>
        [Fact]
        public async void GetCssVariables()
        {
            CssVariableQueryParameters queryParameters = new CssVariableQueryParameters();

            mockVariableService.Setup(mr => mr.GetCssVariables(It.IsAny<CssVariableQueryParameters>())).ReturnsAsync(
                (CssVariableQueryParameters queryParameters) => mockCssVariableData.GetVariables(queryParameters));

            var value = await controller.GetCssVariables(queryParameters);

            Assert.Equal((int)HttpStatusCode.OK, (value as ObjectResult).StatusCode);
        }

        #endregion
    }
}
