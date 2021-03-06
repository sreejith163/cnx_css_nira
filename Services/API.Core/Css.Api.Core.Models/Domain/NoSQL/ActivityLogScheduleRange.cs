﻿using Css.Api.Core.Models.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Css.Api.Core.Models.Domain.NoSQL
{
    public class ActivityLogScheduleRange
    {
        /// <summary>
        /// Gets or sets the agent scheduling group identifier.
        /// </summary>
        public int AgentSchedulingGroupId { get; set; }

        /// <summary>
        /// Gets or sets the date from.
        /// </summary>
        [BsonRepresentation(BsonType.String)]
        public DateTime DateFrom { get; set; }

        /// <summary>
        /// Gets or sets the date to.
        /// </summary>
        [BsonRepresentation(BsonType.String)]
        public DateTime DateTo { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public SchedulingStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the agent schedule charts.
        /// </summary>
        public List<AgentScheduleChart> ScheduleCharts { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentScheduleRange"/> class.
        /// </summary>
        public ActivityLogScheduleRange()
        {
            ScheduleCharts = new List<AgentScheduleChart>();
        }
    }
}
