﻿using Css.Api.Core.Models.Contracts;

namespace Css.Api.Scheduling.Models.Domain
{
    public class AgentGroup : IAgentGroupContract
    {
        /// <summary>
        /// Gets or sets the employee identifier.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public string Value { get; set; }
    }
}
