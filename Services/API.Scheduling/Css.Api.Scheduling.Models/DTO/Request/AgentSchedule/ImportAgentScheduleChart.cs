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
        public List<ImportAgentScheduleRange> Ranges { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportAgentScheduleChart"/> class.
        /// </summary>
        public ImportAgentScheduleChart()
        {
            Ranges = new List<ImportAgentScheduleRange>();
        }
    }
}