using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Core.Models.Contracts
{
    public interface IAgentGroupContract
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
