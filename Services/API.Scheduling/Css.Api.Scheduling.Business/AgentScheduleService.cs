﻿using AutoMapper;
using Css.Api.Core.DataAccess.Repository.UnitOfWork.Interfaces;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Core.Models.DTO.Response;
using Css.Api.Core.Models.Enums;
using Css.Api.Core.Utilities.Extensions;
using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.ActivityLog;
using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedule;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedulingGroup;
using Css.Api.Scheduling.Models.DTO.Response.AgentSchedule;
using Css.Api.Scheduling.Models.Enums;
using Css.Api.Scheduling.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Business
{
    /// <summary>
    /// Service for agent admin part
    /// </summary>
    /// <seealso cref="Css.Api.Setup.Business.Interfaces.AgentScheduleService" />
    public class AgentScheduleService : IAgentScheduleService
    {
        /// <summary>
        /// The HTTP context accessor
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// The activity log repository
        /// </summary>
        private readonly IActivityLogRepository _activityLogRepository;

        /// <summary>
        /// The agent schedule repository
        /// </summary>
        private readonly IAgentScheduleRepository _agentScheduleRepository;

        /// <summary>
        /// The agent schedule manager repository
        /// </summary>
        private readonly IAgentScheduleManagerRepository _agentScheduleManagerRepository;

        /// <summary>
        /// The agent admin repository
        /// </summary>
        private readonly IAgentAdminRepository _agentAdminRepository;

        /// <summary>
        /// The scheduling code repository
        /// </summary>
        private readonly ISchedulingCodeRepository _schedulingCodeRepository;

        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// The uow
        /// </summary>
        private readonly IUnitOfWork _uow;

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentAdminService" /> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="activityLogRepository">The activity log repository.</param>
        /// <param name="agentScheduleRepository">The agent schedule repository.</param>
        /// <param name="agentScheduleManagerRepository">The agent schedule manager repository.</param>
        /// <param name="agentAdminRepository">The agent admin repository.</param>
        /// <param name="schedulingCodeRepository">The scheduling code repository.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="uow">The uow.</param>
        public AgentScheduleService(
            IHttpContextAccessor httpContextAccessor,
            IActivityLogRepository activityLogRepository,
            IAgentScheduleRepository agentScheduleRepository, 
            IAgentScheduleManagerRepository agentScheduleManagerRepository,
            IAgentAdminRepository agentAdminRepository,
            ISchedulingCodeRepository schedulingCodeRepository,
            IMapper mapper,
            IUnitOfWork uow)
        {
            _httpContextAccessor = httpContextAccessor;
            _activityLogRepository = activityLogRepository;
            _agentScheduleRepository = agentScheduleRepository;
            _agentScheduleManagerRepository = agentScheduleManagerRepository;
            _agentAdminRepository = agentAdminRepository;
            _schedulingCodeRepository = schedulingCodeRepository;
            _mapper = mapper;
            _uow = uow;
        }

        /// <summary>
        /// Gets the agent schedules.
        /// </summary>
        /// <param name="agentScheduleQueryparameter">The agent schedule queryparameter.</param>
        /// <returns></returns>
        public async Task<CSSResponse> GetAgentSchedules(AgentScheduleQueryparameter agentScheduleQueryparameter)
        {
            var agentSchedules = await _agentScheduleRepository.GetAgentSchedules(agentScheduleQueryparameter);
            _httpContextAccessor.HttpContext.Response.Headers.Add("X-Pagination", PagedList<Entity>.ToJson(agentSchedules));

            return new CSSResponse(agentSchedules, HttpStatusCode.OK);
        }

        /// <summary>
        /// Gets the agent schedule.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> GetAgentSchedule(AgentScheduleIdDetails agentScheduleIdDetails)
        {
            var agentSchedule = await _agentScheduleRepository.GetAgentSchedule(agentScheduleIdDetails);
            if (agentSchedule == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var mappedAgentSchedule = _mapper.Map<AgentScheduleDetailsDTO>(agentSchedule);
            return new CSSResponse(mappedAgentSchedule, HttpStatusCode.OK);
        }

        /// <summary>
        /// Determines whether [is agent schedule range exist] [the specified agent schedule identifier details].
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="dateRange">The date range.</param>
        /// <returns></returns>
        public async Task<CSSResponse> IsAgentScheduleRangeExist(AgentScheduleIdDetails agentScheduleIdDetails, DateRange dateRange)
        {
            var result = await _agentScheduleRepository.IsAgentScheduleRangeExist(agentScheduleIdDetails, dateRange);
            return new CSSResponse(result, HttpStatusCode.OK);
        }

        /// <summary>
        /// Gets the agent schedule charts.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="agentScheduleChartQueryparameter">The agent schedule chart queryparameter.</param>
        /// <returns></returns>
        public async Task<CSSResponse> GetAgentScheduleCharts(AgentScheduleIdDetails agentScheduleIdDetails, AgentScheduleChartQueryparameter agentScheduleChartQueryparameter)
        {
            var agentSchedule = await _agentScheduleRepository.GetAgentSchedule(agentScheduleIdDetails);
            if (agentSchedule == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            if (agentScheduleChartQueryparameter.FromDate.HasValue && agentScheduleChartQueryparameter.FromDate != default(DateTime))
            {
                var date = agentScheduleChartQueryparameter.FromDate.Value;
                var dateTimeWithZeroTimeSpan = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);

                agentSchedule.Ranges = agentSchedule.Ranges.Where(x => x.DateFrom == dateTimeWithZeroTimeSpan).ToList();
            }

            if (agentScheduleChartQueryparameter.ToDate.HasValue && agentScheduleChartQueryparameter.ToDate != default(DateTime))
            {
                var date = agentScheduleChartQueryparameter.ToDate.Value;
                var dateTimeWithZeroTimeSpan = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);

                agentSchedule.Ranges = agentSchedule.Ranges.Where(x => x.DateTo == dateTimeWithZeroTimeSpan).ToList();
            }

            var mappedAgentScheduleChart = _mapper.Map<AgentScheduleChartDetailsDTO>(agentSchedule);
            return new CSSResponse(mappedAgentScheduleChart, HttpStatusCode.OK);
        }

        /// <summary>
        /// Updates the Agent Admin.
        /// </summary>
        /// <param name="agentScheduleIdDetails"></param>
        /// <param name="agentScheduleDetails"></param>
        /// <returns></returns>
        public async Task<CSSResponse> UpdateAgentSchedule(AgentScheduleIdDetails agentScheduleIdDetails, UpdateAgentSchedule agentScheduleDetails)
        {
            var dateRange = new DateRange { DateFrom = agentScheduleDetails.DateFrom, DateTo = agentScheduleDetails.DateTo };
            var agentSchedule = await _agentScheduleRepository.GetAgentSchedule(agentScheduleIdDetails, dateRange);
            if (agentSchedule == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            _agentScheduleRepository.UpdateAgentSchedule(agentScheduleIdDetails, agentScheduleDetails);

            if (agentScheduleDetails.Status == SchedulingStatus.Released)
            {               
                var activityLogs = new List<ActivityLog>();
                var employeeIdDetails = new EmployeeIdDetails { Id = agentSchedule.EmployeeId };
                var agentScheduleRange = await _agentScheduleRepository.GetAgentScheduleRange(agentScheduleIdDetails, dateRange);

                for (var date = agentScheduleDetails.DateFrom; date <= agentScheduleDetails.DateTo; date = date.AddDays(1))
                {
                    var dayOfWeek = date.Date.DayOfWeek;
                    if (agentScheduleRange.ScheduleCharts.Exists(x => x.Day == (int)dayOfWeek))
                    {
                        AgentScheduleManager scheduleManagerChart = new AgentScheduleManager()
                        {
                            EmployeeId = agentSchedule.EmployeeId,
                            AgentSchedulingGroupId = agentScheduleRange.AgentSchedulingGroupId,
                            Date = date,
                            Charts = agentScheduleRange.ScheduleCharts.FirstOrDefault(x => x.Day == (int)dayOfWeek).Charts,
                            CreatedBy = agentScheduleDetails.ModifiedBy,
                            CreatedDate = DateTimeOffset.UtcNow,
                        };

                        _agentScheduleManagerRepository.UpdateAgentScheduleMangerChart(employeeIdDetails, scheduleManagerChart);

                        var activityLog = GetActivityLogForSchedulingManager(scheduleManagerChart, agentSchedule.EmployeeId,
                                                                             agentScheduleDetails.ModifiedBy, agentScheduleDetails.ModifiedUser,
                                                                             agentScheduleDetails.ActivityOrigin);
                        activityLogs.Add(activityLog);
                    }
                }

                _activityLogRepository.CreateActivityLogs(activityLogs);
            }

            await _uow.Commit();

            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Updates the agent schedule chart.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="agentScheduleDetails">The agent schedule details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> UpdateAgentScheduleChart(AgentScheduleIdDetails agentScheduleIdDetails, UpdateAgentScheduleChart agentScheduleDetails)
        {
            var agentSchedule = await _agentScheduleRepository.GetAgentSchedule(agentScheduleIdDetails);
            if (agentSchedule == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var hasValidCodes = await HasValidSchedulingCodes(agentScheduleDetails);
            if (!hasValidCodes)
            {
                return new CSSResponse("One of the scheduling code does not exists", HttpStatusCode.NotFound);
            }

            agentScheduleDetails.DateFrom = new DateTime(agentScheduleDetails.DateFrom.Year, agentScheduleDetails.DateFrom.Month, agentScheduleDetails.DateFrom.Day, 0, 0, 0);
            agentScheduleDetails.DateTo = new DateTime(agentScheduleDetails.DateTo.Year, agentScheduleDetails.DateTo.Month, agentScheduleDetails.DateTo.Day, 0, 0, 0);

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

                _agentScheduleRepository.UpdateAgentScheduleChart(agentScheduleIdDetails, agentScheduleRange, modifiedUserDetails);

                var scheduleRange = new AgentScheduleRange {
                    AgentSchedulingGroupId = agentScheduleRange.AgentSchedulingGroupId,
                    DateFrom = agentScheduleRange.DateFrom,
                    DateTo = agentScheduleRange.DateTo,
                    Status = SchedulingStatus.Pending_Schedule,
                    ScheduleCharts = agentScheduleDetails.AgentScheduleCharts,
                    CreatedBy = agentScheduleDetails.ModifiedBy,
                    CreatedDate = DateTimeOffset.UtcNow
                };

                var activityLog = GetActivityLogForSchedulingChart(scheduleRange, agentSchedule.EmployeeId, agentScheduleDetails.ModifiedBy,
                                                                   agentScheduleDetails.ModifiedUser, agentScheduleDetails.ActivityOrigin);
                _activityLogRepository.CreateActivityLog(activityLog);
            }
            else
            {
                var agentAdmin = await _agentAdminRepository.GetAgentAdminByEmployeeId(employeeIdDetails);
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

                        _agentScheduleRepository.UpdateAgentScheduleChart(agentScheduleIdDetails, agentScheduleRange, modifiedUserDetails);

                        var activityLog = GetActivityLogForSchedulingChart(agentScheduleRange, agentSchedule.EmployeeId, agentScheduleDetails.ModifiedBy,
                                                                           agentScheduleDetails.ModifiedUser, agentScheduleDetails.ActivityOrigin);
                        _activityLogRepository.CreateActivityLog(activityLog);
                    }
                }
            }

            await _uow.Commit();

            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Imports the agent schedule chart.
        /// </summary>
        /// <param name="agentScheduleDetails">The agent schedule details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> ImportAgentScheduleChart(ImportAgentSchedule agentScheduleDetails)
        {
            var hasValidCodes = await HasValidSchedulingCodes(agentScheduleDetails);
            if (!hasValidCodes)
            {
                return new CSSResponse("One of the scheduling code does not exists", HttpStatusCode.NotFound);
            }

            var activityLogs = new List<ActivityLog>();

            foreach (var importAgentScheduleChart in agentScheduleDetails.ImportAgentScheduleCharts)
            {
                var employeeIdDetails = new EmployeeIdDetails { Id = importAgentScheduleChart.EmployeeId };
                var modifiedUserDetails = new ModifiedUserDetails { ModifiedBy = agentScheduleDetails.ModifiedBy };

                var agentSchedule = await _agentScheduleRepository.GetAgentScheduleByEmployeeId(employeeIdDetails);
                if (agentSchedule != null)
                {
                    var agentAdmin = await _agentAdminRepository.GetAgentAdminByEmployeeId(employeeIdDetails);
                    if (agentAdmin != null)
                    {
                        foreach (var range in importAgentScheduleChart.Ranges)
                        {
                            range.DateFrom = new DateTime(range.DateFrom.Year, range.DateFrom.Month, range.DateFrom.Day, 0, 0, 0);
                            range.DateTo = new DateTime(range.DateTo.Year, range.DateTo.Month, range.DateTo.Day, 0, 0, 0);

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
                                        _agentScheduleRepository.UpdateAgentScheduleChart(agentScheduleIdDetails, availableScheduleRange, modifiedUserDetails);

                                        var activityLog = GetActivityLogForSchedulingChart(availableScheduleRange, agentSchedule.EmployeeId,
                                                                                           agentScheduleDetails.ModifiedBy, agentScheduleDetails.ModifiedUser,
                                                                                           agentScheduleDetails.ActivityOrigin);
                                        activityLogs.Add(activityLog);
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

                                    _agentScheduleRepository.CopyAgentSchedules(employeeIdDetails, agentScheduleRange);

                                    var activityLog = GetActivityLogForSchedulingChart(agentScheduleRange, agentSchedule.EmployeeId,
                                                                                       agentScheduleDetails.ModifiedBy, agentScheduleDetails.ModifiedUser,
                                                                                       agentScheduleDetails.ActivityOrigin);
                                    activityLogs.Add(activityLog);
                                }
                            }
                        }
                    }
                }
            }

            _activityLogRepository.CreateActivityLogs(activityLogs);

            await _uow.Commit();

            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Copies the agent schedule.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="agentScheduleDetails">The agent schedule details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> CopyAgentScheduleChart(AgentScheduleIdDetails agentScheduleIdDetails, CopyAgentSchedule agentScheduleDetails)
        {
            var agentSchedule = await _agentScheduleRepository.GetAgentSchedule(agentScheduleIdDetails);
            if (agentSchedule == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            if (!agentScheduleDetails.EmployeeIds.Any())
            {
                AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails = new AgentSchedulingGroupIdDetails { AgentSchedulingGroupId = agentScheduleDetails.AgentSchedulingGroupId };
                agentScheduleDetails.EmployeeIds = await _agentScheduleRepository.GetEmployeeIdsByAgentScheduleGroupId(agentSchedulingGroupIdDetails);
                agentScheduleDetails.EmployeeIds = agentScheduleDetails.EmployeeIds.FindAll(x => x != agentSchedule.EmployeeId);
            }

            agentScheduleDetails.DateFrom = new DateTime(agentScheduleDetails.DateFrom.Year, agentScheduleDetails.DateFrom.Month, agentScheduleDetails.DateFrom.Day, 0, 0, 0);
            agentScheduleDetails.DateTo = new DateTime(agentScheduleDetails.DateTo.Year, agentScheduleDetails.DateTo.Month, agentScheduleDetails.DateTo.Day, 0, 0, 0);

            var activityLogs = new List<ActivityLog>();

            foreach (var employeeId in agentScheduleDetails.EmployeeIds)
            {
                var employeeIdDetails = new EmployeeIdDetails { Id = employeeId };
                var modifiedUserDetails = new ModifiedUserDetails { ModifiedBy = agentScheduleDetails.ModifiedBy };

                var employeeSchedule = await _agentScheduleRepository.GetAgentScheduleByEmployeeId(employeeIdDetails);
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
                                    _agentScheduleRepository.UpdateAgentScheduleChart(scheduleIdDetails, availableScheduleRange, modifiedUserDetails);

                                    var activityLog = GetActivityLogForSchedulingChart(availableScheduleRange, agentSchedule.EmployeeId,
                                                                                       agentScheduleDetails.ModifiedBy, agentScheduleDetails.ModifiedUser,
                                                                                       agentScheduleDetails.ActivityOrigin);
                                    activityLogs.Add(activityLog);
                                }
                            }
                            else
                            {
                                var agentAdmin = await _agentAdminRepository.GetAgentAdminByEmployeeId(employeeIdDetails);
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

                                    _agentScheduleRepository.CopyAgentSchedules(employeeIdDetails, agentScheduleRange);

                                    var activityLog = GetActivityLogForSchedulingChart(agentScheduleRange, employeeId, agentScheduleDetails.ModifiedBy,
                                                                                       agentScheduleDetails.ModifiedUser, agentScheduleDetails.ActivityOrigin);
                                    activityLogs.Add(activityLog);
                                }
                            }
                        }
                    }
                }
            }

            _activityLogRepository.CreateActivityLogs(activityLogs);

            await _uow.Commit();

            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Updates the agent schedule range.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="dateRangeDetails">The date range details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> UpdateAgentScheduleRange(AgentScheduleIdDetails agentScheduleIdDetails, UpdateAgentScheduleDateRange dateRangeDetails)
        {
            var dateRange = new DateRange { DateFrom = dateRangeDetails.OldDateFrom, DateTo = dateRangeDetails.OldDateTo };
            var agentSchedule = await _agentScheduleRepository.GetAgentSchedule(agentScheduleIdDetails, dateRange, SchedulingStatus.Pending_Schedule);

            if (agentSchedule == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var newDateFrom = new DateTime(dateRangeDetails.NewDateFrom.Year, dateRangeDetails.NewDateFrom.Month, dateRangeDetails.NewDateFrom.Day, 0, 0, 0);
            var newDateTo = new DateTime(dateRangeDetails.NewDateTo.Year, dateRangeDetails.NewDateTo.Month, dateRangeDetails.NewDateTo.Day, 0, 0, 0);
            var oldDateFrom = new DateTime(dateRangeDetails.OldDateFrom.Year, dateRangeDetails.OldDateFrom.Month, dateRangeDetails.OldDateFrom.Day, 0, 0, 0);
            var oldDateTo = new DateTime(dateRangeDetails.OldDateTo.Year, dateRangeDetails.OldDateTo.Month, dateRangeDetails.OldDateTo.Day, 0, 0, 0);

            var hasConflictingSchedules = agentSchedule.Ranges.Exists(x => oldDateFrom != x.DateFrom && oldDateTo != x.DateTo &&
                                                                           ((dateRangeDetails.NewDateFrom < x.DateTo && dateRangeDetails.NewDateTo > x.DateFrom)) ||
                                                                             dateRangeDetails.NewDateFrom == x.DateFrom && dateRangeDetails.NewDateTo == x.DateTo);
            if (hasConflictingSchedules)
            {
                return new CSSResponse(HttpStatusCode.Conflict);
            }

            _agentScheduleRepository.UpdateAgentScheduleRange(agentScheduleIdDetails, dateRangeDetails);

            var employeeIdDetails = new EmployeeIdDetails { Id = agentSchedule.EmployeeId };
            var activityLogRange = new UpdateActivityLogRange
            {
                DateFrom = oldDateFrom,
                DateTo = oldDateTo,
                NewDateFrom = newDateFrom,
                NewDateTo = newDateTo
            };

            _activityLogRepository.UpdateActivityLogsSchedulingRange(employeeIdDetails, activityLogRange);

            await _uow.Commit();

            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Deletes the agent schedule range.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="dateRange">The date range.</param>
        /// <returns></returns>
        public async Task<CSSResponse> DeleteAgentScheduleRange(AgentScheduleIdDetails agentScheduleIdDetails, DateRange dateRange)
        {
            var agentScheduleRange = await _agentScheduleRepository.GetAgentScheduleRange(agentScheduleIdDetails, dateRange);
            if (agentScheduleRange == null || agentScheduleRange.Status == SchedulingStatus.Released)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            _agentScheduleRepository.DeleteAgentScheduleRange(agentScheduleIdDetails, dateRange);

            await _uow.Commit();

            return new CSSResponse(HttpStatusCode.NoContent);
        }       

        /// <summary>
        /// Gets the activity log for scheduling chart.
        /// </summary>
        /// <param name="agentScheduleRange">The agent schedule range.</param>
        /// <param name="employeeId">The employee identifier.</param>
        /// <param name="executedBy">The executed by.</param>
        /// <param name="executedUser">The executed user.</param>
        /// <param name="activityOrigin">The activity origin.</param>
        /// <returns></returns>
        private ActivityLog GetActivityLogForSchedulingChart(AgentScheduleRange agentScheduleRange, int employeeId, string executedBy, int executedUser,
                                                             ActivityOrigin activityOrigin)
        {
            return new ActivityLog()
            {
                EmployeeId = employeeId,
                ExecutedBy = executedBy,
                ExecutedUser = executedUser,
                TimeStamp = DateTimeOffset.UtcNow,
                ActivityOrigin = activityOrigin,
                ActivityStatus = ActivityStatus.Updated,
                ActivityType = ActivityType.SchedulingGrid,
                SchedulingFieldDetails = new SchedulingFieldDetails() { ActivityLogRange = _mapper.Map<ActivityLogScheduleRange>(agentScheduleRange) }
            };
        }

        /// <summary>
        /// Gets the activity log for scheduling manager.
        /// </summary>
        /// <param name="agentScheduleManagerChart">The agent schedule manager chart.</param>
        /// <param name="employeeId">The employee identifier.</param>
        /// <param name="executedBy">The executed by.</param>
        /// <param name="executedUser">The executed user.</param>
        /// <param name="activityOrigin">The activity origin.</param>
        /// <returns></returns>
        private ActivityLog GetActivityLogForSchedulingManager(AgentScheduleManager agentScheduleManagerChart, int employeeId, string executedBy, int executedUser,
                                                               ActivityOrigin activityOrigin)
        {
            return new ActivityLog()
            {
                EmployeeId = employeeId,
                ExecutedBy = executedBy,
                ExecutedUser = executedUser,
                TimeStamp = DateTimeOffset.UtcNow,
                ActivityOrigin = activityOrigin,
                ActivityStatus = ActivityStatus.Updated,
                ActivityType = ActivityType.SchedulingmanagerGrid,
                SchedulingFieldDetails = new SchedulingFieldDetails() { ActivityLogManager = _mapper.Map<ActivityLogScheduleManager>(agentScheduleManagerChart) }
            };
        }
        
        /// <summary>
        /// Determines whether [has valid scheduling codes] [the specified agent schedule details].
        /// </summary>
        /// <param name="agentScheduleDetails">The agent schedule details.</param>
        /// <returns>
        ///   <c>true</c> if [has valid scheduling codes] [the specified agent schedule details]; otherwise, <c>false</c>.
        /// </returns>
        private async Task<bool> HasValidSchedulingCodes(object agentScheduleDetails)
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
                var schedulingCodesCount = await _schedulingCodeRepository.GetSchedulingCodesCountByIds(codes);
                if (schedulingCodesCount != codes.Count())
                {
                    isValid = false;
                }
            }

            return isValid;
        }
    }
}