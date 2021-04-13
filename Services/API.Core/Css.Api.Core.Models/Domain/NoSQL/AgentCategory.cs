using Css.Api.Core.Models.Enums;

namespace Css.Api.Core.Models.Domain.NoSQL
{
    [BsonCollection("agent_category")]
    public class AgentCategory : BaseDocument
    {
        /// <summary>
        /// Gets or sets the agent category identifier.
        /// </summary>
        public int AgentCategoryId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type of the agent category.
        /// </summary>
        public AgentCategoryType AgentCategoryType { get; set; }

        /// <summary>
        /// Gets or sets the data type minimum value.
        /// </summary>
        public string DataTypeMinValue { get; set; }

        /// <summary>
        /// Gets or sets the data type maximum value.
        /// </summary>
        public string DataTypeMaxValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
