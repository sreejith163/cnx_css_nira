using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedule;
using Css.Api.Scheduling.Models.DTO.Request.AgentScheduleManager;
using Css.Api.Scheduling.Models.DTO.Request.MySchedule;
using Microsoft.AspNetCore.Mvc;
using System;
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
    public class AgentScheduleManagersController : ControllerBase
    {
        /// <summary>
        /// The agentAdmin service
        /// </summary>
        private readonly IAgentScheduleManagerService _agentScheduleManagerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentScheduleManagersController" /> class.
        /// </summary>
        /// <param name="agentScheduleManagerService">The agent schedule manager service.</param>
        public AgentScheduleManagersController(IAgentScheduleManagerService agentScheduleManagerService)
        {
            _agentScheduleManagerService = agentScheduleManagerService;
        }

        /// <summary>
        /// Gets the agent schedule manager charts.
        /// </summary>
        /// <param name="agentScheduleManagerChartQueryparameter">The agent schedule manager chart queryparameter.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAgentScheduleManagerCharts([FromQuery] AgentScheduleManagerChartQueryparameter agentScheduleManagerChartQueryparameter)
        {
            var result = await _agentScheduleManagerService.GetAgentScheduleManagerCharts(agentScheduleManagerChartQueryparameter);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>Gets the agent my schedule.</summary>
        /// <param name="agentEmployeeId">The agent employee identifier.</param>
        /// <param name="myScheduleQueryParameter">My schedule query parameter.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpGet("{agentEmployeeId}/myschedule")]
        public async Task<IActionResult> GetAgentMySchedule(int agentEmployeeId, [FromQuery] MyScheduleQueryParameter myScheduleQueryParameter)
        {
            var result = await _agentScheduleManagerService.GetAgentMySchedule(new EmployeeIdDetails { Id = agentEmployeeId }, myScheduleQueryParameter);
            return StatusCode((int)result.Code, result.Value);
        }


 

        /// <summary>
        /// Updates the agent schedule manger chart.
        /// </summary>
        /// <param name="agentScheduleDetails">The agent schedule details.</param>
        /// <returns></returns>
        [HttpPut("charts")]
        public async Task<IActionResult> UpdateAgentScheduleMangerChart([FromBody] UpdateAgentScheduleManager agentScheduleDetails)
        {
            var result = await _agentScheduleManagerService.UpdateAgentScheduleMangerChart(agentScheduleDetails);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Copies the agent schedule.
        /// </summary>
        /// <param name="employeeId">The employee identifier.</param>
        /// <param name="agentScheduleDetails">The agent schedule details.</param>
        /// <returns></returns>
        [HttpPut("{employeeId}/copy")]
        public async Task<IActionResult> CopyAgentScheduleManagerChart(int employeeId, [FromBody] CopyAgentScheduleManager agentScheduleDetails)
        {
            var result = await _agentScheduleManagerService.CopyAgentScheduleManagerChart(new EmployeeIdDetails { Id = employeeId }, agentScheduleDetails);
            return StatusCode((int)result.Code, result.Value);
        }
    }
}