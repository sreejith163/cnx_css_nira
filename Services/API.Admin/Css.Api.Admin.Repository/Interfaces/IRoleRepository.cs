using Css.Api.Core.Models.Domain;
using Css.Api.Admin.Models.Domain;
using Css.Api.Admin.Models.DTO.Request.Role;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Admin.Repository.Interfaces
{
    public interface IRoleRepository
    {
        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <param name="roleParameters">The role parameters.</param>
        /// <returns></returns>
        Task<PagedList<Entity>> GetRoles(RoleQueryParameters roleParameters);

        /// <summary>
        /// Gets the name of the roles by.
        /// </summary>
        /// <param name="roleSsoDetails">The role sso details.</param>
        /// <returns></returns>
        Task<List<int>> GetRolesByName(RoleNameDetails roleNameDetails);

        /// <summary>Gets the name of all roles by.</summary>
        /// <param name="roleSsoDetails">The role sso details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<List<int>> GetAllRolesByName(RoleNameDetails roleNameDetails);


        /// <summary>
        /// Gets the role.
        /// </summary>
        /// <param name="roleIdDetails">The role identifier details.</param>
        /// <returns></returns>
        Task<Role> GetRole(RoleIdDetails roleIdDetails);

        /// <summary>Gets all role.</summary>
        /// <param name="roleIdDetails">The role identifier details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<Role> GetAllRole(RoleIdDetails roleIdDetails);

        /// <summary>
        /// Creates the role.
        /// </summary>
        /// <param name="role">The role.</param>
        void CreateRole(Role role);

        /// <summary>
        /// Updates the role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        void UpdateRole(Role role);

        /// <summary>
        /// Deletes the role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        void DeleteRole(Role role);
    }
}
