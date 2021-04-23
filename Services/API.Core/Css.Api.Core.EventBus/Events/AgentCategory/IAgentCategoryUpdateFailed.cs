using System;

namespace Css.Api.Core.EventBus.Events.AgentCategory
{
    public interface IAgentCategoryUpdateFailed
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
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
        /// <value>
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
    }
}

