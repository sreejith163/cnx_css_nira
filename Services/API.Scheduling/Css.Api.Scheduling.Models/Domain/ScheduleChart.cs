namespace Css.Api.Scheduling.Models.Domain
{
    public class ScheduleChart
    {
        /// <summary>
        /// Gets or sets the start time.
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time.
        /// </summary>
        public string EndTime { get; set; }

        /// <summary>
        /// Gets or sets the scheduling code identifier.
        /// </summary>
        public int SchedulingCodeId { get; set; }
    }
}
