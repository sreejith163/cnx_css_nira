using Css.Api.Admin.Models.Domain;
using Css.Api.Admin.Models.DTO.Request.UserLanguage;
using Css.Api.Core.Models.Domain;
using System.Threading.Tasks;

namespace Css.Api.Admin.Repository.Interfaces
{
    public interface IUserLanguageRepository
    {
        /// <summary>
        /// Gets the User Language Preference.
        /// </summary>
        /// <param name="employeeId">The employeeId parameters.</param>
        /// <returns></returns>
        Task<UserLanguagePreference> GetUserLanguagePreference(UserLanguageEmployeeIdDetails userLanguageEmployeeIdDetails);

        /// <summary>
        /// Updates the User Language Preference.
        /// </summary>
        /// <param name="employeeId">The employeeId parameters.</param>
        /// <returns></returns>
        void UpdateUserLanguagePreference(UserLanguagePreference userLanguagePreference);

        /// <summary>
        /// Creates the User Language Preference.
        /// </summary>
        /// <param name="employeeId">The employeeId parameters.</param>
        /// <returns></returns>
        void CreateUserLanguagePreference(UserLanguagePreference userLanguagePreference);
    }
}
