using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedule;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Controllers
{
    /// <summary>
    /// Controller for handling the agent schedules resource
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AgentSchedulesController : ControllerBase
    {
        /// <summary>
        /// The agentAdmin service
        /// </summary>
        private readonly IAgentScheduleService _agentScheduleService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentSchedulesController"/> class.
        /// </summary>
        /// <param name="aagentScheduleService">The aagent schedule service.</param>
        public AgentSchedulesController(IAgentScheduleService aagentScheduleService)
        {
            _agentScheduleService = aagentScheduleService;
        }

        /// <summary>
        /// Gets the agent schedules.
        /// </summary>
        /// <param name="agentScheduleQueryparameter">The agent schedule queryparameter.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAgentSchedules([FromQuery] AgentScheduleQueryparameter agentScheduleQueryparameter)
        {
            var result = await _agentScheduleService.GetAgentSchedules(agentScheduleQueryparameter);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Gets the agent schedule.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        /// <returns></returns>
        [HttpGet("{agentScheduleId}")]
        public async Task<IActionResult> GetAgentSchedule(string agentScheduleId)
        {
            var result = await _agentScheduleService.GetAgentSchedule(new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId });
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Updates the agent schedule.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        /// <param name="agentScheduleDetails">The agent schedule details.</param>
        /// <returns></returns>
        [HttpPut("{agentScheduleId}")]
        public async Task<IActionResult> UpdateAgentSchedule(string agentScheduleId, [FromBody] UpdateAgentSchedule agentScheduleDetails)
        {
            var result = await _agentScheduleService.UpdateAgentSchedule(new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId }, agentScheduleDetails);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Updates the agent schedule chart.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        /// <param name="agentScheduleDetails">The agent schedule details.</param>
        /// <returns></returns>
        [HttpPut("{agentScheduleId}/charts")]
        public async Task<IActionResult> UpdateAgentScheduleChart(string agentScheduleId, [FromBody] UpdateAgentScheduleChart agentScheduleDetails)
        {
            var result = await _agentScheduleService.UpdateAgentScheduleChart(new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId }, agentScheduleDetails);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Updates the agent schedule chart.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        /// <param name="agentScheduleDetails">The agent schedule details.</param>
        /// <returns></returns>
        [HttpPut("{agentScheduleId}/import")]
        public async Task<IActionResult> ImportAgentScheduleChart(string agentScheduleId, [FromBody] ImportAgentScheduleChart agentScheduleDetails)
        {
            var result = await _agentScheduleService.ImportAgentScheduleChart(new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId }, agentScheduleDetails);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Copies the agent schedule.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        /// <param name="agentScheduleDetails">The agent schedule details.</param>
        /// <returns></returns>
        [HttpPut("{agentScheduleId}/copy")]
        public async Task<IActionResult> CopyAgentSchedule(string agentScheduleId, [FromBody] CopyAgentSchedule agentScheduleDetails)
        {
            var result = await _agentScheduleService.CopyAgentSchedule(new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId }, agentScheduleDetails);
            return StatusCode((int)result.Code, result.Value);
        }
    }
}