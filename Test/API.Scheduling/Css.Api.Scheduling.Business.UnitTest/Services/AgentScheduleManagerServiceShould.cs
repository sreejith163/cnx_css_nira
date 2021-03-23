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
using System;
using Css.Api.Core.Models.Enums;
using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using Css.Api.Scheduling.Models.DTO.Request.AgentScheduleManager;
using Css.Api.Scheduling.Models.DTO.Response.AgentScheduleManager;
using Css.Api.Scheduling.Models.Profiles.ActivityLog;
using Css.Api.Scheduling.Models.DTO.Request.MySchedule;
using Css.Api.Scheduling.Models.DTO.Response.MySchedule;
using Css.Api.Scheduling.Models.DTO.Request.SkillGroup;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedulingGroup;

namespace Css.Api.Scheduling.Business.UnitTest.Services
{
    public class AgentScheduleManagerServiceShould
    {
        /// <summary>
        /// The agent schedule manager service
        /// </summary>
        private readonly IAgentScheduleManagerService agentScheduleManagerService;

        /// <summary>
        /// The mock activity log repository
        /// </summary>
        private readonly Mock<IActivityLogRepository> mockActivityLogRepository;

        /// <summary>
        /// The mock agent schedule manager repository
        /// </summary>
        private readonly Mock<IAgentScheduleManagerRepository> mockAgentScheduleManagerRepository;

        /// <summary>
        /// The mock agent schedule group repository
        /// </summary>
        private readonly Mock<IAgentSchedulingGroupRepository> mockAgentSchedulingGroupRepository;

        /// <summary>
        /// The agent schedule group repository
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
        public AgentScheduleManagerServiceShould()
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
            mockAgentScheduleManagerRepository = new Mock<IAgentScheduleManagerRepository>();
            mockAgentSchedulingGroupRepository = new Mock<IAgentSchedulingGroupRepository>();
            mockAgentAdminRepository = new Mock<IAgentAdminRepository>();
            mockSchedulingCodeRepository = new Mock<ISchedulingCodeRepository>();

            var mockUnitWork = new Mock<IUnitOfWork>();

            mockDataContext = new MockDataContext();

            agentScheduleManagerService = new AgentScheduleManagerService(mockHttContext.Object, mockActivityLogRepository.Object, mockAgentScheduleManagerRepository.Object,
                                                                          mockAgentAdminRepository.Object, mockSchedulingCodeRepository.Object, mockAgentSchedulingGroupRepository.Object,
                                                                          mapper, mockUnitWork.Object);
        }

        #region GetAgentScheduleManagerCharts

        /// <summary>
        /// Gets the agent schedules.
        /// </summary>
        [Fact]
        public async void GetAgentScheduleManagerCharts()
        {
            AgentScheduleManagerChartQueryparameter agentScheduleManagerChartQueryparameter = new AgentScheduleManagerChartQueryparameter();

            mockAgentAdminRepository.Setup(mr => mr.GetAgentAdmins(It.IsAny<AgentAdminQueryParameter>())).ReturnsAsync(
                (AgentAdminQueryParameter agentAdminQueryParameter) => mockDataContext.GetAgentAdmins(agentAdminQueryParameter));

            mockAgentScheduleManagerRepository.Setup(mr => mr.GetAgentScheduleManagerCharts(It.IsAny<AgentScheduleManagerChartQueryparameter>())).ReturnsAsync(
                (AgentScheduleManagerChartQueryparameter agentScheduleManagerChartQueryparameter) => mockDataContext.GetAgentScheduleManagerCharts(agentScheduleManagerChartQueryparameter));
            
            mockAgentScheduleManagerRepository.Setup(mr => mr.HasAgentScheduleManagerChartByEmployeeId(It.IsAny<EmployeeIdDetails>())).ReturnsAsync(
                (EmployeeIdDetails employeeIdDetails) => mockDataContext.HasAgentScheduleManagerChartByEmployeeId(employeeIdDetails));

            var result = await agentScheduleManagerService.GetAgentScheduleManagerCharts(agentScheduleManagerChartQueryparameter);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<List<AgentScheduleManagerChartDetailsDTO>>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region GetAgentScheduledOpen

        /// <summary>
        /// Gets the agent scheduled open with not found for agent sheduling group.
        /// </summary>
        /// <param name="skillGroupId">The skill group identifier.</param>
        [Theory]
        [InlineData(100)]
        [InlineData(101)]
        public async void GetAgentScheduledOpenWithNotFoundForAgentShedulingGroup(int skillGroupId)
        {
            var date = new DateTime(2021, 3, 21);

            MyScheduleQueryParameter myScheduleQueryParameter = new MyScheduleQueryParameter
            {
                AgentSchedulingGroupId = 1,
                StartDate = new DateTime(2021, 3, 21),
                EndDate = new DateTime(2021, 3, 21)
            };

            mockAgentSchedulingGroupRepository.Setup(mr => mr.GetAgentSchedulingGroupBySkillGroupId(It.IsAny<SkillGroupIdDetails>())).ReturnsAsync(
               (SkillGroupIdDetails skillGroupIdDetails) => mockDataContext.GetAgentSchedulingGroupBySkillGroupId(skillGroupIdDetails));

            mockAgentScheduleManagerRepository.Setup(mr => mr.GetAgentScheduleByAgentSchedulingGroupId(It.IsAny<List<int>>(), It.IsAny<DateTimeOffset>())).ReturnsAsync(
                (List<int> agentSchedulingGroupIdDetailsList, DateTimeOffset date) => mockDataContext.GetAgentScheduleByAgentSchedulingGroupId(agentSchedulingGroupIdDetailsList, date));

            var result = await agentScheduleManagerService.GetAgentScheduledOpen(skillGroupId, date);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
        }

        /// <summary>
        /// Gets the agent scheduled open.
        /// </summary>
        /// <param name="skillGroupId">The skill group identifier.</param>
        /// <returns></returns>
        [Theory]
        [InlineData(1)]
        public async void GetAgentScheduledOpen(int skillGroupId)
        {
            var date = new DateTime(2021, 3, 21);

            MyScheduleQueryParameter myScheduleQueryParameter = new MyScheduleQueryParameter
            {
                AgentSchedulingGroupId = 1,
                StartDate = new DateTime(2021, 3, 21),
                EndDate = new DateTime(2021, 3, 21)
            };

            mockAgentSchedulingGroupRepository.Setup(mr => mr.GetAgentSchedulingGroupBySkillGroupId(It.IsAny<SkillGroupIdDetails>())).ReturnsAsync(
               (SkillGroupIdDetails skillGroupIdDetails) => mockDataContext.GetAgentSchedulingGroupBySkillGroupId(skillGroupIdDetails));

            mockAgentScheduleManagerRepository.Setup(mr => mr.GetAgentScheduleByAgentSchedulingGroupId(It.IsAny<List<int>>(), It.IsAny<DateTimeOffset>())).ReturnsAsync(
                (List<int> agentSchedulingGroupIdDetailsList, DateTimeOffset date) => mockDataContext.GetAgentScheduleByAgentSchedulingGroupId(agentSchedulingGroupIdDetailsList, date));

            var result = await agentScheduleManagerService.GetAgentScheduledOpen(skillGroupId, date);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region GetAgentMySchedule

        /// <summary>
        /// Gets the agent my schedule with not found.
        /// </summary>
        /// <param name="employeeId">The employee identifier.</param>
        [Theory]
        [InlineData(100)]
        [InlineData(101)]
        public async void GetAgentMyScheduleWithNotFound(int employeeId)
        {
            EmployeeIdDetails employeeIdDetails = new EmployeeIdDetails { Id = employeeId };

            MyScheduleQueryParameter myScheduleQueryParameter = new MyScheduleQueryParameter
            {
                AgentSchedulingGroupId = 1,
                StartDate = new DateTime(2021, 3, 21),
                EndDate = new DateTime(2021, 3, 21)
            };

            mockAgentScheduleManagerRepository.Setup(mr => mr.GetAgentScheduleManagerChartByEmployeeId(It.IsAny<EmployeeIdDetails>(), It.IsAny<MyScheduleQueryParameter>())).ReturnsAsync(
                (EmployeeIdDetails employeeIdDetails, MyScheduleQueryParameter myScheduleQueryParameter) => mockDataContext.GetAgentScheduleManagerChartByEmployeeId(employeeIdDetails, myScheduleQueryParameter));

            var result = await agentScheduleManagerService.GetAgentMySchedule(employeeIdDetails, myScheduleQueryParameter);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
        }

        /// <summary>
        /// Gets the agent my schedule.
        /// </summary>
        /// <param name="employeeId">The employee identifier.</param>
        [Theory]
        [InlineData(1)]
        public async void GetAgentMySchedule(int employeeId)
        {
            EmployeeIdDetails employeeIdDetails = new EmployeeIdDetails { Id = employeeId };

            MyScheduleQueryParameter myScheduleQueryParameter = new MyScheduleQueryParameter
            {
                AgentSchedulingGroupId = 1,
                StartDate = new DateTime(2021, 3, 21),
                EndDate = new DateTime(2021, 3, 21)
            };

            mockAgentScheduleManagerRepository.Setup(mr => mr.GetAgentScheduleManagerChartByEmployeeId(It.IsAny<EmployeeIdDetails>(), It.IsAny<MyScheduleQueryParameter>())).ReturnsAsync(
                (EmployeeIdDetails employeeIdDetails, MyScheduleQueryParameter myScheduleQueryParameter) => mockDataContext.GetAgentScheduleManagerChartByEmployeeId(employeeIdDetails, myScheduleQueryParameter));

            var result = await agentScheduleManagerService.GetAgentMySchedule(employeeIdDetails, myScheduleQueryParameter);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<AgentMyScheduleDetailsDTO>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region UpdateAgentScheduleMangerChart

        /// <summary>
        /// Gets the agent schedule manager charts with not found for scheduling code.
        /// </summary>
        /// <param name="schedulingCode">The scheduling code.</param>
        /// <param name="employeeId">The employee identifier.</param>
        [Theory]
        [InlineData(100, 1)]
        [InlineData(101, 2)]
        public async void UpdateAgentScheduleMangerChartWithNotFoundForSchedulingCode(int schedulingCode, int employeeId)
        {
            UpdateAgentScheduleManager agentScheduleManager = new UpdateAgentScheduleManager
            {
                ScheduleManagers = new List<UpdateScheduleManagerChart>
                {
                    new UpdateScheduleManagerChart
                    {
                        EmployeeId = employeeId,
                        AgentScheduleManagerCharts = new List<AgentScheduleManagerChartDTO>
                        {
                            new AgentScheduleManagerChartDTO
                            {
                                Date = DateTime.Now,
                                Charts = new List<AgentScheduleManagerChart>
                                {
                                     new AgentScheduleManagerChart
                                     {
                                         StartDateTime = DateTime.Now,
                                         EndDateTime = DateTime.Now,
                                         SchedulingCodeId = schedulingCode
                                     },
                                }
                            }
                        }
                    }
                },
                ModifiedBy = "admin",
                ModifiedUser = 5,
                ActivityOrigin = ActivityOrigin.CSS,
                IsImport = false
            };

            mockSchedulingCodeRepository.Setup(mr => mr.GetSchedulingCodesCountByIds(It.IsAny<List<int>>())).ReturnsAsync(
                (List<int> codes) => mockDataContext.GetSchedulingCodesCountByIds(codes));

            mockAgentScheduleManagerRepository.Setup(mr => mr.IsAgentScheduleManagerChartExists(It.IsAny<EmployeeIdDetails>(), It.IsAny<DateDetails>())).ReturnsAsync(
                (EmployeeIdDetails employeeIdDetails, DateDetails dateDetails) => mockDataContext.IsAgentScheduleManagerChartExists(employeeIdDetails, dateDetails));

            mockAgentAdminRepository.Setup(mr => mr.GetAgentAdminByEmployeeId(It.IsAny<EmployeeIdDetails>())).ReturnsAsync(
                (EmployeeIdDetails employeeIdDetails) => mockDataContext.GetAgentAdminIdByEmployeeId(employeeIdDetails));

            var result = await agentScheduleManagerService.UpdateAgentScheduleMangerChart(agentScheduleManager);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
        }

        /// <summary>
        /// Gets the agent schedule manager charts without import flag.
        /// </summary>
        /// <param name="schedulingCode">The scheduling code.</param>
        /// <param name="employeeId">The employee identifier.</param>
        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        public async void UpdateAgentScheduleMangerChartWithoutImportFlag(int schedulingCode, int employeeId)
        {
            UpdateAgentScheduleManager agentScheduleManager = new UpdateAgentScheduleManager
            {
                ScheduleManagers = new List<UpdateScheduleManagerChart>
                {
                    new UpdateScheduleManagerChart
                    {
                        EmployeeId = employeeId,
                        AgentScheduleManagerCharts = new List<AgentScheduleManagerChartDTO>
                        {
                            new AgentScheduleManagerChartDTO
                            {
                                Date = DateTime.Now,
                                Charts = new List<AgentScheduleManagerChart>
                                {
                                     new AgentScheduleManagerChart
                                     {
                                         StartDateTime = DateTime.Now,
                                         EndDateTime = DateTime.Now,
                                         SchedulingCodeId = schedulingCode
                                     },
                                }
                            }
                        }
                    }
                },
                ModifiedBy = "admin",
                ModifiedUser = 5,
                ActivityOrigin = ActivityOrigin.CSS,
                IsImport = false
            };

            mockSchedulingCodeRepository.Setup(mr => mr.GetSchedulingCodesCountByIds(It.IsAny<List<int>>())).ReturnsAsync(
                (List<int> codes) => mockDataContext.GetSchedulingCodesCountByIds(codes));

            mockAgentScheduleManagerRepository.Setup(mr => mr.IsAgentScheduleManagerChartExists(It.IsAny<EmployeeIdDetails>(), It.IsAny<DateDetails>())).ReturnsAsync(
                (EmployeeIdDetails employeeIdDetails, DateDetails dateDetails) => mockDataContext.IsAgentScheduleManagerChartExists(employeeIdDetails, dateDetails));

            mockAgentAdminRepository.Setup(mr => mr.GetAgentAdminByEmployeeId(It.IsAny<EmployeeIdDetails>())).ReturnsAsync(
                (EmployeeIdDetails employeeIdDetails) => mockDataContext.GetAgentAdminIdByEmployeeId(employeeIdDetails));

            var result = await agentScheduleManagerService.UpdateAgentScheduleMangerChart(agentScheduleManager);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NoContent, result.Code);
        }

        /// <summary>
        /// Gets the agent schedule manager charts with import flag.
        /// </summary>
        /// <param name="schedulingCode">The scheduling code.</param>
        /// <param name="employeeId">The employee identifier.</param>
        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        public async void UpdateAgentScheduleMangerChartWithImportFlag(int schedulingCode, int employeeId)
        {
            UpdateAgentScheduleManager agentScheduleManager = new UpdateAgentScheduleManager
            {
                ScheduleManagers = new List<UpdateScheduleManagerChart>
                {
                    new UpdateScheduleManagerChart
                    {
                        EmployeeId = employeeId,
                        AgentScheduleManagerCharts = new List<AgentScheduleManagerChartDTO>
                        {
                            new AgentScheduleManagerChartDTO
                            {
                                Date = DateTime.Now,
                                Charts = new List<AgentScheduleManagerChart>
                                {
                                     new AgentScheduleManagerChart
                                     {
                                         StartDateTime = DateTime.Now,
                                         EndDateTime = DateTime.Now,
                                         SchedulingCodeId = schedulingCode
                                     },
                                }
                            }
                        }
                    }
                },
                ModifiedBy = "admin",
                ModifiedUser = 5,
                ActivityOrigin = ActivityOrigin.CSS,
                IsImport = true
            };

            mockSchedulingCodeRepository.Setup(mr => mr.GetSchedulingCodesCountByIds(It.IsAny<List<int>>())).ReturnsAsync(
                (List<int> codes) => mockDataContext.GetSchedulingCodesCountByIds(codes));

            mockAgentScheduleManagerRepository.Setup(mr => mr.IsAgentScheduleManagerChartExists(It.IsAny<EmployeeIdDetails>(), It.IsAny<DateDetails>())).ReturnsAsync(
                (EmployeeIdDetails employeeIdDetails, DateDetails dateDetails) => mockDataContext.IsAgentScheduleManagerChartExists(employeeIdDetails, dateDetails));

            mockAgentAdminRepository.Setup(mr => mr.GetAgentAdminByEmployeeId(It.IsAny<EmployeeIdDetails>())).ReturnsAsync(
                (EmployeeIdDetails employeeIdDetails) => mockDataContext.GetAgentAdminIdByEmployeeId(employeeIdDetails));

            var result = await agentScheduleManagerService.UpdateAgentScheduleMangerChart(agentScheduleManager);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NoContent, result.Code);
        }

        #endregion

        #region CopyAgentScheduleManagerChart

        /// <summary>
        /// Copies the agent schedule manager chart with not found.
        /// </summary>
        /// <param name="employeeId">The employee identifier.</param>
        [Theory]
        [InlineData(100)]
        [InlineData(101)]
        public async void CopyAgentScheduleManagerChartWithNotFound(int employeeId)
        {
            EmployeeIdDetails employeeIdDetails = new EmployeeIdDetails { Id = employeeId };
            CopyAgentScheduleManager copyAgentScheduleManager = new CopyAgentScheduleManager
            {
                AgentSchedulingGroupId = 1,
                Date = DateTime.Now,
                ActivityOrigin = ActivityOrigin.CSS,
                EmployeeIds = new List<int> { 1, 2 },
                ModifiedBy = "admin",
                ModifiedUser = 1
            };

            mockAgentScheduleManagerRepository.Setup(mr => mr.GetAgentScheduleManagerChart(It.IsAny<EmployeeIdDetails>(), It.IsAny<DateDetails>())).ReturnsAsync(
                (EmployeeIdDetails employeeIdDetails, DateDetails dateDetails) => mockDataContext.GetAgentScheduleManagerChart(employeeIdDetails, dateDetails));

            mockAgentScheduleManagerRepository.Setup(mr => mr.IsAgentScheduleManagerChartExists(It.IsAny<EmployeeIdDetails>(), It.IsAny<DateDetails>())).ReturnsAsync(
                (EmployeeIdDetails employeeIdDetails, DateDetails dateDetails) => mockDataContext.IsAgentScheduleManagerChartExists(employeeIdDetails, dateDetails));

            mockAgentAdminRepository.Setup(mr => mr.GetAgentAdminByEmployeeId(It.IsAny<EmployeeIdDetails>())).ReturnsAsync(
                (EmployeeIdDetails employeeIdDetails) => mockDataContext.GetAgentAdminIdByEmployeeId(employeeIdDetails));

            var result = await agentScheduleManagerService.CopyAgentScheduleManagerChart(employeeIdDetails, copyAgentScheduleManager);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
        }

        /// <summary>
        /// Copies the agent schedule manager chart with not found.
        /// </summary>
        /// <param name="employeeId">The employee identifier.</param>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        [Theory]
        [InlineData(1, 2021, 3, 21)]
        [InlineData(2, 2021, 3, 22)]
        public async void CopyAgentScheduleManagerChart(int employeeId, int year, int month, int day)
        {
            EmployeeIdDetails employeeIdDetails = new EmployeeIdDetails { Id = employeeId };
            CopyAgentScheduleManager copyAgentScheduleManager = new CopyAgentScheduleManager
            {
                AgentSchedulingGroupId = 1,
                Date = new DateTime(year, month, day),
                ActivityOrigin = ActivityOrigin.CSS,
                EmployeeIds = new List<int>(),
                ModifiedBy = "admin",
                ModifiedUser = 1
            };

            mockAgentScheduleManagerRepository.Setup(mr => mr.GetAgentScheduleManagerChart(It.IsAny<EmployeeIdDetails>(), It.IsAny<DateDetails>())).ReturnsAsync(
                (EmployeeIdDetails employeeIdDetails, DateDetails dateDetails) => mockDataContext.GetAgentScheduleManagerChart(employeeIdDetails, dateDetails));

            mockAgentScheduleManagerRepository.Setup(mr => mr.IsAgentScheduleManagerChartExists(It.IsAny<EmployeeIdDetails>(), It.IsAny<DateDetails>())).ReturnsAsync(
                (EmployeeIdDetails employeeIdDetails, DateDetails dateDetails) => mockDataContext.IsAgentScheduleManagerChartExists(employeeIdDetails, dateDetails));

            mockAgentAdminRepository.Setup(mr => mr.GetAgentAdminByEmployeeId(It.IsAny<EmployeeIdDetails>())).ReturnsAsync(
                (EmployeeIdDetails employeeIdDetails) => mockDataContext.GetAgentAdminIdByEmployeeId(employeeIdDetails));

            mockAgentAdminRepository.Setup(mr => mr.GetEmployeeIdsByAgentSchedulingGroup(It.IsAny<AgentSchedulingGroupIdDetails>())).ReturnsAsync(
                (AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails) => mockDataContext.GetEmployeeIdsByAgentSchedulingGroup(agentSchedulingGroupIdDetails));

            var result = await agentScheduleManagerService.CopyAgentScheduleManagerChart(employeeIdDetails, copyAgentScheduleManager);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NoContent, result.Code);
        }

        #endregion
    }
}
