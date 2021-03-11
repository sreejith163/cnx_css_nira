using System;

namespace Css.Api.Setup.Models.DTO.Request.AgentSchedulingGroup
{
    public class UpdateAgentSchedulingGroup : AgentSchedulingGroupAttribute
    {
        /// <summary>
        /// Gets or sets the modified by.
        /// </summary>
        /// <value>
        /// The modified by.
        /// </value>
        public string ModifiedBy { get; set; }

        /// <summary>Gets or sets the modified date.</summary>
        /// <value>The modified date.</value>
        public DateTimeOffset? ModifiedDate { get; set; }

        /// <summary>Gets or sets a value indicating whether this instance is update revert.</summary>
        /// <value>
        ///   <c>true</c> if this instance is update revert; otherwise, <c>false</c>.</value>
        public bool IsUpdateRevert { get; set; }

        /// <summary>Gets or sets a value indicating whether this instance is deleted.</summary>
        /// <value>
        ///   <c>true</c> if this instance is deleted; otherwise, <c>false</c>.</value>
        public bool IsDeleted { get; set; }

 
    }
}
