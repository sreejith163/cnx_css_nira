using System;

namespace Css.Api.Scheduling.Models.DTO.Request.ActivityLog
{
    public class UpdateActivityLogRange
    {
        /// <summary>
        /// Gets or sets the date from.
        /// </summary>
        public DateTime DateFrom { get; set; }

        /// <summary>
        /// Gets or sets the date to.
        /// </summary>
        public DateTime DateTo { get; set; }

        /// <summary>
        /// Creates new datefrom.
        /// </summary>
        public DateTime NewDateFrom { get; set; }

        /// <summary>
        /// Creates new dateto.
        /// </summary>
        public DateTime NewDateTo { get; set; }
    }
}
