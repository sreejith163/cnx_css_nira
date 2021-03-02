using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Css.Api.Core.Models.Domain.NoSQL
{
    [BsonCollection("agent_schedule")]
    public class AgentSchedule : BaseDocument
    {
        /// <summary>
        /// Gets or sets the employee identifier.
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the active scheduling group identifier.
        /// </summary>
        public int ActiveAgentSchedulingGroupId { get; set; }

        /// <summary>
        /// Gets or sets the ranges.
        /// </summary>
        public List<AgentScheduleRange> Ranges { get; set; }

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
        /// Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentSchedule"/> class.
        /// </summary>
        public AgentSchedule()
        {
            Ranges = new List<AgentScheduleRange>();
        }
    }
}
