using System;
using System.Collections.Generic;
using System.Linq;
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
    public class CssMenuController : ControllerBase
    {
        /// <summary>
        /// The CSS menu service
        /// </summary>
        private readonly ICssMenuService _cssMenuService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CssMenuController"/> class.
        /// </summary>
        /// <param name="cssMenuService">The CSS menu service.</param>
        public CssMenuController(ICssMenuService cssMenuService)
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
    }
}
