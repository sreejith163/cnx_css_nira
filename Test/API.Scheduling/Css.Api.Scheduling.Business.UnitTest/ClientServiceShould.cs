using AutoMapper;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Utilities;
using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Business.UnitTest.Mock;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.Client;
using Css.Api.Scheduling.Models.Profiles.Client;
using Css.Api.Scheduling.Repository;
using Css.Api.Scheduling.Repository.DatabaseContext;
using Css.Api.Scheduling.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq;
using System.Net;
using Xunit;

namespace Css.Api.Scheduling.Business.UnitTest
{
    public class ClientServiceShould
    {
        /// <summary>
        /// The client service
        /// </summary>
        private readonly IClientService clientService;

        /// <summary>
        /// The mock scheduling context
        /// </summary>
        private readonly Mock<SchedulingContext> mockSchedulingContext;

        /// <summary>
        /// The repository wrapper
        /// </summary>
        private readonly IRepositoryWrapper repositoryWrapper;

        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientServiceShould"/> class.
        /// </summary>
        public ClientServiceShould()
        {
            var createClientProfile = new CreateClientProfile();
            var updateClientProfile = new UpdateClientProfile();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(createClientProfile);
                cfg.AddProfile(updateClientProfile);
            });

            mapper = new Mapper(mapperConfig);

            var clientSortHelper = new SortHelper<Client>();
            var clientLObGroupSortHelper = new SortHelper<ClientLobGroup>();
            var clientSchedulingCodeSortHelper = new SortHelper<SchedulingCode>();

            var clientDataShaperHelper = new DataShaper<Client>();
            var clientLObGroupDataShaperHelper = new DataShaper<ClientLobGroup>();
            var clientSchedulingCodeDataShaperHelper = new DataShaper<SchedulingCode>();

            mockSchedulingContext = MockInit.IntializeMockData(true);

            SetClientAsCurrentDbContext();

            repositoryWrapper = new RepositoryWrapper(mockSchedulingContext.Object, clientSortHelper, clientLObGroupSortHelper, clientSchedulingCodeSortHelper,
                                                      clientDataShaperHelper, clientLObGroupDataShaperHelper, clientSchedulingCodeDataShaperHelper);

            clientService = new ClientService(repositoryWrapper, mapper);
        }

        /// <summary>
        /// Creates the client.
        /// </summary>
        /// <param name="clientDetails">The client details.</param>
        [Theory]
        [InlineData(null)]
        public async void CreateClient(CreateClient clientDetails)
        {
            clientDetails = new CreateClient()
            {
                RefId = 4,
                CreatedBy = "admin",
                Name = "D"
            };
            var result = await clientService.CreateClient(clientDetails);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<Client>(result.Value);
            Assert.Equal(HttpStatusCode.Created, result.Code);
        }

        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetClient(int clientId)
        {
            var result = await clientService.GetClient(new ClientIdDetails { ClientId = clientId });

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<Client>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        /// <summary>
        /// Gets the client with no found.
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        [Theory]
        [InlineData(100)]
        public async void GetClientWithNoFound(int clientId)
        {
            var result = await clientService.GetClient(new ClientIdDetails { ClientId = clientId });

            Assert.Equal(HttpStatusCode.NotFound, result.Code);
        }

        /// <summary>
        /// Deletes the client.
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void DeleteClient(int clientId)
        {
            var result = await clientService.DeleteClient(new ClientIdDetails { ClientId = clientId });

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        /// <summary>
        /// Gets the clients.
        /// </summary>
        [Fact]
        public async void GetClients()
        {
            ClientQueryParameters queryParameters = new ClientQueryParameters()
            {
                Fields = "",
                OrderBy = "",
                PageNumber = 1,
                PageSize = 10,
                SearchKeyword = ""
            };
            var result = await clientService.GetClients(queryParameters);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<PagedList<Entity>>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        /// <summary>
        /// Creates the scheduling code.
        /// </summary>
        /// <param name="schedulingCode">The scheduling code.</param>
        [Theory]
        [InlineData(0, null)]
        public async void UpdateSchedulingCode(int clientId, UpdateClient updateClient)
        {
            updateClient = new UpdateClient()
            {
                
            };
            var result = await clientService.UpdateClient(new ClientIdDetails { ClientId = clientId }, updateClient);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<string>(result.Value);
            Assert.Equal(HttpStatusCode.BadRequest, result.Code);
        }

        /// <summary>
        /// Sets the client.
        /// </summary>
        private void SetClientAsCurrentDbContext()
        {
            var mockClient = new Mock<DbSet<Client>>();
            mockClient.As<IQueryable<Client>>().Setup(m => m.Provider).Returns(MockDataContext.clientsDB.Provider);
            mockClient.As<IQueryable<Client>>().Setup(m => m.Expression).Returns(MockDataContext.clientsDB.Expression);
            mockClient.As<IQueryable<Client>>().Setup(m => m.ElementType).Returns(MockDataContext.clientsDB.ElementType);
            mockClient.As<IQueryable<Client>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.clientsDB.GetEnumerator());

            mockSchedulingContext.Setup(x => x.Set<Client>()).Returns(mockClient.Object);
        }
    }
}
