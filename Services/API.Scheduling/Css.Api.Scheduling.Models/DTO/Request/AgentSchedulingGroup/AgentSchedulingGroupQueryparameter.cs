using Css.Api.Core.Models.DTO.Request;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentSchedulingGroup
{
    public class AgentSchedulingGroupQueryparameter : QueryStringParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AgentSchedulingGroupQueryparameter"/> class.
        /// </summary>
        public AgentSchedulingGroupQueryparameter()
        {
            OrderBy = "CreatedDate";
        }

        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        public int? ClientId { get; set; }

        /// <summary>
        /// Gets or sets the client lob group identifier.
        /// </summary>
        public int? ClientLobGroupId { get; set; }

        /// <summary>
        /// Gets or sets the skill group identifier.
        /// </summary>
        public int? SkillGroupId { get; set; }

        /// <summary>
        /// Gets or sets the skill tag identifier.
        /// </summary>
        public int? SkillTagId { get; set; }
    }
}
