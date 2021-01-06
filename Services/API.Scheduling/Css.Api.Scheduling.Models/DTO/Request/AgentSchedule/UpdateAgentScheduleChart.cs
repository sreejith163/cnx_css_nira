using Css.Api.Scheduling.Models.Domain;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentSchedule
{
    public class UpdateAgentScheduleChart
    {
        /// <summary>
        /// Gets or sets the agent schedule charts.
        /// </summary>
        public List<AgentScheduleChart> AgentScheduleCharts { get; set; }

        /// <summary>
        /// Gets or sets the modified by.
        /// </summary>
        public string ModifiedBy { get; set; }
    }
}
