using AutoMapper;
using Css.Api.Core.Models.DTO.Response;
using Css.Api.Core.Utilities;
using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Business.UnitTest.Mock;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Response.Client;
using Css.Api.Scheduling.Models.DTO.Response.ClientLOBGroup;
using Css.Api.Scheduling.Models.DTO.Response.SchedulingCode;
using Css.Api.Scheduling.Models.Profiles.SchedulingCode;
using Css.Api.Scheduling.Repository;
using Css.Api.Scheduling.Repository.DatabaseContext;
using Css.Api.Scheduling.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Xunit;

namespace Css.Api.Scheduling.Business.UnitTest
{
    public class SchedulingCodeIconServiceShould
    {
        /// <summary>
        /// The scheduling code icon service
        /// </summary>
        private readonly ISchedulingCodeIconService schedulingCodeIconService;

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
        /// Initializes a new instance of the <see cref="SchedulingCodeIconServiceShould"/> class.
        /// </summary>
        public SchedulingCodeIconServiceShould()
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

            mockSchedulingContext = MockDataContext.IntializeMockData(true);

            SetSchedulingCodeIconAsCurrentDbContext();

            repositoryWrapper = new RepositoryWrapper(mockSchedulingContext.Object, mapper, clientSortHelper, clientLObGroupSortHelper, clientSchedulingCodeSortHelper,
                                                      clientDataShaperHelper, clientLObGroupDataShaperHelper, clientSchedulingCodeDataShaperHelper);

            schedulingCodeIconService = new SchedulingCodeIconService(repositoryWrapper);
        }

        #region SchedulingCodeIcon

        /// <summary>
        /// Gets the scheduling code icons.
        /// </summary>
        [Fact]
        public async void GetSchedulingCodeIcons()
        {
            var result = await schedulingCodeIconService.GetSchedulingCodeIcons();

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<List<KeyValue>>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region PrivateMethods

        /// <summary>
        /// Sets the scheduling code icon as current database context.
        /// </summary>
        private void SetSchedulingCodeIconAsCurrentDbContext()
        {
            var mockClient = new Mock<DbSet<SchedulingCodeIcon>>();
            mockClient.As<IQueryable<SchedulingCodeIcon>>().Setup(m => m.Provider).Returns(MockDataContext.schedulingCodeIconsDB.Provider);
            mockClient.As<IQueryable<SchedulingCodeIcon>>().Setup(m => m.Expression).Returns(MockDataContext.schedulingCodeIconsDB.Expression);
            mockClient.As<IQueryable<SchedulingCodeIcon>>().Setup(m => m.ElementType).Returns(MockDataContext.schedulingCodeIconsDB.ElementType);
            mockClient.As<IQueryable<SchedulingCodeIcon>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.schedulingCodeIconsDB.GetEnumerator());

            mockSchedulingContext.Setup(x => x.Set<SchedulingCodeIcon>()).Returns(mockClient.Object);
        }

        #endregion
    }
}
