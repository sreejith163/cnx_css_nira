using System;

namespace Css.Api.Scheduling.Models.Domain
{
    public class AgentScheduleManagerChart
    {
        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the charts.
        /// </summary>
        public AgentScheduleChart AgentScheduleCharts { get; set; }
    }
}
