using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.Enums;
using System;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentAdmin
{
    public class UpdateAgentSchedule
    {
        /// <summary>
        /// Gets or sets the employee identifier.
        /// </summary>
        public string EmployeeId { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public SchedulingStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the type of the agent schedule.
        /// </summary>
        public AgentScheduleType AgentScheduleType { get; set; }

        /// <summary>
        /// Gets or sets the date from.
        /// </summary>
        public DateTimeOffset? DateFrom { get; set; }

        /// <summary>
        /// Gets or sets the date to.
        /// </summary>
        public DateTimeOffset? DateTo { get; set; }

        /// <summary>
        /// Gets or sets the charts.
        /// </summary>
        public List<AgentScheduleChart> AgentScheduleCharts { get; set; }

        /// <summary>
        /// Gets or sets the agent schedule manager.
        /// </summary>
        public AgentScheduleManager AgentScheduleManager { get; set; }

        /// <summary>
        /// Gets or sets the modified by.
        /// </summary>
        public string ModifiedBy { get; set; }
    }
}