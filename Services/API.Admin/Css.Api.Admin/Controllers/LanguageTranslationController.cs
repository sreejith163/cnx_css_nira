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
    /// Controller for handling the language translation resource
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LanguageTranslationController : ControllerBase
    {
        /// <summary>
        /// The language translation service
        /// </summary>
        private readonly ILanguageTranslationService _languageTranslationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageTranslationController"/> class.
        /// </summary>
        /// <param name="languageTranslationService">The language translation service.</param>
        public LanguageTranslationController(ILanguageTranslationService languageTranslationService)
        {
            _languageTranslationService = languageTranslationService;
        }

        /// <summary>
        /// Gets the language translations.
        /// </summary>
        /// <param name="translationQueryParameters">The translation query parameters.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetLanguageTranslations([FromQuery] TranslationQueryParameters translationQueryParameters)
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
            var result = await _languageTranslationService.GetLanguageTranslation(new TranslationIdDetails { TranslationId = translationId });
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
            var result = await _languageTranslationService.UpdateLanguageTranslation(new TranslationIdDetails { TranslationId = translationId },
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
            var result = await _languageTranslationService.DeleteLanguageTranslation(new TranslationIdDetails { TranslationId = translationId });
            return StatusCode((int)result.Code, result.Value);
        }
    }
}