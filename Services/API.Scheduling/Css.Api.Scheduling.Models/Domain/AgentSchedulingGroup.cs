using Css.Api.Core.Models.Domain;

namespace Css.Api.Scheduling.Models.Domain
{
    [BsonCollection("agent_scheduling_group")]
    public class AgentSchedulingGroup : BaseDocument
    {
        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        public int ClientId { get; set; }

        /// <summary>
        /// Gets or sets the client lob group identifier.
        /// </summary>
        public int ClientLobGroupId { get; set; }

        /// <summary>
        /// Gets or sets the skill group identifier.
        /// </summary>
        public int SkillGroupId { get; set; }

        /// <summary>
        /// Gets or sets the skill tag identifier.
        /// </summary>
        public int SkillTagId { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public int AgentSchedulingGroupId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }
    }
}
