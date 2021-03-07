using Css.Api.Core.Models.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Css.Api.Core.Models.Domain.NoSQL
{
    public class AgentScheduleRange
    {
        /// <summary>
        /// Gets or sets the agent scheduling group identifier.
        /// </summary>
        public int AgentSchedulingGroupId { get; set; }

        /// <summary>
        /// Gets or sets the date from.
        /// </summary>
        [BsonDateTimeOptions(Representation = BsonType.String, Kind = DateTimeKind.Unspecified)]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime DateFrom { get; set; }

        /// <summary>
        /// Gets or sets the date to.
        /// </summary>
        [BsonDateTimeOptions(Representation = BsonType.String, Kind = DateTimeKind.Unspecified)]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime DateTo { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public SchedulingStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the schedule charts.
        /// </summary>
        public List<AgentScheduleChart> ScheduleCharts { get; set; }

        /// <summary>
        /// Gets or sets the modified by.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the modified date.
        /// </summary>
        [BsonRepresentation(BsonType.String)]
        public DateTimeOffset CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the modified by.
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the modified date.
        /// </summary>
        [BsonRepresentation(BsonType.String)]
        public DateTimeOffset? ModifiedDate { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentScheduleRange"/> class.
        /// </summary>
        public AgentScheduleRange()
        {
            ScheduleCharts = new List<AgentScheduleChart>();
        }
    }
}
