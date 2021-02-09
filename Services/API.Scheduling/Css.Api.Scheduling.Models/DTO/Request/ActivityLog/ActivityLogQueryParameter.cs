using Css.Api.Core.Models.DTO.Request;
using Css.Api.Scheduling.Models.Enums;

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
        /// Gets or sets the employee identifier.
        /// </summary>
        public int? EmployeeId { get; set; }

        /// <summary>
        /// Gets or sets the type of the activity.
        /// </summary>
        public ActivityType? ActivityType { get; set; }
    }
}