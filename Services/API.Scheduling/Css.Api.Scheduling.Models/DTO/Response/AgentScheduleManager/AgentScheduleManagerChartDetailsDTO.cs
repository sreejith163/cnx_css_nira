using Css.Api.Core.Models.Domain.NoSQL;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.DTO.Response.AgentScheduleManager
{
    public class AgentScheduleManagerChartDetailsDTO
    {
        /// <summary>
        /// Gets or sets the agent schedule identifier.
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the employee identifier.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string EmployeeId { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the current agent scheduling group identifier.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? AgentSchedulingGroupId { get; set; }
        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? Date { get; set; }

        /// <summary>
        /// Gets or sets the charts.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<AgentScheduleManagerChart> Charts { get; set; }

        public int ChartsCount { get; set; }

        /// <summary>
        /// Gets or sets the charts.
        /// </summary>
        public DateTime ChartsStartDateTime { get; set; }
    }
}


