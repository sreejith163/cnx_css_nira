using Css.Api.Core.Models.Domain;
using Css.Api.Core.Models.Enums;
using Css.Api.Scheduling.Models.Enums;
using System;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.Domain
{
    [BsonCollection("activity_log")]
    public class ActivityLog : BaseDocument
    {

        /// <summary>
        /// Gets or sets the employee identifier.
        /// </summary>
        public int EmployeeId { get; set; }

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
        public int ExecutedUser { get; set; }

        /// <summary>
        /// Gets or sets the activity status.
        /// </summary>
        public ActivityStatus ActivityStatus { get; set; }

        /// <summary>
        /// Gets or sets the activity origin.
        /// </summary>
        public ActivityOrigin ActivityOrigin { get; set; }

        /// <summary>
        /// Gets or sets the type of the activity.
        /// </summary>
        public ActivityType ActivityType { get; set; }

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
