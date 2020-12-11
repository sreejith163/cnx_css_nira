using System;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.Domain
{
    public class AgentScheduleManager
    {
        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the charts.
        /// </summary>
        public List<AgentScheduleChart> AgentScheduleCharts { get; set; }
    }
}
