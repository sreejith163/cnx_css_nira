using Css.Api.Admin.Models.DTO.Request.LanguageTranslation;
using Css.Api.Core.Models.DTO.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Admin.Repository.Interfaces
{
    public interface ICssVariableRepository
    {
        /// <summary>
        /// Gets the CSS variables.
        /// </summary>
        /// <returns></returns>
        Task<List<KeyValue>> GetCssVariables(CssVariableQueryParameters cssVariableQueryParameters);
    }
}
