using Moq;
using Xunit;
using System.Net;
using AutoMapper;
using System.Linq;
using Css.Api.Core.Utilities;
using Css.Api.Core.Models.Domain;
using System.Collections.Generic;
using Css.Api.Scheduling.Repository;
using Microsoft.EntityFrameworkCore;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Repository.Interfaces;
using Css.Api.Scheduling.Business.UnitTest.Mock;
using Css.Api.Scheduling.Repository.DatabaseContext;
using Css.Api.Scheduling.Models.Profiles.SchedulingCode;
using Css.Api.Scheduling.Models.DTO.Request.SchedulingCode;
using Css.Api.Scheduling.Models.DTO.Response.Client;
using Css.Api.Scheduling.Models.DTO.Response.SchedulingCode;
using Css.Api.Scheduling.Models.DTO.Response.ClientLOBGroup;

namespace Css.Api.Scheduling.Business.UnitTest
{

    public class SchedulingCodeServiceShould
    {
        /// <summary>
        /// The scheduling code service
        /// </summary>
        private readonly ISchedulingCodeService schedulingCodeService;

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
        /// Initializes a new instance of the <see cref="SchedulingCodeServiceShould"/> class.
        /// </summary>
        public SchedulingCodeServiceShould()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new SchedulingCodeProfile());
            });

            mapper = new Mapper(mapperConfig);

            var clientSortHelper = new SortHelper<ClientDTO>();
            var clientLObGroupSortHelper = new SortHelper<ClientLOBGroupDTO>();
            var clientSchedulingCodeSortHelper = new SortHelper<SchedulingCodeDTO>();

            var clientDataShaperHelper = new DataShaper<ClientDTO>();
            var clientLObGroupDataShaperHelper = new DataShaper<ClientLOBGroupDTO>();
            var clientSchedulingCodeDataShaperHelper = new DataShaper<SchedulingCodeDTO>();

            mockSchedulingContext = MockInit.IntializeMockData(true);

            SetSchedulingCodeAsCurrentDbContext();

            repositoryWrapper = new RepositoryWrapper(mockSchedulingContext.Object, mapper, clientSortHelper, clientLObGroupSortHelper, clientSchedulingCodeSortHelper,
                                                      clientDataShaperHelper, clientLObGroupDataShaperHelper, clientSchedulingCodeDataShaperHelper);

            schedulingCodeService = new SchedulingCodeService(repositoryWrapper, mapper);
        }

        #region CreateSchedulingCode

        /// <summary>
        /// Creates the scheduling code.
        /// </summary>
        /// <param name="schedulingCode">The scheduling code.</param>
        [Fact]
        public async void CreateSchedulingCode()
        {
            CreateSchedulingCode schedulingCode = new CreateSchedulingCode()
            {
                RefId = 4,
                CodeTypes = new List<int>(),
                Priority = 4,
                CreatedBy = "admin",
                Description = "test",
                IconId = 1
            };
            var result = await schedulingCodeService.CreateSchedulingCode(schedulingCode);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<SchedulingCodeIdDetails>(result.Value);
            Assert.Equal(HttpStatusCode.Created, result.Code);
        }

        #endregion

        #region UpdateSchedulingCode

        /// <summary>
        /// Creates the scheduling code.
        /// </summary>
        /// <param name="schedulingCode">The scheduling code.</param>
        [Theory]
        [InlineData(1)]
        public async void UpdateSchedulingCode(int schedulingCodeId)
        {
            UpdateSchedulingCode schedulingCode = new UpdateSchedulingCode()
            {
                Priority = 4,
                Description = "test",
                IconId = 2,
                CodeTypes = new List<int>(),
                ModifiedBy = "admin"
            };
            var result = await schedulingCodeService.UpdateSchedulingCode(new SchedulingCodeIdDetails { SchedulingCodeId = schedulingCodeId }, schedulingCode);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NoContent, result.Code);
        }

        /// <summary>
        /// Updates the scheduling code with not found.
        /// </summary>
        /// <param name="schedulingCodeId">The scheduling code identifier.</param>
        /// <param name="schedulingCode">The scheduling code.</param>
        [Theory]
        [InlineData(100)]
        public async void UpdateSchedulingCodeWithNotFound(int schedulingCodeId)
        {
            UpdateSchedulingCode schedulingCode = new UpdateSchedulingCode()
            {
                Priority = 4,
                Description = "test",
                IconId = 2,
                CodeTypes = new List<int>(),
                ModifiedBy = "admin"
            };
            var result = await schedulingCodeService.UpdateSchedulingCode(new SchedulingCodeIdDetails { SchedulingCodeId = schedulingCodeId }, schedulingCode);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
        }

        #endregion

        #region GetSchedulingCode

        /// <summary>
        /// Gets the scheduling code.
        /// </summary>
        /// <param name="schedulingCodeId">The scheduling code identifier.</param>
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetSchedulingCode(int schedulingCodeId)
        {
            var result = await schedulingCodeService.GetSchedulingCode(new SchedulingCodeIdDetails { SchedulingCodeId = schedulingCodeId });

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<SchedulingCodeDTO>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        /// <summary>
        /// Gets the scheduling code with not found.
        /// </summary>
        /// <param name="schedulingCodeId">The scheduling code identifier.</param>
        [Theory]
        [InlineData(100)]
        public async void GetSchedulingCodeWithNotFound(int schedulingCodeId)
        {
            var result = await schedulingCodeService.GetSchedulingCode(new SchedulingCodeIdDetails { SchedulingCodeId = schedulingCodeId });

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
        }

        #endregion

        #region DeleteSceduleCode

        /// <summary>
        /// Deletes the scheduling code.
        /// </summary>
        /// <param name="schedulingCodeId">The scheduling code identifier.</param>
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void DeleteSchedulingCode(int schedulingCodeId)
        {
            var result = await schedulingCodeService.DeleteSchedulingCode(new SchedulingCodeIdDetails { SchedulingCodeId = schedulingCodeId });

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NoContent, result.Code);
        }

        /// <summary>
        /// Deletes the scheduling code.
        /// </summary>
        /// <param name="schedulingCodeId">The scheduling code identifier.</param>
        [Theory]
        [InlineData(100)]
        public async void DeleteSchedulingCodeWithNotFound(int schedulingCodeId)
        {
            var result = await schedulingCodeService.DeleteSchedulingCode(new SchedulingCodeIdDetails { SchedulingCodeId = schedulingCodeId });

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
        }

        #endregion

        #region GetSchedulingCodes

        /// <summary>
        /// Gets the scheduling codes.
        /// </summary>
        [Fact]
        public async void GetSchedulingCodes()
        {
            SchedulingCodeQueryParameters queryParameters = new SchedulingCodeQueryParameters()
            {
                Fields = "",
                OrderBy = "",
                PageNumber = 1,
                PageSize = 10,
                SearchKeyword = ""
            };
            var result = await schedulingCodeService.GetSchedulingCodes(queryParameters);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<PagedList<Entity>>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region PrivateMethods

        /// <summary>
        /// Sets the client.
        /// </summary>
        private void SetSchedulingCodeAsCurrentDbContext()
        {
            var mockClient = new Mock<DbSet<SchedulingCode>>();
            mockClient.As<IQueryable<SchedulingCode>>().Setup(m => m.Provider).Returns(MockDataContext.schedulingCodesDB.Provider);
            mockClient.As<IQueryable<SchedulingCode>>().Setup(m => m.Expression).Returns(MockDataContext.schedulingCodesDB.Expression);
            mockClient.As<IQueryable<SchedulingCode>>().Setup(m => m.ElementType).Returns(MockDataContext.schedulingCodesDB.ElementType);
            mockClient.As<IQueryable<SchedulingCode>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.schedulingCodesDB.GetEnumerator());

            mockSchedulingContext.Setup(x => x.Set<SchedulingCode>()).Returns(mockClient.Object);
        }

        #endregion
    }
}
