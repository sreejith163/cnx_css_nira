using Css.Api.Admin.Business.Interfaces;
using Css.Api.Admin.Models.DTO.Request.UserPermission;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Css.Api.Admin.Controllers
{
    /// <summary>Controller for handling Agent resource</summary>
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserPermissionsController : ControllerBase
    {

        /// <summary>
        /// The agent service
        /// </summary>
        private readonly IUserPermissionService _agentService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserPermissionsController"/> class.
        /// </summary>
        /// <param name="agentService">The agent service.</param>
        public UserPermissionsController(IUserPermissionService agentService)
        {
            _agentService = agentService;
        }

        /// <summary>
        /// Gets the agents.
        /// </summary>
        /// <param name="agentParameters">The agent parameters.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAgents([FromQuery] UserPermissionQueryParameters agentParameters)
        {
            var result = await _agentService.GetUserPermissions(agentParameters);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Gets the agent.
        /// </summary>
        /// <param name="employeeId">The agent identifier.</param>
        /// <returns></returns>
        [HttpGet("{employeeId}")]
        public async Task<IActionResult> GetAgent(string employeeId)
        {
            var result = await _agentService.GetUserPermission(new UserPermissionEmployeeIdDetails { EmployeeId = employeeId });
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Creates the agent.
        /// </summary>
        /// <param name="agentDetails">The agent details.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateAgent([FromBody] CreateUserPermissionDTO agentDetails)
        {
            var result = await _agentService.CreateUserPermission(agentDetails);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Updates the agent.
        /// </summary>
        /// <param name="employeeId">The agent identifier.</param>
        /// <param name="agentDetails">The agent details.</param>
        /// <returns></returns>
        [HttpPut("{employeeId}")]
        public async Task<IActionResult> UpdateAgent(string employeeId, [FromBody] UpdateUserPermissionDTO agentDetails)
        {
            var result = await _agentService.UpdateUserPermission(new UserPermissionEmployeeIdDetails { EmployeeId = employeeId }, agentDetails);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Updates the agent.
        /// </summary>
        /// <param name="employeeId">The agent identifier.</param>
        /// <param name="languagePreferenceDetails">The language details.</param>
        /// <returns></returns>
        [HttpPut("{employeeId}/language")]
        public async Task<IActionResult> UpdateAgentLanguagePreference(string employeeId, [FromBody] UserLanguagePreference languagePreferenceDetails)
        {
            var result = await _agentService.UpdateUserLanguagePreference(new UserPermissionEmployeeIdDetails { EmployeeId = employeeId }, languagePreferenceDetails);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Deletes the agent.
        /// </summary>
        /// <param name="employeeId">The agent identifier.</param>
        /// <returns></returns>
        [HttpDelete("{employeeId}")]
        public async Task<IActionResult> DeleteAgent(string employeeId)
        {
            var result = await _agentService.DeleteUserPermission(new UserPermissionEmployeeIdDetails { EmployeeId = employeeId });
            return StatusCode((int)result.Code, result.Value);
        }

    }
}


