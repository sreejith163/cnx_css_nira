using Css.Api.Scheduling.Models.Domain;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentSchedule
{
    public class ImportAgentScheduleChart : AgentScheduleChartAttributes
    {
        /// <summary>
        /// Gets or sets the date from.
        /// </summary>
        [BsonRepresentation(BsonType.String)]
        public DateTimeOffset? DateFrom { get; set; }

        /// <summary>
        /// Gets or sets the date to.
        /// </summary>
        [BsonRepresentation(BsonType.String)]
        public DateTimeOffset? DateTo { get; set; }

        /// <summary>
        /// Gets or sets the charts.
        /// </summary>
        public List<AgentScheduleChart> AgentScheduleCharts { get; set; }

        /// <summary>
        /// Gets or sets the agent schedule manager.
        /// </summary>
        public List<AgentScheduleManagerChart> AgentScheduleManagerCharts { get; set; }
    }
}