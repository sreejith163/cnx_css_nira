using Moq;
using Xunit;
using System.Net;
using AutoMapper;
using System.Linq;
using Css.Api.Core.Utilities;
using Css.Api.Core.Models.Domain;
using Css.Api.Scheduling.Repository;
using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Repository.Interfaces;
using Css.Api.Scheduling.Models.Profiles.Client;
using Css.Api.Scheduling.Business.UnitTest.Mock;
using Css.Api.Scheduling.Models.DTO.Request.Client;
using Css.Api.Scheduling.Repository.DatabaseContext;
using Css.Api.Scheduling.Models.DTO.Response.Client;
using Css.Api.Scheduling.Models.DTO.Response.ClientLOBGroup;
using Css.Api.Scheduling.Models.DTO.Response.SchedulingCode;
using Microsoft.AspNetCore.Http;

namespace Css.Api.Scheduling.Business.UnitTest.Services
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
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ClientProfile());
            });

            mapper = new Mapper(mapperConfig);

            var clientSortHelper = new SortHelper<ClientDTO>();
            var clientLObGroupSortHelper = new SortHelper<ClientLOBGroupDTO>();
            var clientSchedulingCodeSortHelper = new SortHelper<SchedulingCodeDTO>();

            var clientDataShaperHelper = new DataShaper<ClientDTO>();
            var clientLObGroupDataShaperHelper = new DataShaper<ClientLOBGroupDTO>();
            var clientSchedulingCodeDataShaperHelper = new DataShaper<SchedulingCodeDTO>();

            var context = new DefaultHttpContext();
            Mock<IHttpContextAccessor> mockHttContext = new Mock<IHttpContextAccessor>();
            mockHttContext.Setup(_ => _.HttpContext).Returns(context);

            mockSchedulingContext = MockDataContext.IntializeMockData(true);

            repositoryWrapper = new MockRepositoryWrapper(mockSchedulingContext, mapper, clientSortHelper, clientLObGroupSortHelper, clientSchedulingCodeSortHelper,
                                                      clientDataShaperHelper, clientLObGroupDataShaperHelper, clientSchedulingCodeDataShaperHelper);

            clientService = new ClientService(repositoryWrapper, mockHttContext.Object, mapper);
        }

        #region GetClients

        /// <summary>
        /// Gets the clients.
        /// </summary>
        [Fact]
        public async void GetClients()
        {
            ClientQueryParameters queryParameters = new ClientQueryParameters();
            var result = await clientService.GetClients(queryParameters);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<PagedList<Entity>>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region GetClient

        /// <summary>
        /// Gets the client with no found.
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        [Theory]
        [InlineData(100)]
        public async void GetClientWithNotFound(int clientId)
        {
            var result = await clientService.GetClient(new ClientIdDetails { ClientId = clientId });

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
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
            Assert.IsType<ClientDTO>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region CreateClient

        /// <summary>
        /// Creates the client.
        /// </summary>
        [Fact]
        public async void CreateClient()
        {
            CreateClient clientDetails = new CreateClient()
            {
                RefId = 4,
                CreatedBy = "admin",
                Name = "D"
            };
            var result = await clientService.CreateClient(clientDetails);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<ClientIdDetails>(result.Value);
            Assert.Equal(HttpStatusCode.Created, result.Code);
        }

        /// <summary>
        /// Gets the client with no found.
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        [Fact]
        public async void CreateClientWithConflictFound()
        {
            CreateClient clientDetails = new CreateClient()
            {
                RefId = 4,
                CreatedBy = "admin",
                Name = "B"
            };
            var result = await clientService.CreateClient(clientDetails);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.Conflict, result.Code);
        }

        #endregion

        #region UpdateClient

        /// <summary>
        /// Updates the client with not found.
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        [Theory]
        [InlineData(100)]
        public async void UpdateClientWithNotFound(int clientId)
        {
            UpdateClient updateClient = new UpdateClient()
            {
                Name = "X",
                ModifiedBy = "admin1"
            };
            var result = await clientService.UpdateClient(new ClientIdDetails { ClientId = clientId }, updateClient);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
        }

        /// <summary>
        /// Updates the client.
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        [Theory]
        [InlineData(1)]
        public async void UpdateClient(int clientId)
        {
            UpdateClient updateClient = new UpdateClient()
            {
                Name = "X",
                ModifiedBy = "admin1"
            };
            var result = await clientService.UpdateClient(new ClientIdDetails { ClientId = clientId }, updateClient);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NoContent, result.Code);
        }

        #endregion

        #region DeleteClient

        /// <summary>
        /// Deletes the client with not found.
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        [Theory]
        [InlineData(100)]
        public async void DeleteClientWithNotFound(int clientId)
        {
            var result = await clientService.DeleteClient(new ClientIdDetails { ClientId = clientId });

            Assert.NotNull(result);
            Assert.Null(result.Value);
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
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NoContent, result.Code);
        }

        #endregion
    }
}
