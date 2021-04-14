using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.DTO.Response.AgentAdmin
{
    public class AgentCategoryValueUpdateResponse
    {
        /// <summary>
        /// Gets or sets the employee identifier.
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// Gets or sets the messages.
        /// </summary>
        public List<string> Messages { get; set; } = new List<string>();
    }
}
