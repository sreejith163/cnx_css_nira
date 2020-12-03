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
        /// <param name="translationQueryParameters">The translation query parameters.</param>
        /// <returns></returns>
        Task<CSSResponse> GetLanguageTranslations(TranslationQueryParameters translationQueryParameters);

        /// <summary>
        /// Gets the language translation.
        /// </summary>
        /// <param name="translationIdDetails">The translation identifier details.</param>
        /// <returns></returns>
        Task<CSSResponse> GetLanguageTranslation(TranslationIdDetails translationIdDetails);

        /// <summary>
        /// Creates the language translation.
        /// </summary>
        /// <param name="translationDetails">The translation details.</param>
        /// <returns></returns>
        Task<CSSResponse> CreateLanguageTranslation(CreateLanguageTranslation translationDetails);

        /// <summary>
        /// Updates the language translation.
        /// </summary>
        /// <param name="translationIdDetails">The translation identifier details.</param>
        /// <param name="translationDetails">The translation details.</param>
        /// <returns></returns>
        Task<CSSResponse> UpdateLanguageTranslation(TranslationIdDetails translationIdDetails, UpdateLanguageTranslation translationDetails);

        /// <summary>
        /// Deletes the language translation.
        /// </summary>
        /// <param name="translationIdDetails">The translation identifier details.</param>
        /// <returns></returns>
        Task<CSSResponse> DeleteLanguageTranslation(TranslationIdDetails translationIdDetails);
    }
}
