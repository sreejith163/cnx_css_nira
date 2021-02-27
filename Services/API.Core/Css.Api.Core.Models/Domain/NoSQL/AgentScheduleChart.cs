using System.Collections.Generic;

namespace Css.Api.Core.Models.Domain.NoSQL
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

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentScheduleChart"/> class.
        /// </summary>
        public AgentScheduleChart()
        {
            Charts = new List<ScheduleChart>();
        }
    }
}
