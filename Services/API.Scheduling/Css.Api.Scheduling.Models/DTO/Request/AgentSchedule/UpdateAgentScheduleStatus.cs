using Css.Api.Core.Models.Enums;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentSchedule
{
    public class UpdateAgentScheduleStatus
    {
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public SchedulingStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the modified by.
        /// </summary>
        public string ModifiedBy { get; set; }
    }
}