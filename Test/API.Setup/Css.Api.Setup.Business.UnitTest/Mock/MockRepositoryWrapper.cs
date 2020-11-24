using AutoMapper;
using Css.Api.Setup.Models.Domain;
using Css.Api.Setup.Repository.DatabaseContext;
using Css.Api.Setup.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using Css.Api.Setup.Repository;

namespace Css.Api.Setup.Business.UnitTest.Mock
{
    public class MockRepositoryWrapper: IRepositoryWrapper
    {
        /// <summary>
        /// Gets or sets the repository context.
        /// </summary>
        private Mock<SetupContext> _repositoryContext { get; set; }

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
        /// Gets the skill groups repository.
        /// </summary>
        private ISkillGroupRepository _skillGroupsRepository { get; set; }

        /// <summary>
        /// Gets or sets the skill tags repository.
        /// </summary>
        private ISkillTagRepository _skillTagsRepository { get; set; }

        /// <summary>
        /// Gets the operation hours repository.
        /// </summary>
        private IOperationHourRepository _operationHoursRepository { get; set; }

        /// <summary>
        /// Gets or sets the timezone repository.
        /// </summary>
        /// <value>
        /// The timezone repository.
        /// </value>
        private ITimezoneRepository _timezoneRepository { get; set; }

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

                    _clientRepository = new ClientRepository(_repositoryContext.Object, _mapper);
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

                    _clientLOBGroupRepository = new ClientLOBGroupRepository(_repositoryContext.Object, _mapper);
                }
                return _clientLOBGroupRepository;
            }
        }

        /// <summary>
        /// Gets the skill groups.
        /// </summary>
        /// <value>
        /// The skill groups.
        /// </value>
        public ISkillGroupRepository SkillGroups
        {
            get
            {
                if (_skillGroupsRepository == null)
                {
                    var mockSkillGroup = new Mock<DbSet<SkillGroup>>();
                    mockSkillGroup.As<IQueryable<SkillGroup>>().Setup(m => m.Provider).Returns(new MockDataContext().skillGroupsDB.Provider);
                    mockSkillGroup.As<IQueryable<SkillGroup>>().Setup(m => m.Expression).Returns(new MockDataContext().skillGroupsDB.Expression);
                    mockSkillGroup.As<IQueryable<SkillGroup>>().Setup(m => m.ElementType).Returns(new MockDataContext().skillGroupsDB.ElementType);
                    mockSkillGroup.As<IQueryable<SkillGroup>>().Setup(m => m.GetEnumerator()).Returns(new MockDataContext().skillGroupsDB.GetEnumerator());

                    _repositoryContext.Setup(x => x.Set<SkillGroup>()).Returns(mockSkillGroup.Object);

                    _skillGroupsRepository = new SkillGroupRepository(_repositoryContext.Object, _mapper);
                }
                return _skillGroupsRepository;
            }
        }

        public ISkillTagRepository SkillTags
        {
            get
            {
                if (_skillTagsRepository == null)
                {
                    var mockSkillTag = new Mock<DbSet<SkillTag>>();
                    mockSkillTag.As<IQueryable<SkillTag>>().Setup(m => m.Provider).Returns(new MockDataContext().skillTagsDB.Provider);
                    mockSkillTag.As<IQueryable<SkillTag>>().Setup(m => m.Expression).Returns(new MockDataContext().skillTagsDB.Expression);
                    mockSkillTag.As<IQueryable<SkillTag>>().Setup(m => m.ElementType).Returns(new MockDataContext().skillTagsDB.ElementType);
                    mockSkillTag.As<IQueryable<SkillTag>>().Setup(m => m.GetEnumerator()).Returns(new MockDataContext().skillTagsDB.GetEnumerator());

                    _repositoryContext.Setup(x => x.Set<SkillTag>()).Returns(mockSkillTag.Object);

                    _skillTagsRepository = new SkillTagRepository(_repositoryContext.Object, _mapper);
                }
                return _skillTagsRepository;
            }
        }

        /// <summary>
        /// Gets the operation hours.
        /// </summary>
        /// <value>
        /// The operation hours.
        /// </value>
        public IOperationHourRepository OperationHours
        {
            get
            {
                if (_operationHoursRepository == null)
                {
                    var mockSchedulingCodeTypes = new Mock<DbSet<OperationHour>>();
                    
                    _repositoryContext.Setup(x => x.Set<OperationHour>()).Returns(mockSchedulingCodeTypes.Object);

                    _operationHoursRepository = new OperationHourRepository(_repositoryContext.Object);
                }
                return _operationHoursRepository;
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
        /// Initializes a new instance of the <see cref="RepositoryWrapper" /> class.
        /// </summary>
        /// <param name="repositoryContext">The repository context.</param>
        /// <param name="mapper">The mapper.</param>
        public MockRepositoryWrapper(
            Mock<SetupContext> repositoryContext,
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
            return await _repositoryContext.Object.SaveChangesAsync() >= 0;
        }
    }
}
