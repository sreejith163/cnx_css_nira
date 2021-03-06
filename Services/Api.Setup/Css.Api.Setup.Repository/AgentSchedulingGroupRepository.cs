﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Css.Api.Core.DataAccess.Repository.SQL;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Utilities.Extensions;
using Css.Api.Setup.Models.Domain;
using Css.Api.Setup.Models.DTO.Request.AgentSchedulingGroup;
using Css.Api.Setup.Models.DTO.Request.SkillTag;
using Css.Api.Setup.Models.DTO.Response.AgentSchedulingGroup;
using Css.Api.Setup.Repository.DatabaseContext;
using Css.Api.Setup.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Css.Api.Setup.Repository
{
    public class AgentSchedulingGroupRepository : GenericRepository<AgentSchedulingGroup>, IAgentSchedulingGroupRepository
    {
        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentSchedulingGroupRepository" /> class.
        /// </summary>
        /// <param name="repositoryContext">The repository context.</param>
        /// <param name="mapper">The mapper.</param>
        public AgentSchedulingGroupRepository(
            SetupContext repositoryContext,
            IMapper mapper)
            : base(repositoryContext)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the agent scheduling groups.
        /// </summary>
        /// <param name="agentSchedulingGroupQueryParameter">The agent scheduling group query parameter.</param>
        /// <returns></returns>
        public async Task<PagedList<Entity>> GetAgentSchedulingGroups(AgentSchedulingGroupQueryParameter agentSchedulingGroupQueryParameter)
        {
            var agentSchedulingGroups = FindByCondition(x => x.IsDeleted == false);

            var filteredAgentSchedulingGroups = FilterAgentSchedulingGroup(agentSchedulingGroups, agentSchedulingGroupQueryParameter)
                .Include(x => x.OperationHour);

            var sortedAgentSchedulingGroups = SortHelper.ApplySort(filteredAgentSchedulingGroups, agentSchedulingGroupQueryParameter.OrderBy);

            var pagedAgentSchedulingGroups = sortedAgentSchedulingGroups;

            if (!agentSchedulingGroupQueryParameter.SkipPageSize)
            {
                pagedAgentSchedulingGroups = sortedAgentSchedulingGroups
                   .Skip((agentSchedulingGroupQueryParameter.PageNumber - 1) * agentSchedulingGroupQueryParameter.PageSize)
                   .Take(agentSchedulingGroupQueryParameter.PageSize)
                   .Include(x => x.SkillTag)
                   .Include(x => x.SkillGroup)
                   .Include(x => x.Client)
                   .Include(x => x.ClientLobGroup);
            }

            var mappedAgentSchedulingGroups = pagedAgentSchedulingGroups
                .ProjectTo<AgentSchedulingGroupDTO>(_mapper.ConfigurationProvider);

            var shapedAgentSchedulingGroups = DataShaper.ShapeData(mappedAgentSchedulingGroups, agentSchedulingGroupQueryParameter.Fields);

            return await PagedList<Entity>
                .ToPagedList(shapedAgentSchedulingGroups, filteredAgentSchedulingGroups.Count(), agentSchedulingGroupQueryParameter.PageNumber, agentSchedulingGroupQueryParameter.PageSize);
        }

        /// <summary>
        /// Gets the agent scheduling group.
        /// </summary>
        /// <param name="agentSchedulingGroupIdDetails">The agent scheduling group identifier details.</param>
        /// <returns></returns>
        public async Task<AgentSchedulingGroup> GetAgentSchedulingGroup(AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails)
        {
            var agentSchedulingGroup = FindByCondition(x => x.Id == agentSchedulingGroupIdDetails.AgentSchedulingGroupId && x.IsDeleted == false)
                            .Include(x => x.OperationHour)
                            .Include(x => x.Client)
                            .Include(x => x.ClientLobGroup)
                            .Include(x => x.SkillGroup)
                            .Include(x => x.SkillTag)
                            .Include(x => x.Timezone)
                            .SingleOrDefault();

            return await Task.FromResult(agentSchedulingGroup);
        }

        /// <summary>Gets all agent scheduling group.</summary>
        /// <param name="agentSchedulingGroupIdDetails">The agent scheduling group identifier details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<AgentSchedulingGroup> GetAllAgentSchedulingGroup(AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails)
        {
            var agentSchedulingGroup = FindByCondition(x => x.Id == agentSchedulingGroupIdDetails.AgentSchedulingGroupId)
                            .Include(x => x.OperationHour)
                            .Include(x => x.Client)
                            .Include(x => x.ClientLobGroup)
                            .Include(x => x.SkillGroup)
                            .Include(x => x.SkillTag)
                            .Include(x => x.Timezone)
                            .SingleOrDefault();

            return await Task.FromResult(agentSchedulingGroup);
        }

        /// <summary>
        /// Gets the agent scheduling groups count by skill tag identifier.
        /// </summary>
        /// <param name="skillTagIdDetails">The skill tag identifier details.</param>
        /// <returns></returns>
        public async Task<int> GetAgentSchedulingGroupsCountBySkillTagId(SkillTagIdDetails skillTagIdDetails)
        {
            var count = FindByCondition(x => x.SkillTagId == skillTagIdDetails.SkillTagId && x.IsDeleted == false)
                .Count();

            return await Task.FromResult(count);
        }

        /// <summary>
        /// Gets the agent scheduling groups count by skill or refid tag identifier.
        /// </summary>
        /// <param name="agentSchedulingGroupAttribute"></param>
        /// <returns></returns>
        public async Task<List<AgentSchedulingGroup>> GetAgentSchedulingGroupsCountBySkillTagIdOrRefId(AgentSchedulingGroupAttribute agentSchedulingGroupAttribute)
        {
            var agentSchedulingGroups = FindByCondition(x => ((x.SkillTagId == agentSchedulingGroupAttribute.SkillTagId && string.Equals(x.Name.Trim(), agentSchedulingGroupAttribute.Name.Trim(),
                      StringComparison.OrdinalIgnoreCase)) || x.RefId == (agentSchedulingGroupAttribute.RefId ?? 0)) && x.IsDeleted == false).ToList();

            return await Task.FromResult(agentSchedulingGroups);
        }


        /// <summary>Gets all agent scheduling groups count by skill tag identifier.</summary>
        /// <param name="skillTagIdDetails">The skill tag identifier details.</param>
        /// <param name="agentSchedulingGroupIdDetails"></param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<int> GetAllAgentSchedulingGroupsCountBySkillTagId(SkillTagIdDetails skillTagIdDetails, AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails)
        {
            var count = FindByCondition(x => x.SkillTagId == skillTagIdDetails.SkillTagId && x.Id == agentSchedulingGroupIdDetails.AgentSchedulingGroupId)
                .Count();

            return await Task.FromResult(count);
        }

        /// <summary>Gets the agent scheduling groups by skill tag identifier.</summary>
        /// <param name="skillTagIdDetails">The skill tag identifier details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<List<AgentSchedulingGroup>> GetAgentSchedulingGroupsBySkillTagId(SkillTagIdDetails skillTagIdDetails)
        {
            var agentSchedulingGroups = FindByCondition(x => x.SkillTagId == skillTagIdDetails.SkillTagId && x.IsDeleted == false);

            return await Task.FromResult(agentSchedulingGroups.ToList());
        }

        /// <summary>
        /// Gets the name of the agent scheduling group identifier by skill tag identifier and group.
        /// </summary>
        /// <param name="skillTagIdDetails">The skill tag identifier details.</param>
        /// <param name="agentSchedulingGroupNameDetails">The agent scheduling group name details.</param>
        /// <returns></returns>
        public async Task<List<int>> GetAgentSchedulingGroupIdBySkillTagIdAndTagName(SkillTagIdDetails skillTagIdDetails, AgentSchedulingGroupNameDetails agentSchedulingGroupNameDetails)
        {
            var count = FindByCondition
                (x => x.SkillTagId == skillTagIdDetails.SkillTagId && string.Equals(x.Name, agentSchedulingGroupNameDetails.Name, StringComparison.OrdinalIgnoreCase) && x.IsDeleted == false)
                .Select(x => x.Id)
                .ToList();

            return await Task.FromResult(count);
        }

        /// <summary>
        /// Creates the agent scheduling group.
        /// </summary>
        /// <param name="agentSchedulingGroupRequest">The agent scheduling group request.</param>
        public void CreateAgentSchedulingGroup(AgentSchedulingGroup agentSchedulingGroupRequest)
        {
            Create(agentSchedulingGroupRequest);
        }

        /// <summary>
        /// Updates the agent scheduling group.
        /// </summary>
        /// <param name="agentSchedulingGroupRequest">The agent scheduling group request.</param>
        public void UpdateAgentSchedulingGroup(AgentSchedulingGroup agentSchedulingGroupRequest)
        {
            Update(agentSchedulingGroupRequest);
        }

        /// <summary>
        /// Deletes the agent scheduling group.
        /// </summary>
        /// <param name="agentSchedulingGroupRequest">The agent scheduling group request.</param>
        public void DeleteAgentSchedulingGroup(AgentSchedulingGroup agentSchedulingGroupRequest)
        {
            Delete(agentSchedulingGroupRequest);
        }


        /// <summary>
        /// Filters the agent scheduling group.
        /// </summary>
        /// <param name="agentSchedulingGroups">The agent scheduling groups.</param>
        /// <param name="agentSchedulingGroupQueryParameter">The agent scheduling group query parameter.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        private IQueryable<AgentSchedulingGroup> FilterAgentSchedulingGroup(IQueryable<AgentSchedulingGroup> agentSchedulingGroups, AgentSchedulingGroupQueryParameter agentSchedulingGroupQueryParameter)
        {
            if (!agentSchedulingGroups.Any())
            {
                return agentSchedulingGroups;
            }

            if (agentSchedulingGroupQueryParameter.ClientId.HasValue && agentSchedulingGroupQueryParameter.ClientId != default(int))
            {
                agentSchedulingGroups = agentSchedulingGroups.Where(x => x.ClientId == agentSchedulingGroupQueryParameter.ClientId);
            }

            if (agentSchedulingGroupQueryParameter.ClientLobGroupId.HasValue && agentSchedulingGroupQueryParameter.ClientLobGroupId != default(int))
            {
                agentSchedulingGroups = agentSchedulingGroups.Where(x => x.ClientLobGroupId == agentSchedulingGroupQueryParameter.ClientLobGroupId);
            }

            if (agentSchedulingGroupQueryParameter.SkillGroupId.HasValue && agentSchedulingGroupQueryParameter.SkillGroupId != default(int))
            {
                agentSchedulingGroups = agentSchedulingGroups.Where(x => x.SkillGroupId == agentSchedulingGroupQueryParameter.SkillGroupId);
            }

            if (agentSchedulingGroupQueryParameter.SkillTagId.HasValue && agentSchedulingGroupQueryParameter.SkillTagId != default(int))
            {
                agentSchedulingGroups = agentSchedulingGroups.Where(x => x.SkillTagId == agentSchedulingGroupQueryParameter.SkillTagId);
            }

            if (!string.IsNullOrWhiteSpace(agentSchedulingGroupQueryParameter.SearchKeyword))
            {
                agentSchedulingGroups = agentSchedulingGroups.Where(o => o.Name.ToLower().Contains(agentSchedulingGroupQueryParameter.SearchKeyword.Trim().ToLower()));
            }

            return agentSchedulingGroups;
        }
    }
}
