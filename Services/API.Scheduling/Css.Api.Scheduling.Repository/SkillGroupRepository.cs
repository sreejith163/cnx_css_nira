using AutoMapper;
using Css.Api.Core.DataAccess.Repository.NoSQL;
using Css.Api.Core.DataAccess.Repository.NoSQL.Interfaces;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.SkillGroup;
using Css.Api.Scheduling.Repository.Interfaces;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Repository
{
    public class SkillGroupRepository : GenericRepository<SkillGroupCollection>, ISkillGroupRepository
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


        /// <summary>Gets the skill group.</summary>
        /// <param name="skillGroupIdDetails">The skill group identifier details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<SkillGroupCollection> GetSkillGroup(SkillGroupIdDetails skillGroupIdDetails)
        {
            var query = Builders<SkillGroupCollection>.Filter.Eq(i => i.SkillGroupId, skillGroupIdDetails.SkillGroupId);

            return await FindByIdAsync(query);
        }


        public void CreateSkillGroups(ICollection<SkillGroupCollection> skillGroupRequestCollection)
        {
            InsertManyAsync(skillGroupRequestCollection);
        }

        public async Task<int> GetSkillGroupsCount()
        {
            var count = FilterBy(x => true)
                .Count();
            return await Task.FromResult(count);
        }
    }
}
