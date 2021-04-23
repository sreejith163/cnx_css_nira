using System;

namespace Css.Api.Core.EventBus.Commands.AgentCategory
{
    /// <summary>
    ///   <br />
    /// </summary>
    public class UpdateAgentCategoryCommand
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name old value.
        /// </summary>
        public string NameOldValue { get; set; }

        /// <summary>
        /// Gets or sets the agent category type old value.
        /// </summary>
        public int AgentCategoryTypeOldValue { get; set; }

        /// <summary>
        /// Gets or sets the data type minimum value old value.
        /// </summary>
        public string DataTypeMinValueOldValue { get; set; }

        /// <summary>
        /// Gets or sets the data type maximum value old value.
        /// </summary>
        public string DataTypeMaxValueOldValue { get; set; }

        /// <summary>
        /// Gets or sets the modified by old value.
        /// </summary>
        public string ModifiedByOldValue { get; set; }

        /// <summary>
        /// Gets or sets the modified date old value.
        /// </summary>
        public DateTimeOffset? ModifiedDateOldValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is deleted old value.
        /// </summary>
        public bool IsDeletedOldValue { get; set; }

        /// <summary>
        /// Gets or sets the name new value.
        /// </summary>
        public string NameNewValue { get; set; }

        /// <summary>
        /// Gets or sets the agent category type new value.
        /// </summary>
        public int AgentCategoryTypeNewValue { get; set; }

        /// <summary>
        /// Gets or sets the data type minimum value new value.
        /// </summary>
        public string DataTypeMinValueNewValue { get; set; }

        /// <summary>
        /// Gets or sets the data type maximum value new value.
        /// </summary>
        public string DataTypeMaxValueNewValue { get; set; }

        /// <summary>
        /// Gets or sets the modified by new value.
        /// </summary>
        public string ModifiedByNewValue { get; set; }

        /// <summary>
        /// Gets or sets the modified date new value.
        /// </summary>
        public DateTimeOffset? ModifiedDateNewValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is deleted new value.
        /// </summary>
        public bool IsDeletedNewValue { get; set; }
    }
}