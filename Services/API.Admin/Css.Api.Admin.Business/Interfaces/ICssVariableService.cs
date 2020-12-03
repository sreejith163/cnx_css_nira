using Css.Api.Admin.Models.DTO.Request.LanguageTranslation;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Models.DTO.Response;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Admin.Business.Interfaces
{
    public interface ICssVariableService
    {
        /// <summary>
        /// Gets css variables
        /// </summary>
        /// <returns></returns>
        Task<CSSResponse> GetCssVariables(CssVariableQueryParameters cssVariableQueryParameters);
    }
}
