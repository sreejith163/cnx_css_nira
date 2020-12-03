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
    public class CssLanguageControllerShould
    {
        /// <summary>
        /// The mock language service
        /// </summary>
        private readonly Mock<ICssLanguageService> mockLanguageService;

        /// <summary>
        /// The controller
        /// </summary>
        private readonly CssLanguageController controller;

        /// <summary>
        /// The mock CSS language data
        /// </summary>
        private readonly MockCssLanguageData mockCssLanguageData;

        /// <summary>
        /// Initializes a new instance of the <see cref="CssLanguageControllerShould"/> class.
        /// </summary>
        public CssLanguageControllerShould()
        {
            mockLanguageService = new Mock<ICssLanguageService>();
            mockCssLanguageData = new MockCssLanguageData();
            controller = new CssLanguageController(mockLanguageService.Object);
        }

        #region CssLanguage

        /// <summary>
        /// Gets the scheduling codes.
        /// </summary>
        [Fact]
        public async void GetSchedulingCodes()
        {
            mockLanguageService.Setup(mr => mr.GetCssLanguages()).ReturnsAsync(
                () => mockCssLanguageData.GetLanguages());

            var value = await controller.GetLanguages();

            Assert.Equal((int)HttpStatusCode.OK, (value as ObjectResult).StatusCode);
        }

        #endregion
    }
}
