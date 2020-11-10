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
using Css.Api.Scheduling.Repository.Interfaces;
using Moq;
using System.Collections.Generic;
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

            var clientSortHelper = new SortHelper<Client>();
            var clientLObGroupSortHelper = new SortHelper<ClientLobGroup>();
            var clientSchedulingCodeSortHelper = new SortHelper<SchedulingCode>();

            var clientDataShaperHelper = new DataShaper<ClientDTO>();
            var clientLObGroupDataShaperHelper = new DataShaper<ClientLOBGroupDTO>();
            var clientSchedulingCodeDataShaperHelper = new DataShaper<SchedulingCodeDTO>();

            var mockSchedulingContext = new MockDataContext().IntializeMockData();

            repositoryWrapper = new MockRepositoryWrapper(mockSchedulingContext, mapper, clientSortHelper, clientLObGroupSortHelper, clientSchedulingCodeSortHelper,
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
    }
}
