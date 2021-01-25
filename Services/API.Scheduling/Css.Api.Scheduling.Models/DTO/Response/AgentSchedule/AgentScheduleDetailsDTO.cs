using Css.Api.Core.Models.Domain.NoSQL;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.DTO.Response.AgentSchedule
{
    public class AgentScheduleDetailsDTO: AgentScheduleDTO
    {
        /// <summary>
        /// Gets or sets the charts.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<AgentScheduleChart> AgentScheduleCharts { get; set; }

        /// <summary>
        /// Gets or sets the agent schedule manager.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<AgentScheduleManagerChart> AgentScheduleManagerCharts { get; set; }
    }
}


