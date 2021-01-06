using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Models.DTO.Request.ActivityLog;
using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Controllers
{
    /// <summary>
    /// Controller for handling the agent admins resource
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AgentAdminsController : ControllerBase
    {
        /// <summary>The agentAdmin service</summary>
        private readonly IAgentAdminService _agentAdminService;

        /// <summary>Initializes a new instance of the <see cref="AgentAdminsController" /> class.</summary>
        /// <param name="agentAdminService">The agentAdmin service.</param>
        public AgentAdminsController(IAgentAdminService agentAdminService)
        {
            _agentAdminService = agentAdminService;
        }

        /// <summary>
        /// Gets the agent admins.
        /// </summary>
        /// <param name="agentAdminQueryParameters">The agentAdmin query parameters.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> GetAgentAdmins([FromQuery] AgentAdminQueryParameter agentAdminQueryParameters)
        {
            var result = await _agentAdminService.GetAgentAdmins(agentAdminQueryParameters);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Gets the agent admin.
        /// </summary>
        /// <param name="agentAdminId">The agentAdmin identifier.</param>
        /// <returns></returns>
        [HttpGet("{agentAdminId}")]
        public async Task<IActionResult> GetAgentAdmin(string agentAdminId)
        {
            var result = await _agentAdminService.GetAgentAdmin(new AgentAdminIdDetails { AgentAdminId = agentAdminId });
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Creates the agent admin.
        /// </summary>
        /// <param name="clientDetails">The client details.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateAgentAdmin([FromBody] CreateAgentAdmin clientDetails)
        {
            var result = await _agentAdminService.CreateAgentAdmin(clientDetails);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Updates the agent admin.
        /// </summary>
        /// <param name="agentAdminId">The agentAdmin identifier.</param>
        /// <param name="clientDetails">The client details.</param>
        /// <returns></returns>
        [HttpPut("{agentAdminId}")]
        public async Task<IActionResult> UpdateAgentAdmin(string agentAdminId, [FromBody] UpdateAgentAdmin clientDetails)
        {
            var result = await _agentAdminService.UpdateAgentAdmin(new AgentAdminIdDetails { AgentAdminId = agentAdminId }, clientDetails);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Deletes the agent admin.
        /// </summary>
        /// <param name="agentAdminId">The agentAdmin identifier.</param>
        /// <returns></returns>
        [HttpDelete("{agentAdminId}")]
        public async Task<IActionResult> DeleteAgentAdmin(string agentAdminId)
        {
            var result = await _agentAdminService.DeleteAgentAdmin(new AgentAdminIdDetails { AgentAdminId = agentAdminId });
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Creates the agent activity logs.
        /// </summary>
        /// <param name="agentAdminId">The agent admin identifier.</param>
        /// <param name="agentActivityLogDetails">The agent activity log details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpPost("{agentAdminId}/create")]
        public async Task<IActionResult> CreateAgentActivityLogs(string agentAdminId, [FromBody] CreateAgentActivityLog agentActivityLogDetails)
        {
            var result = await _agentAdminService.CreateAgentActivityLog(agentActivityLogDetails);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>Gets the agent activity logs.</summary>
        /// <param name="activityLogQueryParameter">The activity log query parameter.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpGet("logs")]
        public async Task<IActionResult> GetAgentActivityLogs([FromQuery] ActivityLogQueryParameter activityLogQueryParameter)
        {
            var result = await _agentAdminService.GetAgentActivityLogs(activityLogQueryParameter);
            return StatusCode((int)result.Code, result.Value);
        }
    }
}