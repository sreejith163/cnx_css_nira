using Css.Api.Core.Models.Domain;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedulingGroup;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Repository.Interfaces
{
    /// <summary>
    /// Interface for the agent schedule repository
    /// </summary>
    public interface IAgentSchedulingGroupRepository
    {
        /// <summary>
        /// Gets the agent scheduling group.
        /// </summary>
        /// <param name="agentSchedulingGroupQueryparameter">The agent scheduling group queryparameter.</param>
        /// <returns></returns>
        Task<PagedList<Entity>> GetAgentSchedulingGroups(AgentSchedulingGroupQueryparameter agentSchedulingGroupQueryparameter);

        /// <summary>
        /// Gets the agent scheduling group.
        /// </summary>
        /// <param name="agentSchedulingGroupIdDetails">The agent scheduling group identifier details.</param>
        /// <returns></returns>
        Task<AgentSchedulingGroup> GetAgentSchedulingGroup(AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails);

        /// <summary>
        /// Gets the agent scheduling groups count.
        /// </summary>
        /// <returns></returns>
        Task<int> GetAgentSchedulingGroupsCount();

        /// <summary>
        /// Creates the agent scheduling group.
        /// </summary>
        /// <param name="agentSchedulingGroupRequest">The agent scheduling group request.</param>
        void CreateAgentSchedulingGroup(AgentSchedulingGroup agentSchedulingGroupRequest);

        /// <summary>
        /// Updates the agent scheduling group.
        /// </summary>
        /// <param name="agentSchedulingGroupRequest">The agent scheduling group request.</param>
        void UpdateAgentSchedulingGroup(AgentSchedulingGroup agentSchedulingGroupRequest);
    }
}