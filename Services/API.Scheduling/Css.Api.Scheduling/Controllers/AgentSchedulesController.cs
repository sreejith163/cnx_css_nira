using Css.Api.Scheduling.Business.Interfaces;
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
        /// Determines whether [is agent schedule range exist] [the specified agent schedule identifier].
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        /// <param name="dateRangeDetails">The date range details.</param>
        /// <returns></returns>
        [HttpGet("{agentScheduleId}/exists")]
        public async Task<IActionResult> IsAgentScheduleRangeExist(string agentScheduleId, [FromQuery] DateRange dateRangeDetails)
        {
            var result = await _agentScheduleService.IsAgentScheduleRangeExist(new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId }, dateRangeDetails);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Gets the agent schedule.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        /// <param name="agentScheduleChartQueryparameter">The agent schedule chart queryparameter.</param>
        /// <returns></returns>
        [HttpGet("{agentScheduleId}/charts")]
        public async Task<IActionResult> GetAgentScheduleCharts(string agentScheduleId, [FromQuery] AgentScheduleChartQueryparameter agentScheduleChartQueryparameter)
        {
            var result = await _agentScheduleService.GetAgentScheduleCharts(new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId }, agentScheduleChartQueryparameter);
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
        /// <param name="agentScheduleDetails">The agent schedule details.</param>
        /// <returns></returns>
        [HttpPut("import")]
        public async Task<IActionResult> ImportAgentScheduleChart([FromBody] ImportAgentSchedule agentScheduleDetails)
        {
            var result = await _agentScheduleService.ImportAgentScheduleChart(agentScheduleDetails);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Copies the agent schedule chart.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        /// <param name="agentScheduleDetails">The agent schedule details.</param>
        /// <returns></returns>
        [HttpPut("{agentScheduleId}/copy")]
        public async Task<IActionResult> CopyAgentScheduleChart(string agentScheduleId, [FromBody] CopyAgentSchedule agentScheduleDetails)
        {
            var result = await _agentScheduleService.CopyAgentScheduleChart(new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId }, agentScheduleDetails);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Updates the agent schedule range.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        /// <param name="dateRangeDetails">The date range details.</param>
        /// <returns></returns>
        [HttpPut("{agentScheduleId}/range")]
        public async Task<IActionResult> UpdateAgentScheduleRange(string agentScheduleId, [FromBody] UpdateAgentScheduleDateRange dateRangeDetails)
        {
            var result = await _agentScheduleService.UpdateAgentScheduleRange(new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId }, dateRangeDetails);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Deletes the agent schedule range.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        /// <param name="dateRangeDetails">The date range details.</param>
        /// <returns></returns>
        [HttpDelete("{agentScheduleId}/range")]
        public async Task<IActionResult> DeleteAgentScheduleRange(string agentScheduleId, [FromQuery] DateRange dateRangeDetails)
        {
            var result = await _agentScheduleService.DeleteAgentScheduleRange(new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId }, dateRangeDetails);
            return StatusCode((int)result.Code, result.Value);
        }
    }
}