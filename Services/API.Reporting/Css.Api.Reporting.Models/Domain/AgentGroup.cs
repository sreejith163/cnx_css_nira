using Css.Api.Core.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Models.Domain
{
    public class AgentGroup : IAgentGroupContract
    {
        /// <summary>
        /// Get or sets the description of the group
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Get or sets the value of the group
        /// </summary>
        public string Value { get; set; }
    }
}
