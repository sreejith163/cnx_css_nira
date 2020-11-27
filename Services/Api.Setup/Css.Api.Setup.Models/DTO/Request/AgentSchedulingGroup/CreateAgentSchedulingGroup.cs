namespace Css.Api.Setup.Models.DTO.Request.AgentSchedulingGroup
{
    public class CreateAgentSchedulingGroup : AgentSchedulingGroupAttribute
    {
        /// <summary>
        /// Gets or sets the reference identifier.
        /// </summary>
        public int? RefId { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        public string CreatedBy { get; set; }
    }
}


