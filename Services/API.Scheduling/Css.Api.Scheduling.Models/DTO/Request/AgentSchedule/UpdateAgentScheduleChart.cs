using Css.Api.Scheduling.Models.Domain;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentSchedule
{
    public class UpdateAgentScheduleChart : AgentScheduleChartAttributes
    {
        /// <summary>
        /// Gets or sets the agent schedule charts.
        /// </summary>
        public List<AgentScheduleChart> AgentScheduleCharts { get; set; }

        /// <summary>
        /// Gets or sets the agent schedule manager chart.
        /// </summary>
        public AgentScheduleManagerChart AgentScheduleManagerChart { get; set; }
    }
}
