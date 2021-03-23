using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Controllers;
using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedule;
using Css.Api.Core.Models.Enums;
using Css.Api.Scheduling.UnitTest.Mock;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using Xunit;
using Css.Api.Scheduling.Models.DTO.Request.AgentScheduleManager;
using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using Css.Api.Scheduling.Models.DTO.Request.MySchedule;

namespace Css.Api.Scheduling.UnitTest.Controllers
{
    public class AgentScheduleManagerControllerShould
    {
        /// <summary>
        /// The mock agent schedule service
        /// </summary>
        private readonly Mock<IAgentScheduleManagerService> mockAgentScheduleManagerService;

        /// <summary>
        /// The controller
        /// </summary>
        private readonly AgentScheduleManagersController controller;

        /// <summary>
        /// The mock agent schedule manager data
        /// </summary>
        private readonly MockAgentScheduleManagerData mockAgentScheduleManagerData;

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentCategoryControllerShould" /> class.
        /// </summary>
        public AgentScheduleManagerControllerShould()
        {
            mockAgentScheduleManagerService = new Mock<IAgentScheduleManagerService>();
            mockAgentScheduleManagerData = new MockAgentScheduleManagerData();
            controller = new AgentScheduleManagersController(mockAgentScheduleManagerService.Object);
        }

        #region GetAgentScheduleManagerCharts

        /// <summary>
        /// Gets the agent schedule manager charts.
        /// </summary>
        [Fact]
        public async void GetAgentScheduleManagerCharts()
        {
            AgentScheduleManagerChartQueryparameter agentScheduleManagerChartQueryparameter = new AgentScheduleManagerChartQueryparameter();

            mockAgentScheduleManagerService.Setup(mr => mr.GetAgentScheduleManagerCharts(It.IsAny<AgentScheduleManagerChartQueryparameter>())).ReturnsAsync(
                (AgentScheduleManagerChartQueryparameter agentScheduleManagerChartQueryparameter) =>
              mockAgentScheduleManagerData.GetAgentScheduleManagerCharts(agentScheduleManagerChartQueryparameter));

            var value = await controller.GetAgentScheduleManagerCharts(agentScheduleManagerChartQueryparameter);

            Assert.Equal((int)HttpStatusCode.OK, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region GetAgentMySchedule

        /// <summary>
        /// Gets the agent my schedule returns not found result.
        /// </summary>
        /// <param name="employeeId">The employee identifier.</param>
        [Theory]
        [InlineData(100)]
        [InlineData(101)]
        public async void GetAgentMySchedule_ReturnsNotFoundResult(int employeeId)
        {
            EmployeeIdDetails employeeIdDetails = new EmployeeIdDetails { Id = employeeId };
            MyScheduleQueryParameter myScheduleQueryParameter = new MyScheduleQueryParameter
            {
                AgentSchedulingGroupId = 1,
                StartDate = new DateTime(2021, 3, 21),
                EndDate = new DateTime(2021, 3, 21)
            };

            mockAgentScheduleManagerService.Setup(mr => mr.GetAgentMySchedule(It.IsAny<EmployeeIdDetails>(), It.IsAny<MyScheduleQueryParameter>()))
                .ReturnsAsync((EmployeeIdDetails employeeIdDetails, MyScheduleQueryParameter myScheduleQueryParameter) =>
                mockAgentScheduleManagerData.GetAgentMySchedule(employeeIdDetails, myScheduleQueryParameter));

            var value = await controller.GetAgentMySchedule(employeeId, myScheduleQueryParameter);

            Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        }

        /// <summary>
        /// Gets the agent my schedule returns ok result.
        /// </summary>
        /// <param name="employeeId">The employee identifier.</param>
        [Theory]
        [InlineData(1)]
        public async void GetAgentMySchedule_ReturnsOkResult(int employeeId)
        {
            EmployeeIdDetails employeeIdDetails = new EmployeeIdDetails { Id = employeeId };
            MyScheduleQueryParameter myScheduleQueryParameter = new MyScheduleQueryParameter
            {
                AgentSchedulingGroupId = 1,
                StartDate = new DateTime(2021, 3, 21),
                EndDate = new DateTime(2021, 3, 21)
            };

            mockAgentScheduleManagerService.Setup(mr => mr.GetAgentMySchedule(It.IsAny<EmployeeIdDetails>(), It.IsAny<MyScheduleQueryParameter>()))
                .ReturnsAsync((EmployeeIdDetails employeeIdDetails, MyScheduleQueryParameter myScheduleQueryParameter) =>
                mockAgentScheduleManagerData.GetAgentMySchedule(employeeIdDetails, myScheduleQueryParameter));
            var value = await controller.GetAgentMySchedule(employeeId, myScheduleQueryParameter);

            Assert.Equal((int)HttpStatusCode.OK, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region GetAgentScheduledOpen

        /// <summary>
        /// Gets the agent scheduled open returns not found result.
        /// </summary>
        /// <param name="skillGroupId">The skill group identifier.</param>
        [Theory]
        [InlineData(100)]
        [InlineData(101)]
        public async void GetAgentScheduledOpen_ReturnsNotFoundResult(int skillGroupId)
        {
            var date = new DateTime(2021, 3, 21);

            mockAgentScheduleManagerService.Setup(mr => mr.GetAgentScheduledOpen(It.IsAny<int>(), It.IsAny<DateTimeOffset>())).ReturnsAsync(
                (int skillGroupId, DateTimeOffset date) => mockAgentScheduleManagerData.GetAgentScheduledOpen(skillGroupId, date));

            var value = await controller.GetAgentScheduledOpen(skillGroupId, date);

            Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        }

        /// <summary>
        /// Gets the agent scheduled open returns ok result.
        /// </summary>
        /// <param name="skillGroupId">The skill group identifier.</param>
        [Theory]
        [InlineData(1)]
        public async void GetAgentScheduledOpen_ReturnsOKResult(int skillGroupId)
        {
            var date = new DateTime(2021, 3, 21);

            mockAgentScheduleManagerService.Setup(mr => mr.GetAgentScheduledOpen(It.IsAny<int>(), It.IsAny<DateTimeOffset>())).ReturnsAsync(
                (int skillGroupId, DateTimeOffset date) => mockAgentScheduleManagerData.GetAgentScheduledOpen(skillGroupId, date));

            var value = await controller.GetAgentScheduledOpen(skillGroupId, date);

            Assert.Equal((int)HttpStatusCode.OK, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region UpdateAgentScheduleMangerChart

        /// <summary>
        /// Updates the agent schedule manger chart returns not found result.
        /// </summary>
        /// <param name="schedulingCode">The scheduling code.</param>
        /// <param name="employeeId">The employee identifier.</param>
        [Theory]
        [InlineData(100, 1)]
        [InlineData(101, 2)]
        public async void UpdateAgentScheduleMangerChart_ReturnsNotFoundResult(int schedulingCode, int employeeId)
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

            mockAgentScheduleManagerService.Setup(mr => mr.UpdateAgentScheduleMangerChart(It.IsAny<UpdateAgentScheduleManager>())).ReturnsAsync(
                (UpdateAgentScheduleManager agentScheduleDetails) =>
                mockAgentScheduleManagerData.UpdateAgentScheduleMangerChart(agentScheduleDetails));

            var value = await controller.UpdateAgentScheduleMangerChart(agentScheduleManager);

            Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        }

        /// <summary>
        /// Updates the agent schedule manger chart returns no content result for update.
        /// </summary>
        /// <param name="schedulingCode">The scheduling code.</param>
        /// <param name="employeeId">The employee identifier.</param>
        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        public async void UpdateAgentScheduleMangerChart_ReturnsNoContentResultForUpdate(int schedulingCode, int employeeId)
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

            mockAgentScheduleManagerService.Setup(mr => mr.UpdateAgentScheduleMangerChart(It.IsAny<UpdateAgentScheduleManager>())).ReturnsAsync(
                (UpdateAgentScheduleManager agentScheduleDetails) =>
                mockAgentScheduleManagerData.UpdateAgentScheduleMangerChart(agentScheduleDetails));

            var value = await controller.UpdateAgentScheduleMangerChart(agentScheduleManager);

            Assert.Equal((int)HttpStatusCode.NoContent, (value as ObjectResult).StatusCode);
        }

        /// <summary>
        /// Updates the agent schedule manger chart returns no content result for update.
        /// </summary>
        /// <param name="schedulingCode">The scheduling code.</param>
        /// <param name="employeeId">The employee identifier.</param>
        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        public async void UpdateAgentScheduleMangerChart_ReturnsNoContentResultForImport(int schedulingCode, int employeeId)
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

            mockAgentScheduleManagerService.Setup(mr => mr.UpdateAgentScheduleMangerChart(It.IsAny<UpdateAgentScheduleManager>())).ReturnsAsync(
                (UpdateAgentScheduleManager agentScheduleDetails) =>
                mockAgentScheduleManagerData.UpdateAgentScheduleMangerChart(agentScheduleDetails));

            var value = await controller.UpdateAgentScheduleMangerChart(agentScheduleManager);

            Assert.Equal((int)HttpStatusCode.NoContent, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region CopyAgentScheduleManagerChart

        /// <summary>
        /// Copies the agent schedule manager chart returns not found result.
        /// </summary>
        /// <param name="employeeId">The employee identifier.</param>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        [Theory]
        [InlineData(100, 2021, 3, 21)]
        [InlineData(101, 2021, 3, 22)]
        public async void CopyAgentScheduleManagerChart_ReturnsNotFoundResult(int employeeId, int year, int month, int day)
        {
            EmployeeIdDetails employeeIdDetails = new EmployeeIdDetails { Id = employeeId };
            CopyAgentScheduleManager copyAgentScheduleManager = new CopyAgentScheduleManager
            {
                AgentSchedulingGroupId = 1,
                Date = new DateTime(year, month, day),
                ActivityOrigin = ActivityOrigin.CSS,
                EmployeeIds = new List<int> { 1, 2 },
                ModifiedBy = "admin",
                ModifiedUser = 1
            };

            mockAgentScheduleManagerService.Setup(mr => mr.CopyAgentScheduleManagerChart(It.IsAny<EmployeeIdDetails>(), It.IsAny<CopyAgentScheduleManager>())).ReturnsAsync(
                (EmployeeIdDetails employeeIdDetails, CopyAgentScheduleManager agentScheduleDetails) =>
                mockAgentScheduleManagerData.CopyAgentScheduleManagerChart(employeeIdDetails, agentScheduleDetails));

            var value = await controller.CopyAgentScheduleManagerChart(employeeId, copyAgentScheduleManager);

            Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        }

        /// <summary>
        /// Copies the agent schedule manager chart returns no content result.
        /// </summary>
        /// <param name="employeeId">The employee identifier.</param>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        [Theory]
        [InlineData(1, 2021, 3, 21)]
        [InlineData(2, 2021, 3, 22)]
        public async void CopyAgentScheduleManagerChart_ReturnsNoContentResult(int employeeId, int year, int month, int day)
        {
            EmployeeIdDetails employeeIdDetails = new EmployeeIdDetails { Id = employeeId };
            CopyAgentScheduleManager copyAgentScheduleManager = new CopyAgentScheduleManager
            {
                AgentSchedulingGroupId = 1,
                Date = new DateTime(year, month, day),
                ActivityOrigin = ActivityOrigin.CSS,
                EmployeeIds = new List<int> { 1, 2 },
                ModifiedBy = "admin",
                ModifiedUser = 1
            };

            mockAgentScheduleManagerService.Setup(mr => mr.CopyAgentScheduleManagerChart(It.IsAny<EmployeeIdDetails>(), It.IsAny<CopyAgentScheduleManager>())).ReturnsAsync(
                (EmployeeIdDetails employeeIdDetails, CopyAgentScheduleManager agentScheduleDetails) =>
                mockAgentScheduleManagerData.CopyAgentScheduleManagerChart(employeeIdDetails, agentScheduleDetails));

            var value = await controller.CopyAgentScheduleManagerChart(employeeId, copyAgentScheduleManager);

            Assert.Equal((int)HttpStatusCode.NoContent, (value as ObjectResult).StatusCode);
        }

        #endregion
    }
}
