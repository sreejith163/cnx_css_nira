using System;

namespace Css.Api.Core.EventBus.Commands.ClientLOB
{
    /// <summary>
    ///   <br />
    /// </summary>
    public class UpdateClientLOBCommand
    {
        /// <summary>Gets or sets the identifier.</summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>Gets or sets the name old value.</summary>
        /// <value>The name old value.</value>
        public string NameOldValue { get; set; }

        /// <summary>Gets or sets the client identifier oldvalue.</summary>
        /// <value>The client identifier oldvalue.</value>
        public int ClientIdOldValue { get; set; }

        /// <summary>Gets or sets the timezone identifier oldvalue.</summary>
        /// <value>The timezone identifier oldvalue.</value>
        public int TimezoneIdOldValue { get; set; }

        /// <summary>Gets or sets the first day of week oldvalue.</summary>
        /// <value>The first day of week oldvalue.</value>
        public int FirstDayOfWeekOldValue { get; set; }

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

        /// <summary>Gets or sets the client identifier new value.</summary>
        /// <value>The client identifier new value.</value>
        public int ClientIdNewValue { get; set; }

        /// <summary>Gets or sets a value indicating whether this instance is deleted new value.</summary>
        /// <value>
        ///   <c>true</c> if this instance is deleted new value; otherwise, <c>false</c>.</value>
        public bool IsDeletedNewValue { get; set; }

    }
}

