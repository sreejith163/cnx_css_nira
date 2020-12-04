using Css.Api.Admin.Models.DTO.Request.Language;
using Css.Api.Core.Models.DTO.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Css.Api.Admin.Repository.Interfaces
{
    public interface ICssLanguageRepository
    {
        /// <summary>
        /// Gets the languages.
        /// </summary>
        /// <returns></returns>
        Task<List<KeyValue>> GetCssLanguages();

        /// <summary>
        /// Gets the CSS language.
        /// </summary>
        /// <param name="languageIdDetails">The language identifier details.</param>
        /// <returns></returns>
        Task<KeyValue> GetCssLanguage(LanguageIdDetails languageIdDetails);
    }
}
