using Css.Api.Core.Models.DTO.Request;
using Css.Api.Core.Models.Enums;
using System;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentSchedule
{
    public class AgentScheduleQueryparameter : QueryStringParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AgentScheduleQueryparameter"/> class.
        /// </summary>
        public AgentScheduleQueryparameter()
        {
            OrderBy = "CreatedDate";
            EmployeeIds = new List<int>();
        }

        /// <summary>
        /// Gets or sets the agent scheduling group identifier.
        /// </summary>
        public int? AgentSchedulingGroupId { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public SchedulingStatus? Status { get; set; }

        /// <summary>
        /// Gets or sets the date from.
        /// </summary>
        public DateTime? DateFrom { get; set; }

        /// <summary>
        /// Gets or sets the date to.
        /// </summary>
        public DateTime? DateTo { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [exclude conflict schedule].
        /// </summary>
        public bool? ExcludeConflictSchedule { get; set; }

        /// <summary>
        /// Gets or sets the employee ids.
        /// </summary>
        public List<int> EmployeeIds { get; set; }
    }
}
