using AutoMapper;
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
using System.Collections;
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
        /// The agent scheduling group repository
        /// </summary>
        private readonly IAgentSchedulingGroupRepository _agentSchedulingGroupRepository;

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
        /// <param name="agentSchedulingGroupRepository">The scheduling group repository.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="uow">The uow.</param>
        public AgentScheduleService(
            IHttpContextAccessor httpContextAccessor,
            IActivityLogRepository activityLogRepository,
            IAgentScheduleRepository agentScheduleRepository, 
            IAgentScheduleManagerRepository agentScheduleManagerRepository,
            IAgentAdminRepository agentAdminRepository,
            IAgentSchedulingGroupRepository agentSchedulingGroupRepository,
            ISchedulingCodeRepository schedulingCodeRepository,
            IMapper mapper,
            IUnitOfWork uow)
        {
            _httpContextAccessor = httpContextAccessor;
            _activityLogRepository = activityLogRepository;
            _agentScheduleRepository = agentScheduleRepository;
            _agentScheduleManagerRepository = agentScheduleManagerRepository;
            _agentSchedulingGroupRepository = agentSchedulingGroupRepository;
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
                var dateTimeWithZeroTimeSpan = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Utc);

                agentSchedule.Ranges = agentSchedule.Ranges.Where(x => x.DateFrom == dateTimeWithZeroTimeSpan).ToList();
            }

            if (agentScheduleChartQueryparameter.ToDate.HasValue && agentScheduleChartQueryparameter.ToDate != default(DateTime))
            {
                var date = agentScheduleChartQueryparameter.ToDate.Value;
                var dateTimeWithZeroTimeSpan = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Utc);

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

                var scheduleManagerCharts = ScheduleHelper.GenerateAgentScheduleManagers(agentSchedule.EmployeeId, agentScheduleRange, agentScheduleDetails.ModifiedBy);

                foreach (var scheduleManagerChart in scheduleManagerCharts)
                {
                    _agentScheduleManagerRepository.UpdateAgentScheduleMangerChart(employeeIdDetails, scheduleManagerChart);

                    var activityLog = GetActivityLogForSchedulingManager(scheduleManagerChart, agentSchedule.EmployeeId,
                                                                         agentScheduleDetails.ModifiedBy, agentScheduleDetails.ModifiedUser,
                                                                         agentScheduleDetails.ActivityOrigin);
                    activityLogs.Add(activityLog);
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

            //var hasValidCodes = await HasValidSchedulingCodes(agentScheduleDetails);
            //if (!hasValidCodes)
            //{
            //    return new CSSResponse("One of the scheduling code does not exists", HttpStatusCode.NotFound);
            //}

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
        
        private DateTime GetWeekFirstDay(DateTime DateToCheck)
        {
            DateTime date = DateToCheck;
            DateTime weekFirstDay = date.AddDays(DayOfWeek.Sunday - date.DayOfWeek);

            return weekFirstDay;
        }


        private DateTime GetWeekLastDay(DateTime FirstDayOfWeek)
        {
            DateTime weekLastDay = FirstDayOfWeek.AddDays(6);

            return weekLastDay;
        }

        /// <summary>
        /// Imports the agent schedule chart.
        /// </summary>
        /// <param name="agentScheduleDetails">The agent schedule details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> ImportAgentScheduleChart(AgentScheduleImport agentScheduleImport)
        {


            List<string> errors = new List<string>();
            List<string> success = new List<string>();

            ImportAgentScheduleResponse importAgentScheduleResponse = new ImportAgentScheduleResponse();


            int importCount = 0;
            int importSuccess = 0;

            var activityLogs = new List<ActivityLog>();

            List<SchedulingRangeImport> importRanges = new List<SchedulingRangeImport>();

            // loop the data from excel model
            foreach (var importData in agentScheduleImport.AgentScheduleImportData)
            {
                importCount = importCount + 1;

                var employeeIdDetails = new EmployeeIdDetails { Id = importData.EmployeeId };
                //var modifiedUserDetails = new ModifiedUserDetails { ModifiedBy = agentScheduleImport.ModifiedBy };

                var agentSchedule = await _agentScheduleRepository.GetAgentScheduleByEmployeeId(employeeIdDetails);

                // check if agent schedule exists
                if (agentSchedule != null)
                {
                    var hasValidCodes = await HasValidSchedulingCodes(importData.SchedulingCodeId);
                    if (!hasValidCodes)
                    {
                        return new CSSResponse("One of the scheduling code does not exists", HttpStatusCode.NotFound);
                    }

                    var agentAdmin = await _agentAdminRepository.GetAgentAdminByEmployeeId(employeeIdDetails);

                    // check if agent exists
                    if (agentAdmin != null)
                    {

                        // Get the first day of the week
                        DateTime StartOfWeekDate = GetWeekFirstDay(importData.StartDate);
                        // Get the last day of the week
                        DateTime EndOfWeekDate = GetWeekLastDay(StartOfWeekDate);

                        var weekRange = new SchedulingRangeImport
                        {
                            AgentSchedulingGroupId = agentAdmin.AgentSchedulingGroupId,
                            DateFrom = StartOfWeekDate,
                            DateTo = EndOfWeekDate,
                            ModifiedBy = agentScheduleImport.ModifiedBy,
                            EmployeeId = employeeIdDetails.Id,
                            Status = SchedulingStatus.Pending_Schedule
                        };

                        success.Add($"{employeeIdDetails.Id}");

                        var hasConflictingSchedules = agentSchedule.Ranges.Exists(x => x.Status == SchedulingStatus.Released &&
                                                                                       ((weekRange.DateFrom < x.DateTo && weekRange.DateTo > x.DateFrom) ||
                                                                                       (weekRange.DateFrom == x.DateFrom && weekRange.DateTo == x.DateTo)));

                        // proceed if schedule is not released
                        if (!hasConflictingSchedules)
                        {
                            var availableScheduleRange = agentSchedule.Ranges
                                .Where(x => x.Status == SchedulingStatus.Pending_Schedule &&
                                                     ((weekRange.DateFrom <= x.DateTo && weekRange.DateFrom >= x.DateFrom) ||
                                                     (weekRange.DateTo <= x.DateTo && weekRange.DateTo >= x.DateFrom))).FirstOrDefault();

                            // check if there is an available schedule range
                            if (availableScheduleRange != null)
                            {
                                var agentScheduleIdDetails = new AgentScheduleIdDetails { AgentScheduleId = agentSchedule.Id.ToString() };
                                // delete existing range object
                                //var deleted = await _agentScheduleRepository.DeleteAgentScheduleRangeImport(agentScheduleIdDetails, new DateRange { DateFrom = weekRange.DateFrom, DateTo = weekRange.DateTo });
                                _agentScheduleRepository.DeleteAgentScheduleRangeImport(agentScheduleIdDetails, new DateRange { DateFrom = weekRange.DateFrom, DateTo = weekRange.DateTo });

                                await _uow.Commit();

                                if (importRanges.Exists(x => x.DateFrom == weekRange.DateFrom & x.DateTo == weekRange.DateTo & x.EmployeeId == employeeIdDetails.Id))
                                {
                                    foreach(var range in importRanges.Where(x => x.DateFrom == weekRange.DateFrom & x.DateTo == weekRange.DateTo & x.EmployeeId == employeeIdDetails.Id))
                                    {                                        
                                        // check if Schedule Chart Day already exists in the pre-update model
                                        if(range.ScheduleCharts.Exists(x => x.Day == (int)importData.StartDate.DayOfWeek))
                                        {
                                            // check if chart time has no conflicts inside the charts array
                                            var selectedDayFromImport = range.ScheduleCharts.Find(x => x.Day == (int)importData.StartDate.DayOfWeek);

                                            var checkChart1 = selectedDayFromImport.Charts.Exists(x =>
                                                DateTime.Parse(importData.StartTime).TimeOfDay >= DateTime.Parse(x.StartTime).TimeOfDay
                                                & DateTime.Parse(importData.StartTime).TimeOfDay <= DateTime.Parse(x.EndTime).TimeOfDay
                                                );

                                            var checkChart2 = selectedDayFromImport.Charts.Exists(x =>
                                                DateTime.Parse(importData.EndTime).TimeOfDay >= DateTime.Parse(x.StartTime).TimeOfDay
                                                & DateTime.Parse(importData.EndTime).TimeOfDay <= DateTime.Parse(x.EndTime).TimeOfDay
                                                );

                                            if (!checkChart1
                                               ||
                                               !checkChart2)
                                            {
                                                // create chart if it does not exists
                                                var chart = new ScheduleChart();
                                                chart.StartTime = importData.StartTime;
                                                chart.EndTime = importData.EndTime;
                                                chart.SchedulingCodeId = importData.SchedulingCodeId;

                                                selectedDayFromImport.Charts.Add(chart);

                                                // success
                                                success.Add($"'1 Agent Schedule data updated.");
                                                importSuccess = importSuccess + 1;

                                            }

                                        }
                                        else
                                        {
                                            // create new Chart day if it does not exists
                                            //insert new range in pre-update models
                                            var chart = new ScheduleChart();
                                            chart.StartTime = importData.StartTime;
                                            chart.EndTime = importData.EndTime;
                                            chart.SchedulingCodeId = importData.SchedulingCodeId;

                                            var agentScheduleChart = new AgentScheduleChart();
                                            agentScheduleChart.Charts.Add(chart);
                                            agentScheduleChart.Day = (int)importData.StartDate.DayOfWeek;

                                            range.ScheduleCharts.Add(agentScheduleChart);

                                            // success
                                            success.Add($"'1 Agent Schedule data updated.");
                                            importSuccess = importSuccess + 1;

                                        }
                                    }
                                }
                                else
                                {
                                    var chart = new ScheduleChart();
                                    chart.StartTime = importData.StartTime;
                                    chart.EndTime = importData.EndTime;
                                    chart.SchedulingCodeId = importData.SchedulingCodeId;

                                    var agentScheduleChart = new AgentScheduleChart();
                                    agentScheduleChart.Charts.Add(chart);
                                    agentScheduleChart.Day = (int)importData.StartDate.DayOfWeek;

                                    weekRange.ScheduleCharts.Add(agentScheduleChart);

                                    importRanges.Add(weekRange);

                                    // success
                                    success.Add($"'1 Agent Schedule data updated.");
                                    importSuccess = importSuccess + 1;

                                }

                            }
                            // create range if it does not exist
                            else
                            {

                                if (importRanges.Exists(x => x.DateFrom == weekRange.DateFrom & x.DateTo == weekRange.DateTo & x.EmployeeId == employeeIdDetails.Id))
                                {
                                    foreach (var range in importRanges.Where(x => x.DateFrom == weekRange.DateFrom & x.DateTo == weekRange.DateTo & x.EmployeeId == employeeIdDetails.Id))
                                    {
                                        // check if Schedule Chart Day already exists in the pre-update model
                                        // update range in the pre-update model
                                        if (range.ScheduleCharts.Exists(x => x.Day == (int)importData.StartDate.DayOfWeek))
                                        {
                                            // check if chart time has no conflicts inside the charts array
                                            var selectedDayFromImport = range.ScheduleCharts.Find(x => x.Day == (int)importData.StartDate.DayOfWeek);

                                            var checkChart1 = selectedDayFromImport.Charts.Exists(x =>
                                                DateTime.Parse(importData.StartTime).TimeOfDay >= DateTime.Parse(x.StartTime).TimeOfDay
                                                & DateTime.Parse(importData.StartTime).TimeOfDay <= DateTime.Parse(x.EndTime).TimeOfDay
                                                );

                                            var checkChart2 = selectedDayFromImport.Charts.Exists(x =>
                                                DateTime.Parse(importData.EndTime).TimeOfDay >= DateTime.Parse(x.StartTime).TimeOfDay
                                                & DateTime.Parse(importData.EndTime).TimeOfDay <= DateTime.Parse(x.EndTime).TimeOfDay
                                                );


                                            if (!checkChart1
                                               ||
                                               !checkChart2)
                                            {
                                                // create chart if it does not exists
                                                var chart = new ScheduleChart();
                                                chart.StartTime = importData.StartTime;
                                                chart.EndTime = importData.EndTime;
                                                chart.SchedulingCodeId = importData.SchedulingCodeId;

                                                selectedDayFromImport.Charts.Add(chart);

                                                // success
                                                success.Add($"'1 Agent Schedule data updated.");
                                                importSuccess = importSuccess + 1;

                                            } else if ((checkChart1 & checkChart2) & (importData.StartDate != importData.EndDate))
                                            {
                                                    // create chart if it does not exists
                                                    var chart = new ScheduleChart();
                                                    chart.StartTime = importData.StartTime;
                                                    chart.EndTime = importData.EndTime;
                                                    chart.SchedulingCodeId = importData.SchedulingCodeId;

                                                    selectedDayFromImport.Charts.Add(chart);

                                                    // success
                                                    success.Add($"'1 Agent Schedule data updated.");
                                                    importSuccess = importSuccess + 1;                                               
                                            }
                                        else
                                        {
                                            //display error
                                            errors.Add($"Employee Id {range.EmployeeId} Schedule Chart conflicts on time {importData.StartTime} - {importData.EndTime} with Date {importData.StartDate} - {importData.EndDate}");
                                        }

                                        }
                                        else
                                        {
                                            // create new Chart day if it does not exists
                                            //insert new range in pre-update models
                                            var chart = new ScheduleChart();
                                            chart.StartTime = importData.StartTime;
                                            chart.EndTime = importData.EndTime;
                                            chart.SchedulingCodeId = importData.SchedulingCodeId;

                                            var agentScheduleChart = new AgentScheduleChart();
                                            agentScheduleChart.Charts.Add(chart);
                                            agentScheduleChart.Day = (int)importData.StartDate.DayOfWeek;

                                            range.ScheduleCharts.Add(agentScheduleChart);

                                            // success
                                            success.Add($"'1 Agent Schedule data updated.");
                                            importSuccess = importSuccess + 1;

                                        }
                                    }
                                }
                                else
                                {
                                    var chart = new ScheduleChart();
                                    chart.StartTime = importData.StartTime;
                                    chart.EndTime = importData.EndTime;
                                    chart.SchedulingCodeId = importData.SchedulingCodeId;

                                    var agentScheduleChart = new AgentScheduleChart();
                                    agentScheduleChart.Charts.Add(chart);
                                    agentScheduleChart.Day = (int)importData.StartDate.DayOfWeek;

                                    weekRange.ScheduleCharts.Add(agentScheduleChart);

                                    importRanges.Add(weekRange);

                                    // success
                                    success.Add($"'1 Agent Schedule data updated.");
                                    importSuccess = importSuccess + 1;

                                }
                            }
                        }
                        else
                        {
                            errors.Add($"Date Range {weekRange.DateFrom.ToString("yyyy-MM-dd")} to {weekRange.DateTo.ToString("yyyy-MM-dd")} with {weekRange.EmployeeId} is already approved.");
                        }
                    }

                }

            }

            foreach(var rangeImport in importRanges)
            {

                var employeeIdDetails = new EmployeeIdDetails { Id = rangeImport.EmployeeId };

                var range = new AgentScheduleRange();
                range.AgentSchedulingGroupId = rangeImport.AgentSchedulingGroupId;

                range.DateFrom = new DateTime(rangeImport.DateFrom.Year, rangeImport.DateFrom.Month, rangeImport.DateFrom.Day,
                                         0, 0, 0, DateTimeKind.Utc);
                range.DateTo = new DateTime(rangeImport.DateTo.Year, rangeImport.DateTo.Month, rangeImport.DateTo.Day,
                                         0, 0, 0, DateTimeKind.Utc);

                range.ScheduleCharts = rangeImport.ScheduleCharts;
                range.CreatedBy = agentScheduleImport.ModifiedBy;
                range.CreatedDate = DateTimeOffset.UtcNow;
                range.Status = rangeImport.Status;

                var activityLog = GetActivityLogForSchedulingChart(range, employeeIdDetails.Id,
                                                                   agentScheduleImport.ModifiedBy, employeeIdDetails.Id,
                                                                   agentScheduleImport.ActivityOrigin);
                activityLogs.Add(activityLog);

                _agentScheduleRepository.CopyAgentSchedules(employeeIdDetails, range);

            }

            _activityLogRepository.CreateActivityLogs(activityLogs);

            await _uow.Commit();

            string importedDataCount;
            importedDataCount = $"Successfully imported {importSuccess.ToString()} out of {importCount.ToString()} Schedule Data Rows.";

            importAgentScheduleResponse.Errors = errors;
            importAgentScheduleResponse.Success = success;
            importAgentScheduleResponse.ImportStatus = importedDataCount;

            return new CSSResponse(importAgentScheduleResponse, HttpStatusCode.OK);
        }


        private DateTime GetMyDate(int x, DateTime dateTime, string timeSpan)
        {
            DateTime result = dateTime.AddDays(x) + DateTime.Parse(timeSpan).TimeOfDay;
            return result;
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

            agentScheduleDetails.DateFrom = new DateTime(agentScheduleDetails.DateFrom.Year, agentScheduleDetails.DateFrom.Month, 
                                                         agentScheduleDetails.DateFrom.Day, 0, 0, 0, DateTimeKind.Utc);
            agentScheduleDetails.DateTo = new DateTime(agentScheduleDetails.DateTo.Year, agentScheduleDetails.DateTo.Month, 
                                                       agentScheduleDetails.DateTo.Day, 0, 0, 0, DateTimeKind.Utc);

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
        public async Task<CSSResponse> AgentSchedulingGroupScheduleExport(int agentSchedulingGroupId)
        {
            var agentSchedule = await _agentScheduleRepository.GetAgentSchedulingGroupExport(new AgentSchedulingGroupIdDetails { AgentSchedulingGroupId = agentSchedulingGroupId });
            if (agentSchedule == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var matchSchedulingId = await _schedulingCodeRepository.FindSchedulingCodes();

            var schedCode = (from s in matchSchedulingId
                             select new MatchSchedulingCode
                             {
                                 SchedulingCodeId = s.SchedulingCodeId,
                                 Name = s.Name
                             }
                             ).ToList();
            schedCode.Where(sc => sc.SchedulingCodeId == 23).Select(r => r.Name);

            var msg = new List<object>();

            var items = (from i in agentSchedule
                         from r in i.Ranges
                        
                         from s in r.ScheduleCharts
                         let start_date = r.DateFrom.AddDays(s.Day)

                         let end_date = r.DateFrom.AddDays(s.Day)
                         let check = s.Charts.Zip(s.Charts.Skip(0), (x, y) => y.StartTime == x.EndTime && y.SchedulingCodeId == x.SchedulingCodeId)
                         from c in s.Charts  
                         //end_date.ToString("yyyyMMdd"),
                         select new ExportAgentSchedulingGroupSchedule()
                         {
                             EmployeeId = i.EmployeeId,
                             StartDate = start_date.ToString("yyyyMMdd"),
                             EndDate = end_date.ToString("yyyyMMdd"),
                                       
                             ActivityCode = schedCode
                                            .Where(x => x.SchedulingCodeId == c.SchedulingCodeId)
                                            .Select(x => x.Name).SingleOrDefault(),
                             StartTime = c.StartTime,
                             EndTime = c.EndTime
                         }
                        
                         ).ToList().ToArray();
            //var Items = items.SkipWhile(x => x.EndDate == x.StartTime & x.ActivityCode == x.ActivityCode).Take(1).ToList();


            //var items2 = items.Zip(items.Skip(1), (x, y) => y.StartTime == x.EndTime && y.ActivityCode == x.ActivityCode);

       
            var items2 = items.Select((x, index) => new ExportAgentSchedulingGroupSchedule
                {
                    
                    EmployeeId = x.EmployeeId,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    ActivityCode = x.ActivityCode,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime

                });

          
                //msg.Add(items2);

            for (int i = 0; i < items.Length; i++)
            {
                if (i < items.Length - 1)
                {

                    var convertStartDate = DateTime.ParseExact(items[i + 1].StartDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                    var convertEndDate = DateTime.ParseExact(items[i].EndDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                    DateTime nextday = DateTime.Parse(convertStartDate);
                    DateTime current = DateTime.Parse(convertEndDate);
                    DateTime nextDay2 = nextday.AddDays(-1);
                    if (items[i].EndTime.Equals(items[i + 1].StartTime) &&
                        items[i].ActivityCode.Equals(items[i + 1].ActivityCode) &&
                        current == nextDay2 &&
                        items[i].EmployeeId == items[i + 1].EmployeeId)
                    {
                        var object1 = new ExportAgentSchedulingGroupSchedule
                        {
                            EmployeeId = items[i].EmployeeId,
                            StartDate = items[i].StartDate,
                            EndDate = items[i + 1].EndDate,
                            ActivityCode = items[i].ActivityCode,
                            StartTime = items[i].StartTime,
                            EndTime = items[i + 1].EndTime
                        };
                        msg.Add(object1);
                        i++;
                    } else if (items[i].EndTime == "12:00 AM" || items[i].EndTime == "12:00 am")
                    {
                       var date_plus_one = DateTime.ParseExact(items[i].EndDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                       DateTime date_increment_one = date_plus_one.AddDays(1);
                        var object2 = new ExportAgentSchedulingGroupSchedule
                        {
                            EmployeeId = items[i].EmployeeId,
                            StartDate = items[i].StartDate,
                            EndDate = date_increment_one.ToString("yyyyMMdd"),
                            ActivityCode = items[i].ActivityCode,
                            StartTime = items[i].StartTime,
                            EndTime = items[i].EndTime
                        };
                        msg.Add(object2);
                        i++;
                    }
                    else
                    {
                        msg.Add(items[i]);
                    }
                }
            }
            return new CSSResponse(msg, HttpStatusCode.OK);
        }

        public async Task<CSSResponse> EmployeeScheduleExport(int employeeId)
        {
            var agentSchedule = await _agentScheduleRepository.GetEmployeeScheduleExport(employeeId);
            if (agentSchedule == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var matchSchedulingId = await _schedulingCodeRepository.FindSchedulingCodes();

            var schedCode = (from s in matchSchedulingId
                             select new MatchSchedulingCode
                             {
                                 SchedulingCodeId = s.SchedulingCodeId,
                                 Name = s.Name
                             }
                             ).ToList();
            schedCode.Where(sc => sc.SchedulingCodeId == 23).Select(r => r.Name);

            var msg = new List<object>();

            var items = (from i in agentSchedule
                         from r in i.Ranges

                         from s in r.ScheduleCharts
                         let start_date = r.DateFrom.AddDays(s.Day)

                         let end_date = r.DateFrom.AddDays(s.Day)
                         let check = s.Charts.Zip(s.Charts.Skip(0), (x, y) => y.StartTime == x.EndTime && y.SchedulingCodeId == x.SchedulingCodeId)
                         from c in s.Charts
                             //end_date.ToString("yyyyMMdd"),
                         select new ExportAgentSchedulingGroupSchedule()
                         {
                             EmployeeId = i.EmployeeId,
                             StartDate = start_date.ToString("yyyyMMdd"),
                             EndDate = end_date.ToString("yyyyMMdd"),

                             ActivityCode = schedCode
                                            .Where(x => x.SchedulingCodeId == c.SchedulingCodeId)
                                            .Select(x => x.Name).SingleOrDefault(),
                             StartTime = c.StartTime,
                             EndTime = c.EndTime
                         }

                         ).ToList().ToArray();
            //var Items = items.SkipWhile(x => x.EndDate == x.StartTime & x.ActivityCode == x.ActivityCode).Take(1).ToList();


            //var items2 = items.Zip(items.Skip(1), (x, y) => y.StartTime == x.EndTime && y.ActivityCode == x.ActivityCode);


            var items2 = items.Select((x, index) => new ExportAgentSchedulingGroupSchedule
            {

                EmployeeId = x.EmployeeId,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ActivityCode = x.ActivityCode,
                StartTime = x.StartTime,
                EndTime = x.EndTime

            });


            //msg.Add(items2);

            for (int i = 0; i < items.Length; i++)
            {
                if (i < items.Length - 1)
                {

                    var convertStartDate = DateTime.ParseExact(items[i + 1].StartDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                    var convertEndDate = DateTime.ParseExact(items[i].EndDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                    DateTime nextday = DateTime.Parse(convertStartDate);
                    DateTime current = DateTime.Parse(convertEndDate);
                    DateTime nextDay2 = nextday.AddDays(-1);
                    if (items[i].EndTime.Equals(items[i + 1].StartTime) &&
                        items[i].ActivityCode.Equals(items[i + 1].ActivityCode) &&
                        current == nextDay2 &&
                        items[i].EmployeeId == items[i + 1].EmployeeId)
                    {
                        var object1 = new ExportAgentSchedulingGroupSchedule
                        {
                            EmployeeId = items[i].EmployeeId,
                            StartDate = items[i].StartDate,
                            EndDate = items[i + 1].EndDate,
                            ActivityCode = items[i].ActivityCode,
                            StartTime = items[i].StartTime,
                            EndTime = items[i + 1].EndTime
                        };
                        msg.Add(object1);
                        i++;
                    }
                    else if (items[i].EndTime == "12:00 AM" || items[i].EndTime == "12:00 am")
                    {
                        var date_plus_one = DateTime.ParseExact(items[i].EndDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                        DateTime date_increment_one = date_plus_one.AddDays(1);
                        var object2 = new ExportAgentSchedulingGroupSchedule
                        {
                            EmployeeId = items[i].EmployeeId,
                            StartDate = items[i].StartDate,
                            EndDate = date_increment_one.ToString("yyyyMMdd"),
                            ActivityCode = items[i].ActivityCode,
                            StartTime = items[i].StartTime,
                            EndTime = items[i].EndTime
                        };
                        msg.Add(object2);
                        i++;
                    }
                    else
                    {
                        msg.Add(items[i]);
                    }
                }
            }
            return new CSSResponse(msg, HttpStatusCode.OK);
        }
    }
}
    