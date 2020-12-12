using AutoMapper;
using Css.Api.Core.DataAccess.Repository.NoSQL;
using Css.Api.Core.DataAccess.Repository.NoSQL.Interfaces;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Utilities.Extensions;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.SkillGroup;
using Css.Api.Scheduling.Repository.Interfaces;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Repository
{
    public class SkillGroupRepository : GenericRepository<SkillGroup>, ISkillGroupRepository
    {
        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>Initializes a new instance of the <see cref="SkillGroupRepository" /> class.</summary>
        /// <param name="mongoContext">The mongo context.</param>
        /// <param name="mapper">The mapper.</param>
        public SkillGroupRepository(IMongoContext mongoContext,
            IMapper mapper) : base(mongoContext)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the skill groups.
        /// </summary>
        /// <param name="skillGroupQueryparameter">The skill group queryparameter.</param>
        /// <returns></returns>
        public async Task<PagedList<Entity>> GetSkillGroups(SkillGroupQueryparameter skillGroupQueryparameter)
        {
            var skillGroups = FilterBy(x => true);

            var filteredSkillGroups = FilterSkillGroups(skillGroups, skillGroupQueryparameter);

            var sortedSkillGroups = SortHelper.ApplySort(filteredSkillGroups, skillGroupQueryparameter.OrderBy);

            var pagedSkillGroups = sortedSkillGroups
                .Skip((skillGroupQueryparameter.PageNumber - 1) * skillGroupQueryparameter.PageSize)
                .Take(skillGroupQueryparameter.PageSize);

            var shapedSkillGroups = DataShaper.ShapeData(pagedSkillGroups, skillGroupQueryparameter.Fields);

            return await PagedList<Entity>
                .ToPagedList(shapedSkillGroups, filteredSkillGroups.Count(), skillGroupQueryparameter.PageNumber, skillGroupQueryparameter.PageSize);
        }

        /// <summary>
        /// Gets the skill group.
        /// </summary>
        /// <param name="skillGroupIdDetails">The skill group identifier details.</param>
        /// <returns></returns>
        public async Task<SkillGroup> GetSkillGroup(SkillGroupIdDetails skillGroupIdDetails)
        {
            var query = Builders<SkillGroup>.Filter.Eq(i => i.SkillGroupId, skillGroupIdDetails.SkillGroupId);

            return await FindByIdAsync(query);
        }

        /// <summary>
        /// Gets the skill groups count.
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetSkillGroupsCount()
        {
            var count = FilterBy(x => true)
                .Count();

            return await Task.FromResult(count);
        }

        /// <summary>
        /// Creates the skill group.
        /// </summary>
        /// <param name="skillGroupRequest">The skill group request.</param>
        public void CreateSkillGroup(SkillGroup skillGroupRequest)
        {
            InsertOneAsync(skillGroupRequest);
        }

        /// <summary>
        /// Updates the skill group.
        /// </summary>
        /// <param name="skillGroupRequest">The skill group request.</param>
        public void UpdateSkillGroup(SkillGroup skillGroupRequest)
        {
            ReplaceOneAsync(skillGroupRequest);
        }

        /// <summary>
        /// Filters the skill groups.
        /// </summary>
        /// <param name="skillGroups">The skill groups.</param>
        /// <param name="skillGroupQueryparameter">The skill group queryparameter.</param>
        /// <returns></returns>
        private IQueryable<SkillGroup> FilterSkillGroups(IQueryable<SkillGroup> skillGroups, SkillGroupQueryparameter skillGroupQueryparameter)
        {
            if (!skillGroups.Any())
            {
                return skillGroups;
            }

            if (!string.IsNullOrWhiteSpace(skillGroupQueryparameter.SearchKeyword))
            {
                skillGroups = skillGroups.Where(o => string.Equals(o.Name, skillGroupQueryparameter.SearchKeyword, StringComparison.OrdinalIgnoreCase));
            }

            return skillGroups;
        }
    }
}
