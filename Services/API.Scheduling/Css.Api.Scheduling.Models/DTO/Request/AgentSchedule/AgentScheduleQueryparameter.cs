using Css.Api.Core.Models.DTO.Request;
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
        /// Gets or sets from date.
        /// </summary>
        public DateTime? FromDate { get; set; }

        /// <summary>
        /// Gets or sets the agent scheduling group identifier.
        /// </summary>
        public int? AgentSchedulingGroupId { get; set; }

        /// <summary>
        /// Gets or sets the employee ids.
        /// </summary>
        public List<int> EmployeeIds { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [skip page size].
        /// </summary>
        public bool SkipPageSize { get; set; }
    }
}
