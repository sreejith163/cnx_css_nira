using Css.Api.Core.Models.Domain.NoSQL;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.Domain
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
        public List<ScheduleChart> Charts { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentScheduleManagerChart"/> class.
        /// </summary>
        public ActivityLogScheduleManager()
        {
            Charts = new List<ScheduleChart>();
        }
    }
}
