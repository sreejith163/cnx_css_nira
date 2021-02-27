using System;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentSchedule
{
    public class AgentScheduleChartQueryparameter
    {
        /// <summary>
        /// Gets or sets from date.
        /// </summary>
        public DateTimeOffset? FromDate { get; set; }

        /// <summary>
        /// Converts to date.
        /// </summary>
        public DateTimeOffset? ToDate { get; set; }
    }
}