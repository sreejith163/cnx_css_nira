using Moq;
using Xunit;
using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Business.UnitTest.Mocks;
using Css.Api.Scheduling.Models.Profiles.AgentAdmin;
using Css.Api.Scheduling.Models.Profiles.AgentSchedule;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedule;
using Css.Api.Scheduling.Repository.Interfaces;
using Css.Api.Core.DataAccess.Repository.UnitOfWork.Interfaces;
using System.Collections.Generic;
using Css.Api.Scheduling.Models.DTO.Response.AgentSchedule;
using System;
using Css.Api.Core.Models.Enums;
using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Core.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedulingGroup;
using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using Css.Api.Scheduling.Models.Profiles.ActivityLog;

namespace Css.Api.Scheduling.Business.UnitTest.Services
{
    public class AgentScheduleServiceShould
    {
        /// <summary>
        /// The translation service
        /// </summary>
        private readonly IAgentScheduleService agentScheduleService;

        /// <summary>
        /// The mock activity log repository
        /// </summary>
        private readonly Mock<IActivityLogRepository> mockActivityLogRepository;

        /// <summary>
        /// The mock agent schedule repository
        /// </summary>
        private readonly Mock<IAgentScheduleRepository> mockAgentScheduleRepository;

        /// <summary>
        /// The mock agent schedule manager repository
        /// </summary>
        private readonly Mock<IAgentScheduleManagerRepository> mockAgentScheduleManagerRepository;

        /// <summary>
        /// The mock agent admin repository
        /// </summary>
        private readonly Mock<IAgentAdminRepository> mockAgentAdminRepository;

        /// <summary>
        /// The mock scheduling code repository
        /// </summary>
        private readonly Mock<ISchedulingCodeRepository> mockSchedulingCodeRepository;

        /// <summary>
        /// The mock data context
        /// </summary>
        private readonly MockDataContext mockDataContext;

        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageTranslationServiceShould"/> class.
        /// </summary>
        public AgentScheduleServiceShould()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AgentScheduleProfile());
                cfg.AddProfile(new AgentScheduleManagerProfile());
                cfg.AddProfile(new AgentLogProfile());
                cfg.AddProfile(new AgentAdminProfile());
            });

            mapper = new Mapper(mapperConfig);

            var context = new DefaultHttpContext();
            Mock<IHttpContextAccessor> mockHttContext = new Mock<IHttpContextAccessor>();
            mockHttContext.Setup(_ => _.HttpContext).Returns(context);

            mockActivityLogRepository = new Mock<IActivityLogRepository>();
            mockAgentScheduleRepository = new Mock<IAgentScheduleRepository>();
            mockAgentScheduleManagerRepository = new Mock<IAgentScheduleManagerRepository>();
            mockAgentAdminRepository = new Mock<IAgentAdminRepository>();
            mockSchedulingCodeRepository = new Mock<ISchedulingCodeRepository>();

            var mockUnitWork = new Mock<IUnitOfWork>();

            mockDataContext = new MockDataContext();

            agentScheduleService = new AgentScheduleService(mockHttContext.Object, mockActivityLogRepository.Object, mockAgentScheduleRepository.Object,
                                                            mockAgentScheduleManagerRepository.Object, mockAgentAdminRepository.Object,
                                                            mockSchedulingCodeRepository.Object, mapper, mockUnitWork.Object);
        }

        #region GetAgentSchedules

        /// <summary>
        /// Gets the agent schedules.
        /// </summary>
        [Fact]
        public async void GetAgentSchedules()
        {
            AgentScheduleQueryparameter agentScheduleQueryparameter = new AgentScheduleQueryparameter();

            mockAgentScheduleRepository.Setup(mr => mr.GetAgentSchedules(It.IsAny<AgentScheduleQueryparameter>())).ReturnsAsync(
                (AgentScheduleQueryparameter agentScheduleQueryparameter) => mockDataContext.GetAgentSchedules(agentScheduleQueryparameter));

            var result = await agentScheduleService.GetAgentSchedules(agentScheduleQueryparameter);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<PagedList<Entity>>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region GetAgentSchedule

        /// <summary>
        /// Gets the agent schedule with not found for schedule.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        [Theory]
        [InlineData("6fe0b5ad6a05416894c0718e")]
        [InlineData("6fe0b5ad6a05416894c0718f")]
        public async void GetAgentScheduleWithNotFoundForSchedule(string agentScheduleId)
        {
            mockAgentScheduleRepository.Setup(mr => mr.GetAgentSchedule(It.IsAny<AgentScheduleIdDetails>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails) => mockDataContext.GetAgentSchedule(agentScheduleIdDetails));

            var result = await agentScheduleService.GetAgentSchedule(new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId });

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
        }

        /// <summary>
        /// Gets the agent schedule.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        [Theory]
        [InlineData("5fe0b5ad6a05416894c0718e")]
        [InlineData("5fe0b5ad6a05416894c0718f")]
        public async void GetAgentSchedule(string agentScheduleId)
        {
            mockAgentScheduleRepository.Setup(mr => mr.GetAgentSchedule(It.IsAny<AgentScheduleIdDetails>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails) => mockDataContext.GetAgentSchedule(agentScheduleIdDetails));

            var result = await agentScheduleService.GetAgentSchedule(new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId });

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<AgentScheduleDetailsDTO>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region IsAgentScheduleRangeExist

        /// <summary>
        /// Determines whether [is agent schedule range exist return false if not] [the specified agent schedule identifier].
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        /// <param name="fromYear">From year.</param>
        /// <param name="fromMonth">From month.</param>
        /// <param name="fromDay">From day.</param>
        /// <param name="toYear">To year.</param>
        /// <param name="toMonth">To month.</param>
        /// <param name="toDay">To day.</param>
        [Theory]
        [InlineData("5fe0b5ad6a05416894c0718e", 2021, 3, 10, 2021, 3, 17)]
        [InlineData("5fe0b5ad6a05416894c0718f", 2021, 3, 10, 2021, 3, 17)]
        public async void IsAgentScheduleRangeExistReturnFalseIfNot(string agentScheduleId, int fromYear, int fromMonth, int fromDay,
                                                                    int toYear, int toMonth, int toDay)
        {
            mockAgentScheduleRepository.Setup(mr => mr.IsAgentScheduleRangeExist(It.IsAny<AgentScheduleIdDetails>(), It.IsAny<DateRange>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails, DateRange range) => mockDataContext.IsAgentScheduleRangeExist(agentScheduleIdDetails, range));

            var result = await agentScheduleService.IsAgentScheduleRangeExist(new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId }, 
                                                                              new DateRange { DateFrom = new DateTime(fromYear, fromMonth, fromDay), 
                                                                                              DateTo = new DateTime(toYear, toMonth, toDay) });

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<bool>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        /// <summary>
        /// Determines whether [is agent schedule range exist return true if] [the specified agent schedule identifier].
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        /// <param name="fromYear">From year.</param>
        /// <param name="fromMonth">From month.</param>
        /// <param name="fromDay">From day.</param>
        /// <param name="toYear">To year.</param>
        /// <param name="toMonth">To month.</param>
        /// <param name="toDay">To day.</param>
        [Theory]
        [InlineData("5fe0b5ad6a05416894c0718e", 2021, 3, 21, 2021, 3, 27)]
        [InlineData("5fe0b5ad6a05416894c0718f", 2021, 3, 21, 2021, 3, 27)]
        public async void IsAgentScheduleRangeExistReturnTrueIf(string agentScheduleId, int fromYear, int fromMonth, int fromDay,
                                                                    int toYear, int toMonth, int toDay)
        {
            mockAgentScheduleRepository.Setup(mr => mr.IsAgentScheduleRangeExist(It.IsAny<AgentScheduleIdDetails>(), It.IsAny<DateRange>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails, DateRange range) => mockDataContext.IsAgentScheduleRangeExist(agentScheduleIdDetails, range));

            var result = await agentScheduleService.IsAgentScheduleRangeExist(new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId },
                                                                              new DateRange
                                                                              {
                                                                                  DateFrom = new DateTime(fromYear, fromMonth, fromDay),
                                                                                  DateTo = new DateTime(toYear, toMonth, toDay)
                                                                              });

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<bool>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region GetAgentScheduleCharts

        /// <summary>
        /// Gets the agent schedule charts with not found for schedule.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        [Theory]
        [InlineData("6fe0b5ad6a05416894c0718e")]
        [InlineData("6fe0b5ad6a05416894c0718f")]
        public async void GetAgentScheduleChartsWithNotFoundForSchedule(string agentScheduleId)
        {
            var agentScheduleChartQueryparameter = new AgentScheduleChartQueryparameter();

            mockAgentScheduleRepository.Setup(mr => mr.GetAgentSchedule(It.IsAny<AgentScheduleIdDetails>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails) => mockDataContext.GetAgentSchedule(agentScheduleIdDetails));

            var result = await agentScheduleService.GetAgentScheduleCharts(new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId }, agentScheduleChartQueryparameter);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
        }

        /// <summary>
        /// Gets the agent schedule charts for scheduling tab.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        [Theory]
        [InlineData("5fe0b5ad6a05416894c0718e")]
        [InlineData("5fe0b5ad6a05416894c0718f")]
        public async void GetAgentScheduleCharts(string agentScheduleId)
        {
            var agentScheduleChartQueryparameter = new AgentScheduleChartQueryparameter();

            mockAgentScheduleRepository.Setup(mr => mr.GetAgentSchedule(It.IsAny<AgentScheduleIdDetails>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails) => mockDataContext.GetAgentSchedule(agentScheduleIdDetails));

            var result = await agentScheduleService.GetAgentScheduleCharts(new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId }, agentScheduleChartQueryparameter);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<AgentScheduleChartDetailsDTO>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region UpdateAgentSchedule

        /// <summary>
        /// Updates the agent schedule with not found.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        /// <param name="fromYear">From year.</param>
        /// <param name="fromMonth">From month.</param>
        /// <param name="fromDay">From day.</param>
        /// <param name="toYear">To year.</param>
        /// <param name="toMonth">To month.</param>
        /// <param name="toDay">To day.</param>
        [Theory]
        [InlineData("6fe0b5ad6a05416894c0718e", 2021, 3, 21, 2021, 3, 27)]
        [InlineData("6fe0b5ad6a05416894c0718f", 2021, 3, 21, 2021, 3, 27)]
        public async void UpdateAgentScheduleWithNotFound(string agentScheduleId, int fromYear, int fromMonth, int fromDay,
                                                          int toYear, int toMonth, int toDay)
        {
            AgentScheduleIdDetails agentScheduleIdDetails = new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId };
            UpdateAgentSchedule agentSchedule = new UpdateAgentSchedule
            {
                Status = SchedulingStatus.Released,
                DateFrom = new DateTime(fromYear, fromMonth, fromDay),
                DateTo = new DateTime(toYear, toMonth, toDay),
                ModifiedBy = "admin"
            };

            mockAgentScheduleRepository.Setup(mr => mr.GetAgentScheduleCount(It.IsAny<AgentScheduleIdDetails>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails) => mockDataContext.GetAgentScheduleCount(agentScheduleIdDetails));

            var result = await agentScheduleService.UpdateAgentSchedule(agentScheduleIdDetails, agentSchedule);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
        }

        /// <summary>
        /// Updates the agent schedule.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        /// <param name="fromYear">From year.</param>
        /// <param name="fromMonth">From month.</param>
        /// <param name="fromDay">From day.</param>
        /// <param name="toYear">To year.</param>
        /// <param name="toMonth">To month.</param>
        /// <param name="toDay">To day.</param>
        [Theory]
        [InlineData("5fe0b5ad6a05416894c0718e", 2021, 3, 21, 2021, 3, 27)]
        [InlineData("5fe0b5ad6a05416894c0718f", 2021, 3, 21, 2021, 3, 27)]
        public async void UpdateAgentSchedule(string agentScheduleId, int fromYear, int fromMonth, int fromDay,
                                              int toYear, int toMonth, int toDay)
        {
            AgentScheduleIdDetails agentScheduleIdDetails = new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId };
            UpdateAgentSchedule agentSchedule = new UpdateAgentSchedule
            {
                Status = SchedulingStatus.Released,
                DateFrom = new DateTime(fromYear, fromMonth, fromDay),
                DateTo = new DateTime(toYear, toMonth, toDay),
                ModifiedBy = "admin"
            };

            mockAgentScheduleRepository.Setup(mr => mr.GetAgentSchedule(It.IsAny<AgentScheduleIdDetails>(), It.IsAny<DateRange>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails, DateRange range) => mockDataContext.GetAgentSchedule(agentScheduleIdDetails, range));

            mockAgentScheduleRepository.Setup(mr => mr.GetAgentScheduleRange(It.IsAny<AgentScheduleIdDetails>(), It.IsAny<DateRange>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails, DateRange range) => mockDataContext.GetAgentScheduleRange(agentScheduleIdDetails, range));

            var result = await agentScheduleService.UpdateAgentSchedule(agentScheduleIdDetails, agentSchedule);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NoContent, result.Code);
        }

        #endregion

        #region UpdateAgentScheduleRange

        /// <summary>
        /// Updates the agent schedule range with not found.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        /// <param name="oldFromYear">The old from year.</param>
        /// <param name="oldFromMonth">The old from month.</param>
        /// <param name="oldFromDay">The old from day.</param>
        /// <param name="oldToYear">The old to year.</param>
        /// <param name="oldToMonth">The old to month.</param>
        /// <param name="oldToDay">The old to day.</param>
        /// <param name="newFromYear">The new from year.</param>
        /// <param name="newFromMonth">The new from month.</param>
        /// <param name="newFromDay">The new from day.</param>
        /// <param name="newToYear">The new to year.</param>
        /// <param name="newToMonth">The new to month.</param>
        /// <param name="newToDay">The new to day.</param>
        [Theory]
        [InlineData("6fe0b5ad6a05416894c0718e", 2021, 3, 21, 2021, 3, 27, 2021, 3, 14, 2021, 3, 20)]
        [InlineData("6fe0b5ad6a05416894c0718f", 2021, 3, 21, 2021, 3, 27, 2021, 3, 14, 2021, 3, 20)]
        public async void UpdateAgentScheduleRangeWithNotFound(string agentScheduleId, int oldFromYear, int oldFromMonth, int oldFromDay,
                                                               int oldToYear, int oldToMonth, int oldToDay, int newFromYear, int newFromMonth, 
                                                               int newFromDay, int newToYear, int newToMonth, int newToDay)
        {
            AgentScheduleIdDetails agentScheduleIdDetails = new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId };
            UpdateAgentScheduleDateRange agentScheduleRange = new UpdateAgentScheduleDateRange
            {
                OldDateFrom = new DateTime(oldFromYear, oldFromMonth, oldFromDay),
                OldDateTo = new DateTime(oldToYear, oldToMonth, oldToDay),
                NewDateFrom = new DateTime(newFromYear, newFromMonth, newFromDay),
                NewDateTo = new DateTime(newToYear, newToMonth, newToDay),
                ModifiedBy = "admin",
            };

            mockAgentScheduleRepository.Setup(mr => mr.GetAgentSchedule(It.IsAny<AgentScheduleIdDetails>(), It.IsAny<DateRange>(), It.IsAny<SchedulingStatus>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails, DateRange dateRange, SchedulingStatus status) => mockDataContext.GetAgentSchedule(agentScheduleIdDetails, dateRange, status));

            var result = await agentScheduleService.UpdateAgentScheduleRange(agentScheduleIdDetails, agentScheduleRange);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
        }

        /// <summary>
        /// Updates the agent schedule range with conflict.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        /// <param name="oldFromYear">The old from year.</param>
        /// <param name="oldFromMonth">The old from month.</param>
        /// <param name="oldFromDay">The old from day.</param>
        /// <param name="oldToYear">The old to year.</param>
        /// <param name="oldToMonth">The old to month.</param>
        /// <param name="oldToDay">The old to day.</param>
        /// <param name="newFromYear">The new from year.</param>
        /// <param name="newFromMonth">The new from month.</param>
        /// <param name="newFromDay">The new from day.</param>
        /// <param name="newToYear">The new to year.</param>
        /// <param name="newToMonth">The new to month.</param>
        /// <param name="newToDay">The new to day.</param>
        [Theory]
        [InlineData("5fe0b5ad6a05416894c0718e", 2021, 3, 21, 2021, 3, 27, 2021, 3, 7, 2021, 3, 13)]
        [InlineData("5fe0b5ad6a05416894c0718f", 2021, 3, 21, 2021, 3, 27, 2021, 3, 7, 2021, 3, 13)]
        public async void UpdateAgentScheduleRangeWithConflict(string agentScheduleId, int oldFromYear, int oldFromMonth, int oldFromDay,
                                                               int oldToYear, int oldToMonth, int oldToDay, int newFromYear, int newFromMonth,
                                                               int newFromDay, int newToYear, int newToMonth, int newToDay)
        {
            AgentScheduleIdDetails agentScheduleIdDetails = new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId };
            UpdateAgentScheduleDateRange agentScheduleRange = new UpdateAgentScheduleDateRange
            {
                OldDateFrom = new DateTime(oldFromYear, oldFromMonth, oldFromDay),
                OldDateTo = new DateTime(oldToYear, oldToMonth, oldToDay),
                NewDateFrom = new DateTime(newFromYear, newFromMonth, newFromDay),
                NewDateTo = new DateTime(newToYear, newToMonth, newToDay),
                ModifiedBy = "admin",
            };

            mockAgentScheduleRepository.Setup(mr => mr.GetAgentSchedule(It.IsAny<AgentScheduleIdDetails>(), It.IsAny<DateRange>(), It.IsAny<SchedulingStatus>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails, DateRange dateRange, SchedulingStatus status) => mockDataContext.GetAgentSchedule(agentScheduleIdDetails, dateRange, status));

            var result = await agentScheduleService.UpdateAgentScheduleRange(agentScheduleIdDetails, agentScheduleRange);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.Conflict, result.Code);
        }

        /// <summary>
        /// Updates the agent schedule range.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        /// <param name="oldFromYear">The old from year.</param>
        /// <param name="oldFromMonth">The old from month.</param>
        /// <param name="oldFromDay">The old from day.</param>
        /// <param name="oldToYear">The old to year.</param>
        /// <param name="oldToMonth">The old to month.</param>
        /// <param name="oldToDay">The old to day.</param>
        /// <param name="newFromYear">The new from year.</param>
        /// <param name="newFromMonth">The new from month.</param>
        /// <param name="newFromDay">The new from day.</param>
        /// <param name="newToYear">The new to year.</param>
        /// <param name="newToMonth">The new to month.</param>
        /// <param name="newToDay">The new to day.</param>
        [Theory]
        [InlineData("5fe0b5ad6a05416894c0718e", 2021, 3, 21, 2021, 3, 27, 2021, 3, 14, 2021, 3, 20)]
        [InlineData("5fe0b5ad6a05416894c0718f", 2021, 3, 21, 2021, 3, 27, 2021, 3, 14, 2021, 3, 20)]
        public async void UpdateAgentScheduleRange(string agentScheduleId, int oldFromYear, int oldFromMonth, int oldFromDay,
                                                   int oldToYear, int oldToMonth, int oldToDay, int newFromYear, int newFromMonth,
                                                   int newFromDay, int newToYear, int newToMonth, int newToDay)
        {
            AgentScheduleIdDetails agentScheduleIdDetails = new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId };
            UpdateAgentScheduleDateRange agentScheduleRange = new UpdateAgentScheduleDateRange
            {
                OldDateFrom = new DateTime(oldFromYear, oldFromMonth, oldFromDay),
                OldDateTo = new DateTime(oldToYear, oldToMonth, oldToDay),
                NewDateFrom = new DateTime(newFromYear, newFromMonth, newFromDay),
                NewDateTo = new DateTime(newToYear, newToMonth, newToDay),
                ModifiedBy = "admin",
            };

            mockAgentScheduleRepository.Setup(mr => mr.GetAgentSchedule(It.IsAny<AgentScheduleIdDetails>(), It.IsAny<DateRange>(), It.IsAny<SchedulingStatus>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails, DateRange dateRange, SchedulingStatus status) => mockDataContext.GetAgentSchedule(agentScheduleIdDetails, dateRange, status));

            var result = await agentScheduleService.UpdateAgentScheduleRange(agentScheduleIdDetails, agentScheduleRange);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NoContent, result.Code);
        }

        #endregion

        #region DeleteAgentScheduleRange

        /// <summary>
        /// delete the agent schedule range with not found.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        /// <param name="fromYear">From year.</param>
        /// <param name="fromMonth">From month.</param>
        /// <param name="fromDay">From day.</param>
        /// <param name="toYear">To year.</param>
        /// <param name="toMonth">To month.</param>
        /// <param name="toDay">To day.</param>
        [Theory]
        [InlineData("6fe0b5ad6a05416894c0718e", 2021, 3, 21, 2021, 3, 27)]
        [InlineData("6fe0b5ad6a05416894c0718f", 2021, 3, 21, 2021, 3, 27)]
        public async void DeleteAgentScheduleRangeAgentScheduleRangeWithNotFound(string agentScheduleId, int fromYear, int fromMonth, int fromDay,
                                                                                int toYear, int toMonth, int toDay)
        {
            AgentScheduleIdDetails agentScheduleIdDetails = new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId };
            DateRange daterange = new DateRange
            {
                DateFrom = new DateTime(fromYear, fromMonth, fromDay),
                DateTo = new DateTime(toYear, toMonth, toDay),
            };

            mockAgentScheduleRepository.Setup(mr => mr.GetAgentScheduleRange(It.IsAny<AgentScheduleIdDetails>(), It.IsAny<DateRange>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails, DateRange dateRange) => mockDataContext.GetAgentScheduleRange(agentScheduleIdDetails, dateRange));

            var result = await agentScheduleService.DeleteAgentScheduleRange(agentScheduleIdDetails, daterange);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
        }

        /// <summary>
        /// Delete the agent schedule range.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        /// <param name="oldFromYear">The old from year.</param>
        /// <param name="oldFromMonth">The old from month.</param>
        /// <param name="oldFromDay">The old from day.</param>
        /// <param name="oldToYear">The old to year.</param>
        /// <param name="oldToMonth">The old to month.</param>
        /// <param name="oldToDay">The old to day.</param>
        /// <param name="newFromYear">The new from year.</param>
        /// <param name="newFromMonth">The new from month.</param>
        /// <param name="newFromDay">The new from day.</param>
        /// <param name="newToYear">The new to year.</param>
        /// <param name="newToMonth">The new to month.</param>
        /// <param name="newToDay">The new to day.</param>
        [Theory]
        [InlineData("5fe0b5ad6a05416894c0718e", 2021, 3, 21, 2021, 3, 27)]
        [InlineData("5fe0b5ad6a05416894c0718f", 2021, 3, 21, 2021, 3, 27)]
        public async void DeleteAgentScheduleRangeAgentScheduleRange(string agentScheduleId, int fromYear, int fromMonth, int fromDay,
                                                                     int toYear, int toMonth, int toDay)
        {
            AgentScheduleIdDetails agentScheduleIdDetails = new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId };
            DateRange daterange = new DateRange
            {
                DateFrom = new DateTime(fromYear, fromMonth, fromDay),
                DateTo = new DateTime(toYear, toMonth, toDay),
            };

            mockAgentScheduleRepository.Setup(mr => mr.GetAgentScheduleRange(It.IsAny<AgentScheduleIdDetails>(), It.IsAny<DateRange>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails, DateRange dateRange) => mockDataContext.GetAgentScheduleRange(agentScheduleIdDetails, dateRange));

            var result = await agentScheduleService.DeleteAgentScheduleRange(agentScheduleIdDetails, daterange);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NoContent, result.Code);
        }

        #endregion

        #region UpdateAgentScheduleChart

        /// <summary>
        /// Updates the agent schedule chart with not found.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        /// <param name="schedulingCode">The scheduling code.</param>
        /// <param name="fromYear">From year.</param>
        /// <param name="fromMonth">From month.</param>
        /// <param name="fromDay">From day.</param>
        /// <param name="toYear">To year.</param>
        /// <param name="toMonth">To month.</param>
        /// <param name="toDay">To day.</param>
        [Theory]
        [InlineData("6fe0b5ad6a05416894c0718e", 100, 2021, 3, 10, 2021, 3, 17)]
        [InlineData("6fe0b5ad6a05416894c0718f", 101, 2021, 3, 10, 2021, 3, 17)]
        public async void UpdateAgentScheduleChartWithNotFound(string agentScheduleId, int schedulingCode, int fromYear, int fromMonth, int fromDay,
                                                               int toYear, int toMonth, int toDay)
        {
            AgentScheduleIdDetails agentScheduleIdDetails = new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId };
            UpdateAgentScheduleChart agentScheduleChart = new UpdateAgentScheduleChart
            {
                AgentScheduleCharts = new List<AgentScheduleChart>
                {
                    new AgentScheduleChart
                    {
                        Day = 1,
                        Charts = new List<ScheduleChart>
                        {
                            new ScheduleChart { StartTime = "00:00 am", EndTime = "00:05 pm", SchedulingCodeId = schedulingCode }
                        }
                    }
                },
                Status = SchedulingStatus.Pending_Schedule,
                DateFrom = new DateTime(fromYear, fromMonth, fromDay),
                DateTo = new DateTime(toYear, toMonth, toDay),
                ModifiedBy = "admin",
                ActivityOrigin = ActivityOrigin.CSS,
                ModifiedUser = 123
            };

            mockAgentAdminRepository.Setup(mr => mr.GetAgentAdminByEmployeeId(It.IsAny<EmployeeIdDetails>())).ReturnsAsync(
                (EmployeeIdDetails employeeIdDetails) => mockDataContext.GetAgentAdminByEmployeeId(employeeIdDetails));

            mockSchedulingCodeRepository.Setup(mr => mr.GetSchedulingCodesCountByIds(It.IsAny<List<int>>())).ReturnsAsync(
                (List<int> codes) => mockDataContext.GetSchedulingCodesCountByIds(codes));

            mockAgentScheduleRepository.Setup(mr => mr.GetAgentSchedule(It.IsAny<AgentScheduleIdDetails>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails) => mockDataContext.GetAgentSchedule(agentScheduleIdDetails));

            var result = await agentScheduleService.UpdateAgentScheduleChart(agentScheduleIdDetails, agentScheduleChart);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
        }

        /// <summary>
        /// Updates the agent schedule chart with not found for scheduling code.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        /// <param name="schedulingCode">The scheduling code.</param>
        /// <param name="fromYear">From year.</param>
        /// <param name="fromMonth">From month.</param>
        /// <param name="fromDay">From day.</param>
        /// <param name="toYear">To year.</param>
        /// <param name="toMonth">To month.</param>
        /// <param name="toDay">To day.</param>
        [Theory]
        [InlineData("5fe0b5ad6a05416894c0718e", 100, 2021, 3, 10, 2021, 3, 17)]
        [InlineData("5fe0b5ad6a05416894c0718f", 101, 2021, 3, 10, 2021, 3, 17)]
        public async void UpdateAgentScheduleChartWithNotFoundForSchedulingCode(string agentScheduleId, int schedulingCode, int fromYear, int fromMonth, 
                                                                                int fromDay, int toYear, int toMonth, int toDay)
        {
            AgentScheduleIdDetails agentScheduleIdDetails = new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId };
            UpdateAgentScheduleChart agentScheduleChart = new UpdateAgentScheduleChart
            {
                AgentScheduleCharts = new List<AgentScheduleChart>
                {
                    new AgentScheduleChart
                    {
                        Day = 1,
                        Charts = new List<ScheduleChart>
                        {
                            new ScheduleChart { StartTime = "00:00 am", EndTime = "00:05 pm", SchedulingCodeId = schedulingCode }
                        }
                    }
                },
                Status = SchedulingStatus.Pending_Schedule,
                DateFrom = new DateTime(fromYear, fromMonth, fromDay),
                DateTo = new DateTime(toYear, toMonth, toDay),
                ModifiedBy = "admin",
                ActivityOrigin = ActivityOrigin.CSS,
                ModifiedUser = 123
            };

            mockAgentAdminRepository.Setup(mr => mr.GetAgentAdminByEmployeeId(It.IsAny<EmployeeIdDetails>())).ReturnsAsync(
                (EmployeeIdDetails employeeIdDetails) => mockDataContext.GetAgentAdminByEmployeeId(employeeIdDetails));

            mockSchedulingCodeRepository.Setup(mr => mr.GetSchedulingCodesCountByIds(It.IsAny<List<int>>())).ReturnsAsync(
                (List<int> codes) => mockDataContext.GetSchedulingCodesCountByIds(codes));

            mockAgentScheduleRepository.Setup(mr => mr.GetAgentSchedule(It.IsAny<AgentScheduleIdDetails>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails) => mockDataContext.GetAgentSchedule(agentScheduleIdDetails));

            var result = await agentScheduleService.UpdateAgentScheduleChart(agentScheduleIdDetails, agentScheduleChart);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
        }

        /// <summary>
        /// Updates the agent schedule chart for new range.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        /// <param name="schedulingCode">The scheduling code.</param>
        /// <param name="fromYear">From year.</param>
        /// <param name="fromMonth">From month.</param>
        /// <param name="fromDay">From day.</param>
        /// <param name="toYear">To year.</param>
        /// <param name="toMonth">To month.</param>
        /// <param name="toDay">To day.</param>
        [Theory]
        [InlineData("5fe0b5ad6a05416894c0718e", 1, 2021, 3, 21, 2021, 3, 27)]
        [InlineData("5fe0b5ad6a05416894c0718f", 2, 2021, 2, 28, 2021, 3, 6)]
        public async void UpdateAgentScheduleChart(string agentScheduleId, int schedulingCode, int fromYear, int fromMonth,
                                                   int fromDay, int toYear, int toMonth, int toDay)
        {
            AgentScheduleIdDetails agentScheduleIdDetails = new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId };
            UpdateAgentScheduleChart agentScheduleChart = new UpdateAgentScheduleChart
            {
                AgentScheduleCharts = new List<AgentScheduleChart>
                {
                    new AgentScheduleChart
                    {
                        Day = 1,
                        Charts = new List<ScheduleChart>
                        {
                            new ScheduleChart { StartTime = "00:00 am", EndTime = "00:05 pm", SchedulingCodeId = schedulingCode }
                        }
                    }
                },
                Status = SchedulingStatus.Pending_Schedule,
                DateFrom = new DateTime(fromYear, fromMonth, fromDay),
                DateTo = new DateTime(toYear, toMonth, toDay),
                ModifiedBy = "admin",
                ActivityOrigin = ActivityOrigin.CSS,
                ModifiedUser = 123
            };

            mockAgentAdminRepository.Setup(mr => mr.GetAgentAdminByEmployeeId(It.IsAny<EmployeeIdDetails>())).ReturnsAsync(
                (EmployeeIdDetails employeeIdDetails) => mockDataContext.GetAgentAdminByEmployeeId(employeeIdDetails));

            mockSchedulingCodeRepository.Setup(mr => mr.GetSchedulingCodesCountByIds(It.IsAny<List<int>>())).ReturnsAsync(
                (List<int> codes) => mockDataContext.GetSchedulingCodesCountByIds(codes));

            mockAgentScheduleRepository.Setup(mr => mr.GetAgentSchedule(It.IsAny<AgentScheduleIdDetails>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails) => mockDataContext.GetAgentSchedule(agentScheduleIdDetails));

            var result = await agentScheduleService.UpdateAgentScheduleChart(agentScheduleIdDetails, agentScheduleChart);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NoContent, result.Code);
        }

        #endregion

        #region ImportAgentScheduleChart

        /// <summary>
        /// Imports the agent schedule chart with not found for scheduling code.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        /// <param name="schedulingCode">The scheduling code.</param>
        /// <param name="employeeId">The employee identifier.</param>
        /// <param name="fromYear">From year.</param>
        /// <param name="fromMonth">From month.</param>
        /// <param name="fromDay">From day.</param>
        /// <param name="toYear">To year.</param>
        /// <param name="toMonth">To month.</param>
        /// <param name="toDay">To day.</param>
        [Theory]
        [InlineData(100, 1, 2021, 4, 4, 2021, 4, 10)]
        [InlineData(101, 2, 2021, 4, 4, 2021, 4, 10)]
        public async void ImportAgentScheduleChartWithNotFoundForSchedulingCode(int schedulingCode, int employeeId, int fromYear, 
                                                                                int fromMonth, int fromDay, int toYear, int toMonth, int toDay)
        {
            ImportAgentSchedule importAgentSchedule = new ImportAgentSchedule
            {
                ImportAgentScheduleCharts = new List<ImportAgentScheduleChart>
                {
                    new ImportAgentScheduleChart
                    {
                        EmployeeId = employeeId,
                        Ranges = new List<ImportAgentScheduleRange>
                        {
                            new ImportAgentScheduleRange
                            {
                                DateFrom = new DateTime(fromYear, fromMonth, fromDay),
                                DateTo = new DateTime(toYear, toMonth, toDay),
                                AgentScheduleCharts = new List<AgentScheduleChart>
                                {
                                    new AgentScheduleChart
                                    {
                                        Day = 0,
                                        Charts = new List<ScheduleChart>
                                        {
                                            new ScheduleChart
                                            {
                                                StartTime = "00:00 am",
                                                EndTime = "00:05 pm",
                                                SchedulingCodeId = schedulingCode
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
                ActivityOrigin  = ActivityOrigin.CSS,
                ModifiedBy = "admin",
                ModifiedUser = 123
            };

            mockSchedulingCodeRepository.Setup(mr => mr.GetSchedulingCodesCountByIds(It.IsAny<List<int>>())).ReturnsAsync(
                (List<int> codes) => mockDataContext.GetSchedulingCodesCountByIds(codes));

            mockAgentScheduleRepository.Setup(mr => mr.GetAgentScheduleByEmployeeId(It.IsAny<EmployeeIdDetails>())).ReturnsAsync(
                (EmployeeIdDetails employeeIdDetails) => mockDataContext.GetAgentScheduleByEmployeeId(employeeIdDetails));

            mockAgentAdminRepository.Setup(mr => mr.GetAgentAdminByEmployeeId(It.IsAny<EmployeeIdDetails>())).ReturnsAsync(
                (EmployeeIdDetails employeeIdDetails) => mockDataContext.GetAgentAdminByEmployeeId(employeeIdDetails));

            var result = await agentScheduleService.ImportAgentScheduleChart(importAgentSchedule);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
        }

        /// <summary>
        /// Imports the agent schedule chart for scheduling tab.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        [Theory]
        [InlineData(1, 1, 2021, 4, 4, 2021, 4, 10)]
        [InlineData(2, 2, 2021, 4, 4, 2021, 4, 10)]
        public async void ImportAgentScheduleChart(int schedulingCode, int employeeId, int fromYear, int fromMonth, int fromDay, int toYear, 
                                                   int toMonth, int toDay)
        {
            ImportAgentSchedule importAgentSchedule = new ImportAgentSchedule
            {
                ImportAgentScheduleCharts = new List<ImportAgentScheduleChart>
                {
                    new ImportAgentScheduleChart
                    {
                        EmployeeId = employeeId,
                        Ranges = new List<ImportAgentScheduleRange>
                        {
                            new ImportAgentScheduleRange
                            {
                                DateFrom = new DateTime(fromYear, fromMonth, fromDay),
                                DateTo = new DateTime(toYear, toMonth, toDay),
                                AgentScheduleCharts = new List<AgentScheduleChart>
                                {
                                    new AgentScheduleChart
                                    {
                                        Day = 0,
                                        Charts = new List<ScheduleChart>
                                        {
                                            new ScheduleChart
                                            {
                                                StartTime = "00:00 am",
                                                EndTime = "00:05 pm",
                                                SchedulingCodeId = schedulingCode
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    },
                    new ImportAgentScheduleChart
                    {
                        EmployeeId = employeeId,
                        Ranges = new List<ImportAgentScheduleRange>
                        {
                            new ImportAgentScheduleRange
                            {
                                DateFrom = new DateTime(2021, 3, 21),
                                DateTo = new DateTime(2021, 3, 27),
                                AgentScheduleCharts = new List<AgentScheduleChart>
                                {
                                    new AgentScheduleChart
                                    {
                                        Day = 0,
                                        Charts = new List<ScheduleChart>
                                        {
                                            new ScheduleChart
                                            {
                                                StartTime = "00:00 am",
                                                EndTime = "00:05 pm",
                                                SchedulingCodeId = schedulingCode
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
                ActivityOrigin = ActivityOrigin.CSS,
                ModifiedBy = "admin",
                ModifiedUser = 123
            };

            mockSchedulingCodeRepository.Setup(mr => mr.GetSchedulingCodesCountByIds(It.IsAny<List<int>>())).ReturnsAsync(
                (List<int> codes) => mockDataContext.GetSchedulingCodesCountByIds(codes));

            mockAgentScheduleRepository.Setup(mr => mr.GetAgentScheduleByEmployeeId(It.IsAny<EmployeeIdDetails>())).ReturnsAsync(
                (EmployeeIdDetails employeeIdDetails) => mockDataContext.GetAgentScheduleByEmployeeId(employeeIdDetails));

            mockAgentAdminRepository.Setup(mr => mr.GetAgentAdminByEmployeeId(It.IsAny<EmployeeIdDetails>())).ReturnsAsync(
                (EmployeeIdDetails employeeIdDetails) => mockDataContext.GetAgentAdminByEmployeeId(employeeIdDetails));

            var result = await agentScheduleService.ImportAgentScheduleChart(importAgentSchedule);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NoContent, result.Code);
        }

        #endregion

        #region CopyAgentSchedule

        /// <summary>
        /// Copies the agent schedule with not found.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        /// <param name="fromYear">From year.</param>
        /// <param name="fromMonth">From month.</param>
        /// <param name="fromDay">From day.</param>
        /// <param name="toYear">To year.</param>
        /// <param name="toMonth">To month.</param>
        /// <param name="toDay">To day.</param>
        [Theory]
        [InlineData("6fe0b5ad6a05416894c0718e", 2021, 4, 4, 2021, 4, 10)]
        [InlineData("6fe0b5ad6a05416894c0718f", 2021, 4, 4, 2021, 4, 10)]
        public async void CopyAgentScheduleWithNotFound(string agentScheduleId,int fromYear, int fromMonth, int fromDay, int toYear,
                                                        int toMonth, int toDay)
        {
            AgentScheduleIdDetails agentScheduleIdDetails = new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId };
            CopyAgentSchedule copyAgentSchedule = new CopyAgentSchedule
            {
                AgentSchedulingGroupId = 1,
                DateFrom = new DateTime(fromYear, fromMonth, fromDay),
                DateTo = new DateTime(toYear, toMonth, toDay),
                ActivityOrigin = ActivityOrigin.CSS,
                ModifiedBy = "admin",
                ModifiedUser = 123
            };

            mockAgentScheduleRepository.Setup(mr => mr.GetAgentSchedule(It.IsAny<AgentScheduleIdDetails>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails) => mockDataContext.GetAgentSchedule(agentScheduleIdDetails));

            mockAgentScheduleRepository.Setup(mr => mr.GetEmployeeIdsByAgentScheduleGroupId(It.IsAny<AgentSchedulingGroupIdDetails>())).ReturnsAsync(
                (AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails) => mockDataContext.GetEmployeeIdsByAgentScheduleGroupId(agentSchedulingGroupIdDetails));

            mockAgentScheduleRepository.Setup(mr => mr.GetAgentScheduleByEmployeeId(It.IsAny<EmployeeIdDetails>())).ReturnsAsync(
                (EmployeeIdDetails employeeIdDetails) => mockDataContext.GetAgentScheduleByEmployeeId(employeeIdDetails));

            mockAgentAdminRepository.Setup(mr => mr.GetAgentAdminByEmployeeId(It.IsAny<EmployeeIdDetails>())).ReturnsAsync(
                (EmployeeIdDetails employeeIdDetails) => mockDataContext.GetAgentAdminByEmployeeId(employeeIdDetails));

            var result = await agentScheduleService.CopyAgentScheduleChart(agentScheduleIdDetails, copyAgentSchedule);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
        }

        /// <summary>
        /// Copies the agent schedule.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        /// <param name="fromYear">From year.</param>
        /// <param name="fromMonth">From month.</param>
        /// <param name="fromDay">From day.</param>
        /// <param name="toYear">To year.</param>
        /// <param name="toMonth">To month.</param>
        /// <param name="toDay">To day.</param>
        [Theory]
        [InlineData("5fe0b5ad6a05416894c0718e", 2021, 3, 21, 2021, 3, 27)]
        [InlineData("5fe0b5ad6a05416894c0718f", 2021, 3, 21, 2021, 3, 27)]
        public async void CopyAgentSchedule(string agentScheduleId, int fromYear, int fromMonth, int fromDay, int toYear,
                                            int toMonth, int toDay)
        {
            AgentScheduleIdDetails agentScheduleIdDetails = new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId };
            CopyAgentSchedule copyAgentSchedule = new CopyAgentSchedule
            {
                AgentSchedulingGroupId = 1,
                DateFrom = new DateTime(fromYear, fromMonth, fromDay),
                DateTo = new DateTime(toYear, toMonth, toDay),
                ActivityOrigin = ActivityOrigin.CSS,
                ModifiedBy = "admin",
                ModifiedUser = 123
            };

            mockAgentScheduleRepository.Setup(mr => mr.GetAgentSchedule(It.IsAny<AgentScheduleIdDetails>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails) => mockDataContext.GetAgentSchedule(agentScheduleIdDetails));

            mockAgentScheduleRepository.Setup(mr => mr.GetEmployeeIdsByAgentScheduleGroupId(It.IsAny<AgentSchedulingGroupIdDetails>())).ReturnsAsync(
                (AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails) => mockDataContext.GetEmployeeIdsByAgentScheduleGroupId(agentSchedulingGroupIdDetails));

            mockAgentScheduleRepository.Setup(mr => mr.GetAgentScheduleByEmployeeId(It.IsAny<EmployeeIdDetails>())).ReturnsAsync(
                (EmployeeIdDetails employeeIdDetails) => mockDataContext.GetAgentScheduleByEmployeeId(employeeIdDetails));

            mockAgentAdminRepository.Setup(mr => mr.GetAgentAdminByEmployeeId(It.IsAny<EmployeeIdDetails>())).ReturnsAsync(
                (EmployeeIdDetails employeeIdDetails) => mockDataContext.GetAgentAdminByEmployeeId(employeeIdDetails));

            var result = await agentScheduleService.CopyAgentScheduleChart(agentScheduleIdDetails, copyAgentSchedule);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NoContent, result.Code);
        }

        #endregion
    }
}
