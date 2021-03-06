using Newtonsoft.Json;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentScheduleManager
{
    public class UpdateScheduleManagerChart
    {
        /// <summary>
        /// Gets or sets the employee identifier.
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// Gets or sets the agent schedule manager charts.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<AgentScheduleManagerChartDTO> AgentScheduleManagerCharts { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Models.Domain.NoSQL.AgentScheduleManager"/> class.
        /// </summary>
        public UpdateScheduleManagerChart()
        {
            AgentScheduleManagerCharts = new List<AgentScheduleManagerChartDTO>();
        }
    }
}
