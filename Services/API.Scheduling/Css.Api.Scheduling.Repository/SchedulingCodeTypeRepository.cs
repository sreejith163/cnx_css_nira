using Css.Api.Core.DataAccess.Repository;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Repository.DatabaseContext;
using Css.Api.Scheduling.Repository.Interfaces;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Css.Api.Scheduling.Models.DTO.Response.SchedulingCodeIcon;
using System.Collections.Generic;
using System.Linq;

namespace Css.Api.Scheduling.Repository
{
    public class SchedulingCodeTypeRepository : GenericRepository<SchedulingCodeIcon>, ISchedulingCodeTypeRepository
    {
        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulingCodeTypeRepository" /> class.
        /// </summary>
        /// <param name="repositoryContext">The repository context.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="">The .</param>
        public SchedulingCodeTypeRepository(
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
        public async Task<List<SchedulingCodeTypeDTO>> GetSchedulingCodeTypes()
        {
            var schedulingCodeTypes = FindAll().ProjectTo<SchedulingCodeTypeDTO>(_mapper.ConfigurationProvider).ToList();
            return await Task.FromResult(schedulingCodeTypes);
        }
    }
}
