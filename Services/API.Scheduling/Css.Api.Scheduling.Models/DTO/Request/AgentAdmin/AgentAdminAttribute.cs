using Css.Api.Scheduling.Models.DTO.Request.AgentData;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentAdmin
{
    public class AgentAdminAttribute
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
        /// Gets or sets the SSN.
        /// </summary>
        public string EmployeeId { get; set; }

        /// <summary>Gets or sets the sso.</summary>
        /// <value>The sso.</value>
        public string Sso { get; set; }


        /// <summary>Gets or sets the skill tag identifier.</summary>
        /// <value>The skill tag identifier.</value>
        public int SkillTagId { get; set; }

        /// <summary>
        /// Gets or sets the agent data.
        /// </summary>
        public List<AgentDataAttribute> AgentData { get; set; }
    }
}