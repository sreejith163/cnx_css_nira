using Css.Api.Core.Models.Enums;
using Css.Api.Scheduling.Models.Enums;
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
        [BsonDateTimeOptions(Representation = BsonType.String, Kind = DateTimeKind.Unspecified)]
        public DateTime DateFrom { get; set; }

        /// <summary>
        /// Gets or sets the date to.
        /// </summary>
        [BsonDateTimeOptions(Representation = BsonType.String, Kind = DateTimeKind.Unspecified)]
        public DateTime DateTo { get; set; }

        /// <summary>
        /// Gets or sets the activity origin.
        /// </summary>
        public ActivityOrigin ActivityOrigin { get; set; }

        /// <summary>
        /// Gets or sets the modified user.
        /// </summary>
        public int ModifiedUser { get; set; }

        /// <summary>
        /// Gets or sets the modified by.
        /// </summary>
        public string ModifiedBy { get; set; }
    }
}