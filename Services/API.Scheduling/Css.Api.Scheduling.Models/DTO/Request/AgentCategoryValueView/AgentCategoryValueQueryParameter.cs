using Css.Api.Core.Models.DTO.Request;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentCategoryValueView
{
    public class AgentCategoryValueQueryParameter : QueryStringParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AgentCategoryValueQueryParameter"/> class.
        /// </summary>
        public AgentCategoryValueQueryParameter()
        {
            
        }

        /// <summary>Gets or sets the agent scheduling group identifier.</summary>
        /// <value>The agent scheduling group identifier.</value>
        public int AgentSchedulingGroupId { get; set; }

        /// <summary>Gets or sets the agent category identifier.</summary>
        /// <value>The agent category identifier.</value>
        public int AgentCategoryId { get; set; }
    }
}