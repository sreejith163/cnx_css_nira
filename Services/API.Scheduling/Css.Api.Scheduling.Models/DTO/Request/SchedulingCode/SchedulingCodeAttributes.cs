using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.DTO.Request.SchedulingCode
{
    public class SchedulingCodeAttributes
    {
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        public int PriorityNumber { get; set; }

        /// <summary>
        /// Gets or sets the scheduling type code.
        /// </summary>
        public List<SchedulingCodeTypes> SchedulingTypeCode { get; set; }

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        public int IconId { get; set; }
    }
}
