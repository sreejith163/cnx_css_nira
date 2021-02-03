using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.DTO.Response.MySchedule
{
    public class AgentMyScheduleDetailsDTO
    {
        /// <summary>
        /// Gets or sets the agent schedule identifier.
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        /// <summary>Gets or sets the agent my schedules.</summary>
        /// <value>The agent my schedules.</value>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<AgentMyScheduleDay> AgentMySchedules { get; set; }
    }
}
