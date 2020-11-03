using AutoMapper;
using Css.Api.Core.Utilities.Interfaces;
using Css.Api.Scheduling.Models.DTO.Response.Client;
using Css.Api.Scheduling.Models.DTO.Response.ClientLOBGroup;
using Css.Api.Scheduling.Models.DTO.Response.SchedulingCode;
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
        /// Gets or sets the clients.
        /// </summary>
        private IClientRepository _clientRepository { get; set; }

        /// <summary>
        /// Gets or sets the client lob groups.
        /// </summary>
        private IClientLOBGroupRepository _clientLOBGroupRepository { get; set; }

        /// <summary>
        /// Gets or sets the scheduling codes repository.
        /// </summary>
        private ISchedulingCodeRepository _schedulingCodesRepository { get; set; }

        /// <summary>
        /// The clients sort helper
        /// </summary>
        private readonly ISortHelper<ClientDTO> _clientsSortHelper;

        /// <summary>
        /// The client lob groups sort helper
        /// </summary>
        private readonly ISortHelper<ClientLOBGroupDTO> _clientLOBGroupsSortHelper;

        /// <summary>
        /// The scheduling codes sort helper
        /// </summary>
        private readonly ISortHelper<SchedulingCodeDTO> _schedulingCodesSortHelper;

        /// <summary>
        /// The clients data shaper
        /// </summary>
        private readonly IDataShaper<ClientDTO> _clientsDataShaper;

        /// <summary>
        /// The client lob groups data shaper
        /// </summary>
        private readonly IDataShaper<ClientLOBGroupDTO> _clientLOBGroupsDataShaper;

        /// <summary>
        /// The scheduling codes data shaper
        /// </summary>
        private readonly IDataShaper<SchedulingCodeDTO> _schedulingCodesDataShaper;

        /// <summary>
        /// Gets the clients.
        /// </summary>
        public IClientRepository Clients
        {
            get
            {
                if (_clientRepository == null)
                {
                    _clientRepository = new ClientRepository(_repositoryContext, _mapper, _clientsSortHelper, _clientsDataShaper);
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
                    _clientLOBGroupRepository = new ClientLOBGroupRepository(_repositoryContext, _mapper, _clientLOBGroupsSortHelper, _clientLOBGroupsDataShaper);
                }
                return _clientLOBGroupRepository;
            }
        }

        /// <summary>
        /// Gets the scheduling codes.
        /// </summary>
        public ISchedulingCodeRepository SchedulingCodes
        {
            get
            {
                if (_schedulingCodesRepository == null)
                {
                    _schedulingCodesRepository = new SchedulingCodeRepository(_repositoryContext, _mapper, _schedulingCodesSortHelper, _schedulingCodesDataShaper);
                }
                return _schedulingCodesRepository;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryWrapper" /> class.
        /// </summary>
        /// <param name="repositoryContext">The repository context.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="clientsSortHelper">The clients sort helper.</param>
        /// <param name="clientLOBGroupsSortHelper">The client lob groups sort helper.</param>
        /// <param name="schedulingCodesSortHelper">The scheduling codes sort helper.</param>
        /// <param name="clientsDataShaper">The clients data shaper.</param>
        /// <param name="clientLobGroupsDataShaper">The client lob groups data shaper.</param>
        /// <param name="schedulingCodesDataShaper">The scheduling codes data shaper.</param>
        public RepositoryWrapper(
            SchedulingContext repositoryContext,
            IMapper mapper,
            ISortHelper<ClientDTO> clientsSortHelper,
            ISortHelper<ClientLOBGroupDTO> clientLOBGroupsSortHelper,
            ISortHelper<SchedulingCodeDTO> schedulingCodesSortHelper,
            IDataShaper<ClientDTO> clientsDataShaper,
            IDataShaper<ClientLOBGroupDTO> clientLobGroupsDataShaper,
            IDataShaper<SchedulingCodeDTO> schedulingCodesDataShaper)
        {
            _repositoryContext = repositoryContext;
            _mapper = mapper;
            _clientsSortHelper = clientsSortHelper;
            _clientLOBGroupsSortHelper = clientLOBGroupsSortHelper;
            _schedulingCodesSortHelper = schedulingCodesSortHelper;
            _clientsDataShaper = clientsDataShaper;
            _clientLOBGroupsDataShaper = clientLobGroupsDataShaper;
            _schedulingCodesDataShaper = schedulingCodesDataShaper;
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
