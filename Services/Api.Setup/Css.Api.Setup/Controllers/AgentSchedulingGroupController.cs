using Css.Api.Setup.Business.Interfaces;
using Css.Api.Setup.Models.DTO.Request.AgentSchedulingGroup;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Css.Api.Setup.Controllers
{
    /// <summary>Controller for handling Agent Scheduling Groups resource</summary>
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AgentSchedulingGroupsController : ControllerBase
    {
        /// <summary>The agent scheduling group service</summary>
        private readonly IAgentSchedulingGroupService _agentSchedulingGroupService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentSchedulingGroupsController"/> class.
        /// </summary>
        /// <param name="agentSchedulingGroupService">The agent scheduling group service.</param>
        public AgentSchedulingGroupsController(IAgentSchedulingGroupService agentSchedulingGroupService)
        {
            _agentSchedulingGroupService = agentSchedulingGroupService;
        }

        /// <summary>
        /// Gets the agent scheduling groups.
        /// </summary>
        /// <param name="agentSchedulingGroupQueryParameter">The agent scheduling group query parameter.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAgentSchedulingGroups([FromQuery] AgentSchedulingGroupQueryParameter agentSchedulingGroupQueryParameter)
        {
            var result = await _agentSchedulingGroupService.GetAgentSchedulingGroups(agentSchedulingGroupQueryParameter);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Gets the agent scheduling group.
        /// </summary>
        /// <param name="agentSchedulingGroupId">The agent scheduling group identifier.</param>
        /// <returns></returns>
        [HttpGet("{agentSchedulingGroupId}")]
        public async Task<IActionResult> GetAgentSchedulingGroup(int agentSchedulingGroupId)
        {
            var result = await _agentSchedulingGroupService.GetAgentSchedulingGroup(new AgentSchedulingGroupIdDetails { AgentSchedulingGroupId = agentSchedulingGroupId });
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Creates the agent scheduling group.
        /// </summary>
        /// <param name="agentSchedulingGroupDetails">The agent scheduling group details.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateAgentSchedulingGroup([FromBody] CreateAgentSchedulingGroup agentSchedulingGroupDetails)
        {
            var result = await _agentSchedulingGroupService.CreateAgentSchedulingGroup(agentSchedulingGroupDetails);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Updates the agent scheduling group.
        /// </summary>
        /// <param name="agentSchedulingGroupId">The agent scheduling group identifier.</param>
        /// <param name="agentSchedulingGroupDetails">The agent scheduling group details.</param>
        /// <returns></returns>
        [HttpPut("{agentSchedulingGroupId}")]
        public async Task<IActionResult> UpdateAgentSchedulingGroup(int agentSchedulingGroupId, [FromBody] UpdateAgentSchedulingGroup agentSchedulingGroupDetails)
        {
            var result = await _agentSchedulingGroupService.UpdateAgentSchedulingGroup(new AgentSchedulingGroupIdDetails { AgentSchedulingGroupId = agentSchedulingGroupId }, agentSchedulingGroupDetails);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Deletes the agent scheduling group.
        /// </summary>
        /// <param name="agentSchedulingGroupId">The agent scheduling group identifier.</param>
        /// <returns></returns>
        [HttpDelete("{agentSchedulingGroupId}")]
        public async Task<IActionResult> DeleteAgentSchedulingGroup(int agentSchedulingGroupId)
        {
            var result = await _agentSchedulingGroupService.DeleteAgentSchedulingGroup(new AgentSchedulingGroupIdDetails { AgentSchedulingGroupId = agentSchedulingGroupId });
            return StatusCode((int)result.Code, result.Value);
        }
    }
}


