using System;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.Domain
{
    public class AgentScheduleManagerChart
    {
        /// <summary>
        /// ReplyId
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the charts.
        /// </summary>
        public List<ScheduleChart> Charts { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentScheduleManagerChart"/> class.
        /// </summary>
        public AgentScheduleManagerChart()
        {
            Id = Guid.NewGuid();
        }
    }
}
