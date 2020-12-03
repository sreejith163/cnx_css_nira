using Css.Api.Admin.Models.Domain;
using Css.Api.Admin.Models.DTO.Request.LanguageTranslation;
using Css.Api.Core.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Admin.Repository.Interfaces
{
    public interface ILanguageTranslationRepository
    {
        /// <summary>
        /// Gets the language translations.
        /// </summary>
        /// <param name="translationQueryParameters">The translation query parameters.</param>
        /// <returns></returns>
        Task<PagedList<Entity>> GetLanguageTranslations(TranslationQueryParameters translationQueryParameters);

        /// <summary>
        /// Gets the language translation.
        /// </summary>
        /// <param name="translationIdDetails">The translation identifier details.</param>
        /// <returns></returns>
        Task<LanguageTranslation> GetLanguageTranslation(TranslationIdDetails translationIdDetails);

        /// <summary>
        /// Gets the language translation by other ids.
        /// </summary>
        /// <param name="translationData">The translation data.</param>
        /// <returns></returns>
        Task<List<int>> GetLanguageTranslationByOtherIds(TranslationData translationData);

        /// <summary>
        /// Creates the language translation.
        /// </summary>
        /// <param name="languageTranslationRequest">The language translation request.</param>
        void CreateLanguageTranslation(LanguageTranslation languageTranslationRequest);

        /// <summary>
        /// Updates the language translation.
        /// </summary>
        /// <param name="languageTranslationRequest">The language translation request.</param>
        void UpdateLanguageTranslation(LanguageTranslation languageTranslationRequest);
    }
}
