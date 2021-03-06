﻿using System;

namespace Css.Api.Core.EventBus.Events.SchedulingCode
{
    /// <summary>
    /// An interface event for any failure in creating client
    /// </summary>
    public interface ISchedulingCodeCreateFailed
    {
        /// <summary>Gets or sets the identifier.</summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the reference identifier.
        /// </summary>
        public int? RefId { get; set; }

        /// <summary>Gets or sets the name.</summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>Gets or sets the priority number.</summary>
        /// <value>The priority number.</value>
        public int PriorityNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [time off code].
        /// </summary>
        public bool TimeOffCode { get; set; }

        /// <summary>Gets or sets the icon identifier.</summary>
        /// <value>The icon identifier.</value>
        public int IconId { get; set; }

        /// <summary>Gets or sets the scheduling type code.</summary>
        /// <value>The scheduling type code.</value>
        public string SchedulingTypeCode { get; set; }

        /// <summary>Gets or sets the modified date.</summary>
        /// <value>The modified date.</value>
        public DateTimeOffset? ModifiedDate { get; set; }
    }
}

