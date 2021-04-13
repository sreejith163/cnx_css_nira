using System;

namespace Css.Api.Core.EventBus.Events.AgentCategory
{
    /// <summary>
    /// An interface event for any failure in creating agetn category
    /// </summary>
    public interface IAgentCategoryCreateFailed
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type of the agent category.
        /// </summary>
        public int AgentCategoryType { get; set; }

        /// <summary>
        /// Gets or sets the data type minimum value.
        /// </summary>
        public string DataTypeMinValue { get; set; }

        /// <summary>
        /// Gets or sets the data type maximum value.
        /// </summary>
        /// <value>
        public string DataTypeMaxValue { get; set; }

        /// <summary>
        /// Gets or sets the modified date.
        /// </summary>
        public DateTimeOffset? ModifiedDate { get; set; }
    }
}

