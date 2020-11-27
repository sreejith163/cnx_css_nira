using AutoMapper;
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

            var filteredAgentSchedulingGroups = FilterAgentSchedulingGroup(agentSchedulingGroups, agentSchedulingGroupQueryParameter.SearchKeyword,
                agentSchedulingGroupQueryParameter.ClientId, agentSchedulingGroupQueryParameter.ClientLobGroupId,
                agentSchedulingGroupQueryParameter.SkillGroupId, agentSchedulingGroupQueryParameter.SkillTagId)
                .Include(x => x.OperationHour);

            var sortedAgentSchedulingGroups = SortHelper.ApplySort(filteredAgentSchedulingGroups, agentSchedulingGroupQueryParameter.OrderBy);

            var pagedAgentSchedulingGroups = sortedAgentSchedulingGroups
                .Skip((agentSchedulingGroupQueryParameter.PageNumber - 1) * agentSchedulingGroupQueryParameter.PageSize)
                .Take(agentSchedulingGroupQueryParameter.PageSize)
                .Include(x => x.SkillTag)
                .Include(x => x.SkillGroup)
                .Include(x => x.Client)
                .Include(x => x.ClientLobGroup);

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


        /// <summary>Filters the agent scheduling group.</summary>
        /// <param name="agentSchedulingGroups">The agent scheduling groups.</param>
        /// <param name="agentSchedulingGroupName">Name of the agent scheduling group.</param>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="clientLobGroupId">The client lob group identifier.</param>
        /// <param name="skillGroupId">The skill group identifier.</param>
        /// <param name="skillTagId">The skill tag identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        private IQueryable<AgentSchedulingGroup> FilterAgentSchedulingGroup(IQueryable<AgentSchedulingGroup> agentSchedulingGroups, string agentSchedulingGroupName,
            int? clientId, int? clientLobGroupId, int? skillGroupId, int? skillTagId)
        {
            if (!agentSchedulingGroups.Any())
            {
                return agentSchedulingGroups;
            }

            if (clientId.HasValue && clientId != default(int))
            {
                agentSchedulingGroups = agentSchedulingGroups.Where(x => x.ClientId == clientId);
            }

            if (clientLobGroupId.HasValue && clientLobGroupId != default(int))
            {
                agentSchedulingGroups = agentSchedulingGroups.Where(x => x.ClientLobGroupId == clientLobGroupId);
            }

            if (skillGroupId.HasValue && skillGroupId != default(int))
            {
                agentSchedulingGroups = agentSchedulingGroups.Where(x => x.SkillGroupId == skillGroupId);
            }

            if (skillTagId.HasValue && skillTagId != default(int))
            {
                agentSchedulingGroups = agentSchedulingGroups.Where(x => x.SkillTagId == skillTagId);
            }

            if (!string.IsNullOrWhiteSpace(agentSchedulingGroupName))
            {
                agentSchedulingGroups = agentSchedulingGroups.Where(o => o.Name.ToLower().Contains(agentSchedulingGroupName.Trim().ToLower()));
            }

            return agentSchedulingGroups;
        }
    }
}
