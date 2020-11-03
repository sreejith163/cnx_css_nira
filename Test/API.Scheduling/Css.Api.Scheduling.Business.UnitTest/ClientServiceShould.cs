﻿using Moq;
using Xunit;
using System.Net;
using AutoMapper;
using System.Linq;
using Css.Api.Core.Utilities;
using Css.Api.Core.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Css.Api.Scheduling.Repository;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Repository.Interfaces;
using Css.Api.Scheduling.Models.Profiles.Client;
using Css.Api.Scheduling.Business.UnitTest.Mock;
using Css.Api.Scheduling.Models.DTO.Request.Client;
using Css.Api.Scheduling.Repository.DatabaseContext;
using Css.Api.Scheduling.Models.DTO.Response.Client;
using Css.Api.Scheduling.Models.DTO.Response.ClientLOBGroup;
using Css.Api.Scheduling.Models.DTO.Response.SchedulingCode;

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

            mockSchedulingContext = MockInit.IntializeMockData(true);

            SetClientAsCurrentDbContext();

            repositoryWrapper = new RepositoryWrapper(mockSchedulingContext.Object, mapper, clientSortHelper, clientLObGroupSortHelper, clientSchedulingCodeSortHelper,
                                                      clientDataShaperHelper, clientLObGroupDataShaperHelper, clientSchedulingCodeDataShaperHelper);

            clientService = new ClientService(repositoryWrapper, mapper);
        }

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
                Name = "A"
            };
            var result = await clientService.CreateClient(clientDetails);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.Conflict, result.Code);
        }

        #endregion

        #region GetClient

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

        #endregion

        #region DeleteClient

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

        #endregion

        #region GetClients

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

        #endregion

        #region UpdateClient

        /// <summary>
        /// Creates the scheduling code.
        /// </summary>
        /// <param name="schedulingCode">The scheduling code.</param>
        [Theory]
        [InlineData(1)]
        public async void UpdateSchedulingCode(int clientId)
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

        /// <summary>
        /// Updates the scheduling code with not found.
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="updateClient">The update client.</param>
        [Theory]
        [InlineData(100)]
        public async void UpdateSchedulingCodeWithNotFound(int clientId)
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

        #endregion

        #region PrivateMethods

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

        #endregion
    }
}