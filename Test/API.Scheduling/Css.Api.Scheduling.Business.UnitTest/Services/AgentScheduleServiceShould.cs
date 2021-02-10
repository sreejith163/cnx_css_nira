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
using Css.Api.Scheduling.Models.Enums;
using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Core.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedulingGroup;

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
                cfg.AddProfile(new AgentAdminProfile());
            });

            mapper = new Mapper(mapperConfig);

            var context = new DefaultHttpContext();
            Mock<IHttpContextAccessor> mockHttContext = new Mock<IHttpContextAccessor>();
            mockHttContext.Setup(_ => _.HttpContext).Returns(context);

            mockActivityLogRepository = new Mock<IActivityLogRepository>();
            mockAgentScheduleRepository = new Mock<IAgentScheduleRepository>();
            mockSchedulingCodeRepository = new Mock<ISchedulingCodeRepository>();

            var mockUnitWork = new Mock<IUnitOfWork>();

            mockDataContext = new MockDataContext();

            agentScheduleService = new AgentScheduleService(mockHttContext.Object, mockActivityLogRepository.Object, mockAgentScheduleRepository.Object, 
                                                            mockSchedulingCodeRepository.Object, mapper, mockUnitWork.Object);
        }

        #region GetAgentSchedules

        /// <summary>
        /// Gets the language translations.
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
        /// Gets the agent schedule with not found.
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
        public async void GetAgentScheduleChartsForSchedulingTab(string agentScheduleId)
            {
            var agentScheduleChartQueryparameter = new AgentScheduleChartQueryparameter() { AgentScheduleType = AgentScheduleType.SchedulingTab };

            mockAgentScheduleRepository.Setup(mr => mr.GetAgentSchedule(It.IsAny<AgentScheduleIdDetails>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails) => mockDataContext.GetAgentSchedule(agentScheduleIdDetails));

            var result = await agentScheduleService.GetAgentScheduleCharts(new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId }, agentScheduleChartQueryparameter);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<AgentScheduleChartDetailsDTO>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        /// <summary>
        /// Gets the agent schedule charts for schedling manager tab.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        [Theory]
        [InlineData("5fe0b5ad6a05416894c0718e")]
        [InlineData("5fe0b5ad6a05416894c0718f")]
        public async void GetAgentScheduleChartsForSchedlingManagerTab(string agentScheduleId)
        {
            var agentScheduleChartQueryparameter = new AgentScheduleChartQueryparameter() { AgentScheduleType = AgentScheduleType.SchedulingMangerTab };

            mockAgentScheduleRepository.Setup(mr => mr.GetAgentSchedule(It.IsAny<AgentScheduleIdDetails>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails) => mockDataContext.GetAgentSchedule(agentScheduleIdDetails));

            var result = await agentScheduleService.GetAgentScheduleCharts(new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId }, agentScheduleChartQueryparameter);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<AgentScheduleManagerChartDetailsDTO>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        #endregion

        #region UpdateAgentSchedule

        /// <summary>
        /// Updates the agent schedule with not found.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        [Theory]
        [InlineData("6fe0b5ad6a05416894c0718e")]
        [InlineData("6fe0b5ad6a05416894c0718f")]
        public async void UpdateAgentScheduleWithNotFound(string agentScheduleId)
        {
            AgentScheduleIdDetails agentScheduleIdDetails = new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId };
            UpdateAgentSchedule agentSchedule = new UpdateAgentSchedule
            {
                Status = SchedulingStatus.Approved,
                DateFrom = DateTime.UtcNow,
                DateTo = DateTime.UtcNow,
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
        [Theory]
        [InlineData("5fe0b5ad6a05416894c0718e")]
        [InlineData("5fe0b5ad6a05416894c0718f")]
        public async void UpdateAgentSchedule(string agentScheduleId)
        {
            AgentScheduleIdDetails agentScheduleIdDetails = new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId };
            UpdateAgentSchedule agentSchedule = new UpdateAgentSchedule
            {
                Status = SchedulingStatus.Approved,
                DateFrom = DateTime.UtcNow,
                DateTo = DateTime.UtcNow,
                ModifiedBy = "admin"
            };

            mockAgentScheduleRepository.Setup(mr => mr.GetAgentScheduleCount(It.IsAny<AgentScheduleIdDetails>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails) => mockDataContext.GetAgentScheduleCount(agentScheduleIdDetails));

            var result = await agentScheduleService.UpdateAgentSchedule(agentScheduleIdDetails, agentSchedule);

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
        [Theory]
        [InlineData("6fe0b5ad6a05416894c0718e", 100)]
        [InlineData("6fe0b5ad6a05416894c0718f", 101)]
        public async void UpdateAgentScheduleChartWithNotFound(string agentScheduleId, int schedulingCode)
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
                ModifiedBy = "admin"
            };

            mockAgentScheduleRepository.Setup(mr => mr.GetEmployeeIdByAgentScheduleId(It.IsAny<AgentScheduleIdDetails>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails) => mockDataContext.GetEmployeeIdByAgentScheduleId(agentScheduleIdDetails));

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
        /// Updates the agent schedule chart for scheduling tab with not found for scheduling code.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        /// <param name="schedulingCode">The scheduling code.</param>
        [Theory]
        [InlineData("5fe0b5ad6a05416894c0718e", 100)]
        [InlineData("5fe0b5ad6a05416894c0718f", 101)]
        public async void UpdateAgentScheduleChartForSchedulingTabWithNotFoundForSchedulingCode(string agentScheduleId, int schedulingCode)
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
                ModifiedBy = "admin"
            };

            mockAgentScheduleRepository.Setup(mr => mr.GetEmployeeIdByAgentScheduleId(It.IsAny<AgentScheduleIdDetails>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails) => mockDataContext.GetEmployeeIdByAgentScheduleId(agentScheduleIdDetails));

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
        /// Updates the agent schedule chart.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        /// <param name="schedulingCode">The scheduling code.</param>
        [Theory]
        [InlineData("5fe0b5ad6a05416894c0718e", 1)]
        [InlineData("5fe0b5ad6a05416894c0718f", 2)]
        public async void UpdateAgentScheduleChartForSchedulingTab(string agentScheduleId, int schedulingCode)
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
                ModifiedBy = "admin"
            };

            mockAgentScheduleRepository.Setup(mr => mr.GetEmployeeIdByAgentScheduleId(It.IsAny<AgentScheduleIdDetails>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails) => mockDataContext.GetEmployeeIdByAgentScheduleId(agentScheduleIdDetails));

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

        #region UpdateAgentScheduleMangerChart

        /// <summary>
        /// Updates the agent schedule chart for scheduling manger tab with not found for scheduling code.
        /// </summary>
        /// <param name="schedulingCode">The scheduling code.</param>
        /// <param name="employeeId">The employee identifier.</param>
        [Theory]
        [InlineData(100, 1)]
        [InlineData(101, 2)]
        public async void UpdateAgentScheduleChartForSchedulingMangerTabWithNotFoundForSchedulingCode(int schedulingCode, int employeeId)
        {
            UpdateAgentScheduleManagerChart agentScheduleChart = new UpdateAgentScheduleManagerChart
            {
                AgentScheduleManagers = new List<AgentScheduleManager>()
                {
                    new AgentScheduleManager
                    {
                        EmployeeId = employeeId,
                        AgentScheduleManagerChart = new AgentScheduleManagerChart
                        {
                             Date = DateTime.UtcNow,
                             Charts = new List<ScheduleChart>
                             {
                                new ScheduleChart { StartTime = "00:00 am", EndTime = "00:05 pm", SchedulingCodeId = schedulingCode }
                             }
                        }
                    }
                },
                ModifiedBy = "admin"
            };

            mockSchedulingCodeRepository.Setup(mr => mr.GetSchedulingCodesCountByIds(It.IsAny<List<int>>())).ReturnsAsync(
                (List<int> codes) => mockDataContext.GetSchedulingCodesCountByIds(codes));

            var result = await agentScheduleService.UpdateAgentScheduleMangerChart(agentScheduleChart);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
        }

        /// <summary>
        /// Updates the agent schedule chart.
        /// </summary>
        /// <param name="schedulingCode">The scheduling code.</param>
        /// <param name="employeeId">The employee identifier.</param>
        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        public async void UpdateAgentScheduleChartForSchedulingManagerTab(int schedulingCode, int employeeId)
        {
            UpdateAgentScheduleManagerChart agentScheduleChart = new UpdateAgentScheduleManagerChart
            {
                AgentScheduleManagers = new List<AgentScheduleManager>()
                {
                    new AgentScheduleManager
                    {
                        EmployeeId = employeeId,
                        AgentScheduleManagerChart = new AgentScheduleManagerChart
                        {
                             Date = DateTime.UtcNow,
                             Charts = new List<ScheduleChart>
                             {
                                new ScheduleChart { StartTime = "00:00 am", EndTime = "00:05 pm", SchedulingCodeId = schedulingCode }
                             }
                        }
                    }
                },
                ModifiedBy = "admin"
            };

            mockSchedulingCodeRepository.Setup(mr => mr.GetSchedulingCodesCountByIds(It.IsAny<List<int>>())).ReturnsAsync(
                (List<int> codes) => mockDataContext.GetSchedulingCodesCountByIds(codes));

            var result = await agentScheduleService.UpdateAgentScheduleMangerChart(agentScheduleChart);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NoContent, result.Code);
        }

        #endregion

        #region ImportAgentScheduleChart

        /// <summary>
        /// Imports the agent schedule chart for scheduling tab with not found for scheduling code.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        /// <param name="schedulingCode">The scheduling code.</param>
        [Theory]
        [InlineData(100, 100)]
        [InlineData(101, 101)]
        public async void ImportAgentScheduleChartForSchedulingTabWithNotFoundForSchedulingCode(int schedulingCode, int employeeId)
        {
            ImportAgentSchedule importAgentSchedule = new ImportAgentSchedule
            {
                ImportAgentScheduleCharts = new List<ImportAgentScheduleChart>
                {
                    new ImportAgentScheduleChart
                    {
                        EmployeeId = employeeId,
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
                        }
                    }
                },
                ModifiedBy = "admin"
            };

            mockSchedulingCodeRepository.Setup(mr => mr.GetSchedulingCodesCountByIds(It.IsAny<List<int>>())).ReturnsAsync(
                (List<int> codes) => mockDataContext.GetSchedulingCodesCountByIds(codes));

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
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        public async void ImportAgentScheduleChartForSchedulingTab(int schedulingCode, int employeeId)
        {
            ImportAgentSchedule importAgentSchedule = new ImportAgentSchedule
            {
                ImportAgentScheduleCharts = new List<ImportAgentScheduleChart>
                {
                    new ImportAgentScheduleChart
                    {
                        EmployeeId = employeeId,
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
                        }
                    }
                },
                ModifiedBy = "admin"
            };

            mockSchedulingCodeRepository.Setup(mr => mr.GetSchedulingCodesCountByIds(It.IsAny<List<int>>())).ReturnsAsync(
                (List<int> codes) => mockDataContext.GetSchedulingCodesCountByIds(codes));

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
        [Theory]
        [InlineData("6fe0b5ad6a05416894c0718e")]
        [InlineData("6fe0b5ad6a05416894c0718f")]
        public async void CopyAgentScheduleWithNotFound(string agentScheduleId)
        {
            AgentScheduleIdDetails agentScheduleIdDetails = new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId };
            CopyAgentSchedule copyAgentSchedule = new CopyAgentSchedule
            {
                AgentScheduleType = AgentScheduleType.SchedulingTab,
                EmployeeIds = new List<int> { 1, 2 },
                ModifiedBy = "admin"
            };

            mockAgentScheduleRepository.Setup(mr => mr.GetAgentSchedule(It.IsAny<AgentScheduleIdDetails>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails) => mockDataContext.GetAgentSchedule(agentScheduleIdDetails));

            var result = await agentScheduleService.CopyAgentSchedule(agentScheduleIdDetails, copyAgentSchedule);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
        }

        /// <summary>
        /// Copies the agent schedule for scheduling tab.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        [Theory]
        [InlineData("5fe0b5ad6a05416894c0718e")]
        [InlineData("5fe0b5ad6a05416894c0718f")]
        public async void CopyAgentScheduleForSchedulingTab(string agentScheduleId)
        {
            AgentScheduleIdDetails agentScheduleIdDetails = new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId };
            CopyAgentSchedule copyAgentSchedule = new CopyAgentSchedule
            {
                AgentScheduleType = AgentScheduleType.SchedulingTab,
                EmployeeIds = new List<int> { 1, 2 },
                ModifiedBy = "admin"
            };

            mockAgentScheduleRepository.Setup(mr => mr.GetAgentSchedule(It.IsAny<AgentScheduleIdDetails>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails) => mockDataContext.GetAgentSchedule(agentScheduleIdDetails));

            var result = await agentScheduleService.CopyAgentSchedule(agentScheduleIdDetails, copyAgentSchedule);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NoContent, result.Code);
        }

        /// <summary>
        /// Copies the agent schedule for scheduling manager tab.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        [Theory]
        [InlineData("5fe0b5ad6a05416894c0718e")]
        [InlineData("5fe0b5ad6a05416894c0718f")]
        public async void CopyAgentScheduleForSchedulingManagerTab(string agentScheduleId)
        {
            AgentScheduleIdDetails agentScheduleIdDetails = new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId };
            CopyAgentSchedule copyAgentSchedule = new CopyAgentSchedule
            {
                AgentScheduleType = AgentScheduleType.SchedulingMangerTab,
                EmployeeIds = new List<int> { 1, 2 },
                ModifiedBy = "admin"
            };

            mockAgentScheduleRepository.Setup(mr => mr.GetAgentSchedule(It.IsAny<AgentScheduleIdDetails>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails) => mockDataContext.GetAgentSchedule(agentScheduleIdDetails));

            var result = await agentScheduleService.CopyAgentSchedule(agentScheduleIdDetails, copyAgentSchedule);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NoContent, result.Code);
        }

        #endregion
    }
}
