using AutoMapper;
using Css.Api.Core.DataAccess.Repository.UnitOfWork.Interfaces;
using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Core.Utilities.Extensions;
using Css.Api.Reporting.Business.Interfaces;
using Css.Api.Reporting.Business.Targets;
using Css.Api.Reporting.Business.UnitTest.Mocks;
using Css.Api.Reporting.Models.DTO.Processing;
using Css.Api.Reporting.Models.DTO.Request.UDW;
using Css.Api.Reporting.Models.DTO.Response;
using Css.Api.Reporting.Models.Enums;
using Css.Api.Reporting.Models.Profiles.Agent;
using Css.Api.Reporting.Models.Profiles.AgentData;
using Css.Api.Reporting.Models.Profiles.AgentSchedule;
using Css.Api.Reporting.Models.Profiles.AgentSchedulingGroupProfile;
using Css.Api.Reporting.Repository.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Css.Api.Reporting.Business.UnitTest.Targets
{
    public class UDWImportTargetShould
    {
        #region Private Properties

        /// <summary>
        /// The importer
        /// </summary>
        private readonly ITarget _target;

        /// <summary>
        /// The auto mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// The mock xml data instance
        /// </summary>
        private readonly MockXmlData _data;

        /// <summary>
        /// The mock agent repository
        /// </summary>
        private readonly Mock<IAgentRepository> _mockAgentRepository;

        /// <summary>
        /// The mock agent schedule repository
        /// </summary>
        private readonly Mock<IAgentScheduleRepository> _mockAgentScheduleRepository;

        /// <summary>
        /// The mock agent scheduling group repository
        /// </summary>
        private readonly Mock<IAgentSchedulingGroupRepository> _mockAgentSchedulingGroupRepository;
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor to initialize data
        /// </summary>
        public UDWImportTargetShould()
        {
            var factory = new MockFactoryData();
            _data = new MockXmlData();
            _data.InitializeData();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AgentProfile());
                cfg.AddProfile(new AgentDataProfile());
                cfg.AddProfile(new AgentScheduleProfile());
                cfg.AddProfile(new AgentSchedulingGroupProfile());
                cfg.AddProfile(new AgentScheduleProfile());
            });

            _mapper = new Mapper(mapperConfig);
            _mockAgentRepository = new Mock<IAgentRepository>();
            _mockAgentRepository.SetReturnsDefault<Task<List<Agent>>>(Task.FromResult<List<Agent>>(new List<Agent>()));
            _mockAgentSchedulingGroupRepository = new Mock<IAgentSchedulingGroupRepository>();
            _mockAgentSchedulingGroupRepository.SetReturnsDefault<Task<List<AgentSchedulingGroup>>>(Task.FromResult<List<AgentSchedulingGroup>>(factory.GetAgentSchedulingGroups()));
            _mockAgentScheduleRepository = new Mock<IAgentScheduleRepository>();
            _mockAgentScheduleRepository.SetReturnsDefault<Task<List<AgentSchedule>>>(Task.FromResult<List<AgentSchedule>>(new List<AgentSchedule>()));

            var mockUnitWork = new Mock<IUnitOfWork>();

            _target = new UDWImportTarget(_mapper, _mockAgentRepository.Object, _mockAgentSchedulingGroupRepository.Object, _mockAgentScheduleRepository.Object, mockUnitWork.Object, factory.GetMockFTP().Object);
        }
        #endregion

        #region Name

        /// <summary>
        /// The method to test the name of the service
        /// </summary>
        [Fact]
        public void CheckName()
        {
            var result = _target.Name;
            Assert.NotNull(result);
            Assert.IsType<string>(result);
        }
        #endregion

        #region Push(List<DataFeed> feeds)

        /// <summary>
        /// The method to test the successful processing
        /// </summary>
        [Fact]
        public async void CheckPushSuccess()
        {
            List<DataFeed> feeds = new List<DataFeed>() {
                new DataFeed()
                {
                    Content = _data.GetBytes<UDWAgentList>(),
                    Feeder = "Test"
                }
            };

            var response = await _target.Push(feeds);

            Assert.NotNull(response);
            Assert.IsType<ActivityResponse>(response);
            Assert.Single(response.Completed);
            Assert.Empty(response.Partial);
            Assert.Empty(response.Failed);
            Assert.Equal(1, response.TotalSources);
            Assert.Equal(ProcessStatus.Success.GetDescription(), response.Status);
        }

        /// <summary>
        /// The method to test the partial processing
        /// </summary>
        [Fact]
        public async void CheckPushPartial()
        {
            List<DataFeed> feeds = new List<DataFeed>() {
                new DataFeed()
                {
                    Content = _data.GetBytes<UDWAgentList>((int)ProcessStatus.Partial),
                    Feeder = "Test"
                }
            };

            var response = await _target.Push(feeds);

            Assert.NotNull(response);
            Assert.IsType<ActivityResponse>(response);
            Assert.Empty(response.Completed);
            Assert.Single(response.Partial);
            Assert.Empty(response.Failed);
            Assert.Equal(1, response.TotalSources);
            Assert.Equal(ProcessStatus.Partial.GetDescription(), response.Status);
        }

        /// <summary>
        /// The method to test the failed processing
        /// </summary>
        [Fact]
        public async void CheckPushFailed()
        {
            List<DataFeed> feeds = new List<DataFeed>() {
                new DataFeed()
                {
                    Content = _data.GetBytes<UDWAgentList>((int)ProcessStatus.Failed),
                    Feeder = "Test"
                }
            };

            var response = await _target.Push(feeds);

            Assert.NotNull(response);
            Assert.IsType<ActivityResponse>(response);
            Assert.Empty(response.Completed);
            Assert.Empty(response.Partial);
            Assert.Single(response.Failed);
            Assert.Equal(1, response.TotalSources);
            Assert.Equal(ProcessStatus.Failed.GetDescription(), response.Status);
        }
        #endregion
    }
}
