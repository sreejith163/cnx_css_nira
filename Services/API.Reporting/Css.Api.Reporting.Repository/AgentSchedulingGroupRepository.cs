using Css.Api.Core.DataAccess.Repository.NoSQL;
using Css.Api.Core.DataAccess.Repository.NoSQL.Interfaces;
using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Reporting.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Reporting.Repository
{
    /// <summary>
    /// The repository for agent scheduling group collection
    /// </summary>
    public class AgentSchedulingGroupRepository : GenericRepository<AgentSchedulingGroup>, IAgentSchedulingGroupRepository
    {

        /// <summary>Initializes a new instance of the <see cref="AgentSchedulingGroupRepository" /> class.</summary>
        /// <param name="mongoContext">The mongo context.</param>
        public AgentSchedulingGroupRepository(IMongoContext mongoContext) : base(mongoContext)
        {

        }

        /// <summary>
        /// Method to fetch all agent scheduling groups
        /// </summary>
        /// <returns>A list of instances of AgentSchedulingGroup</returns>
        public async Task<List<AgentSchedulingGroup>> GetAgentSchedulingGroups()
        {
            var agentSchedulingGroups = FilterBy(x => !x.IsDeleted);
            return await Task.FromResult(agentSchedulingGroups.ToList());
        }

        /// <summary>
        /// Method to fetch all agent scheduling groups based on whether the EStart provisioning is enabled or not
        /// </summary>
        /// <param name="EstartProvisioning">The enabled or disable Estart flag</param>
        /// <returns>A list of instances of AgentSchedulingGroup</returns>
        public async Task<List<AgentSchedulingGroup>> GetAgentSchedulingGroups(bool EstartProvisioning)
        {
            var agentSchedulingGroups = FilterBy(x => !x.IsDeleted && x.EstartProvision  == EstartProvisioning);
            return await Task.FromResult(agentSchedulingGroups.ToList());
        }

        /// <summary>
        /// Method to fetch agent scheduling group by input id
        /// </summary>
        /// <param name="agentSchedulingGroupId"></param>
        /// <param name="estartProvisioning"></param>
        /// <returns>An instance of AgentSchedulingGroup if the id matches, else returns null</returns>
        public async Task<AgentSchedulingGroup> GetAgentSchedulingGroupsById(int agentSchedulingGroupId, bool? estartProvisioning = null)
        {
            IQueryable<AgentSchedulingGroup> agentSchedulingGroups;
            if (estartProvisioning.HasValue)
            {
                agentSchedulingGroups = FilterBy(x => x.AgentSchedulingGroupId == agentSchedulingGroupId && x.EstartProvision == estartProvisioning.Value);
            }
            else
            {
                agentSchedulingGroups = FilterBy(x => x.AgentSchedulingGroupId == agentSchedulingGroupId);
            }
            return await Task.FromResult(agentSchedulingGroups.FirstOrDefault());
        }

        /// <summary>
        /// Method to fetch agent scheduling groups by input identifiers
        /// </summary>
        /// <param name="agentSchedulingGroupIds">The agent scheduling group identifiers</param>
        /// <returns>A list of instances of AgentSchedulingGroup in the input identifiers</returns>
        public async Task<List<AgentSchedulingGroup>> GetAgentSchedulingGroupsByIds(List<int> agentSchedulingGroupIds, bool? estartProvisioning = null)
        {
            IQueryable<AgentSchedulingGroup> agentSchedulingGroups;
            if (estartProvisioning.HasValue)
            {
                agentSchedulingGroups = FilterBy(x => agentSchedulingGroupIds.Contains(x.AgentSchedulingGroupId) && x.EstartProvision == estartProvisioning.Value);
            }
            else
            {
                agentSchedulingGroups = FilterBy(x => agentSchedulingGroupIds.Contains(x.AgentSchedulingGroupId));
            }
            return await Task.FromResult(agentSchedulingGroups.ToList());
        }

        /// <summary>
        /// Method to fetch agent scheduling groups in input timezones
        /// </summary>
        /// <param name="timezoneIds">The timezone identifiers</param>
        /// <returns>A list of instances of AgentSchedulingGroup in the input timezones</returns>
        public async Task<List<AgentSchedulingGroup>> GetAgentSchedulingGroups(List<int> timezoneIds, bool? estartProvisioning = null)
        {
            IQueryable<AgentSchedulingGroup> agentSchedulingGroups;
            if (estartProvisioning.HasValue)
            {
                agentSchedulingGroups = FilterBy(x => timezoneIds.Contains(x.TimezoneId) && x.EstartProvision == estartProvisioning.Value);
            }
            else
            {
                agentSchedulingGroups = FilterBy(x => timezoneIds.Contains(x.TimezoneId));
            }
            return await Task.FromResult(agentSchedulingGroups.ToList());
        }
    }
}
