using AutoMapper;
using AutoMapper.QueryableExtensions;
using Css.Api.Core.DataAccess.Repository;
using Css.Api.Core.Models.DTO.Response;
using Css.Api.SetupMenu.Models.Domain;
using Css.Api.SetupMenu.Repository.DatabaseContext;
using Css.Api.SetupMenu.Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Css.Api.SetupMenu.Repository
{
    public class TimezoneRepository : GenericRepository<Timezone>, ITimezoneRepository
    {
        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimezoneRepository" /> class.
        /// </summary>
        /// <param name="repositoryContext">The repository context.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="">The .</param>
        public TimezoneRepository(
            SetupMenuContext repositoryContext,
            IMapper mapper)
            : base(repositoryContext)
        {
            _mapper = mapper;
        }

        /// <summary>Gets the timezones.</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<List<KeyValue>> GetTimezones()
        {
            var timeZones = FindAll().ProjectTo<KeyValue>(_mapper.ConfigurationProvider).ToList();
            return await Task.FromResult(timeZones);
        }
    }
}
