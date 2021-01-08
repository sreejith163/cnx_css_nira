using System;

namespace Css.Api.Core.EventBus.Events.SchedulingCode
{
    public interface ISchedulingCodeUpdateFailed
    {
        /// <summary>Gets or sets the identifier.</summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>Gets or sets the name old value.</summary>
        /// <value>The name old value.</value>
        public string NameOldValue { get; set; }

        /// <summary>Gets or sets the priority number old value.</summary>
        /// <value>The priority number old value.</value>
        public int PriorityNumberOldValue { get; set; }

        /// <summary>Gets or sets the icon identifier old value.</summary>
        /// <value>The icon identifier old value.</value>
        public int IconIdOldValue { get; set; }

        /// <summary>Gets or sets the scheduling type code old value.</summary>
        /// <value>The scheduling type code old value.</value>
        public string SchedulingTypeCodeOldValue { get; set; }

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
    }
}

