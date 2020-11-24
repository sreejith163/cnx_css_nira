using AutoMapper;
using AutoMapper.QueryableExtensions;
using Css.Api.Core.DataAccess.Repository;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Utilities.Extensions;
using Css.Api.SetupMenu.Models.Domain;
using Css.Api.SetupMenu.Models.DTO.Request.ClientLOBGroup;
using Css.Api.SetupMenu.Models.DTO.Request.SkillGroup;
using Css.Api.SetupMenu.Models.DTO.Response.SkillGroup;
using Css.Api.SetupMenu.Repository.DatabaseContext;
using Css.Api.SetupMenu.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Css.Api.SetupMenu.Repository
{
    public class SkillGroupRepository : GenericRepository<SkillGroup>, ISkillGroupRepository
    {
        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="SkillGroupRepository" /> class.
        /// </summary>
        /// <param name="repositoryContext">The repository context.</param>
        /// <param name="mapper">The mapper.</param>
        public SkillGroupRepository(
            SetupMenuContext repositoryContext,
            IMapper mapper)
            : base(repositoryContext)
        {
            _mapper = mapper;
        }

        /// <summary>Gets the skill groups.</summary>
        /// <param name="skillGroupQueryParameter">The skill group query parameter.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<PagedList<Entity>> GetSkillGroups(SkillGroupQueryParameter skillGroupQueryParameter)
        {
            var skillGroups = FindByCondition(x => x.IsDeleted == false);

            var filteredSkillGroups = FilterSkillGroups(skillGroups, skillGroupQueryParameter.SearchKeyword, skillGroupQueryParameter.ClientId, skillGroupQueryParameter.ClientLobGroupId)
                .Include(x => x.Timezone);

            var sortedSkillGroups = SortHelper.ApplySort(filteredSkillGroups, skillGroupQueryParameter.OrderBy);

            var pagedSkillGroups = sortedSkillGroups
                .Skip((skillGroupQueryParameter.PageNumber - 1) * skillGroupQueryParameter.PageSize)
                .Take(skillGroupQueryParameter.PageSize)
                .Include(x => x.Client)
                .Include(x => x.ClientLobGroup);

            var mappedSkillGroups = pagedSkillGroups
                .ProjectTo<SkillGroupDTO>(_mapper.ConfigurationProvider);

            var shapedSKillGroups = DataShaper.ShapeData(mappedSkillGroups, skillGroupQueryParameter.Fields);

            return await PagedList<Entity>
                .ToPagedList(shapedSKillGroups, filteredSkillGroups.Count(), skillGroupQueryParameter.PageNumber, skillGroupQueryParameter.PageSize);
        }

        /// <summary>Gets the skill group.</summary>
        /// <param name="skillGroupIdDetails">The skill group identifier details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<SkillGroup> GetSkillGroup(SkillGroupIdDetails skillGroupIdDetails)
        {
            var skillGroup = FindByCondition(x => x.Id == skillGroupIdDetails.SkillGroupId && x.IsDeleted == false)
                .Include(x => x.Client)
                .Include(x => x.ClientLobGroup)
                .Include(x => x.Timezone)
                .Include(x => x.OperationHour)
                .SingleOrDefault();

            return await Task.FromResult(skillGroup);
        }

        /// <summary>Gets the skill groups count by client lob identifier.</summary>
        /// <param name="clientLOBGroupIdDetails">The client lob group identifier details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<int> GetSkillGroupsCountByClientLobId(ClientLOBGroupIdDetails clientLOBGroupIdDetails)
        {
            var count = FindByCondition(x => x.ClientLobGroupId == clientLOBGroupIdDetails.ClientLOBGroupId && x.IsDeleted == false)
                .Count();

            return await Task.FromResult(count);
        }

        /// <summary>Gets the name of the skill group ids by client lob identifier and skill group.</summary>
        /// <param name="clientLOBGroupIdDetails">The client lob group identifier details.</param>
        /// <param name="skillGroupNameDetails">The skill group name details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<List<int>> GetSkillGroupIdsByClientLobIdAndSkillGroupName(ClientLOBGroupIdDetails clientLOBGroupIdDetails,
            SkillGroupNameDetails skillGroupNameDetails)
        {
            var count = FindByCondition
                (x => x.ClientLobGroupId == clientLOBGroupIdDetails.ClientLOBGroupId && string.Equals(x.Name, skillGroupNameDetails.Name, StringComparison.OrdinalIgnoreCase) && x.IsDeleted == false)
                .Select(x => x.Id)
                .ToList();

            return await Task.FromResult(count);
        }

        /// <summary>Creates the skill group.</summary>
        /// <param name="skillGroupDetails">The skill group details.</param>
        public void CreateSkillGroup(SkillGroup skillGroupDetails)
        {
            Create(skillGroupDetails);
        }

        /// <summary>Updates the skill group.</summary>
        /// <param name="skillGroupDetails">The skill group details.</param>
        public void UpdateSkillGroup(SkillGroup skillGroupDetails)
        {
            Update(skillGroupDetails);
        }

        /// <summary>Deletes the client lob group.</summary>
        /// <param name="clientLobGroup">The client lob group.</param>
        public void DeleteSkillGroup(SkillGroup skillGroupDetails)
        {
            Delete(skillGroupDetails);
        }

        /// <summary>Filters the skill groups.</summary>
        /// <param name="skillGroups">The skill groups.</param>
        /// <param name="skillGroupName">Name of the skill group.</param>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="clientLobGroupId">The client lob group identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        private IQueryable<SkillGroup> FilterSkillGroups(IQueryable<SkillGroup> skillGroups, string skillGroupName, int? clientId, int? clientLobGroupId)
        {
            if (!skillGroups.Any())
            {
                return skillGroups;
            }

            if (clientId.HasValue && clientId != default(int))
            {
                skillGroups = skillGroups.Where(x => x.ClientId == clientId);
            }

            if (clientLobGroupId.HasValue && clientLobGroupId != default(int))
            {
                skillGroups = skillGroups.Where(x => x.ClientLobGroupId == clientLobGroupId);
            }

            if (!string.IsNullOrWhiteSpace(skillGroupName))
            {
                skillGroups = skillGroups.Where(o => o.Name.ToLower().Contains(skillGroupName.Trim().ToLower()));
            }

            return skillGroups;
        }
    }
}