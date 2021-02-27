using System;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentSchedule
{
    public class UpdateAgentScheduleDateRange
    {
        /// <summary>
        /// Gets or sets the old date from.
        /// </summary>
        public DateTimeOffset OldDateFrom { get; set; }

        /// <summary>
        /// Gets or sets the old date to.
        /// </summary>
        public DateTimeOffset OldDateTo { get; set; }

        /// <summary>
        /// Gets or sets the new date from.
        /// </summary>
        public DateTimeOffset NewDateFrom { get; set; }

        /// <summary>
        /// Gets or sets the new date to.
        /// </summary>
        public DateTimeOffset NewDateTo { get; set; }

        /// <summary>
        /// Gets or sets the modified by.
        /// </summary>
        public string ModifiedBy { get; set; }
    }
}
