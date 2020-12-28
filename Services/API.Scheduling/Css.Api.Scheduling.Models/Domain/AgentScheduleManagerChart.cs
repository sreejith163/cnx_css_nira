using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.Domain
{
    public class AgentScheduleManagerChart
    {
        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        [BsonRepresentation(BsonType.String)]
        public DateTimeOffset Date { get; set; }

        /// <summary>
        /// Gets or sets the charts.
        /// </summary>
        public List<ScheduleChart> Charts { get; set; }
    }
}
