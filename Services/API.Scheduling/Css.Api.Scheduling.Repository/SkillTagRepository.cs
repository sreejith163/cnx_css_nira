using AutoMapper;
using Css.Api.Core.DataAccess.Repository.NoSQL;
using Css.Api.Core.DataAccess.Repository.NoSQL.Interfaces;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Utilities.Extensions;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.SkillTag;
using Css.Api.Scheduling.Repository.Interfaces;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Repository
{
    public class SkillTagRepository : GenericRepository<SkillTag>, ISkillTagRepository
    {
        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>Initializes a new instance of the <see cref="SkillTagRepository" /> class.</summary>
        /// <param name="mongoContext">The mongo context.</param>
        /// <param name="mapper">The mapper.</param>
        public SkillTagRepository(
            IMongoContext mongoContext,
            IMapper mapper) : base(mongoContext)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the skill tags.
        /// </summary>
        /// <param name="skillTagQueryparameter">The skill tag queryparameter.</param>
        /// <returns></returns>
        public async Task<PagedList<Entity>> GetSkillTags(SkillTagQueryparameter skillTagQueryparameter)
        {
            var skillTags = FilterBy(x => true);

            var filteredSkillTags = FilterSkillTags(skillTags, skillTagQueryparameter);

            var sortedSkillTags = SortHelper.ApplySort(filteredSkillTags, skillTagQueryparameter.OrderBy);

            var pagedSkillTags = sortedSkillTags
                .Skip((skillTagQueryparameter.PageNumber - 1) * skillTagQueryparameter.PageSize)
                .Take(skillTagQueryparameter.PageSize);

            var shapedSkillTags = DataShaper.ShapeData(pagedSkillTags, skillTagQueryparameter.Fields);

            return await PagedList<Entity>
                .ToPagedList(shapedSkillTags, filteredSkillTags.Count(), skillTagQueryparameter.PageNumber, skillTagQueryparameter.PageSize);
        }

        /// <summary>
        /// Gets the skill tag.
        /// </summary>
        /// <param name="skillTagIdDetails">The skill tag identifier details.</param>
        /// <returns></returns>
        public async Task<SkillTag> GetSkillTag(SkillTagIdDetails skillTagIdDetails)
        {
            var query = Builders<SkillTag>.Filter.Eq(i => i.SkillTagId, skillTagIdDetails.SkillTagId);

            return await FindByIdAsync(query);
        }

        /// <summary>
        /// Gets the skill tags count.
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetSkillTagsCount()
        {
            var count = FilterBy(x => true)
                .Count();

            return await Task.FromResult(count);
        }

        /// <summary>
        /// Creates the skill tag.
        /// </summary>
        /// <param name="skillTagRequest">The skill tag request.</param>
        public void CreateSkillTag(SkillTag skillTagRequest)
        {
            InsertOneAsync(skillTagRequest);
        }

        /// <summary>
        /// Updates the skill tag.
        /// </summary>
        /// <param name="skillTagRequest">The skill tag request.</param>
        public void UpdateSkillTag(SkillTag skillTagRequest)
        {
            ReplaceOneAsync(skillTagRequest);
        }

        /// <summary>
        /// Filters the skill tags.
        /// </summary>
        /// <param name="skillTags">The skill tags.</param>
        /// <param name="skillTagQueryparameter">The skill tag queryparameter.</param>
        /// <returns></returns>
        private IQueryable<SkillTag> FilterSkillTags(IQueryable<SkillTag> skillTags, SkillTagQueryparameter skillTagQueryparameter)
        {
            if (!skillTags.Any())
            {
                return skillTags;
            }

            if (!string.IsNullOrWhiteSpace(skillTagQueryparameter.SearchKeyword))
            {
                skillTags = skillTags.Where(o => o.Name.ToLower().Contains(skillTagQueryparameter.SearchKeyword.ToLower()));
            }

            return skillTags;
        }
    }
}