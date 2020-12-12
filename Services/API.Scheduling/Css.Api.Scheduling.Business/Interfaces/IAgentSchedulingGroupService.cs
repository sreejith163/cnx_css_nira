using Css.Api.Core.Models.DTO.Response;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedulingGroup;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Business.Interfaces
{
    public interface IAgentSchedulingGroupService
    {
        /// <summary>
        /// Gets the agent scheduling groups.
        /// </summary>
        /// <param name="agentSchedulingGroupQueryparameter">The agent scheduling group queryparameter.</param>
        /// <returns></returns>
        Task<CSSResponse> GetAgentSchedulingGroups(AgentSchedulingGroupQueryparameter agentSchedulingGroupQueryparameter);

        /// <summary>
        /// Gets the agent scheduling group.
        /// </summary>
        /// <param name="agentSchedulingGroupIdDetails">The agent scheduling group identifier details.</param>
        /// <returns></returns>
        Task<CSSResponse> GetAgentSchedulingGroup(AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails);
    }
}