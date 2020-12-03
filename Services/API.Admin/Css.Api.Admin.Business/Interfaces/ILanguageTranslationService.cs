using Css.Api.Admin.Models.DTO.Request.LanguageTranslation;
using Css.Api.Core.Models.DTO.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Admin.Business.Interfaces
{
    public interface ILanguageTranslationService
    {
        /// <summary>
        /// Gets the language translations.
        /// </summary>
        /// <param name="languageTranslationQueryParameters">The language translation query parameters.</param>
        /// <returns></returns>
        Task<CSSResponse> GetLanguageTranslations(LanguageTranslationQueryParameters languageTranslationQueryParameters);

        /// <summary>
        /// Gets the language translation.
        /// </summary>
        /// <param name="languageTranslationIdDetails">The translation identifier details.</param>
        /// <returns></returns>
        Task<CSSResponse> GetLanguageTranslation(LanguageTranslationIdDetails languageTranslationIdDetails);

        /// <summary>
        /// Creates the language translation.
        /// </summary>
        /// <param name="translationDetails">The translation details.</param>
        /// <returns></returns>
        Task<CSSResponse> CreateLanguageTranslation(CreateLanguageTranslation translationDetails);

        /// <summary>
        /// Updates the language translation.
        /// </summary>
        /// <param name="languageTranslationIdDetails">The translation identifier details.</param>
        /// <param name="translationDetails">The translation details.</param>
        /// <returns></returns>
        Task<CSSResponse> UpdateLanguageTranslation(LanguageTranslationIdDetails languageTranslationIdDetails, UpdateLanguageTranslation translationDetails);

        /// <summary>
        /// Deletes the language translation.
        /// </summary>
        /// <param name="languageTranslationIdDetails">The translation identifier details.</param>
        /// <returns></returns>
        Task<CSSResponse> DeleteLanguageTranslation(LanguageTranslationIdDetails languageTranslationIdDetails);
    }
}
