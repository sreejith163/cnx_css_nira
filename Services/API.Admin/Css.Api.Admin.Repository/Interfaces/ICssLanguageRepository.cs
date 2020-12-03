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
    }
}
