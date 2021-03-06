﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Css.Api.Core.Models.Domain.NoSQL
{
    public class ActivityLogScheduleManager
    {
        /// <summary>
        /// Gets or sets the agent scheduling group identifier.
        /// </summary>
        public int AgentSchedulingGroupId { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        [BsonRepresentation(BsonType.String)]
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the charts.
        /// </summary>
        public List<AgentScheduleManagerChart> Charts { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentScheduleManager"/> class.
        /// </summary>
        public ActivityLogScheduleManager()
        {
            Charts = new List<AgentScheduleManagerChart>();
        }
    }
}
