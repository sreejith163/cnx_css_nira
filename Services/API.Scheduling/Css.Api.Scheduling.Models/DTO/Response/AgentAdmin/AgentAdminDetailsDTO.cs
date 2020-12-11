namespace Css.Api.Scheduling.Models.DTO.Response.AgentAdmin
{
    public class AgentAdminDetailsDTO : AgentAdminDTO
    {
        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        public int ClientId { get; set; }

        /// <summary>
        /// Gets or sets the name of the client.
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// Gets or sets the client lob group identifier.
        /// </summary>
        public int ClientLobGroupId { get; set; }

        /// <summary>
        /// Gets or sets the name of the client lob group.
        /// </summary>
        public string ClientLobGroupName { get; set; }

        /// <summary>
        /// Gets or sets the skill group identifier.
        /// </summary>
        public int SkillGroupId { get; set; }

        /// <summary>
        /// Gets or sets the name of the skill group.
        /// </summary>
        public string SkillGroupName { get; set; }

        /// <summary>Gets or sets the name of the skill tag.</summary>
        /// <value>The name of the skill tag.</value>
        public string SkillTagName { get; set; }
    }
}

