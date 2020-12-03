using Css.Api.Core.Models.DTO.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Admin.Business.Interfaces
{
    public interface ICssLanguageService
    {
        /// <summary>
        /// Gets the CSS languages.
        /// </summary>
        /// <returns></returns>
        Task<CSSResponse> GetCssLanguages();
    }
}
