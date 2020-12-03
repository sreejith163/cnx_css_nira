using System.Threading.Tasks;
using Css.Api.Admin.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Css.Api.Admin.Controllers
{
    /// <summary>
    /// Controller for handling the css menu resource
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CssMenusController : ControllerBase
    {
        /// <summary>
        /// The CSS menu service
        /// </summary>
        private readonly ICssMenuService _cssMenuService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CssMenusController"/> class.
        /// </summary>
        /// <param name="cssMenuService">The CSS menu service.</param>
        public CssMenusController(ICssMenuService cssMenuService)
        {
            _cssMenuService = cssMenuService;
        }

        /// <summary>
        /// Gets the menus.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetMenus()
        {
            var result = await _cssMenuService.GetCssMenus();
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Gets the menu variables.
        /// </summary>
        /// <param name="menuId">The menu identifier.</param>
        /// <returns></returns>
        [HttpGet("{menuId/variables}")]
        public async Task<IActionResult> GetMenuVariables(int menuId)
        {
            var result = await _cssMenuService.GetCssMenuVariables(menuId);
            return StatusCode((int)result.Code, result.Value);
        }
    }
}
