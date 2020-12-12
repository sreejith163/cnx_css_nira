using Css.Api.Scheduling.Models.Enums;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentAdmin
{
    public class CopyAgentSchedule
    {
        /// <summary>
        /// Gets or sets the employee identifier.
        /// </summary>
        public List<string> EmployeeIds { get; set; }

        /// <summary>
        /// Gets or sets the type of the agent schedule.
        /// </summary>
        public AgentScheduleType AgentScheduleType { get; set; }

        /// <summary>
        /// Gets or sets the modified by.
        /// </summary>
        public string ModifiedBy { get; set; }
    }
}