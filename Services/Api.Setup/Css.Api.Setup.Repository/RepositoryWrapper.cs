using AutoMapper;
using Css.Api.SetupMenu.Repository.DatabaseContext;
using Css.Api.SetupMenu.Repository.Interfaces;
using System.Threading.Tasks;

namespace Css.Api.SetupMenu.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        /// <summary>
        /// Gets or sets the repository context.
        /// </summary>
        private SetupMenuContext _repositoryContext { get; set; }

        /// <summary>
        /// Gets or sets the mapper.
        /// </summary>
        private IMapper _mapper { get; set; }

        /// <summary>
        /// Gets or sets the clients.
        /// </summary>
        private IClientRepository _clientRepository { get; set; }

        /// <summary>
        /// Gets or sets the client lob groups.
        /// </summary>
        private IClientLOBGroupRepository _clientLOBGroupRepository { get; set; }

        /// <summary>
        /// Gets or sets the skill tag repository.
        /// </summary>
        /// <value>
        /// The skill tag repository.
        /// </value>
        private ISkillTagRepository _skillTagRepository { get; set; }

        /// <summary>Gets or sets the skill group repository.</summary>
        /// <value>The skill group repository.</value>
        private ISkillGroupRepository _skillGroupRepository { get; set; }

        /// <summary>
        /// Gets or sets the timezone repository.
        /// </summary>
        private ITimezoneRepository _timezoneRepository { get; set; }

        /// <summary>Gets or sets the operation hours repository.</summary>
        /// <value>The operation hours repository.</value>
        private IOperationHourRepository _operationHoursRepository { get; set; }

        /// <summary>
        /// Gets the clients.
        /// </summary>
        public IClientRepository Clients
        {
            get
            {
                if (_clientRepository == null)
                {
                    _clientRepository = new ClientRepository(_repositoryContext, _mapper);
                }
                return _clientRepository;
            }
        }

        /// <summary>
        /// Gets the client lob groups.
        /// </summary>
        public IClientLOBGroupRepository ClientLOBGroups
        {
            get
            {
                if (_clientLOBGroupRepository == null)
                {
                    _clientLOBGroupRepository = new ClientLOBGroupRepository(_repositoryContext, _mapper);
                }
                return _clientLOBGroupRepository;
            }
        }

        /// <summary>Gets the skill groups.</summary>
        /// <value>The skill groups.</value>
        public ISkillGroupRepository SkillGroups
        {
            get
            {
                if (_skillGroupRepository == null)
                {
                    _skillGroupRepository = new SkillGroupRepository(_repositoryContext, _mapper);
                }
                return _skillGroupRepository;
            }
        }
                
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

        /// <summary>Gets the operation hours.</summary>
        /// <value>The operation hours.</value>
        public IOperationHourRepository OperationHours
        {
            get
            {
                if (_operationHoursRepository == null)
                {
                    _operationHoursRepository = new OperationHourRepository(_repositoryContext);
                }
                return _operationHoursRepository;
            }
        }
        public ISkillTagRepository SkillTags
        {
            get
            {
                if (_skillTagRepository == null)
                {
                    _skillTagRepository = new SkillTagRepository(_repositoryContext, _mapper);
                }
                return _skillTagRepository;
            }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryWrapper" /> class.
        /// </summary>
        /// <param name="repositoryContext">The repository context.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="">The .</param>
        public RepositoryWrapper(
            SetupMenuContext repositoryContext,
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
