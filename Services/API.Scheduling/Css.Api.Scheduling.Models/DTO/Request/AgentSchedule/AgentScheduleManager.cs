using Css.Api.Core.Models.Domain.NoSQL;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentSchedule
{
    public class AgentScheduleManager
    {
        /// <summary>
        /// Gets or sets the employee identifier.
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// Gets or sets the agent schedule manager chart.
        /// </summary>
        public AgentScheduleManagerChart AgentScheduleManagerChart { get; set; }
    }
}
