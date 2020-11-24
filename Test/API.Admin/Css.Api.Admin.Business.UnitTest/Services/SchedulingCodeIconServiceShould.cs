using AutoMapper;
using Css.Api.Core.Models.DTO.Response;
using Css.Api.Admin.Business.Interfaces;
using Css.Api.Admin.Business.UnitTest.Mock;
using Css.Api.Admin.Models.Profiles.SchedulingCode;
using Css.Api.Admin.Repository;
using Css.Api.Admin.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Collections.Generic;
using System.Net;
using Xunit;

namespace Css.Api.Admin.Business.UnitTest.Services
{
    public class SchedulingCodeIconServiceShould
    {
        /// <summary>
        /// The scheduling code icon service
        /// </summary>
        private readonly ISchedulingCodeIconService schedulingCodeIconService;

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

            var context = new DefaultHttpContext();
            Mock<IHttpContextAccessor> mockHttContext = new Mock<IHttpContextAccessor>();
            mockHttContext.Setup(_ => _.HttpContext).Returns(context);

            var mockSchedulingContext = new MockDataContext().IntializeMockData();

            repositoryWrapper = new MockRepositoryWrapper(mockSchedulingContext, mapper);

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
    }
}
