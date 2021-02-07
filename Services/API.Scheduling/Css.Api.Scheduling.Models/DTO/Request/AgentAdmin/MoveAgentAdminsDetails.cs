using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentAdmin
{
    public class MoveAgentAdminsDetails
    {
        /// <summary>Gets or sets the agent admin ids.</summary>
        /// <value>The agent admin ids.</value>
        public List<string> AgentAdminIds { get; set; }

        /// <summary>Gets or sets the destination agent scheduling group identifier.</summary>
        /// <value>The destination agent scheduling group identifier.</value>
        public int SourceSchedulingGroupId { get; set; }

        /// <summary>Gets or sets the target agent scheduling group identifier.</summary>
        /// <value>The target agent scheduling group identifier.</value>
        public int DestinationSchedulingGroupId { get; set; }

        /// <summary>
        /// Gets or sets the modified by.
        /// </summary>
        public string ModifiedBy { get; set; }
    }
}
