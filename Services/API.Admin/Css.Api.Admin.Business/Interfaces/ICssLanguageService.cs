using Css.Api.Core.Models.DTO.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Admin.Business.Interfaces
{
    public interface ICssLanguageService
    {
        Task<CSSResponse> GetCssLanguages();
    }
}
