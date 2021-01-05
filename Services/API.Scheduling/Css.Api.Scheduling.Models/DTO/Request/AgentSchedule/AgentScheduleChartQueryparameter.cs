using Css.Api.Scheduling.Models.Enums;
using System;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentSchedule
{
    public class AgentScheduleChartQueryparameter
    {
        /// <summary>
        /// Gets or sets the day.
        /// </summary>
        public int? Day { get; set; }

        /// <summary>
        /// Gets or sets from date.
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// Gets or sets the type of the agent schedule.
        /// </summary>
        public AgentScheduleType? AgentScheduleType { get; set; }
    }
}