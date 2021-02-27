using System.Collections.Generic;

namespace Css.Api.Core.Models.Domain.NoSQL
{
    [BsonCollection("agent_scheduling_group_id")]
    public class AgentSchedulingGroupHistory : BaseDocument
    {
        /// <summary>
        /// Gets or sets the employee identifier.
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// Gets or sets the agent scheduling group details.
        /// </summary>
        public List<AgentSchedulingGroupDetails> AgentSchedulingGroupDetails { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentSchedulingGroupHistory"/> class.
        /// </summary>
        public AgentSchedulingGroupHistory()
        {
            AgentSchedulingGroupDetails = new List<AgentSchedulingGroupDetails>();
        }
    }
}
