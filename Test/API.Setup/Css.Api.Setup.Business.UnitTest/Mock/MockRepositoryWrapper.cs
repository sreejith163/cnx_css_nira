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
        private Mock<SetupContext> RepositoryContext { get; set; }

        /// <summary>
        /// Gets or sets the mapper.
        /// </summary>
        private IMapper Mapper { get; set; }

        /// <summary>
        /// Gets or sets the clients.
        /// </summary>
        private IClientRepository ClientRepository { get; set; }

        /// <summary>
        /// Gets or sets the client lob groups.
        /// </summary>
        private IClientLOBGroupRepository ClientLOBGroupRepository { get; set; }

        /// <summary>
        /// Gets the skill groups repository.
        /// </summary>
        private ISkillGroupRepository SkillGroupsRepository { get; set; }

        /// <summary>
        /// Gets or sets the skill tags repository.
        /// </summary>
        private ISkillTagRepository SkillTagsRepository { get; set; }

        /// <summary>
        /// Gets or sets the agent scheduling group repository.
        /// </summary>
        private IAgentSchedulingGroupRepository AgentSchedulingGroupRepository { get; set; }

        /// <summary>
        /// Gets the operation hours repository.
        /// </summary>
        private IOperationHourRepository OperationHoursRepository { get; set; }

        /// <summary>
        /// Gets or sets the timezone repository.
        /// </summary>
        /// <value>
        /// The timezone repository.
        /// </value>
        private ITimezoneRepository TimezoneRepository { get; set; }

        /// <summary>
        /// Gets the clients.
        /// </summary>
        public IClientRepository Clients
        {
            get
            {
                if (ClientRepository == null)
                {
                    var mockClient = new Mock<DbSet<Client>>();
                    mockClient.As<IQueryable<Client>>().Setup(m => m.Provider).Returns(new MockDataContext().clientsDB.Provider);
                    mockClient.As<IQueryable<Client>>().Setup(m => m.Expression).Returns(new MockDataContext().clientsDB.Expression);
                    mockClient.As<IQueryable<Client>>().Setup(m => m.ElementType).Returns(new MockDataContext().clientsDB.ElementType);
                    mockClient.As<IQueryable<Client>>().Setup(m => m.GetEnumerator()).Returns(new MockDataContext().clientsDB.GetEnumerator());

                    RepositoryContext.Setup(x => x.Set<Client>()).Returns(mockClient.Object);

                    ClientRepository = new ClientRepository(RepositoryContext.Object, Mapper);
                }
                return ClientRepository;
            }
        }

        /// <summary>
        /// Gets the client lob groups.
        /// </summary>
        public IClientLOBGroupRepository ClientLOBGroups
        {
            get
            {
                if (ClientLOBGroupRepository == null)
                {
                    var mockClientLobGroup = new Mock<DbSet<ClientLobGroup>>();
                    mockClientLobGroup.As<IQueryable<ClientLobGroup>>().Setup(m => m.Provider).Returns(new MockDataContext().clientLobGroupsDB.Provider);
                    mockClientLobGroup.As<IQueryable<ClientLobGroup>>().Setup(m => m.Expression).Returns(new MockDataContext().clientLobGroupsDB.Expression);
                    mockClientLobGroup.As<IQueryable<ClientLobGroup>>().Setup(m => m.ElementType).Returns(new MockDataContext().clientLobGroupsDB.ElementType);
                    mockClientLobGroup.As<IQueryable<ClientLobGroup>>().Setup(m => m.GetEnumerator()).Returns(new MockDataContext().clientLobGroupsDB.GetEnumerator());

                    RepositoryContext.Setup(x => x.Set<ClientLobGroup>()).Returns(mockClientLobGroup.Object);

                    ClientLOBGroupRepository = new ClientLOBGroupRepository(RepositoryContext.Object, Mapper);
                }
                return ClientLOBGroupRepository;
            }
        }

        /// <summary>
        /// Gets the skill groups.
        /// </summary>
        public ISkillGroupRepository SkillGroups
        {  
            get
            {
                if (SkillGroupsRepository == null)
                {
                    var mockSkillGroup = new Mock<DbSet<SkillGroup>>();
                    mockSkillGroup.As<IQueryable<SkillGroup>>().Setup(m => m.Provider).Returns(new MockDataContext().skillGroupsDB.Provider);
                    mockSkillGroup.As<IQueryable<SkillGroup>>().Setup(m => m.Expression).Returns(new MockDataContext().skillGroupsDB.Expression);
                    mockSkillGroup.As<IQueryable<SkillGroup>>().Setup(m => m.ElementType).Returns(new MockDataContext().skillGroupsDB.ElementType);
                    mockSkillGroup.As<IQueryable<SkillGroup>>().Setup(m => m.GetEnumerator()).Returns(new MockDataContext().skillGroupsDB.GetEnumerator());

                    RepositoryContext.Setup(x => x.Set<SkillGroup>()).Returns(mockSkillGroup.Object);

                    SkillGroupsRepository = new SkillGroupRepository(RepositoryContext.Object, Mapper);
                }
                return SkillGroupsRepository;
            }
        }

        /// <summary>
        /// Gets the skill tags.
        /// </summary>
        public ISkillTagRepository SkillTags
        {
            get
            {
                if (SkillTagsRepository == null)
                {
                    var mockSkillTag = new Mock<DbSet<SkillTag>>();
                    mockSkillTag.As<IQueryable<SkillTag>>().Setup(m => m.Provider).Returns(new MockDataContext().skillTagsDB.Provider);
                    mockSkillTag.As<IQueryable<SkillTag>>().Setup(m => m.Expression).Returns(new MockDataContext().skillTagsDB.Expression);
                    mockSkillTag.As<IQueryable<SkillTag>>().Setup(m => m.ElementType).Returns(new MockDataContext().skillTagsDB.ElementType);
                    mockSkillTag.As<IQueryable<SkillTag>>().Setup(m => m.GetEnumerator()).Returns(new MockDataContext().skillTagsDB.GetEnumerator());

                    RepositoryContext.Setup(x => x.Set<SkillTag>()).Returns(mockSkillTag.Object);

                    SkillTagsRepository = new SkillTagRepository(RepositoryContext.Object, Mapper);
                }
                return SkillTagsRepository;
            }
        }

        /// <summary>
        /// Gets the agent scheduling groups.
        /// </summary>
        public IAgentSchedulingGroupRepository AgentSchedulingGroups
        {
            get
            {
                if (AgentSchedulingGroupRepository == null)
                {
                    var mockAgentSchedulingGroup = new Mock<DbSet<AgentSchedulingGroup>>();
                    mockAgentSchedulingGroup.As<IQueryable<AgentSchedulingGroup>>().Setup(m => m.Provider).Returns(new MockDataContext().agentSchedulingGroupsDB.Provider);
                    mockAgentSchedulingGroup.As<IQueryable<AgentSchedulingGroup>>().Setup(m => m.Expression).Returns(new MockDataContext().agentSchedulingGroupsDB.Expression);
                    mockAgentSchedulingGroup.As<IQueryable<AgentSchedulingGroup>>().Setup(m => m.ElementType).Returns(new MockDataContext().agentSchedulingGroupsDB.ElementType);
                    mockAgentSchedulingGroup.As<IQueryable<AgentSchedulingGroup>>().Setup(m => m.GetEnumerator()).Returns(new MockDataContext().agentSchedulingGroupsDB.GetEnumerator());

                    RepositoryContext.Setup(x => x.Set<AgentSchedulingGroup>()).Returns(mockAgentSchedulingGroup.Object);

                    AgentSchedulingGroupRepository = new AgentSchedulingGroupRepository(RepositoryContext.Object, Mapper);
                }
                return AgentSchedulingGroupRepository;
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
                if (OperationHoursRepository == null)
                {
                    var mockOperationHours = new Mock<DbSet<OperationHour>>();
                    mockOperationHours.As<IQueryable<OperationHour>>().Setup(m => m.Provider).Returns(new MockDataContext().operationHoursDB.Provider);
                    mockOperationHours.As<IQueryable<OperationHour>>().Setup(m => m.Expression).Returns(new MockDataContext().operationHoursDB.Expression);
                    mockOperationHours.As<IQueryable<OperationHour>>().Setup(m => m.ElementType).Returns(new MockDataContext().operationHoursDB.ElementType);
                    mockOperationHours.As<IQueryable<OperationHour>>().Setup(m => m.GetEnumerator()).Returns(new MockDataContext().operationHoursDB.GetEnumerator());

                    RepositoryContext.Setup(x => x.Set<OperationHour>()).Returns(mockOperationHours.Object);

                    OperationHoursRepository = new OperationHourRepository(RepositoryContext.Object);
                }
                return OperationHoursRepository;
            }
        }

        /// <summary>Gets the time zones.</summary>
        /// <value>The time zones.</value>
        public ITimezoneRepository TimeZones
        {
            get
            {
                if (TimezoneRepository == null)
                {
                    var mockTimezone = new Mock<DbSet<Timezone>>();
                    mockTimezone.As<IQueryable<Timezone>>().Setup(m => m.Provider).Returns(new MockDataContext().timezonesDB.Provider);
                    mockTimezone.As<IQueryable<Timezone>>().Setup(m => m.Expression).Returns(new MockDataContext().timezonesDB.Expression);
                    mockTimezone.As<IQueryable<Timezone>>().Setup(m => m.ElementType).Returns(new MockDataContext().timezonesDB.ElementType);
                    mockTimezone.As<IQueryable<Timezone>>().Setup(m => m.GetEnumerator()).Returns(new MockDataContext().timezonesDB.GetEnumerator());

                    RepositoryContext.Setup(x => x.Set<Timezone>()).Returns(mockTimezone.Object);

                    TimezoneRepository = new TimezoneRepository(RepositoryContext.Object, Mapper);
                }
                return TimezoneRepository;
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
            RepositoryContext = repositoryContext;
            Mapper = mapper;
        }

        /// <summary>
        /// Saves the asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SaveAsync()
        {
            return await RepositoryContext.Object.SaveChangesAsync() >= 0;
        }
    }
}
