using System;

namespace Css.Api.Core.EventBus.Commands.SchedulingCode
{
    /// <summary>
    ///   <br />
    /// </summary>
    public class UpdateSchedulingCodeCommand
    {
        /// <summary>Gets or sets the identifier.</summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>Gets or sets the name old value.</summary>
        /// <value>The name old value.</value>
        public string NameOldValue { get; set; }

        /// <summary>
        /// Gets or sets the reference identifier old value.
        /// </summary>
        public int? RefIdOldValue { get; set; }

        /// <summary>Gets or sets the priority number old value.</summary>
        /// <value>The priority number old value.</value>
        public int PriorityNumberOldValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [time off code old value].
        /// </summary>
        public bool TimeOffCodeOldValue { get; set; }

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

        /// <summary>Gets or sets the name new value.</summary>
        /// <value>The name new value.</value>
        public string NameNewValue { get; set; }

        /// <summary>
        /// Gets or sets the reference identifier new value.
        /// </summary>
        public int? RefIdNewValue { get; set; }

        /// <summary>Gets or sets the priority number new value.</summary>
        /// <value>The priority number new value.</value>
        public int PriorityNumberNewValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [time off code new value].
        /// </summary>
        public bool TimeOffCodeNewValue { get; set; }

        /// <summary>Gets or sets the icon identifier new value.</summary>
        /// <value>The icon identifier new value.</value>
        public int IconIdNewValue { get; set; }

        /// <summary>Gets or sets a value indicating whether this instance is deleted new value.</summary>
        /// <value>
        ///   <c>true</c> if this instance is deleted new value; otherwise, <c>false</c>.</value>
        public bool IsDeletedNewValue { get; set; }
    }
}