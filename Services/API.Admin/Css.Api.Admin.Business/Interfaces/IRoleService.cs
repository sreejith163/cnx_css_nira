using Css.Api.Core.Models.DTO.Response;
using Css.Api.Admin.Models.DTO.Request.Role;
using System.Threading.Tasks;

namespace Css.Api.Admin.Business.Interfaces
{
    public interface IRoleService
    {
        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <param name="roleParameters">The role parameters.</param>
        /// <returns></returns>
        Task<CSSResponse> GetRoles(RoleQueryParameters roleParameters);

        /// <summary>
        /// Gets the role.
        /// </summary>
        /// <param name="roleIdDetails">The role identifier details.</param>
        /// <returns></returns>
        Task<CSSResponse> GetRole(RoleIdDetails roleIdDetails);

        /// <summary>
        /// Creates the role.
        /// </summary>
        /// <param name="roleDetails">The role details.</param>
        /// <returns></returns>
        Task<CSSResponse> CreateRole(CreateRoleDTO roleDetails);

        /// <summary>
        /// Updates the role.
        /// </summary>
        /// <param name="roleIdDetails">The role identifier details.</param>
        /// <param name="roleDetails">The role details.</param>
        /// <returns></returns>
        Task<CSSResponse> UpdateRole(RoleIdDetails roleIdDetails, UpdateRoleDTO roleDetails);

        /// <summary>Reverts the role.</summary>
        /// <param name="roleIdDetails">The role identifier details.</param>
        /// <param name="roleDetails">The role details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<CSSResponse> RevertRole(RoleIdDetails roleIdDetails, UpdateRoleDTO roleDetails);

        /// <summary>
        /// Deletes the role.
        /// </summary>
        /// <param name="roleIdDetails">The role identifier details.</param>
        /// <returns></returns>
        Task<CSSResponse> DeleteRole(RoleIdDetails roleIdDetails);
    }
}