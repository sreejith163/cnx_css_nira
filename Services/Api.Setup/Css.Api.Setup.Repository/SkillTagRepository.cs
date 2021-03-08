using AutoMapper;
using AutoMapper.QueryableExtensions;
using Css.Api.Core.DataAccess.Repository.SQL;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Utilities.Extensions;
using Css.Api.Setup.Models.Domain;
using Css.Api.Setup.Models.DTO.Request.SkillGroup;
using Css.Api.Setup.Models.DTO.Request.SkillTag;
using Css.Api.Setup.Models.DTO.Response.SkillTag;
using Css.Api.Setup.Repository.DatabaseContext;
using Css.Api.Setup.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Css.Api.Setup.Repository
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
            SetupContext repositoryContext,
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

            var filteredSkillTags = FilterSkillTag(skillTags, skillTagQueryParameter)
                .Include(x => x.OperationHour);

            var sortedSkillTags = SortHelper.ApplySort(filteredSkillTags, skillTagQueryParameter.OrderBy);

            var pagedSkillTags = sortedSkillTags;

            if (!skillTagQueryParameter.SkipPageSize)
            {
                pagedSkillTags = sortedSkillTags
                   .Skip((skillTagQueryParameter.PageNumber - 1) * skillTagQueryParameter.PageSize)
                   .Take(skillTagQueryParameter.PageSize)
                   .Include(x => x.SkillGroup)
                   .Include(x => x.Client)
                   .Include(x => x.ClientLobGroup);
            }

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

        /// <summary>Gets all skill tag.</summary>
        /// <param name="skillTagIdDetails">The skill tag identifier details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<SkillTag> GetAllSkillTag(SkillTagIdDetails skillTagIdDetails)
        {
            var skillTag = FindByCondition(x => x.Id == skillTagIdDetails.SkillTagId)
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
        public async Task<int> GetSkillTagsCountBySkillGroupId(SkillGroupIdDetails skillGroupIdDetails)
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
                (x => x.SkillGroupId == skillGroupIdDetails.SkillGroupId && string.Equals(x.Name.Trim(), skillTagNameDetails.Name.Trim(),
                      StringComparison.OrdinalIgnoreCase) && x.IsDeleted == false)
                .Select(x => x.Id)
                .ToList();

            return await Task.FromResult(count);
        }

        /// <summary>
        /// Gets the name or refid of the skill tag identifier by skill group identifier and group.
        /// </summary>
        /// <param name="skillTagAttribute"></param>
        /// <returns></returns>
        public async Task<List<SkillTag>> GetSkillTagIdBySkillGroupIdAndGroupNameOrRefId(SkillTagAttribute skillTagAttribute)
        {
            var skillTags = FindByCondition
            (x => x.SkillGroupId == skillTagAttribute.SkillGroupId && (string.Equals(x.Name.Trim(), skillTagAttribute.Name.Trim(),
                      StringComparison.OrdinalIgnoreCase) || x.RefId == (skillTagAttribute.RefId ?? 0)) && x.IsDeleted == false).ToList();

            return await Task.FromResult(skillTags);
        }

        /// <summary>Gets the name of all skill tag identifier by skill group identifier and group.</summary>
        /// <param name="skillGroupIdDetails">The skill group identifier details.</param>
        /// <param name="skillTagNameDetails">The skill tag name details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<List<int>> GetAllSkillTagIdBySkillGroupIdAndGroupName(SkillGroupIdDetails skillGroupIdDetails, SkillTagNameDetails skillTagNameDetails)
        {
            var count = FindByCondition
                (x => x.SkillGroupId == skillGroupIdDetails.SkillGroupId && string.Equals(x.Name.Trim(), skillTagNameDetails.Name.Trim(),
                      StringComparison.OrdinalIgnoreCase))
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
        /// <param name="skillTagQueryParameter">The skill tag query parameter.</param>
        /// <returns></returns>
        private IQueryable<SkillTag> FilterSkillTag(IQueryable<SkillTag> skillTags, SkillTagQueryParameter skillTagQueryParameter)
        {
            if (!skillTags.Any())
            {
                return skillTags;
            }

            if (skillTagQueryParameter.ClientId.HasValue && skillTagQueryParameter.ClientId != default(int))
            {
                skillTags = skillTags.Where(x => x.ClientId == skillTagQueryParameter.ClientId);
            }

            if (skillTagQueryParameter.ClientLobGroupId.HasValue && skillTagQueryParameter.ClientLobGroupId != default(int))
            {
                skillTags = skillTags.Where(x => x.ClientLobGroupId == skillTagQueryParameter.ClientLobGroupId);
            }

            if (skillTagQueryParameter.SkillGroupId.HasValue && skillTagQueryParameter.SkillGroupId != default(int))
            {
                skillTags = skillTags.Where(x => x.SkillGroupId == skillTagQueryParameter.SkillGroupId);
            }

            if (!string.IsNullOrWhiteSpace(skillTagQueryParameter.SearchKeyword))
            {
                skillTags = skillTags.Where(o => o.Name.ToLower().Contains(skillTagQueryParameter.SearchKeyword.Trim().ToLower()) ||
                                                 o.CreatedBy.ToLower().Contains(skillTagQueryParameter.SearchKeyword.Trim().ToLower()) ||
                                                 o.ModifiedBy.ToLower().Contains(skillTagQueryParameter.SearchKeyword.Trim().ToLower()));
            }

            return skillTags;
        }
    }
}
