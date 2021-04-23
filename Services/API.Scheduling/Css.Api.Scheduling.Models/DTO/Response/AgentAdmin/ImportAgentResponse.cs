using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Scheduling.Models.DTO.Response.AgentAdmin
{
   public class ImportAgentResponse
    {
        /// <summary>
        /// Gets or sets the employee identifier.
        /// </summary>
        public string EmployeeId { get; set; }

        /// <summary>
        /// Gets or sets the messages.
        /// </summary>
        public List<string> Messages { get; set; } = new List<string>();
    }
}
