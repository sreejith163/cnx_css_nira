using Css.Api.Core.Models.Enums;
using System;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentSchedule
{
    public class MultipleCopyAgentScheduleRequest
    {
        /// <summary>
        /// Gets or sets the employee identifier.
        /// </summary>
        public List<string> EmployeeIds { get; set; }

        /// <summary>
        /// Gets or sets the selected date ranges to be imported to.
        /// </summary>
        public List<DateRange> SelectedDateRanges { get; set; }

        /// <summary>
        /// Gets or sets the agent scheduling group identifier.
        /// </summary>
        public int AgentSchedulingGroupId { get; set; }

        /// <summary>
        /// Gets or sets the activity origin.
        /// </summary>
        public ActivityOrigin ActivityOrigin { get; set; }

        /// <summary>
        /// Gets or sets the date from.
        /// </summary>
        public DateTime DateFrom { get; set; }

        /// <summary>
        /// Gets or sets the date to.
        /// </summary>
        public DateTime DateTo { get; set; }

        /// <summary>
        /// Gets or sets the modified user.
        /// </summary>
        public string ModifiedUser { get; set; }

        /// <summary>
        /// Gets or sets the modified by.
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyAgentSchedule"/> class.
        /// </summary>
        public MultipleCopyAgentScheduleRequest()
        {
            EmployeeIds = new List<string>();
            SelectedDateRanges = new List<DateRange>();
        }
    }
}