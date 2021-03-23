using Css.Api.Core.Models.DTO.Response;
using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Scheduling.Business.UnitTest.Mocks;
using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedule;
using Css.Api.Scheduling.Models.DTO.Response.AgentSchedule;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System;
using Css.Api.Core.Models.Enums;
using Newtonsoft.Json;
using Css.Api.Core.Utilities.Extensions;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedulingGroup;

namespace Css.Api.Scheduling.UnitTest.Mock
{
    public class MockAgentScheduleData
    {
        /// <summary>
        /// Gets the agent schedules.
        /// </summary>
        /// <param name="agentScheduleQueryparameter">The agent schedule queryparameter.</param>
        /// <returns></returns>
        public CSSResponse GetAgentSchedules(AgentScheduleQueryparameter agentScheduleQueryparameter)
        {
            var agentSchedules = new MockDataContext().GetAgentSchedules(agentScheduleQueryparameter);
            return new CSSResponse(agentSchedules, HttpStatusCode.OK);
        }

        /// <summary>
        /// Gets the agent schedule.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <returns></returns>
        public CSSResponse GetAgentSchedule(AgentScheduleIdDetails agentScheduleIdDetails)
        {
            var agentSchedule = new MockDataContext().GetAgentSchedule(agentScheduleIdDetails);
            if (agentSchedule == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var mappedAgentSchedule = JsonConvert.DeserializeObject<AgentScheduleDetailsDTO>(JsonConvert.SerializeObject(agentSchedule));
            return new CSSResponse(mappedAgentSchedule, HttpStatusCode.OK);
        }

        /// <summary>
        /// Determines whether [is agent schedule range exist] [the specified agent schedule identifier details].
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="dateRange">The date range.</param>
        /// <returns></returns>
        public CSSResponse IsAgentScheduleRangeExist(AgentScheduleIdDetails agentScheduleIdDetails, DateRange dateRange)
        {
            var result = new MockDataContext().IsAgentScheduleRangeExist(agentScheduleIdDetails, dateRange);
            return new CSSResponse(result, HttpStatusCode.OK);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MockAgentScheduleData"/> class.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="agentScheduleChartQueryparameter">The agent schedule chart queryparameter.</param>
        public CSSResponse GetAgentScheduleCharts(AgentScheduleIdDetails agentScheduleIdDetails, AgentScheduleChartQueryparameter agentScheduleChartQueryparameter)
        {
            var agentSchedule = new MockDataContext().GetAgentSchedule(agentScheduleIdDetails);
            if (agentSchedule == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            if (agentScheduleChartQueryparameter.FromDate.HasValue && agentScheduleChartQueryparameter.FromDate != default(DateTime))
            {
                var date = agentScheduleChartQueryparameter.FromDate.Value;
                var dateTimeWithZeroTimeSpan = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Utc);

                agentSchedule.Ranges = agentSchedule.Ranges.Where(x => x.DateFrom == dateTimeWithZeroTimeSpan).ToList();
            }

            if (agentScheduleChartQueryparameter.ToDate.HasValue && agentScheduleChartQueryparameter.ToDate != default(DateTime))
            {
                var date = agentScheduleChartQueryparameter.ToDate.Value;
                var dateTimeWithZeroTimeSpan = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Utc);

                agentSchedule.Ranges = agentSchedule.Ranges.Where(x => x.DateTo == dateTimeWithZeroTimeSpan).ToList();
            }

            var mappedAgentScheduleChart = JsonConvert.DeserializeObject<AgentScheduleChartDetailsDTO>(JsonConvert.SerializeObject(agentSchedule));
            return new CSSResponse(mappedAgentScheduleChart, HttpStatusCode.OK);
        }

        /// <summary>
        /// Updates the Agent Admin.
        /// </summary>
        /// <param name="agentScheduleIdDetails"></param>
        /// <param name="agentScheduleDetails"></param>
        /// <returns></returns>
        public CSSResponse UpdateAgentSchedule(AgentScheduleIdDetails agentScheduleIdDetails, UpdateAgentSchedule agentScheduleDetails)
        {
            var dateRange = new DateRange { DateFrom = agentScheduleDetails.DateFrom, DateTo = agentScheduleDetails.DateTo };
            var agentSchedule = new MockDataContext().GetAgentSchedule(agentScheduleIdDetails, dateRange);
            if (agentSchedule == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            if (agentScheduleDetails.Status == SchedulingStatus.Released)
            {
                var employeeIdDetails = new EmployeeIdDetails { Id = agentSchedule.EmployeeId };
                var agentScheduleRange = new MockDataContext().GetAgentScheduleRange(agentScheduleIdDetails, dateRange);

                var scheduleManagerCharts = ScheduleHelper.GenerateAgentScheduleManagers(agentSchedule.EmployeeId, agentScheduleRange, agentScheduleDetails.ModifiedBy);

                foreach (var scheduleManagerChart in scheduleManagerCharts)
                {
                    new MockDataContext().UpdateAgentScheduleMangerChart(employeeIdDetails, scheduleManagerChart);
                }
            }

            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Updates the agent schedule chart.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="agentScheduleDetails">The agent schedule details.</param>
        /// <returns></returns>
        public CSSResponse UpdateAgentScheduleChart(AgentScheduleIdDetails agentScheduleIdDetails, UpdateAgentScheduleChart agentScheduleDetails)
        {
            var agentSchedule = new MockDataContext().GetAgentSchedule(agentScheduleIdDetails);
            if (agentSchedule == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var hasValidCodes = HasValidSchedulingCodes(agentScheduleDetails);
            if (!hasValidCodes)
            {
                return new CSSResponse("One of the scheduling code does not exists", HttpStatusCode.NotFound);
            }

            agentScheduleDetails.DateFrom = new DateTime(agentScheduleDetails.DateFrom.Year, agentScheduleDetails.DateFrom.Month, agentScheduleDetails.DateFrom.Day,
                                                         0, 0, 0, DateTimeKind.Utc);
            agentScheduleDetails.DateTo = new DateTime(agentScheduleDetails.DateTo.Year, agentScheduleDetails.DateTo.Month, agentScheduleDetails.DateTo.Day,
                                                       0, 0, 0, DateTimeKind.Utc);

            var employeeIdDetails = new EmployeeIdDetails { Id = agentSchedule.EmployeeId };
            var modifiedUserDetails = new ModifiedUserDetails { ModifiedBy = agentScheduleDetails.ModifiedBy };

            var agentScheduleRange = agentSchedule.Ranges.FirstOrDefault(x => x.DateFrom == agentScheduleDetails.DateFrom &&
                                                                              x.DateTo == agentScheduleDetails.DateTo);

            if (agentScheduleRange != null && agentScheduleRange.ScheduleCharts.Any())
            {
                foreach (var agentScheduleChart in agentScheduleDetails.AgentScheduleCharts)
                {
                    if (agentScheduleRange.ScheduleCharts.Exists(x => x.Day == agentScheduleChart.Day))
                    {
                        var scheduleChart = agentScheduleRange.ScheduleCharts.Find(x => x.Day == agentScheduleChart.Day);
                        scheduleChart.Charts = agentScheduleChart.Charts;
                    }
                    else
                    {
                        agentScheduleRange.ScheduleCharts.Add(agentScheduleChart);
                    }
                }

                agentScheduleRange.ScheduleCharts = agentScheduleRange.ScheduleCharts.Where(x => x.Charts.Any()).ToList();
                agentScheduleRange.ModifiedBy = agentScheduleDetails.ModifiedBy;
                agentScheduleRange.ModifiedDate = DateTimeOffset.UtcNow;

                new MockDataContext().UpdateAgentScheduleChart(agentScheduleIdDetails, agentScheduleRange, modifiedUserDetails);

                var scheduleRange = new AgentScheduleRange
                {
                    AgentSchedulingGroupId = agentScheduleRange.AgentSchedulingGroupId,
                    DateFrom = agentScheduleRange.DateFrom,
                    DateTo = agentScheduleRange.DateTo,
                    Status = SchedulingStatus.Pending_Schedule,
                    ScheduleCharts = agentScheduleDetails.AgentScheduleCharts,
                    CreatedBy = agentScheduleDetails.ModifiedBy,
                    CreatedDate = DateTimeOffset.UtcNow
                };
            }
            else
            {
                var agentAdmin = new MockDataContext().GetAgentAdminByEmployeeId(employeeIdDetails);
                if (agentAdmin != null)
                {
                    agentScheduleDetails.AgentScheduleCharts = agentScheduleDetails.AgentScheduleCharts.Where(x => x.Charts.Any()).ToList();
                    if (agentScheduleDetails.AgentScheduleCharts.Any())
                    {
                        agentScheduleRange = new AgentScheduleRange()
                        {
                            AgentSchedulingGroupId = agentAdmin.AgentSchedulingGroupId,
                            DateFrom = agentScheduleDetails.DateFrom,
                            DateTo = agentScheduleDetails.DateTo,
                            Status = SchedulingStatus.Pending_Schedule,
                            ScheduleCharts = agentScheduleDetails.AgentScheduleCharts,
                            CreatedBy = agentScheduleDetails.ModifiedBy,
                            CreatedDate = DateTimeOffset.UtcNow
                        };

                        new MockDataContext().UpdateAgentScheduleChart(agentScheduleIdDetails, agentScheduleRange, modifiedUserDetails);
                    }
                }
            }

            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Imports the agent schedule chart.
        /// </summary>
        /// <param name="agentScheduleDetails">The agent schedule details.</param>
        /// <returns></returns>
        public CSSResponse ImportAgentScheduleChart(ImportAgentSchedule agentScheduleDetails)
        {
            var hasValidCodes = HasValidSchedulingCodes(agentScheduleDetails);
            if (!hasValidCodes)
            {
                return new CSSResponse("One of the scheduling code does not exists", HttpStatusCode.NotFound);
            }

            var activityLogs = new List<ActivityLog>();

            foreach (var importAgentScheduleChart in agentScheduleDetails.ImportAgentScheduleCharts)
            {
                var employeeIdDetails = new EmployeeIdDetails { Id = importAgentScheduleChart.EmployeeId };
                var modifiedUserDetails = new ModifiedUserDetails { ModifiedBy = agentScheduleDetails.ModifiedBy };

                var agentSchedule = new MockDataContext().GetAgentScheduleByEmployeeId(employeeIdDetails);
                if (agentSchedule != null)
                {
                    var agentAdmin = new MockDataContext().GetAgentAdminByEmployeeId(employeeIdDetails);
                    if (agentAdmin != null)
                    {
                        foreach (var range in importAgentScheduleChart.Ranges)
                        {
                            range.DateFrom = new DateTime(range.DateFrom.Year, range.DateFrom.Month, range.DateFrom.Day, 0, 0, 0, DateTimeKind.Utc);
                            range.DateTo = new DateTime(range.DateTo.Year, range.DateTo.Month, range.DateTo.Day, 0, 0, 0, DateTimeKind.Utc);

                            var hasConflictingSchedules = agentSchedule.Ranges.Exists(x => x.Status == SchedulingStatus.Released &&
                                                                                           ((range.DateFrom < x.DateTo && range.DateTo > x.DateFrom) ||
                                                                                           (range.DateFrom == x.DateFrom && range.DateTo == x.DateTo)));

                            if (!hasConflictingSchedules)
                            {
                                var availableScheduleRange = agentSchedule.Ranges
                                    .FirstOrDefault(x => x.Status == SchedulingStatus.Pending_Schedule &&
                                                         ((range.DateFrom < x.DateTo && range.DateTo > x.DateFrom) ||
                                                         (range.DateFrom == x.DateFrom && range.DateTo == x.DateTo)));

                                if (availableScheduleRange != null)
                                {
                                    if (availableScheduleRange != null && availableScheduleRange.ScheduleCharts.Any())
                                    {
                                        availableScheduleRange.ScheduleCharts = range.AgentScheduleCharts;
                                        availableScheduleRange.CreatedBy = modifiedUserDetails.ModifiedBy;
                                        availableScheduleRange.CreatedDate = DateTimeOffset.UtcNow;

                                        var agentScheduleIdDetails = new AgentScheduleIdDetails { AgentScheduleId = agentSchedule.Id.ToString() };
                                        new MockDataContext().UpdateAgentScheduleChart(agentScheduleIdDetails, availableScheduleRange, modifiedUserDetails);
                                    }
                                }
                                else
                                {
                                    var agentScheduleRange = new AgentScheduleRange
                                    {
                                        AgentSchedulingGroupId = agentAdmin.AgentSchedulingGroupId,
                                        DateFrom = range.DateFrom,
                                        DateTo = range.DateTo,
                                        Status = SchedulingStatus.Pending_Schedule,
                                        ScheduleCharts = range.AgentScheduleCharts,
                                        CreatedBy = modifiedUserDetails.ModifiedBy,
                                        CreatedDate = DateTimeOffset.UtcNow
                                    };

                                    new MockDataContext().CopyAgentSchedules(employeeIdDetails, agentScheduleRange);
                                }
                            }
                        }
                    }
                }
            }

            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Copies the agent schedule.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="agentScheduleDetails">The agent schedule details.</param>
        /// <returns></returns>
        public CSSResponse CopyAgentScheduleChart(AgentScheduleIdDetails agentScheduleIdDetails, CopyAgentSchedule agentScheduleDetails)
        {
            var agentSchedule = new MockDataContext().GetAgentSchedule(agentScheduleIdDetails);
            if (agentSchedule == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            if (!agentScheduleDetails.EmployeeIds.Any())
            {
                AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails = new AgentSchedulingGroupIdDetails { AgentSchedulingGroupId = agentScheduleDetails.AgentSchedulingGroupId };
                agentScheduleDetails.EmployeeIds = new MockDataContext().GetEmployeeIdsByAgentScheduleGroupId(agentSchedulingGroupIdDetails);
                agentScheduleDetails.EmployeeIds = agentScheduleDetails.EmployeeIds.FindAll(x => x != agentSchedule.EmployeeId);
            }

            agentScheduleDetails.DateFrom = new DateTime(agentScheduleDetails.DateFrom.Year, agentScheduleDetails.DateFrom.Month,
                                                         agentScheduleDetails.DateFrom.Day, 0, 0, 0, DateTimeKind.Utc);
            agentScheduleDetails.DateTo = new DateTime(agentScheduleDetails.DateTo.Year, agentScheduleDetails.DateTo.Month,
                                                       agentScheduleDetails.DateTo.Day, 0, 0, 0, DateTimeKind.Utc);

            var activityLogs = new List<ActivityLog>();

            foreach (var employeeId in agentScheduleDetails.EmployeeIds)
            {
                var employeeIdDetails = new EmployeeIdDetails { Id = employeeId };
                var modifiedUserDetails = new ModifiedUserDetails { ModifiedBy = agentScheduleDetails.ModifiedBy };

                var employeeSchedule = new MockDataContext().GetAgentScheduleByEmployeeId(employeeIdDetails);
                if (employeeSchedule != null)
                {
                    var hasConflictingSchedules = employeeSchedule.Ranges.Exists(x => x.Status == SchedulingStatus.Released &&
                                                                                      ((agentScheduleDetails.DateFrom < x.DateTo && agentScheduleDetails.DateTo > x.DateFrom) ||
                                                                                      (agentScheduleDetails.DateFrom == x.DateFrom && agentScheduleDetails.DateTo == x.DateTo)));

                    if (!hasConflictingSchedules)
                    {
                        var availableScheduleRange = employeeSchedule.Ranges
                                    .FirstOrDefault(x => x.Status == SchedulingStatus.Pending_Schedule &&
                                                         ((agentScheduleDetails.DateFrom < x.DateTo && agentScheduleDetails.DateTo > x.DateFrom) ||
                                                         (agentScheduleDetails.DateFrom == x.DateFrom && agentScheduleDetails.DateTo == x.DateTo)));

                        var copiedAgentScheduleRange = agentSchedule.Ranges
                            .FirstOrDefault(x => x.DateFrom == agentScheduleDetails.DateFrom &&
                                                 x.DateTo == agentScheduleDetails.DateTo);

                        if (copiedAgentScheduleRange != null)
                        {
                            if (availableScheduleRange != null)
                            {
                                if (availableScheduleRange != null && availableScheduleRange.ScheduleCharts.Any())
                                {
                                    availableScheduleRange.ScheduleCharts = copiedAgentScheduleRange.ScheduleCharts;
                                    availableScheduleRange.CreatedBy = modifiedUserDetails.ModifiedBy;
                                    availableScheduleRange.CreatedDate = DateTimeOffset.UtcNow;

                                    var scheduleIdDetails = new AgentScheduleIdDetails { AgentScheduleId = employeeSchedule.Id.ToString() };
                                    new MockDataContext().UpdateAgentScheduleChart(scheduleIdDetails, availableScheduleRange, modifiedUserDetails);
                                }
                            }
                            else
                            {
                                var agentAdmin = new MockDataContext().GetAgentAdminByEmployeeId(employeeIdDetails);
                                if (agentAdmin != null)
                                {
                                    var agentScheduleRange = new AgentScheduleRange
                                    {
                                        AgentSchedulingGroupId = agentAdmin.AgentSchedulingGroupId,
                                        DateFrom = agentScheduleDetails.DateFrom,
                                        DateTo = agentScheduleDetails.DateTo,
                                        Status = SchedulingStatus.Pending_Schedule,
                                        ScheduleCharts = copiedAgentScheduleRange.ScheduleCharts,
                                        CreatedBy = modifiedUserDetails.ModifiedBy,
                                        CreatedDate = DateTimeOffset.UtcNow
                                    };

                                    var agentScheduleCharts = agentSchedule.Ranges
                                        .FirstOrDefault(x => x.DateFrom == agentScheduleDetails.DateFrom && x.DateTo == agentScheduleDetails.DateTo)?.ScheduleCharts;

                                    new MockDataContext().CopyAgentSchedules(employeeIdDetails, agentScheduleRange);
                                }
                            }
                        }
                    }
                }
            }

            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Updates the agent schedule range.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="dateRangeDetails">The date range details.</param>
        /// <returns></returns>
        public CSSResponse UpdateAgentScheduleRange(AgentScheduleIdDetails agentScheduleIdDetails, UpdateAgentScheduleDateRange dateRangeDetails)
        {
            var dateRange = new DateRange { DateFrom = dateRangeDetails.OldDateFrom, DateTo = dateRangeDetails.OldDateTo };
            var agentSchedule = new MockDataContext().GetAgentSchedule(agentScheduleIdDetails, dateRange, SchedulingStatus.Pending_Schedule);

            if (agentSchedule == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var newDateFrom = new DateTime(dateRangeDetails.NewDateFrom.Year, dateRangeDetails.NewDateFrom.Month, dateRangeDetails.NewDateFrom.Day,
                                           0, 0, 0, DateTimeKind.Utc);
            var newDateTo = new DateTime(dateRangeDetails.NewDateTo.Year, dateRangeDetails.NewDateTo.Month, dateRangeDetails.NewDateTo.Day,
                                         0, 0, 0, DateTimeKind.Utc);
            var oldDateFrom = new DateTime(dateRangeDetails.OldDateFrom.Year, dateRangeDetails.OldDateFrom.Month, dateRangeDetails.OldDateFrom.Day,
                                           0, 0, 0, DateTimeKind.Utc);
            var oldDateTo = new DateTime(dateRangeDetails.OldDateTo.Year, dateRangeDetails.OldDateTo.Month, dateRangeDetails.OldDateTo.Day,
                                         0, 0, 0, DateTimeKind.Utc);

            var hasConflictingSchedules = agentSchedule.Ranges.Exists(x => oldDateFrom != x.DateFrom && oldDateTo != x.DateTo &&
                                                                           ((dateRangeDetails.NewDateFrom < x.DateTo && dateRangeDetails.NewDateTo > x.DateFrom)) ||
                                                                             dateRangeDetails.NewDateFrom == x.DateFrom && dateRangeDetails.NewDateTo == x.DateTo);
            if (hasConflictingSchedules)
            {
                return new CSSResponse(HttpStatusCode.Conflict);
            }

            new MockDataContext().UpdateAgentScheduleRange(agentScheduleIdDetails, dateRangeDetails);

            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Deletes the agent schedule range.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="dateRange">The date range.</param>
        /// <returns></returns>
        public CSSResponse DeleteAgentScheduleRange(AgentScheduleIdDetails agentScheduleIdDetails, DateRange dateRange)
        {
            var agentScheduleRange = new MockDataContext().GetAgentScheduleRange(agentScheduleIdDetails, dateRange);
            if (agentScheduleRange == null || agentScheduleRange.Status == SchedulingStatus.Released)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            new MockDataContext().DeleteAgentScheduleRange(agentScheduleIdDetails, dateRange);

            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// Determines whether [has valid scheduling codes] [the specified agent schedule details].
        /// </summary>
        /// <param name="agentScheduleDetails">The agent schedule details.</param>
        /// <returns>
        ///   <c>true</c> if [has valid scheduling codes] [the specified agent schedule details]; otherwise, <c>false</c>.
        /// </returns>
        private bool HasValidSchedulingCodes(object agentScheduleDetails)
        {
            bool isValid = true;
            List<int> codes = new List<int>();

            if (agentScheduleDetails is UpdateAgentScheduleChart)
            {
                var details = agentScheduleDetails as UpdateAgentScheduleChart;
                foreach (var agentScheduleChart in details.AgentScheduleCharts)
                {
                    var scheduleCodes = agentScheduleChart.Charts.Select(x => x.SchedulingCodeId).ToList();
                    codes.AddRange(scheduleCodes);
                }
            }
            else if (agentScheduleDetails is ImportAgentSchedule)
            {

                var details = agentScheduleDetails as ImportAgentSchedule;
                foreach (var importAgentScheduleChart in details.ImportAgentScheduleCharts)
                {
                    foreach (var range in importAgentScheduleChart.Ranges)
                    {
                        foreach (var agentScheduleChart in range.AgentScheduleCharts)
                        {
                            var scheduleCodes = agentScheduleChart.Charts.Select(x => x.SchedulingCodeId).ToList();
                            codes.AddRange(scheduleCodes);
                        }
                    }
                }
            }

            codes = codes.Distinct().ToList();

            if (codes.Any())
            {
                var schedulingCodesCount = new MockDataContext().GetSchedulingCodesCountByIds(codes);
                if (schedulingCodesCount != codes.Count())
                {
                    isValid = false;
                }
            }

            return isValid;
        }
    }
}
