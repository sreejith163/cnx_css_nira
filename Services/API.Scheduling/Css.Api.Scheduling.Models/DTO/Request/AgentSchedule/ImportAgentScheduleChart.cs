using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentSchedule
{
    public class ImportAgentScheduleChart
    {
        /// <summary>
        /// Gets or sets the employee identifier.
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// Gets or sets the charts.
        /// </summary>
        public List<ImportAgentScheduleRanges> Ranges { get; set; }

        public ImportAgentScheduleChart()
        {
            Ranges = new List<ImportAgentScheduleRanges>();
        }
    }
}