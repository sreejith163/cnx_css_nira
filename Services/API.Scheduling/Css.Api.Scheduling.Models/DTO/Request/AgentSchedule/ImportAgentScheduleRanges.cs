using Css.Api.Core.Models.Domain.NoSQL;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentSchedule
{
    public class ImportAgentScheduleRanges
    {
        /// Gets or sets the date from.
        /// </summary>
        [BsonDateTimeOptions(Representation = BsonType.String, Kind = DateTimeKind.Unspecified)]
        public DateTime DateFrom { get; set; }

        /// <summary>
        /// Gets or sets the date to.
        /// </summary>
        [BsonDateTimeOptions(Representation = BsonType.String, Kind = DateTimeKind.Unspecified)]
        public DateTime DateTo { get; set; }

        /// <summary>
        /// Gets or sets the charts.
        /// </summary>
        public List<AgentScheduleChart> AgentScheduleCharts { get; set; }
    }
}
