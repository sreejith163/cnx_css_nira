using Css.Api.Admin.Models.Domain;
using Css.Api.Admin.Models.DTO.Request.LanguageTranslation;
using Css.Api.Admin.Models.DTO.Request.Menu;
using Css.Api.Core.Models.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Css.Api.Admin.Repository.Interfaces
{
    public interface ILanguageTranslationRepository
    {
        /// <summary>
        /// Gets the language translations.
        /// </summary>
        /// <param name="languageTranslationQueryParameters">The language translation query parameters.</param>
        /// <returns></returns>
        Task<PagedList<Entity>> GetLanguageTranslations(LanguageTranslationQueryParameters languageTranslationQueryParameters);

        /// <summary>
        /// Gets the language translation.
        /// </summary>
        /// <param name="languageTranslationIdDetails">The translation identifier details.</param>
        /// <returns></returns>
        Task<LanguageTranslation> GetLanguageTranslation(LanguageTranslationIdDetails languageTranslationIdDetails);

        /// <summary>
        /// Gets the language translation by other ids.
        /// </summary>
        /// <param name="languageIdDetails">The language identifier details.</param>
        /// <param name="menuIdDetails">The menu identifier details.</param>
        /// <param name="variableIdDetails">The variable identifier details.</param>
        /// <returns></returns>
        Task<List<int>> GetLanguageTranslationByOtherIds(LanguageIdDetails languageIdDetails, MenuIdDetails menuIdDetails, VariableIdDetails variableIdDetails);

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
