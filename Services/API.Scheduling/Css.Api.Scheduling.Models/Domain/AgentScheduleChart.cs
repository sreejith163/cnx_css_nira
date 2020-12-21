using System;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.Domain
{
    public class AgentScheduleChart
    {
        /// <summary>
        /// Gets or sets the day.
        /// </summary>
        public int Day { get; set; }

        /// <summary>
        /// Gets or sets the charts.
        /// </summary>
        public List<ScheduleChart> Charts { get; set; }
    }
}
