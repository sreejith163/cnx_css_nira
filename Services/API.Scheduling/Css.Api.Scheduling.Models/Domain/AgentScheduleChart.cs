using System;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.Domain
{
    public class AgentScheduleChart
    {
        /// <summary>
        /// ReplyId
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the day.
        /// </summary>
        public int Day { get; set; }

        /// <summary>
        /// Gets or sets the charts.
        /// </summary>
        public List<ScheduleChart> Charts { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentScheduleChart"/> class.
        /// </summary>
        public AgentScheduleChart()
        {
            Id = Guid.NewGuid();
        }
    }
}
