using Css.Api.Core.Models.Domain.NoSQL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Reporting.Repository.Interfaces
{
    /// <summary>
    /// The interface for agent scheduling group collection repository
    /// </summary>
    public interface IAgentSchedulingGroupRepository
    {
        /// <summary>
        /// Method to fetch all agent scheduling groups
        /// </summary>
        /// <returns>A list of instances of AgentSchedulingGroup</returns>
        Task<List<AgentSchedulingGroup>> GetAgentSchedulingGroups();

        /// <summary>
        /// Method to fetch agent scheduling groups by input identifiers
        /// </summary>
        /// <param name="agentSchedulingGroupIds">The agent scheduling group identifiers</param>
        /// <param name="estartProvisioning">The estart provisioning flag</param>
        /// <returns>A list of instances of AgentSchedulingGroup in the input identifiers</returns>
        Task<List<AgentSchedulingGroup>> GetAgentSchedulingGroupsByIds(List<int> agentSchedulingGroupIds, bool? estartProvisioning = null);

        /// <summary>
        /// Method to fetch agent scheduling groups in input timezones
        /// </summary>
        /// <param name="timezoneIds">The timezone identifiers</param>
        /// <param name="estartProvisioning">The estart provisioning flag</param>
        /// <returns>A list of instances of AgentSchedulingGroup in the input timezones</returns>
        Task<List<AgentSchedulingGroup>> GetAgentSchedulingGroups(List<int> timezoneIds, bool? estartProvisioning = null);
    }
}
