using System;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentSchedule
{
    public class UpdateAgentScheduleDateRange
    {
        /// <summary>
        /// Gets or sets the old date from.
        /// </summary>
        public DateTime OldDateFrom { get; set; }

        /// <summary>
        /// Gets or sets the old date to.
        /// </summary>
        public DateTime OldDateTo { get; set; }

        /// <summary>
        /// Gets or sets the new date from.
        /// </summary>
        public DateTime NewDateFrom { get; set; }

        /// <summary>
        /// Gets or sets the new date to.
        /// </summary>
        public DateTime NewDateTo { get; set; }

        /// <summary>
        /// Gets or sets the modified by.
        /// </summary>
        public string ModifiedBy { get; set; }
    }
}
