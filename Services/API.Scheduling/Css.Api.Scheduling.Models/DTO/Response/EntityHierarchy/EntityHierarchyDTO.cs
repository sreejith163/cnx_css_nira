using Newtonsoft.Json;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.DTO.Response.EntityHierarchy
{
    public class EntityHierarchyDTO
    {
        /// <summary>Gets or sets the client.</summary>
        /// <value>The client.</value>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ClientDTO Client { get; set; }

        /// <summary>Gets or sets the agent scheduling groups.</summary>
        /// <value>The agent scheduling groups.</value>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<AgentSchedulingGroupDTO> AgentSchedulingGroups { get; set; }
    }
}
