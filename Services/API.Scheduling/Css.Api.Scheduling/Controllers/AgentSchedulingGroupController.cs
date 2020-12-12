using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedulingGroup;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Controllers
{
    /// <summary>
    /// Controller for handling the Time zones resource
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AgentSchedulingGroupController : ControllerBase
    {
        /// <summary>
        /// The agent scheduling group service
        /// </summary>
        private readonly IAgentSchedulingGroupService _agentSchedulingGroupService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeZonesController" /> class.
        /// </summary>
        /// <param name="agentSchedulingGroupService">The agent scheduling group service.</param>
        public AgentSchedulingGroupController(IAgentSchedulingGroupService agentSchedulingGroupService)
        {
            _agentSchedulingGroupService = agentSchedulingGroupService;
        }

        /// <summary>
        /// Gets the agent scheduling groups.
        /// </summary>
        /// <param name="agentSchedulingGroupQueryparameter">The agent scheduling group queryparameter.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAgentSchedulingGroups([FromQuery] AgentSchedulingGroupQueryparameter agentSchedulingGroupQueryparameter)
        {
            var result = await _agentSchedulingGroupService.GetAgentSchedulingGroups(agentSchedulingGroupQueryparameter);
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
    }
}

