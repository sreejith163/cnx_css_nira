﻿using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedule;
using Css.Api.Scheduling.Models.DTO.Request.AgentScheduleManager;
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

        /// <summary>
        /// Gets the agent schedule manager chart.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns></returns>
        [HttpGet("{date}")]
        public async Task<IActionResult> GetAgentScheduleManagerChart(DateTime dateTime)
        {
            var result = await _agentScheduleManagerService.GetAgentScheduleManagerChart(new DateDetails { Date = dateTime });
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Updates the agent schedule manger chart.
        /// </summary>
        /// <param name="agentScheduleDetails">The agent schedule details.</param>
        /// <returns></returns>
        [HttpPut("charts")]
        public async Task<IActionResult> UpdateAgentScheduleMangerChart([FromBody] UpdateAgentScheduleManagerChart agentScheduleDetails)
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