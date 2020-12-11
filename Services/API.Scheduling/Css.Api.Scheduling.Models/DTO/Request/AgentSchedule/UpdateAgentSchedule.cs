using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.Enums;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentAdmin
{
    public class UpdateAgentSchedule
    {
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public SchedulingStatus Status { get; set; }

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