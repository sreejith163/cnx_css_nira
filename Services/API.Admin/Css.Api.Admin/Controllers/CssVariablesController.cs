using System.Threading.Tasks;
using Css.Api.Admin.Business.Interfaces;
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
    public class CssVariablesController : ControllerBase
    {
        /// <summary>
        /// The CSS variable service
        /// </summary>
        private readonly ICssVariableService _cssVariableService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CssVariablesController"/> class.
        /// </summary>
        /// <param name="cssVariableService">The CSS variable service.</param>
        public CssVariablesController(ICssVariableService cssVariableService)
        {
            _cssVariableService = cssVariableService;
        }

        /// <summary>
        /// Gets the CSS variables.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetCssVariables()
        {
            var result = await _cssVariableService.GetCssVariables();
            return StatusCode((int)result.Code, result.Value);
        }
    }
}
