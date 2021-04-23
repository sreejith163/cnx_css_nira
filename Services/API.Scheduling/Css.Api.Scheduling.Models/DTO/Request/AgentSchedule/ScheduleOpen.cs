using System;
namespace Css.Api.Scheduling.Models.DTO.Request.AgentSchedule
{
    public class ScheduleOpen
    {

        /// <summary>
        /// Gets or sets the time.
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// Gets or sets the time.
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// Gets or sets the schedule open.
        /// </summary>
        public int scheduleOpen { get; set; } = 1;
    }
}
