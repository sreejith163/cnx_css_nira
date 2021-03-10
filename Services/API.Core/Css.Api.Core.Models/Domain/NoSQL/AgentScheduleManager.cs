using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Css.Api.Core.Models.Domain.NoSQL
{
    [BsonCollection("agent_schedule_manager")]
    public class AgentScheduleManager : BaseDocument
    {
        /// <summary>
        /// Gets or sets the employee identifier.
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// Gets or sets the agent scheduling group identifier.
        /// </summary>
        public int AgentSchedulingGroupId { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        [BsonDateTimeOptions(Representation = BsonType.String, Kind = DateTimeKind.Unspecified)]
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the charts.
        /// </summary>
        public List<AgentScheduleManagerChart> Charts { get; set; }

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
        /// Initializes a new instance of the <see cref="AgentScheduleManager"/> class.
        /// </summary>
        public AgentScheduleManager()
        {
            Charts = new List<AgentScheduleManagerChart>();
        }
    }
}
