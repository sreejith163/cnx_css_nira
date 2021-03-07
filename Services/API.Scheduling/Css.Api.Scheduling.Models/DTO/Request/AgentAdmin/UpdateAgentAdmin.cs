using Css.Api.Core.Models.Enums;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentAdmin
{
    public class UpdateAgentAdmin : AgentAdminAttribute
    {
        /// <summary>
        /// Gets or sets the modified by.
        /// </summary>
        /// <value>
        /// The modified by.
        /// </value>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the activity origin.
        /// </summary>
        public ActivityOrigin ActivityOrigin { get; set; }
    }
}