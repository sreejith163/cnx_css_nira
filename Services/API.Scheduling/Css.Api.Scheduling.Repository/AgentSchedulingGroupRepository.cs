﻿using Css.Api.Core.DataAccess.Repository.NoSQL;
using Css.Api.Core.DataAccess.Repository.NoSQL.Interfaces;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Core.Utilities.Extensions;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedulingGroup;
using Css.Api.Scheduling.Models.DTO.Request.SkillGroup;
using Css.Api.Scheduling.Models.DTO.Request.SkillTag;
using Css.Api.Scheduling.Repository.Interfaces;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Repository
{
    public class AgentSchedulingGroupRepository : GenericRepository<AgentSchedulingGroup>, IAgentSchedulingGroupRepository
    {
        /// <summary>Initializes a new instance of the <see cref="AgentSchedulingGroupRepository" /> class.</summary>
        /// <param name="mongoContext">The mongo context.</param>
        /// <param name="mapper">The mapper.</param>
        public AgentSchedulingGroupRepository(IMongoContext mongoContext) : base(mongoContext)
        {
        }

        /// <summary>
        /// Gets the agent scheduling groups.
        /// </summary>
        /// <param name="agentSchedulingGroupQueryparameter">The agent scheduling group queryparameter.</param>
        /// <returns></returns>
        public async Task<PagedList<Entity>> GetAgentSchedulingGroups(AgentSchedulingGroupQueryparameter agentSchedulingGroupQueryparameter)
        {
            var agentSchedulingGroups = FilterBy(x => x.IsDeleted == false);

            var filteredAgentSchedulingGroups = FilterAgentSchedulingGroups(agentSchedulingGroups, agentSchedulingGroupQueryparameter);

            var sortedAgentSchedulingGroups = SortHelper.ApplySort(filteredAgentSchedulingGroups, agentSchedulingGroupQueryparameter.OrderBy);

            var pagedAgentSchedulingGroups = sortedAgentSchedulingGroups;

            if (!agentSchedulingGroupQueryparameter.SkipPageSize)
            {
                pagedAgentSchedulingGroups = sortedAgentSchedulingGroups
                   .Skip((agentSchedulingGroupQueryparameter.PageNumber - 1) * agentSchedulingGroupQueryparameter.PageSize)
                   .Take(agentSchedulingGroupQueryparameter.PageSize);
            }

            var shapedAgentSchedulingGroups = DataShaper.ShapeData(pagedAgentSchedulingGroups, agentSchedulingGroupQueryparameter.Fields);

            return await PagedList<Entity>
                .ToPagedList(shapedAgentSchedulingGroups, filteredAgentSchedulingGroups.Count(), agentSchedulingGroupQueryparameter.PageNumber, agentSchedulingGroupQueryparameter.PageSize);
        }

        /// <summary>
        /// Gets the agent scheduling group.
        /// </summary>
        /// <param name="agentSchedulingGroupIdDetails">The agent scheduling group identifier details.</param>
        /// <returns></returns>
        public async Task<AgentSchedulingGroup> GetAgentSchedulingGroup(AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails)
        {
            var query = Builders<AgentSchedulingGroup>.Filter.Eq(i => i.AgentSchedulingGroupId, agentSchedulingGroupIdDetails.AgentSchedulingGroupId) &
               Builders<AgentSchedulingGroup>.Filter.Eq(i => i.IsDeleted, false);

            return await FindByIdAsync(query);
        }

        /// <summary>
        /// Gets the agent scheduling group by skill tag.
        /// </summary>
        /// <param name="skillTagIdDetails">The skill tag identifier details.</param>
        /// <returns></returns>
        public async Task<IQueryable<AgentSchedulingGroup>> GetAgentSchedulingGroupBySkillTag(SkillTagIdDetails skillTagIdDetails)
        {
            var agentSchedulingGroups = FilterBy(x => x.IsDeleted == false && x.SkillTagId == skillTagIdDetails.SkillTagId);
            return await Task.FromResult(agentSchedulingGroups);
        }

        /// <summary>
        /// Gets the agent scheduling group by skill group identifier.
        /// </summary>
        /// <param name="skillGroupIdDetails">The skill group identifier details.</param>
        /// <returns></returns>
        public async Task<List<AgentSchedulingGroup>> GetAgentSchedulingGroupBySkillGroupId(int skillGroupId)
        {
            var query = Builders<AgentSchedulingGroup>.Filter.Eq(i => i.SkillGroupId, skillGroupId) &
               Builders<AgentSchedulingGroup>.Filter.Eq(i => i.IsDeleted, false);

            var result = FilterBy(query);
            return await Task.FromResult(result.ToList());
        }


        /// <summary>
        /// Gets the agent scheduling group list.
        /// </summary>
   
        /// <returns></returns>
        public async Task<List<AgentSchedulingGroup>> GetAgentSchedulingGroupList()
        {
            var query = Builders<AgentSchedulingGroup>.Filter.Eq(i => i.IsDeleted, false);

            var result = FilterBy(query);
            return await Task.FromResult(result.ToList());
        }
        /// <summary>
        /// Creates the agent scheduling group.
        /// </summary>
        /// <param name="agentSchedulingGroupRequest">The agent scheduling group request.</param>
        public void CreateAgentSchedulingGroup(AgentSchedulingGroup agentSchedulingGroupRequest)
        {
            InsertOneAsync(agentSchedulingGroupRequest);
        }

        /// <summary>
        /// Updates the agent scheduling group.
        /// </summary>
        /// <param name="agentSchedulingGroupRequest">The agent scheduling group request.</param>
        public void UpdateAgentSchedulingGroup(AgentSchedulingGroup agentSchedulingGroupRequest)
        {
            ReplaceOneAsync(agentSchedulingGroupRequest);
        }

        /// <summary>
        /// Filters the agent scheduling groups.
        /// </summary>
        /// <param name="agentSchedulingGroups">The agent scheduling groups.</param>
        /// <param name="agentSchedulingGroupQueryparameter">The agent scheduling group queryparameter.</param>
        /// <returns></returns>
        private IQueryable<AgentSchedulingGroup> FilterAgentSchedulingGroups(IQueryable<AgentSchedulingGroup> agentSchedulingGroups, AgentSchedulingGroupQueryparameter agentSchedulingGroupQueryparameter)
        {
            if (!agentSchedulingGroups.Any())
            {
                return agentSchedulingGroups;
            }

            if (agentSchedulingGroupQueryparameter.ClientId.HasValue && agentSchedulingGroupQueryparameter.ClientId != default(int))
            {
                agentSchedulingGroups = agentSchedulingGroups.Where(x => x.ClientId == agentSchedulingGroupQueryparameter.ClientId);
            }

            if (agentSchedulingGroupQueryparameter.ClientLobGroupId.HasValue && agentSchedulingGroupQueryparameter.ClientLobGroupId != default(int))
            {
                agentSchedulingGroups = agentSchedulingGroups.Where(x => x.ClientLobGroupId == agentSchedulingGroupQueryparameter.ClientLobGroupId);
            }

            if (agentSchedulingGroupQueryparameter.SkillGroupId.HasValue && agentSchedulingGroupQueryparameter.SkillGroupId != default(int))
            {
                agentSchedulingGroups = agentSchedulingGroups.Where(x => x.SkillGroupId == agentSchedulingGroupQueryparameter.SkillGroupId);
            }

            if (agentSchedulingGroupQueryparameter.SkillTagId.HasValue && agentSchedulingGroupQueryparameter.SkillTagId != default(int))
            {
                agentSchedulingGroups = agentSchedulingGroups.Where(x => x.SkillTagId == agentSchedulingGroupQueryparameter.SkillTagId);
            }

            if (!string.IsNullOrWhiteSpace(agentSchedulingGroupQueryparameter.SearchKeyword))
            {
                agentSchedulingGroups = agentSchedulingGroups.Where(o => o.Name.ToLower().Contains(agentSchedulingGroupQueryparameter.SearchKeyword.Trim().ToLower()));
            }

            return agentSchedulingGroups;
        }

        public async Task<List<AgentSchedulingGroup>> FindSchedulingGroup()
        {
            var query = Builders<AgentSchedulingGroup>.Filter.Eq(i => i.IsDeleted, false);


            var result = FilterBy(query);
            return await Task.FromResult(result.ToList());
        }
    }
}