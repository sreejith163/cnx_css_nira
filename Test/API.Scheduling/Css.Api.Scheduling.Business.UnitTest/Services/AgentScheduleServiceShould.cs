using Moq;
using Xunit;
using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Business.UnitTest.Mocks;
using Css.Api.Scheduling.Models.Profiles.AgentAdmin;
using Css.Api.Scheduling.Models.Profiles.AgentSchedule;
using Css.Api.Scheduling.Business;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedule;
using Css.Api.Scheduling.Repository.Interfaces;
using Css.Api.Core.DataAccess.Repository.UnitOfWork.Interfaces;
using System.Collections.Generic;
using Css.Api.Scheduling.Models.DTO.Response.AgentSchedule;
using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using System;
using Css.Api.Scheduling.Models.Enums;
using Css.Api.Scheduling.Models.Domain;

namespace Css.Api.Admin.Business.UnitTest.Services
{
    public class AgentScheduleServiceShould
    {
        /// <summary>
        /// The translation service
        /// </summary>
        private readonly IAgentScheduleService agentScheduleService;

        /// <summary>
        /// The mock agent schedule repository
        /// </summary>
        private readonly Mock<IAgentScheduleRepository> mockAgentScheduleRepository;

        /// <summary>
        /// The mock agent admin repository
        /// </summary>
        private readonly Mock<IAgentAdminRepository> mockAgentAdminRepository;

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

            mockAgentScheduleRepository = new Mock<IAgentScheduleRepository>();
            mockAgentAdminRepository = new Mock<IAgentAdminRepository>();
            var mockUnitWork = new Mock<IUnitOfWork>();

            mockDataContext = new MockDataContext();

            agentScheduleService = new AgentScheduleService(mockHttContext.Object, mockAgentScheduleRepository.Object, mockAgentAdminRepository.Object,
                                                            mapper, mockUnitWork.Object);
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

            mockAgentAdminRepository.Setup(mr => mr.GetAgentAdminsByEmployeeIds(It.IsAny<List<int>>())).ReturnsAsync(
                (List<int> employeeIds) => mockDataContext.GetAgentAdminsByEmployeeIds(employeeIds));

            var result = await agentScheduleService.GetAgentSchedules(agentScheduleQueryparameter);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<List<AgentScheduleDTO>>(result.Value);
            Assert.Equal(HttpStatusCode.OK, result.Code);
        }

        [Fact]
        public async void GetAgentSchedulesWithEmployeeNameSort()
        {
            AgentScheduleQueryparameter agentScheduleQueryparameter = new AgentScheduleQueryparameter {
                OrderBy = "employeename asc"
            };

            mockAgentScheduleRepository.Setup(mr => mr.GetAgentSchedules(It.IsAny<AgentScheduleQueryparameter>())).ReturnsAsync(
                (AgentScheduleQueryparameter agentScheduleQueryparameter) => mockDataContext.GetAgentSchedules(agentScheduleQueryparameter));

            mockAgentAdminRepository.Setup(mr => mr.GetAgentAdmins(It.IsAny<AgentAdminQueryParameter>())).ReturnsAsync(
                (AgentAdminQueryParameter agentAdminQueryParameter) => mockDataContext.GetAgentAdmins(agentAdminQueryParameter));

            var result = await agentScheduleService.GetAgentSchedules(agentScheduleQueryparameter);

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<List<AgentScheduleDTO>>(result.Value);
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

            mockAgentAdminRepository.Setup(mr => mr.GetAgentAdminIdsByEmployeeIdAndSso(It.IsAny<EmployeeIdDetails>(), It.IsAny<AgentAdminSsoDetails>())).ReturnsAsync(
                (EmployeeIdDetails employeeIdDetails, AgentAdminSsoDetails agentAdminSsoDetails) => mockDataContext.GetAgentAdminIdsByEmployeeIdAndSso(employeeIdDetails, agentAdminSsoDetails));

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

            mockAgentAdminRepository.Setup(mr => mr.GetAgentAdminIdsByEmployeeIdAndSso(It.IsAny<EmployeeIdDetails>(), It.IsAny<AgentAdminSsoDetails>())).ReturnsAsync(
                (EmployeeIdDetails employeeIdDetails, AgentAdminSsoDetails agentAdminSsoDetails) => mockDataContext.GetAgentAdminIdsByEmployeeIdAndSso(employeeIdDetails, agentAdminSsoDetails));


            var result = await agentScheduleService.GetAgentSchedule(new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId });

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<AgentScheduleDetailsDTO>(result.Value);
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
        [Theory]
        [InlineData("6fe0b5ad6a05416894c0718e")]
        [InlineData("6fe0b5ad6a05416894c0718f")]
        public async void UpdateAgentScheduleChartWithNotFound(string agentScheduleId)
        {
            AgentScheduleIdDetails agentScheduleIdDetails = new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId };
            UpdateAgentScheduleChart agentScheduleChart = new UpdateAgentScheduleChart
            {
                AgentScheduleType = AgentScheduleType.SchedulingTab,
                AgentScheduleCharts = new List<AgentScheduleChart>
                {
                    new AgentScheduleChart
                    {
                        Day = 1,
                        Charts = new List<ScheduleChart>
                        {
                            new ScheduleChart { StartTime = "00:00 am", EndTime = "00:05 pm", SchedulingCodeId = 1 }
                        }
                    }
                },
                ModifiedBy = "admin"
            };

            mockAgentScheduleRepository.Setup(mr => mr.GetAgentScheduleCount(It.IsAny<AgentScheduleIdDetails>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails) => mockDataContext.GetAgentScheduleCount(agentScheduleIdDetails));

            var result = await agentScheduleService.UpdateAgentScheduleChart(agentScheduleIdDetails, agentScheduleChart);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
        }

        /// <summary>
        /// Updates the agent schedule chart.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        [Theory]
        [InlineData("5fe0b5ad6a05416894c0718e")]
        [InlineData("5fe0b5ad6a05416894c0718f")]
        public async void UpdateAgentScheduleChartForSchedulingTab(string agentScheduleId)
        {
            AgentScheduleIdDetails agentScheduleIdDetails = new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId };
            UpdateAgentScheduleChart agentScheduleChart = new UpdateAgentScheduleChart
            {
                AgentScheduleType = AgentScheduleType.SchedulingTab,
                AgentScheduleCharts = new List<AgentScheduleChart>
                {
                    new AgentScheduleChart
                    {
                        Day = 1,
                        Charts = new List<ScheduleChart>
                        {
                            new ScheduleChart { StartTime = "00:00 am", EndTime = "00:05 pm", SchedulingCodeId = 1 }
                        }
                    }
                },
                ModifiedBy = "admin"
            };

            mockAgentScheduleRepository.Setup(mr => mr.GetAgentScheduleCount(It.IsAny<AgentScheduleIdDetails>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails) => mockDataContext.GetAgentScheduleCount(agentScheduleIdDetails));

            var result = await agentScheduleService.UpdateAgentScheduleChart(agentScheduleIdDetails, agentScheduleChart);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NoContent, result.Code);
        }

        /// <summary>
        /// Updates the agent schedule chart.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        [Theory]
        [InlineData("5fe0b5ad6a05416894c0718e")]
        [InlineData("5fe0b5ad6a05416894c0718f")]
        public async void UpdateAgentScheduleChartForSchedulingManagerTab(string agentScheduleId)
        {
            AgentScheduleIdDetails agentScheduleIdDetails = new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId };
            UpdateAgentScheduleChart agentScheduleChart = new UpdateAgentScheduleChart
            {
                AgentScheduleType = AgentScheduleType.SchedulingMangerTab,
                AgentScheduleManagerChart = new AgentScheduleManagerChart 
                {
                    Date = DateTime.UtcNow,
                    Charts = new List<ScheduleChart>
                    {
                        new ScheduleChart { StartTime = "00:00 am", EndTime = "00:05 pm", SchedulingCodeId = 1 }
                    }
                },
                ModifiedBy = "admin"
            };

            mockAgentScheduleRepository.Setup(mr => mr.GetAgentScheduleCount(It.IsAny<AgentScheduleIdDetails>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails) => mockDataContext.GetAgentScheduleCount(agentScheduleIdDetails));

            var result = await agentScheduleService.UpdateAgentScheduleChart(agentScheduleIdDetails, agentScheduleChart);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NoContent, result.Code);
        }

        #endregion

        #region ImportAgentScheduleChart

        /// <summary>
        /// Imports the agent schedule chart with not found.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        [Theory]
        [InlineData("6fe0b5ad6a05416894c0718e")]
        [InlineData("6fe0b5ad6a05416894c0718f")]
        public async void ImportAgentScheduleChartWithNotFound(string agentScheduleId)
        {
            AgentScheduleIdDetails agentScheduleIdDetails = new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId };
            ImportAgentScheduleChart importAgentScheduleChart = new ImportAgentScheduleChart
            {
                AgentScheduleType = AgentScheduleType.SchedulingTab,
                AgentScheduleCharts = new List<AgentScheduleChart>
                {
                    new AgentScheduleChart
                    {
                        Day = 1,
                        Charts = new List<ScheduleChart>
                        {
                            new ScheduleChart { StartTime = "00:00 am", EndTime = "00:05 pm", SchedulingCodeId = 1 }
                        }
                    }
                },
                ModifiedBy = "admin"
            };

            mockAgentScheduleRepository.Setup(mr => mr.GetAgentScheduleCount(It.IsAny<AgentScheduleIdDetails>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails) => mockDataContext.GetAgentScheduleCount(agentScheduleIdDetails));

            var result = await agentScheduleService.ImportAgentScheduleChart(agentScheduleIdDetails, importAgentScheduleChart);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NotFound, result.Code);
        }

        /// <summary>
        /// Imports the agent schedule chart for scheduling tab.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        [Theory]
        [InlineData("5fe0b5ad6a05416894c0718e")]
        [InlineData("5fe0b5ad6a05416894c0718f")]
        public async void ImportAgentScheduleChartForSchedulingTab(string agentScheduleId)
        {
            AgentScheduleIdDetails agentScheduleIdDetails = new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId };
            ImportAgentScheduleChart importAgentScheduleChart = new ImportAgentScheduleChart
            {
                AgentScheduleType = AgentScheduleType.SchedulingTab,
                AgentScheduleCharts = new List<AgentScheduleChart>
                {
                    new AgentScheduleChart
                    {
                        Day = 1,
                        Charts = new List<ScheduleChart>
                        {
                            new ScheduleChart { StartTime = "00:00 am", EndTime = "00:05 pm", SchedulingCodeId = 1 }
                        }
                    }
                },
                ModifiedBy = "admin"
            };

            mockAgentScheduleRepository.Setup(mr => mr.GetAgentScheduleCount(It.IsAny<AgentScheduleIdDetails>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails) => mockDataContext.GetAgentScheduleCount(agentScheduleIdDetails));

            var result = await agentScheduleService.ImportAgentScheduleChart(agentScheduleIdDetails, importAgentScheduleChart);

            Assert.NotNull(result);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.NoContent, result.Code);
        }

        /// <summary>
        /// Imports the agent schedule chart for scheduling manager tab.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        [Theory]
        [InlineData("5fe0b5ad6a05416894c0718e")]
        [InlineData("5fe0b5ad6a05416894c0718f")]
        public async void ImportAgentScheduleChartForSchedulingManagerTab(string agentScheduleId)
        {
            AgentScheduleIdDetails agentScheduleIdDetails = new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId };
            ImportAgentScheduleChart importAgentScheduleChart = new ImportAgentScheduleChart
            {
                AgentScheduleType = AgentScheduleType.SchedulingMangerTab,
                AgentScheduleManagerCharts = new List<AgentScheduleManagerChart>
                {
                    new AgentScheduleManagerChart
                    {
                        Date = DateTime.UtcNow,
                        Charts = new List<ScheduleChart>
                        {
                            new ScheduleChart { StartTime = "00:00 am", EndTime = "00:05 pm", SchedulingCodeId = 1 }
                        }
                    }
                },
                ModifiedBy = "admin"
            };

            mockAgentScheduleRepository.Setup(mr => mr.GetAgentScheduleCount(It.IsAny<AgentScheduleIdDetails>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails) => mockDataContext.GetAgentScheduleCount(agentScheduleIdDetails));

            var result = await agentScheduleService.ImportAgentScheduleChart(agentScheduleIdDetails, importAgentScheduleChart);

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

        #endregion
    }
}
