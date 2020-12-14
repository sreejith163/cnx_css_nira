using Css.Api.Scheduling.Models.Domain;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.DTO.Response.AgentSchedule
{
    public class AgentScheduleDetailsDTO: AgentScheduleDTO
    {
        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int ClientId { get; set; }

        /// <summary>
        /// Gets or sets the client lob group identifier.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int ClientLobGroupId { get; set; }

        /// <summary>
        /// Gets or sets the skill group identifier.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int SkillGroupId { get; set; }

        /// <summary>
        /// Gets or sets the skill tag identifier.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int SkillTagId { get; set; }

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


