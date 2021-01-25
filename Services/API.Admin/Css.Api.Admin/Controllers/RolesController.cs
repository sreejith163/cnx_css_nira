using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Css.Api.Admin.Models.DTO.Request.Role;
using Css.Api.Admin.Business.Interfaces;

namespace Css.Api.Admin.Controllers
{

    /// <summary>Controller for handling User Role resource</summary>
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {

        /// <summary>
        /// The role service
        /// </summary>
        private readonly IRoleService _roleService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RolesController"/> class.
        /// </summary>
        /// <param name="roleService">The role service.</param>
        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <param name="roleParameters">The role parameters.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetRoles([FromQuery] RoleQueryParameters roleParameters)
        {
            var result = await _roleService.GetRoles(roleParameters);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Gets the role.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        [HttpGet("{roleId}")]
        public async Task<IActionResult> GetRole(int roleId)
        {
            var result = await _roleService.GetRole(new RoleIdDetails { RoleId = roleId });
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Creates the role.
        /// </summary>
        /// <param name="roleDetails">The role details.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDTO roleDetails)
        {
            var result = await _roleService.CreateRole(roleDetails);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Updates the role.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <param name="roleDetails">The role details.</param>
        /// <returns></returns>
        [HttpPut("{roleId}")]
        public async Task<IActionResult> UpdateRole(int roleId, [FromBody] UpdateRoleDTO roleDetails)
        {
            var result = await _roleService.UpdateRole(new RoleIdDetails { RoleId = roleId }, roleDetails);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Deletes the role.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        [HttpDelete("{roleId}")]
        public async Task<IActionResult> DeleteRole(int roleId)
        {
            var result = await _roleService.DeleteRole(new RoleIdDetails { RoleId = roleId });
            return StatusCode((int)result.Code, result.Value);
        }

    }
}
