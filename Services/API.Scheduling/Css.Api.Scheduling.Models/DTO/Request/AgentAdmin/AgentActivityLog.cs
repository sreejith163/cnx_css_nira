using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.Enums;
using System;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentAdmin
{
    public class AgentActivityLog
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
        /// Gets or sets the field details.
        /// </summary>
        public List<FieldDetail> FieldDetails { get; set; }

        /// <summary>
        /// Gets or sets the activity status.
        /// </summary>
        public ActivityStatus ActivityStatus { get; set; }

        /// <summary>
        /// Gets or sets the activity origin.
        /// </summary>
        public ActivityOrigin ActivityOrigin { get; set; }
    }
}

