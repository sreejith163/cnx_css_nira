using Css.Api.Admin.Business.Interfaces;
using Css.Api.Admin.Controllers;
using Css.Api.Admin.Models.DTO.Request.Variable;
using Css.Api.Admin.UnitTest.Mock;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
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
        private readonly CssVariablesController controller;

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
            controller = new CssVariablesController(mockVariableService.Object);
        }

        #region CssVariables

        /// <summary>
        /// Gets the CSS variables.
        /// </summary>
        [Fact]
        public async void GetCssVariables()
        {
            VariableQueryParams variableQueryParams = new VariableQueryParams();

            mockVariableService.Setup(mr => mr.GetCssVariables(It.IsAny<VariableQueryParams>())).ReturnsAsync(
                (VariableQueryParams queryParameters) => mockCssVariableData.GetVariables(variableQueryParams));

            var value = await controller.GetCssVariables(variableQueryParams);

            Assert.Equal((int)HttpStatusCode.OK, (value as ObjectResult).StatusCode);
        }

        #endregion
    }
}
