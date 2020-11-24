using AutoMapper;
using Css.Api.Core.Models.Domain;
using Css.Api.Setup.Business.Interfaces;
using Css.Api.Setup.Business.UnitTest.Mock;
using Css.Api.Setup.Models.DTO.Request.ClientLOBGroup;
using Css.Api.Setup.Models.DTO.Response.ClientLOBGroup;
using Css.Api.Setup.Models.Profiles.Client;
using Css.Api.Setup.Models.Profiles.ClientLOBGroup;
using Css.Api.Setup.Models.Profiles.Timezone;
using Css.Api.Setup.Repository;
using Css.Api.Setup.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Net;
using Xunit;

namespace Css.Api.Setup.Business.UnitTest.Services
{
    public class ClientLOBGroupServiceShould
    {
        /// <summary>
        /// The client lob group service
        /// </summary>
        private readonly IClientLOBGroupService clientLOBGroupService;

        /// <summary>
        /// The repository wrapper
        /// </summary>
        private readonly IRepositoryWrapper repositoryWrapper;

        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientLOBGroupServiceShould"/> class.
        /// </summary>
        public ClientLOBGroupServiceShould()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ClientProfile());
                cfg.AddProfile(new ClientLOBGroupProfile());
                cfg.AddProfile(new TimezoneProfile());
            });

            mapper = new Mapper(mapperConfig);

            var context = new DefaultHttpContext();
            Mock<IHttpContextAccessor> mockHttContext = new Mock<IHttpContextAccessor>();
            mockHttContext.Setup(_ => _.HttpContext).Returns(context);

            var mockSchedulingContext = new MockDataContext().IntializeMockData();

            repositoryWrapper = new MockRepositoryWrapper(mockSchedulingContext, mapper);

            clientLOBGroupService = new ClientLOBGroupService(repositoryWrapper, mockHttContext.Object, mapper);
        }

        #region GetClientLOBGroups

        /// <summary>
        /// Gets the client lob groups.
        /// </summary>
        [Fact]
        public async void GetClientLOBGroups()
        {
            ClientLOBGroupQueryParameter queryParameters = new ClientLOBGroupQueryParameter();
            var result = await clientLOBGroupService.GetClientLOBGroups(queryParameters);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<PagedList<Entity>>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region GetClientLOBGroup

        /// <summary>
        /// Gets the client lob group with not found.
        /// </summary>
        /// <param name="clientLOBGroupId">The client lob group identifier.</param>
        [Theory]
        [InlineData(100)]
        public async void GetClientLOBGroupWithNotFound(int clientLOBGroupId)
        {
            var result = await clientLOBGroupService.GetClientLOBGroup(new ClientLOBGroupIdDetails { ClientLOBGroupId = clientLOBGroupId });

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
        }

        /// <summary>
        /// Gets the client lob group.
        /// </summary>
        /// <param name="clientLOBGroupId">The client lob group identifier.</param>
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetClientLOBGroup(int clientLOBGroupId)
        {
            var result = await clientLOBGroupService.GetClientLOBGroup(new ClientLOBGroupIdDetails { ClientLOBGroupId = clientLOBGroupId });

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<ClientLOBGroupDTO>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region CreateClientLOBGroup

        /// <summary>
        /// Creates the client lob group.
        /// </summary>
        [Fact]
        public async void CreateClientLOBGroupWithConflict()
        {
            CreateClientLOBGroup clientLOBGroup = new CreateClientLOBGroup()
            {
                ClientId = 1,
                CreatedBy = "Admin",
                Name = "A"
            };
            var result = await clientLOBGroupService.CreateClientLOBGroup(clientLOBGroup);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.Conflict, result.Code);
        }

        /// <summary>
        /// Creates the client lob group.
        /// </summary>
        [Fact]
        public async void CreateClientLOBGroup()
        {
            CreateClientLOBGroup clientLOBGroup = new CreateClientLOBGroup()
            {
                ClientId = 1,
                CreatedBy = "Admin",
                Name = "F"
            };
            var result = await clientLOBGroupService.CreateClientLOBGroup(clientLOBGroup);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<ClientLOBGroupIdDetails>(result.Value);
            Assert.Equal(HttpStatusCode.Created, result.Code);
        }

        #endregion

        #region UpdateClientLOBGroup

        /// <summary>
        /// Updates the scheduling code with not found.
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="updateClient">The update client.</param>
        [Theory]
        [InlineData(100)]
        public async void UpdateClientLOBGroupWithNotFound(int clientLOBGroupId)
        {
            UpdateClientLOBGroup clientLOBGroup = new UpdateClientLOBGroup()
            {
                ClientId = 1,
                Name = "X",
                ModifiedBy = "admin1"
            };
            var result = await clientLOBGroupService.UpdateClientLOBGroup(new ClientLOBGroupIdDetails { ClientLOBGroupId = clientLOBGroupId }, clientLOBGroup);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
        }

        [Theory]
        [InlineData(1)]
        public async void UpdateClientLOBGroupWithConflict(int clientLOBGroupId)
        {
            UpdateClientLOBGroup clientLOBGroup = new UpdateClientLOBGroup()
            {
                ClientId = 2,
                Name = "B",
                ModifiedBy = "admin"
            };
            var result = await clientLOBGroupService.UpdateClientLOBGroup(new ClientLOBGroupIdDetails { ClientLOBGroupId = clientLOBGroupId }, clientLOBGroup);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.Conflict, result.Code);
        }

        /// <summary>
        /// Updates the client lob group.
        /// </summary>
        /// <param name="clientLOBGroupId">The client lob group identifier.</param>
        [Theory]
        [InlineData(1)]
        public async void UpdateClientLOBGroup(int clientLOBGroupId)
        {
            UpdateClientLOBGroup clientLOBGroup = new UpdateClientLOBGroup()
            {
                ClientId = 1,
                Name = "A",
                ModifiedBy = "admin1"
            };
            var result = await clientLOBGroupService.UpdateClientLOBGroup(new ClientLOBGroupIdDetails { ClientLOBGroupId = clientLOBGroupId }, clientLOBGroup);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NoContent, result.Code);
        }

        #endregion

        #region DeleteClientLOBGroup

        /// <summary>
        /// Deletes the client lob group with not found.
        /// </summary>
        /// <param name="clientLOBGroupId">The client lob group identifier.</param>
        [Theory]
        [InlineData(100)]
        public async void DeleteClientLOBGroupWithNotFound(int clientLOBGroupId)
        {
            var result = await clientLOBGroupService.DeleteClientLOBGroup(new ClientLOBGroupIdDetails { ClientLOBGroupId = clientLOBGroupId });

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
        }

        /// <summary>
        /// Deletes the client lob group.
        /// </summary>
        /// <param name="clientLOBGroupId">The client lob group identifier.</param>
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void DeleteClientLOBGroup(int clientLOBGroupId)
        {
            var result = await clientLOBGroupService.DeleteClientLOBGroup(new ClientLOBGroupIdDetails { ClientLOBGroupId = clientLOBGroupId });

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NoContent, result.Code);
        }

        #endregion
    }
}
