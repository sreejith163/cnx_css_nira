using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentAdmin
{
    public class AgentAdminAttribute
    {
        /// <summary>
        /// Gets or sets the SSN.
        /// </summary>
        public string EmployeeId { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the sso.
        /// </summary>
        public string Sso { get; set; }

        /// <summary>
        /// Gets or sets the agent scheduling group identifier.
        /// </summary>
        public int AgentSchedulingGroupId { get; set; }

        /// <summary>
        /// Gets or sets the supervisor identifier.
        /// </summary>
        public string SupervisorId { get; set; }

        /// <summary>
        /// Gets or sets the name of the supervisor.
        /// </summary>
        public string SupervisorName { get; set; }

        /// <summary>
        /// Gets or sets the supervisor sso.
        /// </summary>
        public string SupervisorSso { get; set; }

        /// <summary>
        /// Gets or sets the agent data.
        /// </summary>
        public List<AgentDataAttribute> AgentData { get; set; }

        /// <summary>Gets or sets the pto.</summary>
        /// <value>The pto.</value>
        public AgentPtoAttribute Pto { get; set; }
    }
}