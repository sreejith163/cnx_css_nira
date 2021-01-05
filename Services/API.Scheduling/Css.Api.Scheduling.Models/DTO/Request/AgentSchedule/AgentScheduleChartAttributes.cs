using Css.Api.Scheduling.Models.Enums;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentSchedule
{
    public class AgentScheduleChartAttributes
    {
        /// <summary>
        /// Gets or sets the type of the agent schedule.
        /// </summary>
        public AgentScheduleType AgentScheduleType { get; set; }

        /// <summary>
        /// Gets or sets the modified by.
        /// </summary>
        public string ModifiedBy { get; set; }
    }
}
