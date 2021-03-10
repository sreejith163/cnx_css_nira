using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Core.Models.Domain.NoSQL
{
    public class AgentScheduleManagerChart
    {
        /// <summary>
        /// Gets or sets the start time
        /// </summary>
        public DateTime StartDateTime { get; set; }

        /// <summary>
        /// Gets or sets the end time.
        /// </summary>
        public DateTime EndDateTime { get; set; }

        /// <summary>
        /// Gets or sets the scheduling code identifier.
        /// </summary>
        public int SchedulingCodeId { get; set; }
    }
}
