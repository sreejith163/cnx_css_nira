using Css.Api.Core.Models.Domain.NoSQL;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentScheduleManager
{
    public class AgentScheduleManagerChartDTO
    {
        /// <summary>
        /// Gets or sets the employee identifier.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int EmployeeId { get; set; }

        /// <summary>
        /// Gets or sets the charts.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ScheduleChart> Charts { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Models.Domain.NoSQL.AgentScheduleManager"/> class.
        /// </summary>
        public AgentScheduleManagerChartDTO()
        {
            Charts = new List<ScheduleChart>();
        }
    }
}
