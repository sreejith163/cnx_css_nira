using Css.Api.Core.Models.DTO.Request;
using Css.Api.Core.Models.Enums;
using Css.Api.Scheduling.Models.Enums;
using System;

namespace Css.Api.Scheduling.Models.DTO.Request.ActivityLog
{
    public class ActivityLogQueryParameter : QueryStringParameters
    {
        /// <summary>Initializes a new instance of the <see cref="ActivityLogQueryParameter" /> class.</summary>
        public ActivityLogQueryParameter()
        {
            OrderBy = "TimeStamp";
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the employee identifier.
        /// </summary>
        public int? EmployeeId { get; set; }

        /// <summary>
        /// Gets or sets the executed user.
        /// </summary>
        public int? ExecutedUser { get; set; }

        /// <summary>
        /// Gets or sets the executed user.
        /// </summary>
        public string ExecutedBy { get; set; }

        /// <summary>
        /// Gets or sets the field.
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// Creates new value.
        /// </summary>
        public string NewValue { get; set; }

        /// <summary>
        /// Gets or sets the old value.
        /// </summary>
        public string OldValue { get; set; }

        /// <summary>
        /// Gets or sets the type of the activity.
        /// </summary>
        public ActivityType? ActivityType { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// Gets or sets the date from.
        /// </summary>
        public DateTime? DateFrom { get; set; }

        /// <summary>
        /// Gets or sets the date to.
        /// </summary>
        public DateTime? DateTo { get; set; }
    }
}