using Css.Api.Core.Models.Domain.NoSQL;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.DTO.Response.AgentSchedule
{
    public class AgentScheduleChartDetailsDTO
    {
        /// <summary>
        /// Gets or sets the agent schedule identifier.
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the charts.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<AgentScheduleChart> AgentScheduleCharts { get; set; }
    }
}


