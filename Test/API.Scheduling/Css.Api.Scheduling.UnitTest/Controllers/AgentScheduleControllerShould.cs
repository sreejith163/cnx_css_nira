using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Controllers;
using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedule;
using Css.Api.Core.Models.Enums;
using Css.Api.Scheduling.Models.Enums;
using Css.Api.Scheduling.UnitTest.Mock;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using Xunit;

namespace Css.Api.Admin.UnitTest.Controllers
{
    public class AgentScheduleControllerShould
    {
        /// <summary>
        /// The mock agent schedule service
        /// </summary>
        private readonly Mock<IAgentScheduleService> mockAgentScheduleService;

        /// <summary>
        /// The controller
        /// </summary>
        private readonly AgentSchedulesController controller;

        /// <summary>
        /// The mock agent schedule data
        /// </summary>
        private readonly MockAgentScheduleData mockAgentScheduleData;

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentCategoryControllerShould" /> class.
        /// </summary>
        public AgentScheduleControllerShould()
        {
            mockAgentScheduleService = new Mock<IAgentScheduleService>();
            mockAgentScheduleData = new MockAgentScheduleData();
            controller = new AgentSchedulesController(mockAgentScheduleService.Object);
        }

        #region GetAgentSchedules

        /// <summary>
        /// Gets the agentCategories.
        /// </summary>
        [Fact]
        public async void GetAgentSchedules()
        {
            AgentScheduleQueryparameter agentScheduleQueryparameter = new AgentScheduleQueryparameter();

            mockAgentScheduleService.Setup(mr => mr.GetAgentSchedules(It.IsAny<AgentScheduleQueryparameter>())).ReturnsAsync((AgentScheduleQueryparameter agentCategory) =>
              mockAgentScheduleData.GetAgentSchedules(agentScheduleQueryparameter));

            var value = await controller.GetAgentSchedules(agentScheduleQueryparameter);

            Assert.Equal((int)HttpStatusCode.OK, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region GetAgentScheduleCharts

        /// <summary>
        /// Gets the agent schedule charts returns not found result.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        [Theory]
        [InlineData("6fe0b5ad6a05416894c0718e")]
        [InlineData("6fe0b5ad6a05416894c0718f")]
        public async void GetAgentScheduleCharts_ReturnsNotFoundResult(string agentScheduleId)
        {
            var agentScheduleChartQueryparameter = new AgentScheduleChartQueryparameter();

            mockAgentScheduleService.Setup(mr => mr.GetAgentScheduleCharts(It.IsAny<AgentScheduleIdDetails>(), It.IsAny<AgentScheduleChartQueryparameter>()))
                .ReturnsAsync((AgentScheduleIdDetails agentScheduleIdDetails, AgentScheduleChartQueryparameter agentScheduleChartQueryparameter) =>
                mockAgentScheduleData.GetAgentScheduleCharts(new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId }, agentScheduleChartQueryparameter));

            var value = await controller.GetAgentScheduleCharts(agentScheduleId, agentScheduleChartQueryparameter);

            Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        }

        /// <summary>
        /// Gets the agent schedule charts for scheduling tab returns ok result.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        [Theory]
        [InlineData("5fe0b5ad6a05416894c0718e")]
        [InlineData("5fe0b5ad6a05416894c0718f")]
        public async void GetAgentScheduleChartsForSchedulingTab_ReturnsOKResult(string agentScheduleId)
        {
            var agentScheduleChartQueryparameter = new AgentScheduleChartQueryparameter() { AgentScheduleType = AgentScheduleType.SchedulingTab };

            mockAgentScheduleService.Setup(mr => mr.GetAgentScheduleCharts(It.IsAny<AgentScheduleIdDetails>(), It.IsAny<AgentScheduleChartQueryparameter>()))
                .ReturnsAsync((AgentScheduleIdDetails agentScheduleIdDetails, AgentScheduleChartQueryparameter agentScheduleChartQueryparameter) =>
                mockAgentScheduleData.GetAgentScheduleCharts(new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId }, agentScheduleChartQueryparameter));

            var value = await controller.GetAgentScheduleCharts(agentScheduleId, agentScheduleChartQueryparameter);

            Assert.Equal((int)HttpStatusCode.OK, (value as ObjectResult).StatusCode);
        }

        /// <summary>
        /// Gets the agent schedule charts for scheduling manager tab returns ok result.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        [Theory]
        [InlineData("5fe0b5ad6a05416894c0718e")]
        [InlineData("5fe0b5ad6a05416894c0718f")]
        public async void GetAgentScheduleChartsForSchedulingManagerTab_ReturnsOKResult(string agentScheduleId)
        {
            var agentScheduleChartQueryparameter = new AgentScheduleChartQueryparameter() { AgentScheduleType = AgentScheduleType.SchedulingMangerTab };

            mockAgentScheduleService.Setup(mr => mr.GetAgentScheduleCharts(It.IsAny<AgentScheduleIdDetails>(), It.IsAny<AgentScheduleChartQueryparameter>()))
                .ReturnsAsync((AgentScheduleIdDetails agentScheduleIdDetails, AgentScheduleChartQueryparameter agentScheduleChartQueryparameter) =>
                mockAgentScheduleData.GetAgentScheduleCharts(new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId }, agentScheduleChartQueryparameter));

            var value = await controller.GetAgentScheduleCharts(agentScheduleId, agentScheduleChartQueryparameter);

            Assert.Equal((int)HttpStatusCode.OK, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region GetAgentSchedule

        /// <summary>
        /// Gets the agent schedule returns not found result.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        [Theory]
        [InlineData("6fe0b5ad6a05416894c0718e")]
        [InlineData("6fe0b5ad6a05416894c0718f")]
        public async void GetAgentSchedule_ReturnsNotFoundResult(string agentScheduleId)
        {
            mockAgentScheduleService.Setup(mr => mr.GetAgentSchedule(It.IsAny<AgentScheduleIdDetails>())).ReturnsAsync((AgentScheduleIdDetails agentScheduleIdDetails) =>
                mockAgentScheduleData.GetAgentSchedule(new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId }));

            var value = await controller.GetAgentSchedule(agentScheduleId);

            Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        }

        /// <summary>
        /// Gets the agent schedule returns ok result.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        [Theory]
        [InlineData("5fe0b5ad6a05416894c0718e")]
        [InlineData("5fe0b5ad6a05416894c0718f")]
        public async void GetAgentSchedule_ReturnsOKResult(string agentScheduleId)
        {
            mockAgentScheduleService.Setup(mr => mr.GetAgentSchedule(It.IsAny<AgentScheduleIdDetails>())).ReturnsAsync((AgentScheduleIdDetails agentScheduleIdDetails) =>
                mockAgentScheduleData.GetAgentSchedule(new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId }));

            var value = await controller.GetAgentSchedule(agentScheduleId);

            Assert.Equal((int)HttpStatusCode.OK, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region UpdateAgentSchedule

        /// <summary>
        /// Updates the agent schedule returns not found result.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        [Theory]
        [InlineData("6fe0b5ad6a05416894c0718e")]
        [InlineData("6fe0b5ad6a05416894c0718f")]
        public async void UpdateAgentSchedule_ReturnsNotFoundResult(string agentScheduleId)
        {
            UpdateAgentSchedule updateAgentSchedule = new UpdateAgentSchedule()
            {
                Status = SchedulingStatus.Approved,
                DateFrom = DateTime.UtcNow,
                DateTo = DateTime.UtcNow,
                ModifiedBy = "admin"
            };

            mockAgentScheduleService.Setup(mr => mr.UpdateAgentSchedule(It.IsAny<AgentScheduleIdDetails>(), It.IsAny<UpdateAgentSchedule>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails, UpdateAgentSchedule updateAgentSchedule) =>
                mockAgentScheduleData.UpdateAgentSchedule(new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId }, updateAgentSchedule));

            var value = await controller.UpdateAgentSchedule(agentScheduleId, updateAgentSchedule);

            Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        }

        /// <summary>
        /// Updates the agent schedule returns no content result.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        [Theory]
        [InlineData("5fe0b5ad6a05416894c0718e")]
        [InlineData("5fe0b5ad6a05416894c0718f")]
        public async void UpdateAgentSchedule_ReturnsNoContentResult(string agentScheduleId)
        {
            UpdateAgentSchedule updateAgentSchedule = new UpdateAgentSchedule()
            {
                Status = SchedulingStatus.Approved,
                DateFrom = DateTime.UtcNow,
                DateTo = DateTime.UtcNow,
                ModifiedBy = "admin"
            };

            mockAgentScheduleService.Setup(mr => mr.UpdateAgentSchedule(It.IsAny<AgentScheduleIdDetails>(), It.IsAny<UpdateAgentSchedule>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails, UpdateAgentSchedule updateAgentSchedule) =>
                mockAgentScheduleData.UpdateAgentSchedule(new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId }, updateAgentSchedule));

            var value = await controller.UpdateAgentSchedule(agentScheduleId, updateAgentSchedule);

            Assert.Equal((int)HttpStatusCode.NoContent, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region UpdateAgentScheduleChart

        /// <summary>
        /// Updates the agent schedule returns not found result.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        /// <param name="schedulingCodeId">The scheduling code identifier.</param>
        [Theory]
        [InlineData("6fe0b5ad6a05416894c0718e", 100)]
        [InlineData("6fe0b5ad6a05416894c0718f", 101)]
        public async void UpdateAgentScheduleChart_ReturnsNotFoundResult(string agentScheduleId, int schedulingCodeId)
        {
            UpdateAgentScheduleChart agentScheduleChart = new UpdateAgentScheduleChart
            {
                AgentScheduleCharts = new List<AgentScheduleChart>
                {
                    new AgentScheduleChart
                    {
                        Day = 1,
                        Charts = new List<ScheduleChart>
                        {
                            new ScheduleChart { StartTime = "00:00 am", EndTime = "00:05 pm", SchedulingCodeId = schedulingCodeId }
                        }
                    }
                },
                ModifiedBy = "admin"
            };

            mockAgentScheduleService.Setup(mr => mr.UpdateAgentScheduleChart(It.IsAny<AgentScheduleIdDetails>(), It.IsAny<UpdateAgentScheduleChart>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails, UpdateAgentScheduleChart updateAgentScheduleChart) =>
                mockAgentScheduleData.UpdateAgentScheduleChart(new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId }, agentScheduleChart));

            var value = await controller.UpdateAgentScheduleChart(agentScheduleId, agentScheduleChart);

            Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        }

        [Theory]
        [InlineData("5fe0b5ad6a05416894c0718e", 100)]
        [InlineData("5fe0b5ad6a05416894c0718f", 101)]
        public async void UpdateAgentScheduleChart_ReturnsNotFoundResultForSchedulingCodes(string agentScheduleId, int schedulingCodeId)
        {
            UpdateAgentScheduleChart agentScheduleChart = new UpdateAgentScheduleChart
            {
                AgentScheduleCharts = new List<AgentScheduleChart>
                {
                    new AgentScheduleChart
                    {
                        Day = 1,
                        Charts = new List<ScheduleChart>
                        {
                            new ScheduleChart { StartTime = "00:00 am", EndTime = "00:05 pm", SchedulingCodeId = schedulingCodeId }
                        }
                    }
                },
                ModifiedBy = "admin"
            };

            mockAgentScheduleService.Setup(mr => mr.UpdateAgentScheduleChart(It.IsAny<AgentScheduleIdDetails>(), It.IsAny<UpdateAgentScheduleChart>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails, UpdateAgentScheduleChart updateAgentScheduleChart) =>
                mockAgentScheduleData.UpdateAgentScheduleChart(new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId }, agentScheduleChart));

            var value = await controller.UpdateAgentScheduleChart(agentScheduleId, agentScheduleChart);

            Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        }

        /// <summary>
        /// Updates the agent schedule returns no content result.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        /// <param name="schedulingCodeId">The scheduling code identifier.</param>
        [Theory]
        [InlineData("5fe0b5ad6a05416894c0718e", 1)]
        [InlineData("5fe0b5ad6a05416894c0718f", 2)]
        public async void UpdateAgentScheduleChart_ReturnsNoContentResultForSchedulingTab(string agentScheduleId, int schedulingCodeId)
        {
            UpdateAgentScheduleChart agentScheduleChart = new UpdateAgentScheduleChart
            {
                AgentScheduleCharts = new List<AgentScheduleChart>
                {
                    new AgentScheduleChart
                    {
                        Day = 1,
                        Charts = new List<ScheduleChart>
                        {
                            new ScheduleChart { StartTime = "00:00 am", EndTime = "00:05 pm", SchedulingCodeId = schedulingCodeId }
                        }
                    }
                },
                ModifiedBy = "admin"
            };

            mockAgentScheduleService.Setup(mr => mr.UpdateAgentScheduleChart(It.IsAny<AgentScheduleIdDetails>(), It.IsAny<UpdateAgentScheduleChart>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails, UpdateAgentScheduleChart updateAgentScheduleChart) =>
                mockAgentScheduleData.UpdateAgentScheduleChart(new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId }, agentScheduleChart));

            var value = await controller.UpdateAgentScheduleChart(agentScheduleId, agentScheduleChart);

            Assert.Equal((int)HttpStatusCode.NoContent, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region UpdateAgentScheduleManagerChart

        /// <summary>
        /// Imports the agent schedule manager chart returns not found result.
        /// </summary>
        /// <param name="schedulingCode">The scheduling code.</param>
        /// <param name="employeeId">The employee identifier.</param>
        [Theory]
        [InlineData(100, 100)]
        [InlineData(101, 101)]
        public async void ImportAgentScheduleManagerChart_ReturnsNotFoundResult(int schedulingCode, int employeeId)
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

            mockAgentScheduleService.Setup(mr => mr.UpdateAgentScheduleMangerChart(It.IsAny<UpdateAgentScheduleManagerChart>())).ReturnsAsync(
                (UpdateAgentScheduleManagerChart updateAgentScheduleChart) =>
                mockAgentScheduleData.UpdateAgentScheduleMangerChart(agentScheduleChart));

            var value = await controller.UpdateAgentScheduleManagerChart(agentScheduleChart);

            Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        }

        /// <summary>
        /// Updates the agent schedule chart returns no content result for scheduling manager tab.
        /// </summary>
        /// <param name="schedulingCode">The scheduling code.</param>
        /// <param name="employeeId">The employee identifier.</param>
        [Theory]
        [InlineData(1, 1)]
        [InlineData(1, 2)]
        public async void UpdateAgentScheduleChart_ReturnsNoContentResultForSchedulingManagerTab(int schedulingCode, int employeeId)
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

            mockAgentScheduleService.Setup(mr => mr.UpdateAgentScheduleMangerChart(It.IsAny<UpdateAgentScheduleManagerChart>())).ReturnsAsync(
                (UpdateAgentScheduleManagerChart updateAgentScheduleChart) =>
                mockAgentScheduleData.UpdateAgentScheduleMangerChart(agentScheduleChart));

            var value = await controller.UpdateAgentScheduleManagerChart(agentScheduleChart);

            Assert.Equal((int)HttpStatusCode.NoContent, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region ImportAgentScheduleChart

        /// <summary>
        /// Updates the agent schedule returns not found result.
        /// </summary>
        /// <param name="schedulingCode">The scheduling code.</param>
        /// <param name="agentSchedulingGroupId">The agent scheduling group identifier.</param>
        [Theory]
        [InlineData(100, 1)]
        [InlineData(101, 2)]
        public async void ImportAgentScheduleChart_ReturnsNotFoundResult(int schedulingCode, int agentSchedulingGroupId)
        {
            ImportAgentSchedule importAgentSchedule = new ImportAgentSchedule
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
                AgentSchedulingGroupId = agentSchedulingGroupId,
                ModifiedBy = "admin"
            };

            mockAgentScheduleService.Setup(mr => mr.ImportAgentScheduleChart(It.IsAny<ImportAgentSchedule>())).ReturnsAsync(
                (ImportAgentSchedule importAgentSchedule) =>
                mockAgentScheduleData.ImportAgentScheduleChart(importAgentSchedule));

            var value = await controller.ImportAgentScheduleChart(importAgentSchedule);

            Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        }

        /// <summary>
        /// Updates the agent schedule returns no content result.
        /// </summary>
        /// <param name="schedulingCode">The scheduling code.</param>
        /// <param name="agentSchedulingGroupId">The agent scheduling group identifier.</param>
        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        public async void ImportAgentScheduleChart_ReturnsNoContentResultForSchedulingTab(int schedulingCode, int agentSchedulingGroupId)
        {
            ImportAgentSchedule importAgentSchedule = new ImportAgentSchedule
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
                AgentSchedulingGroupId = agentSchedulingGroupId,
                ModifiedBy = "admin"
            };

            mockAgentScheduleService.Setup(mr => mr.ImportAgentScheduleChart(It.IsAny<ImportAgentSchedule>())).ReturnsAsync(
                (ImportAgentSchedule importAgentSchedule) =>
                mockAgentScheduleData.ImportAgentScheduleChart(importAgentSchedule));

            var value = await controller.ImportAgentScheduleChart(importAgentSchedule);

            Assert.Equal((int)HttpStatusCode.NoContent, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region CopyAgentScheduleChart

        /// <summary>
        /// Updates the agent schedule returns not found result.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        [Theory]
        [InlineData("6fe0b5ad6a05416894c0718e")]
        [InlineData("6fe0b5ad6a05416894c0718f")]
        public async void CopyAgentScheduleChart_ReturnsNotFoundResult(string agentScheduleId)
        {
            CopyAgentSchedule copyAgentSchedule = new CopyAgentSchedule
            {
                AgentScheduleType = AgentScheduleType.SchedulingTab,
                EmployeeIds = new List<int> { 1, 2 },
                ModifiedBy = "admin"
            };

            mockAgentScheduleService.Setup(mr => mr.CopyAgentSchedule(It.IsAny<AgentScheduleIdDetails>(), It.IsAny<CopyAgentSchedule>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails, CopyAgentSchedule copyAgentSchedule) =>
                mockAgentScheduleData.CopyAgentSchedule(new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId }, copyAgentSchedule));

            var value = await controller.CopyAgentSchedule(agentScheduleId, copyAgentSchedule);

            Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        }

        /// <summary>
        /// Copies the agent schedule chart returns no content result for scheduling tab.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        [Theory]
        [InlineData("5fe0b5ad6a05416894c0718e")]
        [InlineData("5fe0b5ad6a05416894c0718f")]
        public async void CopyAgentScheduleChart_ReturnsNoContentResultForSchedulingTab(string agentScheduleId)
        {
            CopyAgentSchedule copyAgentSchedule = new CopyAgentSchedule
            {
                AgentScheduleType = AgentScheduleType.SchedulingTab,
                EmployeeIds = new List<int> { 1, 2 },
                ModifiedBy = "admin"
            };

            mockAgentScheduleService.Setup(mr => mr.CopyAgentSchedule(It.IsAny<AgentScheduleIdDetails>(), It.IsAny<CopyAgentSchedule>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails, CopyAgentSchedule copyAgentSchedule) =>
                mockAgentScheduleData.CopyAgentSchedule(new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId }, copyAgentSchedule));

            var value = await controller.CopyAgentSchedule(agentScheduleId, copyAgentSchedule);

            Assert.Equal((int)HttpStatusCode.NoContent, (value as ObjectResult).StatusCode);
        }

        /// <summary>
        /// Copies the agent schedule chart returns no content result for scheduling manager tab.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        [Theory]
        [InlineData("5fe0b5ad6a05416894c0718e")]
        [InlineData("5fe0b5ad6a05416894c0718f")]
        public async void CopyAgentScheduleChart_ReturnsNoContentResultForSchedulingManagerTab(string agentScheduleId)
        {
            CopyAgentSchedule copyAgentSchedule = new CopyAgentSchedule
            {
                AgentScheduleType = AgentScheduleType.SchedulingMangerTab,
                EmployeeIds = new List<int> { 1, 2 },
                ModifiedBy = "admin"
            };

            mockAgentScheduleService.Setup(mr => mr.CopyAgentSchedule(It.IsAny<AgentScheduleIdDetails>(), It.IsAny<CopyAgentSchedule>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails, CopyAgentSchedule copyAgentSchedule) =>
                mockAgentScheduleData.CopyAgentSchedule(new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId }, copyAgentSchedule));

            var value = await controller.CopyAgentSchedule(agentScheduleId, copyAgentSchedule);

            Assert.Equal((int)HttpStatusCode.NoContent, (value as ObjectResult).StatusCode);
        }

        #endregion
    }
}
