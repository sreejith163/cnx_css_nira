using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.DTO.Response.ActivityLog
{
    public class ActivityLogDTO
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the employee identifier.
        /// </summary>
        public string EmployeeId { get; set; }

        /// <summary>
        /// Gets or sets the time stamp.
        /// </summary>
        public DateTimeOffset TimeStamp { get; set; }

        /// <summary>
        /// Gets or sets the executed by.
        /// </summary>
        public string ExecutedBy { get; set; }

        /// <summary>
        /// Gets or sets the executed user.
        /// </summary>
        public string ExecutedUser { get; set; }

        /// <summary>
        /// Gets or sets the activity status.
        /// </summary>
        public ActivityStatus ActivityStatus { get; set; }

        /// <summary>
        /// Gets or sets the activity origin.
        /// </summary>
        public ActivityOrigin ActivityOrigin { get; set; }

        /// <summary>
        /// Gets or sets the field details.
        /// </summary>
        public List<FieldDetail> FieldDetails { get; set; }

        /// <summary>
        /// Gets or sets the scheduling field details.
        /// </summary>
        public SchedulingFieldDetails SchedulingFieldDetails { get; set; }
    }
}