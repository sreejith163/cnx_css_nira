using Css.Api.Core.Models.DTO.Response;
using Css.Api.Admin.Models.DTO.Request.UserPermission;
using System.Threading.Tasks;

namespace Css.Api.Admin.Business.Interfaces
{
    public interface IUserPermissionService
    {
        /// <summary>
        /// Gets the userPermissions.
        /// </summary>
        /// <param name="userPermissionParameters">The userPermission parameters.</param>
        /// <returns></returns>
        Task<CSSResponse> GetUserPermissions(UserPermissionQueryParameters userPermissionParameters);

        /// <summary>
        /// Gets the userPermission.
        /// </summary>
        /// <param name="userPermissionEmployeeIdDetails">The userPermission identifier details.</param>
        /// <returns></returns>
        Task<CSSResponse> GetUserPermission(UserPermissionEmployeeIdDetails userPermissionEmployeeIdDetails);

        /// <summary>
        /// Creates the userPermission.
        /// </summary>
        /// <param name="userPermissionDetails">The userPermission details.</param>
        /// <returns></returns>
        Task<CSSResponse> CreateUserPermission(CreateUserPermissionDTO userPermissionDetails);

        /// <summary>
        /// Updates the userPermission.
        /// </summary>
        /// <param name="userPermissionEmployeeIdDetails">The userPermission identifier details.</param>
        /// <param name="userPermissionDetails">The userPermission details.</param>
        /// <returns></returns>
        Task<CSSResponse> UpdateUserPermission(UserPermissionEmployeeIdDetails userPermissionEmployeeIdDetails, UpdateUserPermissionDTO userPermissionDetails);

        /// <summary>
        /// Updates the userPermission's Language Preference.
        /// </summary>
        /// <param name="userPermissionEmployeeIdDetails">The userPermission identifier details.</param>
        /// <param name="userPermissionLanguagePreference">The language details.</param>
        /// <returns></returns>
        Task<CSSResponse> UpdateUserLanguagePreference(UserPermissionEmployeeIdDetails userPermissionEmployeeIdDetails, UserLanguagePreference userPermissionLanguagePreference);

        /// <summary>Reverts the userPermission.</summary>
        /// <param name="userPermissionEmployeeIdDetails">The userPermission identifier details.</param>
        /// <param name="userPermissionDetails">The userPermission details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<CSSResponse> RevertUserPermission(UserPermissionEmployeeIdDetails userPermissionEmployeeIdDetails, UpdateUserPermissionDTO userPermissionDetails);

        /// <summary>
        /// Deletes the userPermission.
        /// </summary>
        /// <param name="userPermissionEmployeeIdDetails">The userPermission identifier details.</param>
        /// <returns></returns>
        Task<CSSResponse> DeleteUserPermission(UserPermissionEmployeeIdDetails userPermissionEmployeeIdDetails);
    }
}
