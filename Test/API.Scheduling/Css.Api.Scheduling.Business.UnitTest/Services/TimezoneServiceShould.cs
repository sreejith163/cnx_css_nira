using AutoMapper;
using Css.Api.Core.Models.DTO.Response;
using Css.Api.Core.Utilities;
using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Business.UnitTest.Mock;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Response.Client;
using Css.Api.Scheduling.Models.DTO.Response.ClientLOBGroup;
using Css.Api.Scheduling.Models.DTO.Response.SchedulingCode;
using Css.Api.Scheduling.Models.Profiles.Timezone;
using Css.Api.Scheduling.Repository;
using Css.Api.Scheduling.Repository.DatabaseContext;
using Css.Api.Scheduling.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Xunit;

namespace Css.Api.Scheduling.Business.UnitTest.Services
{
    public class TimezoneServiceShould
    {
        /// <summary>
        /// The timezone service
        /// </summary>
        private readonly ITimezoneService timezoneService;

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
        /// Initializes a new instance of the <see cref="SchedulingCodeTypeServiceShould"/> class.
        /// </summary>
        public TimezoneServiceShould()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new TimezoneProfile());
            });

            mapper = new Mapper(mapperConfig);

            var clientSortHelper = new SortHelper<ClientDTO>();
            var clientLObGroupSortHelper = new SortHelper<ClientLOBGroupDTO>();
            var clientSchedulingCodeSortHelper = new SortHelper<SchedulingCodeDTO>();

            var clientDataShaperHelper = new DataShaper<ClientDTO>();
            var clientLObGroupDataShaperHelper = new DataShaper<ClientLOBGroupDTO>();
            var clientSchedulingCodeDataShaperHelper = new DataShaper<SchedulingCodeDTO>();

            mockSchedulingContext = MockDataContext.IntializeMockData(true);

            SetTimeZoneAsCurrentDbContext();

            repositoryWrapper = new RepositoryWrapper(mockSchedulingContext.Object, mapper, clientSortHelper, clientLObGroupSortHelper, clientSchedulingCodeSortHelper,
                                                      clientDataShaperHelper, clientLObGroupDataShaperHelper, clientSchedulingCodeDataShaperHelper);

            timezoneService = new TimezoneService(repositoryWrapper);
        }

        #region GetTimezones

        /// <summary>
        /// Gets the scheduling code types.
        /// </summary>
        [Fact]
        public async void GetTimezones()
        {
            var result = await timezoneService.GetTimezones();

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<List<KeyValue>>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region PrivateMethods

        /// <summary>
        /// Sets the scheduling code type as current database context.
        /// </summary>
        private void SetTimeZoneAsCurrentDbContext()
        {
            var mockTimezone = new Mock<DbSet<Timezone>>();
            mockTimezone.As<IQueryable<Timezone>>().Setup(m => m.Provider).Returns(MockDataContext.timezonesDB.Provider);
            mockTimezone.As<IQueryable<Timezone>>().Setup(m => m.Expression).Returns(MockDataContext.timezonesDB.Expression);
            mockTimezone.As<IQueryable<Timezone>>().Setup(m => m.ElementType).Returns(MockDataContext.timezonesDB.ElementType);
            mockTimezone.As<IQueryable<Timezone>>().Setup(m => m.GetEnumerator()).Returns(MockDataContext.timezonesDB.GetEnumerator());

            mockSchedulingContext.Setup(x => x.Set<Timezone>()).Returns(mockTimezone.Object);
        }

        #endregion
    }
}
