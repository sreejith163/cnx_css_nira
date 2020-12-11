using Css.Api.Core.Models.Domain;
using Css.Api.Scheduling.Models.Enums;
using System;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.Domain
{
    [BsonCollection("agent_schedule")]
    public class AgentSchedule : BaseDocument
    {
        /// <summary>
        /// Gets or sets the employee identifier.
        /// </summary>
        public string EmployeeId { get; set; }

        /// <summary>
        /// Gets or sets the type of the agent schedule.
        /// </summary>
        public AgentScheduleType AgentScheduleType { get; set; }

        /// <summary>
        /// Gets or sets the date from.
        /// </summary>
        public DateTimeOffset DateFrom { get; set; }

        /// <summary>
        /// Gets or sets the date to.
        /// </summary>
        public DateTimeOffset DateTo { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public SchedulingStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        public int ClientId { get; set; }

        /// <summary>
        /// Gets or sets the client lob group identifier.
        /// </summary>
        public int ClientLobGroupId { get; set; }

        /// <summary>
        /// Gets or sets the skill group identifier.
        /// </summary>
        public int SkillGroupId { get; set; }

        /// <summary>
        /// Gets or sets the skill tag identifier.
        /// </summary>
        public int SkillTagId { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public int AgentSchedulingGroupId { get; set; }

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

        /// <summary>
        /// Gets or sets the modified date.
        /// </summary>
        public DateTimeOffset? ModifiedDate { get; set; }
    }
}
