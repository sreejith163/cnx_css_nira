using Css.Api.Core.Models.DTO.Request;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentSchedulingGroup
{
    public class AgentSchedulingGroupQueryparameter : QueryStringParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AgentSchedulingGroupQueryparameter"/> class.
        /// </summary>
        public AgentSchedulingGroupQueryparameter()
        {
            OrderBy = "CreatedDate";
        }
    }
}
