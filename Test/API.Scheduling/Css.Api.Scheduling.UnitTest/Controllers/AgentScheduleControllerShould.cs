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
                mockAgentScheduleData.GetAgentSchedule(agentScheduleIdDetails));

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
                mockAgentScheduleData.GetAgentSchedule(agentScheduleIdDetails));

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
                mockAgentScheduleData.GetAgentScheduleCharts(agentScheduleIdDetails, agentScheduleChartQueryparameter));

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
                mockAgentScheduleData.GetAgentScheduleCharts(agentScheduleIdDetails, agentScheduleChartQueryparameter));

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
                mockAgentScheduleData.UpdateAgentSchedule(agentScheduleIdDetails, updateAgentSchedule));

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
                mockAgentScheduleData.UpdateAgentSchedule(agentScheduleIdDetails, updateAgentSchedule));

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
                ModifiedUser = "123"
            };

            mockAgentScheduleService.Setup(mr => mr.UpdateAgentScheduleChart(It.IsAny<AgentScheduleIdDetails>(), It.IsAny<UpdateAgentScheduleChart>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails, UpdateAgentScheduleChart updateAgentScheduleChart) =>
                mockAgentScheduleData.UpdateAgentScheduleChart(agentScheduleIdDetails, updateAgentScheduleChart));

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
                ModifiedUser = "123"
            };

            mockAgentScheduleService.Setup(mr => mr.UpdateAgentScheduleChart(It.IsAny<AgentScheduleIdDetails>(), It.IsAny<UpdateAgentScheduleChart>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails, UpdateAgentScheduleChart updateAgentScheduleChart) =>
                mockAgentScheduleData.UpdateAgentScheduleChart(agentScheduleIdDetails, updateAgentScheduleChart));

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
                ModifiedUser = "123"
            };

            mockAgentScheduleService.Setup(mr => mr.UpdateAgentScheduleChart(It.IsAny<AgentScheduleIdDetails>(), It.IsAny<UpdateAgentScheduleChart>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails, UpdateAgentScheduleChart updateAgentScheduleChart) =>
                mockAgentScheduleData.UpdateAgentScheduleChart(agentScheduleIdDetails, updateAgentScheduleChart));

            var value = await controller.UpdateAgentScheduleChart(agentScheduleId, agentScheduleChart);

            Assert.Equal((int)HttpStatusCode.NoContent, (value as ObjectResult).StatusCode);
        }

        #endregion

        //#region ImportAgentScheduleChart

        ///// <summary>
        ///// Imports the agent schedule chart returns not found result.
        ///// </summary>
        ///// <param name="schedulingCode">The scheduling code.</param>
        ///// <param name="employeeId">The employee identifier.</param>
        ///// <param name="fromYear">From year.</param>
        ///// <param name="fromMonth">From month.</param>
        ///// <param name="fromDay">From day.</param>
        ///// <param name="toYear">To year.</param>
        ///// <param name="toMonth">To month.</param>
        ///// <param name="toDay">To day.</param>
        //[Theory]
        //[InlineData(100, 1, 2021, 4, 4, 2021, 4, 10)]
        //[InlineData(101, 2, 2021, 4, 4, 2021, 4, 10)]
        //public async void ImportAgentScheduleChart_ReturnsNotFoundResult(int schedulingCode, int employeeId, int fromYear,
        //                                                                 int fromMonth, int fromDay, int toYear, int toMonth, int toDay)
        //{
        //    AgentScheduleImport importAgentSchedule = new AgentScheduleImport
        //    {
        //        AgentScheduleImportData = new List<AgentScheduleImportData>
        //        {
        //            new AgentScheduleImportData
        //            {
        //                EmployeeId = employeeId,
        //                StartDate = new DateTime(fromYear, fromMonth, fromDay),
        //                EndDate = new DateTime(toYear, toMonth, toDay),
        //                StartTime = "00:00 am",
        //                EndTime = "00:05 pm",
        //                SchedulingCodeId = schedulingCode
        //            }
        //        },
        //        ActivityOrigin = ActivityOrigin.CSS,
        //        ModifiedBy = "admin"
        //    };

        //    mockAgentScheduleService.Setup(mr => mr.ImportAgentScheduleChart(It.IsAny<AgentScheduleImport>())).ReturnsAsync(
        //        (AgentScheduleImport importAgentSchedule) =>
        //        mockAgentScheduleData.ImportAgentScheduleChart(importAgentSchedule));

        //    var value = await controller.ImportAgentScheduleChart(importAgentSchedule);

        //    Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        //}

        ///// <summary>
        ///// Imports the agent schedule chart returns no content result.
        ///// </summary>
        ///// <param name="schedulingCode">The scheduling code.</param>
        ///// <param name="employeeId">The employee identifier.</param>
        ///// <param name="fromYear">From year.</param>
        ///// <param name="fromMonth">From month.</param>
        ///// <param name="fromDay">From day.</param>
        ///// <param name="toYear">To year.</param>
        ///// <param name="toMonth">To month.</param>
        ///// <param name="toDay">To day.</param>
        //[Theory]
        //[InlineData(1, 1, 2021, 4, 4, 2021, 4, 10)]
        //[InlineData(2, 2, 2021, 4, 4, 2021, 4, 10)]
        //public async void ImportAgentScheduleChart_ReturnsNoContentResult(int schedulingCode, int employeeId, int fromYear,
        //                                                                  int fromMonth, int fromDay, int toYear, int toMonth, int toDay)
        //{
        //    AgentScheduleImport importAgentSchedule = new AgentScheduleImport
        //    {
        //        AgentScheduleImportData = new List<AgentScheduleImportData>
        //        {
        //            new AgentScheduleImportData
        //            {
        //                EmployeeId = employeeId,
        //                StartDate = new DateTime(fromYear, fromMonth, fromDay),
        //                EndDate = new DateTime(toYear, toMonth, toDay),
        //                StartTime = "00:00 am",
        //                EndTime = "00:05 pm",
        //                SchedulingCodeId = schedulingCode
        //            },
        //            new AgentScheduleImportData
        //            {
        //                EmployeeId = employeeId,
        //                StartDate = new DateTime(2021, 3, 21),
        //                EndDate =  new DateTime(2021, 3, 27),
        //                StartTime = "00:00 am",
        //                EndTime = "00:05 pm",
        //                SchedulingCodeId = schedulingCode
        //            }
        //        },
        //        ActivityOrigin = ActivityOrigin.CSS,
        //        ModifiedBy = "admin"
        //    };

        //    mockAgentScheduleService.Setup(mr => mr.ImportAgentScheduleChart(It.IsAny<AgentScheduleImport>())).ReturnsAsync(
        //        (AgentScheduleImport importAgentSchedule) =>
        //        mockAgentScheduleData.ImportAgentScheduleChart(importAgentSchedule));

        //    var value = await controller.ImportAgentScheduleChart(importAgentSchedule);

        //    Assert.Equal((int)HttpStatusCode.NoContent, (value as ObjectResult).StatusCode);
        //}

        //#endregion

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
                ModifiedUser = "123"
            };

            mockAgentScheduleService.Setup(mr => mr.CopyAgentScheduleChart(It.IsAny<AgentScheduleIdDetails>(), It.IsAny<CopyAgentSchedule>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails, CopyAgentSchedule copyAgentSchedule) =>
                mockAgentScheduleData.CopyAgentScheduleChart(agentScheduleIdDetails, copyAgentSchedule));

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
                ModifiedUser = "123"
            };

            mockAgentScheduleService.Setup(mr => mr.CopyAgentScheduleChart(It.IsAny<AgentScheduleIdDetails>(), It.IsAny<CopyAgentSchedule>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails, CopyAgentSchedule copyAgentSchedule) =>
                mockAgentScheduleData.CopyAgentScheduleChart(agentScheduleIdDetails, copyAgentSchedule));

            var value = await controller.CopyAgentScheduleChart(agentScheduleId, copyAgentSchedule);

            Assert.Equal((int)HttpStatusCode.NoContent, (value as ObjectResult).StatusCode);
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
        public async void UpdateAgentScheduleRange_ReturnsNotFound(string agentScheduleId, int oldFromYear, int oldFromMonth, int oldFromDay,
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

            mockAgentScheduleService.Setup(mr => mr.UpdateAgentScheduleRange(It.IsAny<AgentScheduleIdDetails>(), It.IsAny<UpdateAgentScheduleDateRange>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails, UpdateAgentScheduleDateRange dateRange) => mockAgentScheduleData.UpdateAgentScheduleRange(agentScheduleIdDetails, dateRange));

            var value = await controller.UpdateAgentScheduleRange(agentScheduleId, agentScheduleRange);

            Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
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
        public async void UpdateAgentScheduleRange_ReturnsConflict(string agentScheduleId, int oldFromYear, int oldFromMonth, int oldFromDay,
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

            mockAgentScheduleService.Setup(mr => mr.UpdateAgentScheduleRange(It.IsAny<AgentScheduleIdDetails>(), It.IsAny<UpdateAgentScheduleDateRange>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails, UpdateAgentScheduleDateRange dateRange) => mockAgentScheduleData.UpdateAgentScheduleRange(agentScheduleIdDetails, dateRange));

            var value = await controller.UpdateAgentScheduleRange(agentScheduleId, agentScheduleRange);

            Assert.Equal((int)HttpStatusCode.Conflict, (value as ObjectResult).StatusCode);
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
        public async void UpdateAgentScheduleRange_ReturnsNoContent(string agentScheduleId, int oldFromYear, int oldFromMonth, int oldFromDay,
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

            mockAgentScheduleService.Setup(mr => mr.UpdateAgentScheduleRange(It.IsAny<AgentScheduleIdDetails>(), It.IsAny<UpdateAgentScheduleDateRange>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails, UpdateAgentScheduleDateRange dateRange) => mockAgentScheduleData.UpdateAgentScheduleRange(agentScheduleIdDetails, dateRange));

            var value = await controller.UpdateAgentScheduleRange(agentScheduleId, agentScheduleRange);

            Assert.Equal((int)HttpStatusCode.NoContent, (value as ObjectResult).StatusCode);
        }

        #endregion

        #region DeleteAgentScheduleRange

        /// <summary>
        /// Deletes the agent schedule range return not found.
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
        public async void DeleteAgentScheduleRange_ReturnNotFound(string agentScheduleId, int fromYear, int fromMonth, int fromDay,
                                                                                int toYear, int toMonth, int toDay)
        {
            AgentScheduleIdDetails agentScheduleIdDetails = new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId };
            DateRange daterange = new DateRange
            {
                DateFrom = new DateTime(fromYear, fromMonth, fromDay),
                DateTo = new DateTime(toYear, toMonth, toDay),
            };

            mockAgentScheduleService.Setup(mr => mr.DeleteAgentScheduleRange(It.IsAny<AgentScheduleIdDetails>(), It.IsAny<DateRange>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails, DateRange dateRange) => mockAgentScheduleData.DeleteAgentScheduleRange(agentScheduleIdDetails, dateRange));

            var value = await controller.DeleteAgentScheduleRange(agentScheduleId, daterange);

            Assert.Equal((int)HttpStatusCode.NotFound, (value as ObjectResult).StatusCode);
        }

        /// <summary>
        /// Deletes the content of the agent schedule range return no.
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
        public async void DeleteAgentScheduleRange_ReturnNoContent(string agentScheduleId, int fromYear, int fromMonth, int fromDay,
                                                                    int toYear, int toMonth, int toDay)
        {
            AgentScheduleIdDetails agentScheduleIdDetails = new AgentScheduleIdDetails { AgentScheduleId = agentScheduleId };
            DateRange daterange = new DateRange
            {
                DateFrom = new DateTime(fromYear, fromMonth, fromDay),
                DateTo = new DateTime(toYear, toMonth, toDay),
            };

            mockAgentScheduleService.Setup(mr => mr.DeleteAgentScheduleRange(It.IsAny<AgentScheduleIdDetails>(), It.IsAny<DateRange>())).ReturnsAsync(
                (AgentScheduleIdDetails agentScheduleIdDetails, DateRange dateRange) => mockAgentScheduleData.DeleteAgentScheduleRange(agentScheduleIdDetails, dateRange));

            var value = await controller.DeleteAgentScheduleRange(agentScheduleId, daterange);

            Assert.Equal((int)HttpStatusCode.NoContent, (value as ObjectResult).StatusCode);
        }

        #endregion
    }
}
