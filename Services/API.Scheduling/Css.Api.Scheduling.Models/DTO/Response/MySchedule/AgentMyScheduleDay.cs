using Css.Api.Core.Models.Domain.NoSQL;
using System;

namespace Css.Api.Scheduling.Models.DTO.Response.MySchedule
{
    public class AgentMyScheduleDay : AgentScheduleChart
    {
        /// <summary>Gets or sets the date.</summary>
        /// <value>The date.</value>
        public DateTime Date { get; set; }

        /// <summary>Gets or sets the first start time.</summary>
        /// <value>The first start time.</value>
        public string FirstStartTime { get; set; }

        /// <summary>Gets or sets the last start time.</summary>
        /// <value>The last start time.</value>
        public string LastEndTime { get; set; }
    }
}
