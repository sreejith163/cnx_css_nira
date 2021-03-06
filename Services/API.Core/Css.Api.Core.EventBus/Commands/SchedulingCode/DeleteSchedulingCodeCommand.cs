﻿using System;

namespace Css.Api.Core.EventBus.Commands.SchedulingCode
{
    public class DeleteSchedulingCodeCommand
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

        /// <summary>Gets or sets the modified by old value.</summary>
        /// <value>The modified by old value.</value>
        public string ModifiedByOldValue { get; set; }

        /// <summary>Gets or sets the modified date old value.</summary>
        /// <value>The modified date old value.</value>
        public DateTimeOffset? ModifiedDateOldValue { get; set; }

        /// <summary>Gets or sets a value indicating whether this instance is deleted old value.</summary>
        /// <value>
        ///   <c>true</c> if this instance is deleted old value; otherwise, <c>false</c>.</value>
        public bool IsDeletedOldValue { get; set; }

        /// <summary>Gets or sets a value indicating whether this instance is deleted new value.</summary>
        /// <value>
        ///   <c>true</c> if this instance is deleted new value; otherwise, <c>false</c>.</value>
        public bool IsDeletedNewValue { get; set; }
    }
}




