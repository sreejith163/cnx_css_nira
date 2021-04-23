using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentAdmin
{
    public class CreateAgentCategoryValue
    {
        /// <summary>
        /// Gets or sets the agent category details.
        /// </summary>
        public List<AgentCategoryDetails> AgentCategoryDetails { get; set; }
    }
}
