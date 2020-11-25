using AutoMapper;
using Css.Api.Scheduling.Repository.DatabaseContext;
using Css.Api.Scheduling.Repository.Interfaces;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        /// <summary>
        /// Gets or sets the repository context.
        /// </summary>
        private SchedulingContext _repositoryContext { get; set; }

        /// <summary>
        /// Gets or sets the mapper.
        /// </summary>
        private IMapper _mapper { get; set; }

        /// <summary>
        /// Gets or sets the timezone repository.
        /// </summary>
        private ITimezoneRepository _timezoneRepository { get; set; }
                
        /// <summary>Gets the time zones.</summary>
        /// <value>The time zones.</value>
        public ITimezoneRepository TimeZones
        {
            get
            {
                if (_timezoneRepository == null)
                {
                    _timezoneRepository = new TimezoneRepository(_repositoryContext, _mapper);
                }
                return _timezoneRepository;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryWrapper" /> class.
        /// </summary>
        /// <param name="repositoryContext">The repository context.</param>
        /// <param name="mapper">The mapper.</param>
        public RepositoryWrapper(
            SchedulingContext repositoryContext,
            IMapper mapper)
        {
            _repositoryContext = repositoryContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Saves the asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SaveAsync()
        {
            return await _repositoryContext.SaveChangesAsync() >= 0;
        }
    }
}
