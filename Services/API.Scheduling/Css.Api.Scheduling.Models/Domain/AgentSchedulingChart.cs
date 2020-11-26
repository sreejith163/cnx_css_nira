namespace Css.Api.Scheduling.Models.Domain
{
    public class AgentSchedulingChart
    {
        /// <summary>
        /// Gets or sets the day.
        /// </summary>
        public int Day { get; set; }

        /// <summary>
        /// Gets or sets the start time.
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time.
        /// </summary>
        public string EndTime { get; set; }

        /// <summary>
        /// Gets or sets the meridian.
        /// </summary>
        public string Meridian { get; set; }

        /// <summary>
        /// Gets or sets the icon identifier.
        /// </summary>
        public string IconId { get; set; }

        /// <summary>
        /// Gets or sets the icon value.
        /// </summary>
        public string IconValue { get; set; }
    }
}
