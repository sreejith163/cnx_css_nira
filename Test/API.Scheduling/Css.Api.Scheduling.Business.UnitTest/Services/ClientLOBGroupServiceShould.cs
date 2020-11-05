using AutoMapper;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Utilities;
using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Business.UnitTest.Mock;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.ClientLOBGroup;
using Css.Api.Scheduling.Models.DTO.Response.Client;
using Css.Api.Scheduling.Models.DTO.Response.ClientLOBGroup;
using Css.Api.Scheduling.Models.DTO.Response.SchedulingCode;
using Css.Api.Scheduling.Models.Profiles.Client;
using Css.Api.Scheduling.Models.Profiles.ClientLOBGroup;
using Css.Api.Scheduling.Models.Profiles.Timezone;
using Css.Api.Scheduling.Repository;
using Css.Api.Scheduling.Repository.DatabaseContext;
using Css.Api.Scheduling.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq;
using System.Net;
using Xunit;

namespace Css.Api.Scheduling.Business.UnitTest.Services
{
    public class ClientLOBGroupServiceShould
    {
        /// <summary>
        /// The client lob group service
        /// </summary>
        private readonly IClientLOBGroupService clientLOBGroupService;

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

            SetClientLOBGroupAsCurrentDbContext();

            repositoryWrapper = new MockRepositoryWrapper(mockSchedulingContext, mapper, clientSortHelper, clientLObGroupSortHelper, clientSchedulingCodeSortHelper,
                                                      clientDataShaperHelper, clientLObGroupDataShaperHelper, clientSchedulingCodeDataShaperHelper);

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
        public async void CreateClientLOBGroup()
        {
            CreateClientLOBGroup clientLOBGroup = new CreateClientLOBGroup()
            {
                RefId = 4,
                CreatedBy = "Admin",
                name = "A"
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
                name = "X",
                ModifiedBy = "admin1"
            };
            var result = await clientLOBGroupService.UpdateClientLOBGroup(new ClientLOBGroupIdDetails { ClientLOBGroupId = clientLOBGroupId }, clientLOBGroup);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
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
                name = "X",
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

        #region PrivateMethods

        /// <summary>
        /// Sets the client.
        /// </summary>
        private void SetClientLOBGroupAsCurrentDbContext()
        {
            var mockClientLobGroup = new Mock<DbSet<ClientLobGroup>>();
            mockClientLobGroup.As<IQueryable<ClientLobGroup>>().Setup(m => m.Provider).Returns(MockDataContext.clientLobGroupsDB.Provider);
            mockClientLobGroup.As<IQueryable<ClientLobGroup>>().Setup(m => m.Expression).Returns(MockDataContext.clientLobGroupsDB.Expression);
            mockClientLobGroup.As<IQueryable<ClientLobGroup>>().Setup(m => m.ElementType).Returns(MockDataContext.clientLobGroupsDB.ElementType);
            mockClientLobGroup.As<IQueryable<ClientLobGroup>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.clientLobGroupsDB.GetEnumerator());

            mockSchedulingContext.Setup(x => x.Set<ClientLobGroup>()).Returns(mockClientLobGroup.Object);
        }

        #endregion
    }
}
