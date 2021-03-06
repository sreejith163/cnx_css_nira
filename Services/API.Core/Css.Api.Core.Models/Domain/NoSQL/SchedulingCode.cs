﻿namespace Css.Api.Core.Models.Domain.NoSQL
{
    [BsonCollection("scheduling_code")]
    public class SchedulingCode : BaseDocument
    {
        /// <summary>
        /// Gets or sets the scheduling code identifier.
        /// </summary>
        public int SchedulingCodeId { get; set; }

        /// <summary>
        /// Gets or sets the anme.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the reference identifier.
        /// </summary>
        public int? RefId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [time off code].
        /// </summary>
        public bool TimeOffCode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
