using Css.Api.Core.DataAccess.Repository;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Repository.DatabaseContext;
using Css.Api.Scheduling.Repository.Interfaces;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Collections.Generic;
using System.Linq;
using Css.Api.Core.Models.DTO.Response;

namespace Css.Api.Scheduling.Repository
{
    public class SchedulingCodeIconRepository : GenericRepository<SchedulingCodeIcon>, ISchedulingCodeIconRepository
    {
        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulingCodeRepository" /> class.
        /// </summary>
        /// <param name="repositoryContext">The repository context.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="">The .</param>
        public SchedulingCodeIconRepository(
            SchedulingContext repositoryContext,
            IMapper mapper)
            : base(repositoryContext)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the scheduling code icons.
        /// </summary>
        /// <returns></returns>
        public async Task<List<KeyValue>> GetSchedulingCodeIcons()
        {
            var schedulingCodeIcons = FindAll().ProjectTo<KeyValue>(_mapper.ConfigurationProvider).ToList();
            return await Task.FromResult(schedulingCodeIcons);
        }
    }
}
