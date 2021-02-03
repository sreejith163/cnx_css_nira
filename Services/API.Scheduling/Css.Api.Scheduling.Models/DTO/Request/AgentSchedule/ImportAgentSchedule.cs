using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Scheduling.Models.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentSchedule
{
    public class ImportAgentSchedule
    {
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
        /// Gets or sets the agent scheduling group identifier.
        /// </summary>
        public int AgentSchedulingGroupId { get; set; }

        /// <summary>
        /// Gets or sets the activity origin.
        /// </summary>
        public ActivityOrigin ActivityOrigin { get; set; }

        /// <summary>
        /// Gets or sets the modified by.
        /// </summary>
        public string ModifiedBy { get; set; }
    }
}