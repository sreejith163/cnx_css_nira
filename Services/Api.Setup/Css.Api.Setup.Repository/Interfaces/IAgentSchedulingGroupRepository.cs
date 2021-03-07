using Css.Api.Core.Models.Domain;
using Css.Api.Setup.Models.Domain;
using Css.Api.Setup.Models.DTO.Request.AgentSchedulingGroup;
using Css.Api.Setup.Models.DTO.Request.SkillTag;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Css.Api.Setup.Repository.Interfaces
{
    public interface IAgentSchedulingGroupRepository
    {
        /// <summary>
        /// Gets the agent scheduling groups.
        /// </summary>
        /// <param name="agentSchedulingGroupQueryParameter">The agent scheduling group query parameter.</param>
        /// <returns></returns>
        Task<PagedList<Entity>> GetAgentSchedulingGroups(AgentSchedulingGroupQueryParameter agentSchedulingGroupQueryParameter);

        /// <summary>
        /// Gets the agent scheduling group.
        /// </summary>
        /// <param name="agentSchedulingGroupIdDetails">The agent scheduling group identifier details.</param>
        /// <returns></returns>
        Task<AgentSchedulingGroup> GetAgentSchedulingGroup(AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails);

        /// <summary>Gets all agent scheduling group.</summary>
        /// <param name="agentSchedulingGroupIdDetails">The agent scheduling group identifier details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<AgentSchedulingGroup> GetAllAgentSchedulingGroup(AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails);

        /// <summary>
        /// Gets the agent scheduling groups count by skill tag identifier.
        /// </summary>
        /// <param name="skillTagIdDetails">The skill tag identifier details.</param>
        /// <returns></returns>
        Task<int> GetAgentSchedulingGroupsCountBySkillTagId(SkillTagIdDetails skillTagIdDetails);

        /// <summary>
        /// Gets the agent scheduling groups count by skill or refid tag identifier.
        /// </summary>
        /// <param name="agentSchedulingGroupAttribute"></param>
        /// <returns></returns>
        Task<List<AgentSchedulingGroup>> GetAgentSchedulingGroupsCountBySkillTagIdOrRefId(AgentSchedulingGroupAttribute agentSchedulingGroupAttribute);

        /// <summary>Gets all agent scheduling groups count by skill tag identifier.</summary>
        /// <param name="skillTagIdDetails">The skill tag identifier details.</param>
        /// <param name="agentSchedulingGroupIdDetails">The agent scheduling group identifier details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<int> GetAllAgentSchedulingGroupsCountBySkillTagId(SkillTagIdDetails skillTagIdDetails, AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails);

        /// <summary>Gets the agent scheduling groups by skill tag identifier.</summary>
        /// <param name="skillTagIdDetails">The skill tag identifier details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<List<AgentSchedulingGroup>> GetAgentSchedulingGroupsBySkillTagId(SkillTagIdDetails skillTagIdDetails);

        /// <summary>
        /// Gets the name of the agent scheduling group identifier by skill tag identifier and group.
        /// </summary>
        /// <param name="skillTagIdDetails">The skill tag identifier details.</param>
        /// <param name="agentSchedulingGroupNameDetails">The agent scheduling group name details.</param>
        /// <returns></returns>
        Task<List<int>> GetAgentSchedulingGroupIdBySkillTagIdAndTagName(SkillTagIdDetails skillTagIdDetails, AgentSchedulingGroupNameDetails agentSchedulingGroupNameDetails);

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

        /// <summary>
        /// Deletes the agent scheduling group.
        /// </summary>
        /// <param name="agentSchedulingGroupRequest">The agent scheduling group request.</param>
        void DeleteAgentSchedulingGroup(AgentSchedulingGroup agentSchedulingGroupRequest);
    }
}
