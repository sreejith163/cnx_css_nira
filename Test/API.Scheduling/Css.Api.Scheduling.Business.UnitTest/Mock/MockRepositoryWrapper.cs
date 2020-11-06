using AutoMapper;
using Css.Api.Core.Utilities.Interfaces;
using Css.Api.Scheduling.Business.UnitTest.Mock;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Response.Client;
using Css.Api.Scheduling.Models.DTO.Response.ClientLOBGroup;
using Css.Api.Scheduling.Models.DTO.Response.SchedulingCode;
using Css.Api.Scheduling.Repository.DatabaseContext;
using Css.Api.Scheduling.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Repository
{
    public class MockRepositoryWrapper: IRepositoryWrapper
    {
        /// <summary>
        /// Gets or sets the repository context.
        /// </summary>
        private Mock<SchedulingContext> _repositoryContext { get; set; }

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
        /// Gets or sets the scheduling code icons repository.
        /// </summary>
        private ISchedulingCodeIconRepository _schedulingCodeIconsRepository { get; set; }

        /// <summary>
        /// Gets or sets the scheduling code types repository.
        /// </summary>
        private ISchedulingCodeTypeRepository _schedulingCodeTypesRepository { get; set; }


        /// <summary>Gets or sets the timezone repository.</summary>
        /// <value>The timezone repository.</value>
        private ITimezoneRepository _timezoneRepository { get; set; }

        /// <summary>
        /// Gets or sets the scheduling type codes repository.
        /// </summary>
        private ISchedulingTypeCodeRepository _schedulingTypeCodesRepository { get; set; }

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
                    var mockClient = new Mock<DbSet<Client>>();
                    mockClient.As<IQueryable<Client>>().Setup(m => m.Provider).Returns(new MockDataContext().clientsDB.Provider);
                    mockClient.As<IQueryable<Client>>().Setup(m => m.Expression).Returns(new MockDataContext().clientsDB.Expression);
                    mockClient.As<IQueryable<Client>>().Setup(m => m.ElementType).Returns(new MockDataContext().clientsDB.ElementType);
                    mockClient.As<IQueryable<Client>>().Setup(m => m.GetEnumerator()).Returns(new MockDataContext().clientsDB.GetEnumerator());

                    _repositoryContext.Setup(x => x.Set<Client>()).Returns(mockClient.Object);

                    _clientRepository = new ClientRepository(_repositoryContext.Object, _mapper, _clientsSortHelper, _clientsDataShaper);
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
                    var mockClientLobGroup = new Mock<DbSet<ClientLobGroup>>();
                    mockClientLobGroup.As<IQueryable<ClientLobGroup>>().Setup(m => m.Provider).Returns(new MockDataContext().clientLobGroupsDB.Provider);
                    mockClientLobGroup.As<IQueryable<ClientLobGroup>>().Setup(m => m.Expression).Returns(new MockDataContext().clientLobGroupsDB.Expression);
                    mockClientLobGroup.As<IQueryable<ClientLobGroup>>().Setup(m => m.ElementType).Returns(new MockDataContext().clientLobGroupsDB.ElementType);
                    mockClientLobGroup.As<IQueryable<ClientLobGroup>>().Setup(m => m.GetEnumerator()).Returns(new MockDataContext().clientLobGroupsDB.GetEnumerator());

                    _repositoryContext.Setup(x => x.Set<ClientLobGroup>()).Returns(mockClientLobGroup.Object);

                    _clientLOBGroupRepository = new ClientLOBGroupRepository(_repositoryContext.Object, _mapper, _clientLOBGroupsSortHelper, _clientLOBGroupsDataShaper);
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
                    var mockSchedulingCode = new Mock<DbSet<SchedulingCode>>();
                    mockSchedulingCode.As<IQueryable<SchedulingCode>>().Setup(m => m.Provider).Returns(new MockDataContext().schedulingCodesDB.Provider);
                    mockSchedulingCode.As<IQueryable<SchedulingCode>>().Setup(m => m.Expression).Returns(new MockDataContext().schedulingCodesDB.Expression);
                    mockSchedulingCode.As<IQueryable<SchedulingCode>>().Setup(m => m.ElementType).Returns(new MockDataContext().schedulingCodesDB.ElementType);
                    mockSchedulingCode.As<IQueryable<SchedulingCode>>().Setup(m => m.GetEnumerator()).Returns(new MockDataContext().schedulingCodesDB.GetEnumerator());

                    _repositoryContext.Setup(x => x.Set<SchedulingCode>()).Returns(mockSchedulingCode.Object);

                    _schedulingCodesRepository = new SchedulingCodeRepository(_repositoryContext.Object, _mapper, _schedulingCodesSortHelper, _schedulingCodesDataShaper);
                }
                return _schedulingCodesRepository;
            }
        }

        /// <summary>
        /// Gets the scheduling code icons.
        /// </summary>
        public ISchedulingCodeIconRepository SchedulingCodeIcons
        {
            get
            {
                if (_schedulingCodeIconsRepository == null)
                {
                    var mockSchedulingCodeIcon = new Mock<DbSet<SchedulingCodeIcon>>();
                    mockSchedulingCodeIcon.As<IQueryable<SchedulingCodeIcon>>().Setup(m => m.Provider).Returns(new MockDataContext().schedulingCodeIconsDB.Provider);
                    mockSchedulingCodeIcon.As<IQueryable<SchedulingCodeIcon>>().Setup(m => m.Expression).Returns(new MockDataContext().schedulingCodeIconsDB.Expression);
                    mockSchedulingCodeIcon.As<IQueryable<SchedulingCodeIcon>>().Setup(m => m.ElementType).Returns(new MockDataContext().schedulingCodeIconsDB.ElementType);
                    mockSchedulingCodeIcon.As<IQueryable<SchedulingCodeIcon>>().Setup(m => m.GetEnumerator()).Returns(new MockDataContext().schedulingCodeIconsDB.GetEnumerator());

                    _repositoryContext.Setup(x => x.Set<SchedulingCodeIcon>()).Returns(mockSchedulingCodeIcon.Object);

                    _schedulingCodeIconsRepository = new SchedulingCodeIconRepository(_repositoryContext.Object, _mapper);
                }
                return _schedulingCodeIconsRepository;
            }
        }

        /// <summary>
        /// Gets the scheduling code types.
        /// </summary>
        public ISchedulingCodeTypeRepository SchedulingCodeTypes
        {
            get
            {
                if (_schedulingCodeTypesRepository == null)
                {
                    var mockSchedulingCodeTypes = new Mock<DbSet<SchedulingCodeType>>();
                    mockSchedulingCodeTypes.As<IQueryable<SchedulingCodeType>>().Setup(m => m.Provider).Returns(new MockDataContext().schedulingCodeTypesDB.Provider);
                    mockSchedulingCodeTypes.As<IQueryable<SchedulingCodeType>>().Setup(m => m.Expression).Returns(new MockDataContext().schedulingCodeTypesDB.Expression);
                    mockSchedulingCodeTypes.As<IQueryable<SchedulingCodeType>>().Setup(m => m.ElementType).Returns(new MockDataContext().schedulingCodeTypesDB.ElementType);
                    mockSchedulingCodeTypes.As<IQueryable<SchedulingCodeType>>().Setup(m => m.GetEnumerator()).Returns(new MockDataContext().schedulingCodeTypesDB.GetEnumerator());

                    _repositoryContext.Setup(x => x.Set<SchedulingCodeType>()).Returns(mockSchedulingCodeTypes.Object);

                    _schedulingCodeTypesRepository = new SchedulingCodeTypeRepository(_repositoryContext.Object, _mapper);
                }
                return _schedulingCodeTypesRepository;
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
                    var mockTimezone = new Mock<DbSet<Timezone>>();
                    mockTimezone.As<IQueryable<Timezone>>().Setup(m => m.Provider).Returns(new MockDataContext().timezonesDB.Provider);
                    mockTimezone.As<IQueryable<Timezone>>().Setup(m => m.Expression).Returns(new MockDataContext().timezonesDB.Expression);
                    mockTimezone.As<IQueryable<Timezone>>().Setup(m => m.ElementType).Returns(new MockDataContext().timezonesDB.ElementType);
                    mockTimezone.As<IQueryable<Timezone>>().Setup(m => m.GetEnumerator()).Returns(new MockDataContext().timezonesDB.GetEnumerator());

                    _repositoryContext.Setup(x => x.Set<Timezone>()).Returns(mockTimezone.Object);

                    _timezoneRepository = new TimezoneRepository(_repositoryContext.Object, _mapper);
                }
                return _timezoneRepository;
            }
        }        

        /// <summary>
        /// Gets the scheduling type codes.
        /// </summary>
        public ISchedulingTypeCodeRepository SchedulingTypeCodes
        {
            get
            {
                if (_schedulingTypeCodesRepository == null)
                {
                    var mockSchedulingTypeCode = new Mock<DbSet<SchedulingTypeCode>>();
                    mockSchedulingTypeCode.As<IQueryable<SchedulingTypeCode>>().Setup(m => m.Provider).Returns(new MockDataContext().schedulingTypeCodes.Provider);
                    mockSchedulingTypeCode.As<IQueryable<SchedulingTypeCode>>().Setup(m => m.Expression).Returns(new MockDataContext().schedulingTypeCodes.Expression);
                    mockSchedulingTypeCode.As<IQueryable<SchedulingTypeCode>>().Setup(m => m.ElementType).Returns(new MockDataContext().schedulingTypeCodes.ElementType);
                    mockSchedulingTypeCode.As<IQueryable<SchedulingTypeCode>>().Setup(m => m.GetEnumerator()).Returns(new MockDataContext().schedulingTypeCodes.GetEnumerator());

                    _repositoryContext.Setup(x => x.Set<SchedulingTypeCode>()).Returns(mockSchedulingTypeCode.Object);

                    _schedulingTypeCodesRepository = new SchedulingTypeCodeRepository(_repositoryContext.Object);
                }
                return _schedulingTypeCodesRepository;
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
        public MockRepositoryWrapper(
            Mock<SchedulingContext> repositoryContext,
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
            return await _repositoryContext.Object.SaveChangesAsync() >= 0;
        }
    }
}
