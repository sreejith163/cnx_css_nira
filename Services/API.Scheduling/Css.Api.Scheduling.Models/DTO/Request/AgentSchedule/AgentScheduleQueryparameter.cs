using Css.Api.Core.Models.DTO.Request;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentSchedule
{
    public class AgentScheduleQueryparameter : QueryStringParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AgentScheduleQueryparameter"/> class.
        /// </summary>
        public AgentScheduleQueryparameter()
        {
            OrderBy = "CreatedDate";
        }

        /// <summary>
        /// Gets or sets the agent scheduling group identifier.
        /// </summary>
        public int? AgentSchedulingGroupId { get; set; }
    }
}
