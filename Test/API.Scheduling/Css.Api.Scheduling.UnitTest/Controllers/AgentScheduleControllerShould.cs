using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Controllers;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedule;
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
        private AgentSchedulesController controller;

        /// <summary>
        /// The mock agent schedule data
        /// </summary>
        private MockAgentScheduleData mockAgentScheduleData;

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
        [Theory]
        [InlineData("6fe0b5ad6a05416894c0718e")]
        [InlineData("6fe0b5ad6a05416894c0718f")]
        public async void UpdateAgentScheduleChart_ReturnsNotFoundResult(string agentScheduleId)
        {
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
        [Theory]
        [InlineData("5fe0b5ad6a05416894c0718e")]
        [InlineData("5fe0b5ad6a05416894c0718f")]
        public async void UpdateAgentScheduleChart_ReturnsNoContentResultForSchedulingTab(string agentScheduleId)
        {
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

            mockAgentScheduleService.Setup(mr => mr.UpdateAgentScheduleChart(It.IsAny<AgentScheduleIdDetails>(), It.IsAny<UpdateAgentScheduleChart>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails, UpdateAgentScheduleChart updateAgentScheduleChart) =>
                mockAgentScheduleData.UpdateAgentScheduleChart(new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId }, agentScheduleChart));

            var value = await controller.UpdateAgentScheduleChart(agentScheduleId, agentScheduleChart);

            Assert.Equal((int)HttpStatusCode.NoContent, (value as ObjectResult).StatusCode);
        }

        /// <summary>
        /// Updates the agent schedule returns no content result.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        [Theory]
        [InlineData("5fe0b5ad6a05416894c0718e")]
        [InlineData("5fe0b5ad6a05416894c0718f")]
        public async void UpdateAgentScheduleChart_ReturnsNoContentResultForSchedulingManagerTab(string agentScheduleId)
        {
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

            mockAgentScheduleService.Setup(mr => mr.UpdateAgentScheduleChart(It.IsAny<AgentScheduleIdDetails>(), It.IsAny<UpdateAgentScheduleChart>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails, UpdateAgentScheduleChart updateAgentScheduleChart) =>
                mockAgentScheduleData.UpdateAgentScheduleChart(new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId }, agentScheduleChart));

            var value = await controller.UpdateAgentScheduleChart(agentScheduleId, agentScheduleChart);

            Assert.Equal((int)HttpStatusCode.NoContent, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region ImportAgentScheduleChart

        /// <summary>
        /// Updates the agent schedule returns not found result.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        [Theory]
        [InlineData("6fe0b5ad6a05416894c0718e")]
        [InlineData("6fe0b5ad6a05416894c0718f")]
        public async void ImportAgentScheduleChart_ReturnsNotFoundResult(string agentScheduleId)
        {
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

            mockAgentScheduleService.Setup(mr => mr.ImportAgentScheduleChart(It.IsAny<AgentScheduleIdDetails>(), It.IsAny<ImportAgentScheduleChart>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails, ImportAgentScheduleChart importAgentScheduleChart) =>
                mockAgentScheduleData.ImportAgentScheduleChart(new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId }, importAgentScheduleChart));

            var value = await controller.ImportAgentScheduleChart(agentScheduleId, importAgentScheduleChart);

            Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        }

        /// <summary>
        /// Updates the agent schedule returns no content result.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        [Theory]
        [InlineData("5fe0b5ad6a05416894c0718e")]
        [InlineData("5fe0b5ad6a05416894c0718f")]
        public async void ImportAgentScheduleChart_ReturnsNoContentResultForSchedulingTab(string agentScheduleId)
        {
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

            mockAgentScheduleService.Setup(mr => mr.ImportAgentScheduleChart(It.IsAny<AgentScheduleIdDetails>(), It.IsAny<ImportAgentScheduleChart>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails, ImportAgentScheduleChart importAgentScheduleChart) =>
                mockAgentScheduleData.ImportAgentScheduleChart(new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId }, importAgentScheduleChart));

            var value = await controller.ImportAgentScheduleChart(agentScheduleId, importAgentScheduleChart);

            Assert.Equal((int)HttpStatusCode.NoContent, (value as ObjectResult).StatusCode);
        }

        /// <summary>
        /// Updates the agent schedule returns no content result.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        [Theory]
        [InlineData("5fe0b5ad6a05416894c0718e")]
        [InlineData("5fe0b5ad6a05416894c0718f")]
        public async void ImportAgentScheduleChart_ReturnsNoContentResultForSchedulingManagerTab(string agentScheduleId)
        {
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

            mockAgentScheduleService.Setup(mr => mr.ImportAgentScheduleChart(It.IsAny<AgentScheduleIdDetails>(), It.IsAny<ImportAgentScheduleChart>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails, ImportAgentScheduleChart importAgentScheduleChart) =>
                mockAgentScheduleData.ImportAgentScheduleChart(new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId }, importAgentScheduleChart));

            var value = await controller.ImportAgentScheduleChart(agentScheduleId, importAgentScheduleChart);

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
