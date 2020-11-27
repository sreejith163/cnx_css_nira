using Css.Api.Core.Models.DTO.Response;
using Css.Api.Setup.Models.DTO.Request.AgentSchedulingGroup;
using System.Threading.Tasks;

namespace Css.Api.Setup.Business.Interfaces
{
    /// <summary>Interface for agent scheduling group service</summary>
    public interface IAgentSchedulingGroupService
    {
        /// <summary>
        /// Gets the agent scheduling groups.
        /// </summary>
        /// <param name="agentSchedulingGroupQueryParameter">The agent scheduling group query parameter.</param>
        /// <returns></returns>
        Task<CSSResponse> GetAgentSchedulingGroups(AgentSchedulingGroupQueryParameter agentSchedulingGroupQueryParameter);

        /// <summary>
        /// Gets the agent scheduling group.
        /// </summary>
        /// <param name="agentSchedulingGroupIdDetails">The agent scheduling group identifier details.</param>
        /// <returns></returns>
        Task<CSSResponse> GetAgentSchedulingGroup(AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails);

        /// <summary>
        /// Creates the agent scheduling group.
        /// </summary>
        /// <param name="agentSchedulingGroupDetails">The agent scheduling group details.</param>
        /// <returns></returns>
        Task<CSSResponse> CreateAgentSchedulingGroup(CreateAgentSchedulingGroup agentSchedulingGroupDetails);

        /// <summary>
        /// Updates the agent scheduling group.
        /// </summary>
        /// <param name="agentSchedulingGroupIdDetails">The agent scheduling group identifier details.</param>
        /// <param name="agentSchedulingGroupDetails">The agent scheduling group details.</param>
        /// <returns></returns>
        Task<CSSResponse> UpdateAgentSchedulingGroup(AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails, UpdateAgentSchedulingGroup agentSchedulingGroupDetails);

        /// <summary>
        /// Deletes the agent scheduling group.
        /// </summary>
        /// <param name="agentSchedulingGroupIdDetails">The agent scheduling group identifier details.</param>
        /// <returns></returns>
        Task<CSSResponse> DeleteAgentSchedulingGroup(AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails);
    }
}

