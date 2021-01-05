using Css.Api.Core.Models.Domain;
using Css.Api.Core.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Models.Domain
{
    [BsonCollection("agent")]
    public class Agent : BaseDocument, IAgentContract
    {
        /// <summary>
        /// Gets or sets the first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the agent ssn
        /// </summary>
        public int Ssn { get; set; }

        /// <summary>
        /// Gets or sets the agent sso
        /// </summary>
        public string Sso { get; set; }

        /// <summary>
        /// Gets or sets the delete flag
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the creator information
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the created date
        /// </summary>
        public DateTimeOffset CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the modifier information
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the modified date
        /// </summary>
        public DateTimeOffset? ModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets the mu.
        /// </summary>
        public string Mu { get; set; }

        /// <summary>
        /// Gets or sets the sen date.
        /// </summary>
        public DateTime? SenDate { get; set; }

        /// <summary>
        /// Gets or sets the sen ext.
        /// </summary>
        public DateTime? SenExt { get; set; }

        /// <summary>
        /// Gets or sets the agent role.
        /// </summary>
        public string AgentRole { get; set; }

        /// <summary>
        /// Gets or sets the agent data.
        /// </summary>
        public List<AgentData> AgentData { get; set; }
        
    }
}
