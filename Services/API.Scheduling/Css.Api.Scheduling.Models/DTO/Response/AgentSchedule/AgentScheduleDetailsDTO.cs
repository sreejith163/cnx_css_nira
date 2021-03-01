using Css.Api.Core.Models.Domain.NoSQL;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.DTO.Response.AgentSchedule
{
    public class AgentScheduleDetailsDTO: AgentScheduleDTO
    {
        /// <summary>
        /// Gets or sets the ranges.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public new List<AgentScheduleRange> Ranges { get; set; }
    }
}


