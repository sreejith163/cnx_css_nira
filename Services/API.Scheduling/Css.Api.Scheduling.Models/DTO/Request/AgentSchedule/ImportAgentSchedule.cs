using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentSchedule
{
    public class ImportAgentSchedule
    {
        /// <summary>
        /// Gets or sets the charts.
        /// </summary>
        public List<ImportAgentScheduleChart> ImportAgentScheduleCharts { get; set; }

        /// <summary>
        /// Gets or sets the modified by.
        /// </summary>
        public string ModifiedBy { get; set; }
    }
}