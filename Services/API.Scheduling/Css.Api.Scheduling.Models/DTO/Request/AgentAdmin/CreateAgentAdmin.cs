using Css.Api.Core.Models.Enums;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentAdmin
{
    public class CreateAgentAdmin : AgentAdminAttribute
    {
        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the activity origin.
        /// </summary>
        public ActivityOrigin ActivityOrigin { get; set; }
    }
}