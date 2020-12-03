using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Css.Api.Admin.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Css.Api.Admin.Controllers
{
    /// <summary>
    /// Controller for handling the language resource
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CssLanguageController : ControllerBase
    {
        /// <summary>
        /// The lanmguage service
        /// </summary>
        private readonly ICssLanguageService _languageService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CssLanguageController"/> class.
        /// </summary>
        /// <param name="languageService">The language service.</param>
        public CssLanguageController(ICssLanguageService languageService)
        {
            _languageService = languageService;
        }

        /// <summary>
        /// Gets the languages.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetLanguages()
        {
            var result = await _languageService.GetCssLanguages();
            return StatusCode((int)result.Code, result.Value);
        }
    }
}
