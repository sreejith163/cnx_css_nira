using Css.Api.Core.Models.Domain;
using Css.Api.Admin.Models.Domain;
using Css.Api.Admin.Models.DTO.Request.UserPermission;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Admin.Repository.Interfaces
{
    public interface IUserPermissionRepository
    {
        /// <summary>
        /// Gets the Agents.
        /// </summary>
        /// <param name="userPermissionParameters">The userPermission parameters.</param>
        /// <returns></returns>
        Task<PagedList<Entity>> GetUserPermissions(UserPermissionQueryParameters userPermissionParameters);

        /// <summary>
        /// Gets the name of the Agents by.
        /// </summary>
        /// <param name="userPermissionSsoDetails">The userPermission sso details.</param>
        /// <returns></returns>
        Task<List<int>> GetUserPermissionsBySso(UserPermissionSsoDetails userPermissionSsoDetails);

        /// <summary>Gets the name of all userPermissions by.</summary>
        /// <param name="userPermissionSsoDetails">The userPermission sso details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<List<int>> GetAllUserPermissionsBySso(UserPermissionSsoDetails userPermissionSsoDetails);

        /// <summary>
        /// Gets the name of the Agents by.
        /// </summary>
        /// <param name="userPermissionEmployeeIdDetails">The userPermission employee id details.</param>
        /// <returns></returns>
        Task<List<int>> GetUserPermissionsByEmployeeId(UserPermissionEmployeeIdDetails userPermissionEmployeeIdDetails);

        /// <summary>Gets the name of all userPermissions by.</summary>
        /// <param name="userPermissionEmployeeIdDetails">The userPermission employee id details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<List<int>> GetAllUserPermissionsByEmployeeId(UserPermissionEmployeeIdDetails userPermissionEmployeeIdDetails);

        /// <summary>
        /// Gets the userPermission.
        /// </summary>
        /// <param name="userPermissionIdDetails">The userPermission identifier details.</param>
        /// <returns></returns>
        Task<UserPermission> GetUserPermission(UserPermissionEmployeeIdDetails userPermissionEmployeeIdDetails);

        /// <summary>Gets all userPermission.</summary>
        /// <param name="userPermissionIdDetails">The userPermission identifier details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<UserPermission> GetAllUserPermission(UserPermissionEmployeeIdDetails userPermissionEmployeeIdDetails);

        /// <summary>
        /// Creates the userPermission.
        /// </summary>
        /// <param name="userPermission">The userPermission.</param>
        void CreateUserPermission(UserPermission userPermission);

        /// <summary>
        /// Updates the userPermission.
        /// </summary>
        /// <param name="userPermission">The userPermission.</param>
        /// <returns></returns>
        void UpdateUserPermission(UserPermission userPermission);

        /// <summary>
        /// Deletes the userPermission.
        /// </summary>
        /// <param name="userPermission">The userPermission.</param>
        /// <returns></returns>
        void DeleteUserPermission(UserPermission userPermission);
    }
}
