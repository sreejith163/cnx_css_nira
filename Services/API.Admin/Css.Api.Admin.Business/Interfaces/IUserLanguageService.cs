using Css.Api.Admin.Models.DTO.Request.UserLanguage;
using Css.Api.Admin.Models.DTO.Response.UserLanguage;
using Css.Api.Core.Models.DTO.Response;
using System.Threading.Tasks;

namespace Css.Api.Admin.Business.Interfaces
{
    public interface IUserLanguageService
    {
        Task<CSSResponse> UpdateUserLanguagePreference(UserLanguageEmployeeIdDetails userLanguageEmployeeIdDetails, UpdateUserLanguagePreferenceDTO userLanguagePreference);

        Task<CSSResponse> GetUserLanguagePreference(string employeeId);

    }
}
