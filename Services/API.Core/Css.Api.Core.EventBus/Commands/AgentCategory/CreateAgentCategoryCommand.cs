using System;

namespace Css.Api.Core.EventBus.Commands.AgentCategory
{
    public class CreateAgentCategoryCommand
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
        public string DataTypeMaxValue { get; set; }

        /// <summary>
        /// Gets or sets the modified date.
        /// </summary>
        public DateTimeOffset? ModifiedDate { get; set; }
    }
}

