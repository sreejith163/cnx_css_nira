using Css.Api.Core.Models.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentSchedule
{
    public class UpdateAgentSchedule
    {
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public SchedulingStatus Status { get; set; }

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
        /// Gets or sets the modified by.
        /// </summary>
        public string ModifiedBy { get; set; }
    }
}