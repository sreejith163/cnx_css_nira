using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Css.Api.Core.Models.Domain.NoSQL
{
    public class AgentScheduleManagerChart
    {
        /// <summary>
        /// Gets or sets the start time
        /// </summary>
        [BsonDateTimeOptions(Representation = BsonType.String, Kind = DateTimeKind.Utc)]
        public DateTime StartDateTime { get; set; }

        /// <summary>
        /// Gets or sets the end time.
        /// </summary>
        [BsonDateTimeOptions(Representation = BsonType.String, Kind = DateTimeKind.Utc)]
        public DateTime EndDateTime { get; set; }

        /// <summary>
        /// Gets or sets the scheduling code identifier.
        /// </summary>
        public int SchedulingCodeId { get; set; }
    }
}
