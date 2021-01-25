using AutoMapper;
using Css.Api.Setup.Repository.DatabaseContext;
using Css.Api.Setup.Repository.Interfaces;
using System.Threading.Tasks;

namespace Css.Api.Setup.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        /// <summary>
        /// Gets or sets the repository context.
        /// </summary>
        private SetupContext _repositoryContext { get; set; }

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
        /// <value>
        /// The client lob group repository.
        /// </value>
        private IClientLOBGroupRepository _clientLOBGroupRepository { get; set; }

        /// <summary>
        /// Gets or sets the skill tag repository.
        /// </summary>
        /// <value>
        /// The skill tag repository.
        /// </value>
        private ISkillTagRepository _skillTagRepository { get; set; }

        /// <summary>
        /// Gets or sets the skill group repository.
        /// </summary>
        /// <value>
        /// The skill group repository.
        /// </value>
        private ISkillGroupRepository _skillGroupRepository { get; set; }

        /// <summary>
        /// Gets or sets the agent scheduling group repository.
        /// </summary>
        /// <value>
        /// The agent scheduling group repository.
        /// </value>
        private IAgentSchedulingGroupRepository _agentSchedulingGroupRepository { get; set; }

        /// <summary>
        /// Gets or sets the timezone repository.
        /// </summary>
        /// <value>
        /// The timezone repository.
        /// </value>
        private ITimezoneRepository _timezoneRepository { get; set; }

        /// <summary>
        /// Gets or sets the operation hours repository.
        /// </summary>
        /// <value>
        /// The operation hours repository.
        /// </value>
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

        /// <summary>
        /// Gets the skill groups.
        /// </summary>
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
        /// <summary>
        /// Gets the skill tags.
        /// </summary>
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
        /// Gets the operation hours.
        /// </summary>
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

        /// <summary>
        /// Gets the time zones.
        /// </summary>
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
        /// Gets the agent scheduling group repository.
        /// </summary>
        public IAgentSchedulingGroupRepository AgentSchedulingGroups
        {
            get
            {
                if (_agentSchedulingGroupRepository == null)
                {
                    _agentSchedulingGroupRepository = new AgentSchedulingGroupRepository(_repositoryContext, _mapper);
                }
                return _agentSchedulingGroupRepository;
            }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryWrapper" /> class.
        /// </summary>
        /// <param name="repositoryContext">The repository context.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="">The .</param>
        public RepositoryWrapper(
            SetupContext repositoryContext,
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
