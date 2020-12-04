using System.Threading.Tasks;
using Css.Api.Admin.Business.Interfaces;
using Css.Api.Admin.Models.DTO.Request.Language;
using Css.Api.Admin.Models.DTO.Request.LanguageTranslation;
using Css.Api.Admin.Models.DTO.Request.Menu;
using Microsoft.AspNetCore.Mvc;

namespace Css.Api.Admin.Controllers
{
    /// <summary>
    /// Controller for handling the language translation resource
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LanguageTranslationsController : ControllerBase
    {
        /// <summary>
        /// The language translation service
        /// </summary>
        private readonly ILanguageTranslationService _languageTranslationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageTranslationsController" /> class.
        /// </summary>
        /// <param name="languageTranslationService">The language translation service.</param>
        public LanguageTranslationsController(ILanguageTranslationService languageTranslationService)
        {
            _languageTranslationService = languageTranslationService;
        }

        /// <summary>
        /// Gets the language translations.
        /// </summary>
        /// <param name="translationQueryParameters">The translation query parameters.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetLanguageTranslations([FromQuery] LanguageTranslationQueryParameters translationQueryParameters)
        {
            var result = await _languageTranslationService.GetLanguageTranslations(translationQueryParameters);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Gets the language translation.
        /// </summary>
        /// <param name="translationId">The translation identifier.</param>
        /// <returns></returns>
        [HttpGet("{translationId}")]
        public async Task<IActionResult> GetLanguageTranslation(int translationId)
        {
            var result = await _languageTranslationService.GetLanguageTranslation(new LanguageTranslationIdDetails { TranslationId = translationId });
            return StatusCode((int)result.Code, result.Value);
        }


        /// <summary>
        /// Gets the language translations by menu and language.
        /// </summary>
        /// <param name="languageId">The language identifier.</param>
        /// <param name="menuId">The menu identifier.</param>
        /// <returns></returns>
        [HttpGet("languages/{languageId}/menus/{menuId}")]
        public async Task<IActionResult> GetLanguageTranslationsByMenuAndLanguage(int languageId, int menuId)
        {
            var result = await _languageTranslationService.GetLanguageTranslationsByMenuAndLanguage(new LanguageIdDetails { LanguageId = languageId }, 
                                                                                                    new MenuIdDetails { MenuId = menuId });
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Creates the language translation.
        /// </summary>
        /// <param name="translationDetails">The translation details.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateLanguageTranslation([FromBody] CreateLanguageTranslation translationDetails)
        {
            var result = await _languageTranslationService.CreateLanguageTranslation(translationDetails);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Updates the language translation.
        /// </summary>
        /// <param name="translationId">The translation identifier.</param>
        /// <param name="translationDetails">The translation details.</param>
        /// <returns></returns>
        [HttpPut("{translationId}")]
        public async Task<IActionResult> UpdateLanguageTranslation(int translationId, [FromBody] UpdateLanguageTranslation translationDetails)
        {
            var result = await _languageTranslationService.UpdateLanguageTranslation(new LanguageTranslationIdDetails { TranslationId = translationId },
                translationDetails);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Deletes the language translation.
        /// </summary>
        /// <param name="translationId">The translation identifier.</param>
        /// <returns></returns>
        [HttpDelete("{translationId}")]
        public async Task<IActionResult> DeleteLanguageTranslation(int translationId)
        {
            var result = await _languageTranslationService.DeleteLanguageTranslation(new LanguageTranslationIdDetails { TranslationId = translationId });
            return StatusCode((int)result.Code, result.Value);
        }
    }
}