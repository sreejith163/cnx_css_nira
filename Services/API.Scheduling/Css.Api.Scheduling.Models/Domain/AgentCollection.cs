using Css.Api.Core.Models.Domain;
using System;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.Domain
{
    [BsonCollection("agent_collection")]
    public class AgentCollection : BaseDocument
    {

        /// <summary>Gets or sets the agent admin identifier.</summary>
        /// <value>The agent admin identifier.</value>
        public int AgentAdminId { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the mu.
        /// </summary>
        public string Mu { get; set; }

        /// <summary>
        /// Gets or sets the SSN.
        /// </summary>
        public string Ssn { get; set; }

        /// <summary>Gets or sets the sso.</summary>
        /// <value>The sso.</value>
        public string Sso { get; set; }

        /// <summary>Gets or sets the client identifier.</summary>
        /// <value>The client identifier.</value>
        public int ClientId { get; set; }

        /// <summary>Gets or sets the client lob group identifier.</summary>
        /// <value>The client lob group identifier.</value>
        public int ClientLobGroupId { get; set; }

        /// <summary>Gets or sets the skill group identifier.</summary>
        /// <value>The skill group identifier.</value>
        public int SkillGroupId { get; set; }

        /// <summary>Gets or sets the skill tag identifier.</summary>
        /// <value>The skill tag identifier.</value>
        public int SkillTagId { get; set; }    

        /// <summary>
        /// Gets or sets the sen date.
        /// </summary>
        public DateTime SenDate { get; set; }

        /// <summary>
        /// Gets or sets the sen ext.
        /// </summary>
        public DateTime SenExt { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>Gets or sets the created by.</summary>
        /// <value>The created by.</value>
        public string CreatedBy { get; set; }

        /// <summary>Gets or sets the created date.</summary>
        /// <value>The created date.</value>
        public DateTimeOffset CreatedDate { get; set; }

        /// <summary>Gets or sets the modified by.</summary>
        /// <value>The modified by.</value>
        public string ModifiedBy { get; set; }

        /// <summary>Gets or sets the modified date.</summary>
        /// <value>The modified date.</value>
        public DateTimeOffset? ModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets the agent data.
        /// </summary>
        public List<AgentData> AgentData { get; set; }
    }
}
