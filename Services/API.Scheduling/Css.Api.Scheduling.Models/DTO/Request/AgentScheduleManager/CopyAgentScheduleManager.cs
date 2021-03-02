using Css.Api.Core.Models.Enums;
using System;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentSchedule
{
    public class CopyAgentScheduleManager
    {
        /// <summary>
        /// Gets or sets the employee identifier.
        /// </summary>
        public List<int> EmployeeIds { get; set; }

        /// <summary>
        /// Gets or sets the agent scheduling group identifier.
        /// </summary>
        public int AgentSchedulingGroupId { get; set; }

        /// <summary>
        /// Gets or sets the activity origin.
        /// </summary>
        public ActivityOrigin ActivityOrigin { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        public DateTime Date { get; set; }

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