using AutoMapper;
using Css.Api.Core.DataAccess.Repository.NoSQL;
using Css.Api.Core.DataAccess.Repository.NoSQL.Interfaces;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.SkillTag;
using Css.Api.Scheduling.Repository.Interfaces;
using MongoDB.Driver;
using System.Collections.Generic;
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
        public SkillTagRepository(IMongoContext mongoContext,
            IMapper mapper) : base(mongoContext)
        {
            _mapper = mapper;
        }

        /// <summary>Gets the skill tag.</summary>
        /// <param name="skillTagIdDetails">The skill tag identifier details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<SkillTag> GetSkillTag(SkillTagIdDetails skillTagIdDetails)
        {
            var query = Builders<SkillTag>.Filter.Eq(i => i.SkillTagId, skillTagIdDetails.SkillTagId);

            return await FindByIdAsync(query);
        }
        public void CreateSkillTags(ICollection<SkillTag> skillTagRequestCollection)
        {
            InsertManyAsync(skillTagRequestCollection);
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
    }
}