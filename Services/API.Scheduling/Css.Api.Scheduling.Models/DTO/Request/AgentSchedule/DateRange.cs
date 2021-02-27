using System;
namespace Css.Api.Scheduling.Models.DTO.Request.AgentSchedule
{
    public class DateRange
    {
        /// <summary>
        /// Gets or sets the date from.
        /// </summary>
        public DateTimeOffset DateFrom { get; set; }

        /// <summary>
        /// Gets or sets the date to.
        /// </summary>
        public DateTimeOffset DateTo { get; set; }
    }
}
