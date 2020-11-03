﻿using System.Collections.Generic;

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
        /// Gets or sets the code types.
        /// </summary>
        public List<int> CodeTypes { get; set; }

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        public int IconId { get; set; }
    }
}
