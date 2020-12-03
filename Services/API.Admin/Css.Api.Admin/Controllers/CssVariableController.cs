using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Css.Api.Admin.Business.Interfaces;
using Css.Api.Admin.Models.DTO.Request.LanguageTranslation;
using Microsoft.AspNetCore.Mvc;

namespace Css.Api.Admin.Controllers
{
    /// <summary>
    /// Controller for handling the css variable resource
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CssVariableController : ControllerBase
    {
        /// <summary>
        /// The CSS variable service
        /// </summary>
        private readonly ICssVariableService _cssVariableService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CssVariableController"/> class.
        /// </summary>
        /// <param name="cssVariableService">The CSS variable service.</param>
        public CssVariableController(ICssVariableService cssVariableService)
        {
            _cssVariableService = cssVariableService;
        }

        /// <summary>
        /// Gets the CSS variables.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetCssVariables([FromQuery] CssVariableQueryParameters cssVariableQueryParameters)
        {
            var result = await _cssVariableService.GetCssVariables(cssVariableQueryParameters);
            return StatusCode((int)result.Code, result.Value);
        }
    }
}
