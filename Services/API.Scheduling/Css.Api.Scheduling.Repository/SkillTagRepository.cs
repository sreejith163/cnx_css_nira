using AutoMapper;
using AutoMapper.QueryableExtensions;
using Css.Api.Core.DataAccess.Repository;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Utilities.Extensions;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.SkillGroup;
using Css.Api.Scheduling.Models.DTO.Request.SkillTag;
using Css.Api.Scheduling.Models.DTO.Response.SkillTag;
using Css.Api.Scheduling.Repository.DatabaseContext;
using Css.Api.Scheduling.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Repository
{
    public class SkillTagRepository : GenericRepository<SkillTag>, ISkillTagRepository
    {
        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientLOBGroupRepository" /> class.
        /// </summary>
        /// <param name="repositoryContext">The repository context.</param>
        /// <param name="mapper">The mapper.</param>
        public SkillTagRepository(
            SchedulingContext repositoryContext,
            IMapper mapper)
            : base(repositoryContext)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the skill tags.
        /// </summary>
        /// <param name="skillTagQueryParameter">The skill tag query parameter.</param>
        /// <returns></returns>
        public async Task<PagedList<Entity>> GetSkillTags(SkillTagQueryParameter skillTagQueryParameter)
        {
            var skillTags = FindByCondition(x => x.IsDeleted == false);

            var filteredSkillTags = FilterSkillTag(skillTags, skillTagQueryParameter.SearchKeyword, skillTagQueryParameter.SkillGroupId,
                skillTagQueryParameter.ClientId, skillTagQueryParameter.ClientLobGroupId)
                .Include(x => x.OperationHour);

            var sortedSkillTags = SortHelper.ApplySort(filteredSkillTags, skillTagQueryParameter.OrderBy);

            var pagedSkillTags = sortedSkillTags
                .Skip((skillTagQueryParameter.PageNumber - 1) * skillTagQueryParameter.PageSize)
                .Take(skillTagQueryParameter.PageSize)
                .Include(x => x.SkillGroup)
                .Include(x => x.Client)
                .Include(x => x.ClientLobGroup);

            var mappedSkillTags = pagedSkillTags
                .ProjectTo<SkillTagDTO>(_mapper.ConfigurationProvider);

            var shapedSkillTags = DataShaper.ShapeData(mappedSkillTags, skillTagQueryParameter.Fields);

            return await PagedList<Entity>
                .ToPagedList(shapedSkillTags, filteredSkillTags.Count(), skillTagQueryParameter.PageNumber, skillTagQueryParameter.PageSize);
        }

        /// <summary>
        /// Gets the skill tag.
        /// </summary>
        /// <param name="skillTagIdDetails">The skill tag identifier details.</param>
        /// <returns></returns>
        public async Task<SkillTag> GetSkillTag(SkillTagIdDetails skillTagIdDetails)
        {
            var skillTag = FindByCondition(x => x.Id == skillTagIdDetails.SkillTagId && x.IsDeleted == false)
                            .Include(x => x.OperationHour)
                            .Include(x => x.Client)
                            .Include(x => x.ClientLobGroup)
                            .Include(x => x.SkillGroup)
                            .SingleOrDefault();

            return await Task.FromResult(skillTag);
        }

        /// <summary>
        /// Gets the skill tags count by skill group identifier.
        /// </summary>
        /// <param name="skillGroupIdDetails">The skill group identifier details.</param>
        /// <returns></returns>
        public async Task<int>  GetSkillTagsCountBySkillGroupId(SkillGroupIdDetails skillGroupIdDetails)
        {
            var count = FindByCondition(x => x.SkillGroupId == skillGroupIdDetails.SkillGroupId && x.IsDeleted == false)
                .Count();

            return await Task.FromResult(count);
        }

        /// <summary>
        /// Gets the name of the skill tag identifier by skill group identifier and group.
        /// </summary>
        /// <param name="skillGroupIdDetails">The skill group identifier details.</param>
        /// <param name="skillTagNameDetails">The skill tag name details.</param>
        /// <returns></returns>
        public async Task<List<int>> GetSkillTagIdBySkillGroupIdAndGroupName(SkillGroupIdDetails skillGroupIdDetails, SkillTagNameDetails skillTagNameDetails)
        {
            var count = FindByCondition
                (x => x.SkillGroupId == skillGroupIdDetails.SkillGroupId && string.Equals(x.Name, skillTagNameDetails.Name, StringComparison.OrdinalIgnoreCase) && x.IsDeleted == false)
                .Select(x => x.Id)
                .ToList();

            return await Task.FromResult(count);
        }

        /// <summary>
        /// Creates the skill tag.
        /// </summary>
        /// <param name="skillTagRequest">The skill tag request.</param>
        public void CreateSkillTag(SkillTag skillTagRequest)
        {
            Create(skillTagRequest);
        }

        /// <summary>
        /// Updates the skill tag.
        /// </summary>
        /// <param name="skillTagRequest">The skill tag request.</param>
        public void UpdateSkillTag(SkillTag skillTagRequest)
        {
            Update(skillTagRequest);
        }

        /// <summary>
        /// Deletes the skill tag.
        /// </summary>
        /// <param name="skillTagRequest">The skill tag request.</param>
        public void DeleteSkillTag(SkillTag skillTagRequest)
        {
            Delete(skillTagRequest);
        }

        /// <summary>
        /// Filters the skill tag.
        /// </summary>
        /// <param name="skillTags">The skill tags.</param>
        /// <param name="skillTagName">Name of the skill tag.</param>
        /// <param name="skillGroupId">The skill group identifier.</param>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="clientLobGroupId">The client lob group identifier.</param>
        /// <returns></returns>
        private IQueryable<SkillTag> FilterSkillTag(IQueryable<SkillTag> skillTags, string skillTagName, int? skillGroupId,
             int? clientId, int? clientLobGroupId)
        {
            if (!skillTags.Any())
            {
                return skillTags;
            }

            if (clientId.HasValue && clientId != default(int))
            {
                skillTags = skillTags.Where(x => x.ClientId == clientId);
            }

            if (clientLobGroupId.HasValue && clientLobGroupId != default(int))
            {
                skillTags = skillTags.Where(x => x.ClientLobGroupId == clientLobGroupId);
            }

            if (skillGroupId.HasValue && skillGroupId != default(int))
            {
                skillTags = skillTags.Where(x => x.SkillGroupId == skillGroupId);
            }

            if (!string.IsNullOrWhiteSpace(skillTagName))
            {
                skillTags = skillTags.Where(o => o.Name.ToLower().Contains(skillTagName.Trim().ToLower()));
            }

            return skillTags;
        }
    }
}
