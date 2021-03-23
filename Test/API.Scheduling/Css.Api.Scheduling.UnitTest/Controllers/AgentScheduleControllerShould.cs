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

namespace Css.Api.Scheduling.UnitTest.Controllers
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

        #region IsAgentScheduleRangeExist

        /// <summary>
        /// Determines whether [is agent schedule range exist returns false if not] [the specified agent schedule identifier].
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
        public async void IsAgentScheduleRangeExist_ReturnsFalseIfNot(string agentScheduleId, int fromYear, int fromMonth, int fromDay,
                                                                      int toYear, int toMonth, int toDay)
        {
            mockAgentScheduleService.Setup(mr => mr.IsAgentScheduleRangeExist(It.IsAny<AgentScheduleIdDetails>(), It.IsAny<DateRange>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails, DateRange range) =>
                mockAgentScheduleData.IsAgentScheduleRangeExist(agentScheduleIdDetails, range));

            var value = await controller.IsAgentScheduleRangeExist(agentScheduleId, new DateRange { 
                                                                   DateFrom = new DateTime(fromYear, fromMonth, fromDay), DateTo = new DateTime(toYear, toMonth, toDay)
            });

            Assert.Equal((int)HttpStatusCode.OK, (value as ObjectResult).StatusCode);
        }

        /// <summary>
        /// Determines whether [is agent schedule range exist returns true if] [the specified agent schedule identifier].
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
        public async void IsAgentScheduleRangeExist_ReturnsTrueIf(string agentScheduleId, int fromYear, int fromMonth, int fromDay,
                                                                  int toYear, int toMonth, int toDay)
        {
            mockAgentScheduleService.Setup(mr => mr.IsAgentScheduleRangeExist(It.IsAny<AgentScheduleIdDetails>(), It.IsAny<DateRange>())).ReturnsAsync(
                   (AgentScheduleIdDetails agentScheduleIdDetails, DateRange range) =>
                   mockAgentScheduleData.IsAgentScheduleRangeExist(agentScheduleIdDetails, range));

            var value = await controller.IsAgentScheduleRangeExist(agentScheduleId, new DateRange
            {
                DateFrom = new DateTime(fromYear, fromMonth, fromDay),
                DateTo = new DateTime(toYear, toMonth, toDay)
            });

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
        public async void GetAgentScheduleCharts_ReturnsOKResult(string agentScheduleId)
        {
            var agentScheduleChartQueryparameter = new AgentScheduleChartQueryparameter();

            mockAgentScheduleService.Setup(mr => mr.GetAgentScheduleCharts(It.IsAny<AgentScheduleIdDetails>(), It.IsAny<AgentScheduleChartQueryparameter>()))
                .ReturnsAsync((AgentScheduleIdDetails agentScheduleIdDetails, AgentScheduleChartQueryparameter agentScheduleChartQueryparameter) =>
                mockAgentScheduleData.GetAgentScheduleCharts(new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId }, agentScheduleChartQueryparameter));

            var value = await controller.GetAgentScheduleCharts(agentScheduleId, agentScheduleChartQueryparameter);

            Assert.Equal((int)HttpStatusCode.OK, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region UpdateAgentSchedule

        /// <summary>
        /// Updates the agent schedule returns not found result.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        [Theory]
        [InlineData("6fe0b5ad6a05416894c0718e", 2021, 3, 21, 2021, 3, 27)]
        [InlineData("6fe0b5ad6a05416894c0718f", 2021, 3, 21, 2021, 3, 27)]
        public async void UpdateAgentSchedule_ReturnsNotFoundResult(string agentScheduleId, int fromYear, int fromMonth, int fromDay,
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

            mockAgentScheduleService.Setup(mr => mr.UpdateAgentSchedule(It.IsAny<AgentScheduleIdDetails>(), It.IsAny<UpdateAgentSchedule>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails, UpdateAgentSchedule updateAgentSchedule) =>
                mockAgentScheduleData.UpdateAgentSchedule(new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId }, updateAgentSchedule));

            var value = await controller.UpdateAgentSchedule(agentScheduleId, agentSchedule);

            Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        }

        /// <summary>
        /// Updates the agent schedule returns no content result.
        /// </summary>
        /// <param name="agentScheduleId">The agent schedule identifier.</param>
        [Theory]
        [InlineData("5fe0b5ad6a05416894c0718e", 2021, 3, 21, 2021, 3, 27)]
        [InlineData("5fe0b5ad6a05416894c0718f", 2021, 3, 21, 2021, 3, 27)]
        public async void UpdateAgentSchedule_ReturnsNoContentResult(string agentScheduleId, int fromYear, int fromMonth, int fromDay,
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

            mockAgentScheduleService.Setup(mr => mr.UpdateAgentSchedule(It.IsAny<AgentScheduleIdDetails>(), It.IsAny<UpdateAgentSchedule>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails, UpdateAgentSchedule updateAgentSchedule) =>
                mockAgentScheduleData.UpdateAgentSchedule(new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId }, updateAgentSchedule));

            var value = await controller.UpdateAgentSchedule(agentScheduleId, agentSchedule);

            Assert.Equal((int)HttpStatusCode.NoContent, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region UpdateAgentScheduleChart

        /// <summary>
        /// Updates the agent schedule chart returns not found result.
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
        public async void UpdateAgentScheduleChart_ReturnsNotFoundResult(string agentScheduleId, int schedulingCode, int fromYear,
                                                                         int fromMonth, int fromDay, int toYear, int toMonth, int toDay)
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

            mockAgentScheduleService.Setup(mr => mr.UpdateAgentScheduleChart(It.IsAny<AgentScheduleIdDetails>(), It.IsAny<UpdateAgentScheduleChart>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails, UpdateAgentScheduleChart updateAgentScheduleChart) =>
                mockAgentScheduleData.UpdateAgentScheduleChart(new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId }, agentScheduleChart));

            var value = await controller.UpdateAgentScheduleChart(agentScheduleId, agentScheduleChart);

            Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        }

        /// <summary>
        /// Updates the agent schedule chart returns not found result for scheduling codes.
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
        public async void UpdateAgentScheduleChart_ReturnsNotFoundResultForSchedulingCodes(string agentScheduleId, int schedulingCode, int fromYear, 
                                                                                           int fromMonth, int fromDay, int toYear, int toMonth, int toDay)
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

            mockAgentScheduleService.Setup(mr => mr.UpdateAgentScheduleChart(It.IsAny<AgentScheduleIdDetails>(), It.IsAny<UpdateAgentScheduleChart>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails, UpdateAgentScheduleChart updateAgentScheduleChart) =>
                mockAgentScheduleData.UpdateAgentScheduleChart(new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId }, agentScheduleChart));

            var value = await controller.UpdateAgentScheduleChart(agentScheduleId, agentScheduleChart);

            Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        }

        /// <summary>
        /// Updates the agent schedule chart returns no content result.
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
        public async void UpdateAgentScheduleChart_ReturnsNoContentResult(string agentScheduleId, int schedulingCode, int fromYear, int fromMonth,
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

            mockAgentScheduleService.Setup(mr => mr.UpdateAgentScheduleChart(It.IsAny<AgentScheduleIdDetails>(), It.IsAny<UpdateAgentScheduleChart>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails, UpdateAgentScheduleChart updateAgentScheduleChart) =>
                mockAgentScheduleData.UpdateAgentScheduleChart(new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId }, agentScheduleChart));

            var value = await controller.UpdateAgentScheduleChart(agentScheduleId, agentScheduleChart);

            Assert.Equal((int)HttpStatusCode.NoContent, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region ImportAgentScheduleChart

        /// <summary>
        /// Imports the agent schedule chart returns not found result.
        /// </summary>
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
        public async void ImportAgentScheduleChart_ReturnsNotFoundResult(int schedulingCode, int employeeId, int fromYear,
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
                ActivityOrigin = ActivityOrigin.CSS,
                ModifiedBy = "admin",
                ModifiedUser = 123
            };

            mockAgentScheduleService.Setup(mr => mr.ImportAgentScheduleChart(It.IsAny<ImportAgentSchedule>())).ReturnsAsync(
                (ImportAgentSchedule importAgentSchedule) =>
                mockAgentScheduleData.ImportAgentScheduleChart(importAgentSchedule));

            var value = await controller.ImportAgentScheduleChart(importAgentSchedule);

            Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        }

        /// <summary>
        /// Imports the agent schedule chart returns no content result.
        /// </summary>
        /// <param name="schedulingCode">The scheduling code.</param>
        /// <param name="employeeId">The employee identifier.</param>
        /// <param name="fromYear">From year.</param>
        /// <param name="fromMonth">From month.</param>
        /// <param name="fromDay">From day.</param>
        /// <param name="toYear">To year.</param>
        /// <param name="toMonth">To month.</param>
        /// <param name="toDay">To day.</param>
        [Theory]
        [InlineData(1, 1, 2021, 4, 4, 2021, 4, 10)]
        [InlineData(2, 2, 2021, 4, 4, 2021, 4, 10)]
        public async void ImportAgentScheduleChart_ReturnsNoContentResult(int schedulingCode, int employeeId, int fromYear,
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

            mockAgentScheduleService.Setup(mr => mr.ImportAgentScheduleChart(It.IsAny<ImportAgentSchedule>())).ReturnsAsync(
                (ImportAgentSchedule importAgentSchedule) =>
                mockAgentScheduleData.ImportAgentScheduleChart(importAgentSchedule));

            var value = await controller.ImportAgentScheduleChart(importAgentSchedule);

            Assert.Equal((int)HttpStatusCode.NoContent, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region CopyAgentScheduleChart

        /// <summary>
        /// Copies the agent schedule chart returns not found result.
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
        public async void CopyAgentScheduleChart_ReturnsNotFoundResult(string agentScheduleId, int fromYear, int fromMonth, int fromDay, int toYear,
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

            mockAgentScheduleService.Setup(mr => mr.CopyAgentScheduleChart(It.IsAny<AgentScheduleIdDetails>(), It.IsAny<CopyAgentSchedule>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails, CopyAgentSchedule copyAgentSchedule) =>
                mockAgentScheduleData.CopyAgentScheduleChart(new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId }, copyAgentSchedule));

            var value = await controller.CopyAgentScheduleChart(agentScheduleId, copyAgentSchedule);

            Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        }

        /// <summary>
        /// Copies the agent schedule chart returns no content result.
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
        public async void CopyAgentScheduleChart_ReturnsNoContentResult(string agentScheduleId, int fromYear, int fromMonth, int fromDay, 
                                                                                        int toYear, int toMonth, int toDay)
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

            mockAgentScheduleService.Setup(mr => mr.CopyAgentScheduleChart(It.IsAny<AgentScheduleIdDetails>(), It.IsAny<CopyAgentSchedule>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails, CopyAgentSchedule copyAgentSchedule) =>
                mockAgentScheduleData.CopyAgentScheduleChart(new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId }, copyAgentSchedule));

            var value = await controller.CopyAgentScheduleChart(agentScheduleId, copyAgentSchedule);

            Assert.Equal((int)HttpStatusCode.NoContent, (value as ObjectResult).StatusCode);
        }

        #endregion
    }
}
