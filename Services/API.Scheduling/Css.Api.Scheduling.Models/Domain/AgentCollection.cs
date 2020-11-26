using Css.Api.Core.Models.Domain;
using System;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.Domain
{
    [BsonCollection("agent_collection")]
    public class AgentCollection : BaseDocument
    {
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

        /// <summary>
        /// Gets or sets the sen date.
        /// </summary>
        public DateTime SenDate { get; set; }

        /// <summary>
        /// Gets or sets the sen ext.
        /// </summary>
        public DateTime SenExt { get; set; }

        /// <summary>
        /// Gets or sets the agent data.
        /// </summary>
        public List<AgentData> AgentData { get; set; }
    }
}
