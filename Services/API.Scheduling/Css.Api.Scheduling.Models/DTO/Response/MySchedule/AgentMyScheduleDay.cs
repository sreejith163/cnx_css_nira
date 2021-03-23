using Css.Api.Core.Models.Domain.NoSQL;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.DTO.Response.MySchedule
{
    public class AgentMyScheduleDay
    {
        /// <summary>
        /// Gets or sets the agent schedule identifier.
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AgentScheduleManagerId { get; set; }

        /// <summary>
        /// Gets or sets the day.
        /// </summary>
        public int Day { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        [BsonDateTimeOptions(Representation = BsonType.Document, Kind = DateTimeKind.Utc)]
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the charts.
        /// </summary>
        public List<AgentScheduleManagerChart> Charts { get; set; }

        /// <summary>Gets or sets the first start time.</summary>
        /// <value>The first start time.</value>
        public string FirstStartTime { get; set; }

        /// <summary>Gets or sets the last start time.</summary>
        /// <value>The last start time.</value>
        public string LastEndTime { get; set; }
    }
}
