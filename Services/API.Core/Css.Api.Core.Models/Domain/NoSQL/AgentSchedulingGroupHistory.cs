using Css.Api.Core.Models.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Css.Api.Core.Models.Domain.NoSQL
{
    [BsonCollection("agent_schedule_group_history")]
    public class AgentSchedulingGroupHistory : BaseDocument
    {
        /// <summary>
        /// Gets or sets the agent scheduling group identifier.
        /// </summary>
        public int AgentSchedulingGroupId { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the activity origin.
        /// </summary>
        public ActivityOrigin ActivityOrigin { get; set; }

        /// <summary>
        /// Gets or sets the created by.
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
    }
}
