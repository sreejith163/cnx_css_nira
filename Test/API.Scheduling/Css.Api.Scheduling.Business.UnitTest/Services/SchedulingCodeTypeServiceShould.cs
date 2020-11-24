using AutoMapper;
using Css.Api.Core.Models.DTO.Response;
using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Business.UnitTest.Mock;
using Css.Api.Scheduling.Models.Profiles.SchedulingCode;
using Css.Api.Scheduling.Repository;
using Css.Api.Scheduling.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Collections.Generic;
using System.Net;
using Xunit;

namespace Css.Api.Scheduling.Business.UnitTest.Services
{
    public class SchedulingCodeTypeServiceShould
    {
        /// <summary>
        /// The scheduling code type service
        /// </summary>
        private readonly ISchedulingCodeTypeService schedulingCodeTypeService;

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
        public SchedulingCodeTypeServiceShould()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new SchedulingCodeProfile());
            });

            mapper = new Mapper(mapperConfig);

            var context = new DefaultHttpContext();
            Mock<IHttpContextAccessor> mockHttContext = new Mock<IHttpContextAccessor>();
            mockHttContext.Setup(_ => _.HttpContext).Returns(context);

            var mockSchedulingContext = new MockDataContext().IntializeMockData();

            repositoryWrapper = new MockRepositoryWrapper(mockSchedulingContext, mapper);

            schedulingCodeTypeService = new SchedulingCodeTypeService(repositoryWrapper);
        }

        #region SchedulingCodeIcon

        /// <summary>
        /// Gets the scheduling code types.
        /// </summary>
        [Fact]
        public async void GetSchedulingCodeTypes()
        {
            var result = await schedulingCodeTypeService.GetSchedulingCodeTypes();

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<List<KeyValue>>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion
    }
}
