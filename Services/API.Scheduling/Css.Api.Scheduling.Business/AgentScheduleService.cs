using AutoMapper;
using Css.Api.Core.DataAccess.Repository.UnitOfWork.Interfaces;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Core.Models.DTO.Response;
using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedule;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedulingGroup;
using Css.Api.Scheduling.Models.DTO.Request.MySchedule;
using Css.Api.Scheduling.Models.DTO.Response.AgentSchedule;
using Css.Api.Scheduling.Models.DTO.Response.MySchedule;
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
        /// <param name="schedulingCodeRepository">The scheduling code repository.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="uow">The uow.</param>
        public AgentScheduleService(
            IHttpContextAccessor httpContextAccessor,
            IActivityLogRepository activityLogRepository,
            IAgentScheduleRepository agentScheduleRepository,
            ISchedulingCodeRepository schedulingCodeRepository,
            IMapper mapper,
            IUnitOfWork uow)
        {
            _httpContextAccessor = httpContextAccessor;
            _activityLogRepository = activityLogRepository;
            _agentScheduleRepository = agentScheduleRepository;
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

            if (agentScheduleChartQueryparameter.AgentScheduleType == AgentScheduleType.SchedulingTab)
            {
                if (agentScheduleChartQueryparameter.Day.HasValue && agentScheduleChartQueryparameter.Day != default(int))
                {
                    agentSchedule.AgentScheduleCharts = agentSchedule.AgentScheduleCharts.FindAll(x => x.Day == agentScheduleChartQueryparameter.Day);
                }

                var mappedAgentScheduleChart = _mapper.Map<AgentScheduleChartDetailsDTO>(agentSchedule);
                return new CSSResponse(mappedAgentScheduleChart, HttpStatusCode.OK);
            }
            else
            {
                if (agentScheduleChartQueryparameter.Date.HasValue && agentScheduleChartQueryparameter.Date != default(DateTimeOffset))
                {
                    var dateTimeWithZeroTimeSpan = new DateTimeOffset(agentScheduleChartQueryparameter.Date.Value.Date, TimeSpan.Zero);
                    agentSchedule.AgentScheduleManagerCharts = agentSchedule.AgentScheduleManagerCharts.FindAll(x => x.Date == dateTimeWithZeroTimeSpan);
                }

                var mappedAgentScheduleChart = _mapper.Map<AgentScheduleManagerChartDetailsDTO>(agentSchedule);
                return new CSSResponse(mappedAgentScheduleChart, HttpStatusCode.OK);
            }
        }

        /// <summary>
        /// Updates the Agent Admin.
        /// </summary>
        /// <param name="agentScheduleIdDetails"></param>
        /// <param name="agentScheduleDetails"></param>
        /// <returns></returns>
        public async Task<CSSResponse> UpdateAgentSchedule(AgentScheduleIdDetails agentScheduleIdDetails, UpdateAgentSchedule agentScheduleDetails)
        {
            var agentScheduleCount = await _agentScheduleRepository.GetAgentScheduleCount(agentScheduleIdDetails);
            if (agentScheduleCount < 1)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            _agentScheduleRepository.UpdateAgentSchedule(agentScheduleIdDetails, agentScheduleDetails);

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
            var agentScheduleCount = await _agentScheduleRepository.GetAgentScheduleCount(agentScheduleIdDetails);
            if (agentScheduleCount < 1)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var hasValidCodes = await HasValidSchedulingCodes(agentScheduleDetails);
            if (!hasValidCodes)
            {
                return new CSSResponse("One of the scheduling code does not exists", HttpStatusCode.NotFound);
            }

            _agentScheduleRepository.UpdateAgentScheduleChart(agentScheduleIdDetails, agentScheduleDetails);

            var employeeId = await _agentScheduleRepository.GetEmployeeIdByAgentScheduleId(agentScheduleIdDetails);

            var activityLog = GetActivityLogForSchedulingChart(agentScheduleDetails.AgentScheduleCharts, employeeId, agentScheduleDetails.ModifiedBy, 
                                                               agentScheduleDetails.ActivityOrigin);

            _activityLogRepository.CreateActivityLog(activityLog);

            await _uow.Commit();

            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Updates the agent schedule manger chart.
        /// </summary>
        /// <param name="agentScheduleManagerChartDetails">The agent schedule manager chart details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> UpdateAgentScheduleMangerChart(UpdateAgentScheduleManagerChart agentScheduleManagerChartDetails)
        {
            var hasValidCodes = await HasValidSchedulingCodes(agentScheduleManagerChartDetails);
            if (!hasValidCodes)
            {
                return new CSSResponse("One of the scheduling code does not exists", HttpStatusCode.NotFound);
            }

            foreach (var agentScheduleManager in agentScheduleManagerChartDetails.AgentScheduleManagers)
            {
                var employeeIdDetails = new EmployeeIdDetails { Id = agentScheduleManager.EmployeeId };
                var modifiedUserDetails = new ModifiedUserDetails { ModifiedBy = agentScheduleManagerChartDetails.ModifiedBy };
                _agentScheduleRepository.UpdateAgentScheduleMangerChart(employeeIdDetails, agentScheduleManager.AgentScheduleManagerChart, modifiedUserDetails);
            }

            var activityLogs = new List<ActivityLog>();

            foreach (var agentScheduleManagerChartDetail in agentScheduleManagerChartDetails.AgentScheduleManagers)
            {
                var activityLog = GetActivityLogForSchedulingChart(agentScheduleManagerChartDetail.AgentScheduleManagerChart, agentScheduleManagerChartDetail.EmployeeId,
                                                                   agentScheduleManagerChartDetails.ModifiedBy, agentScheduleManagerChartDetails.ActivityOrigin);
                activityLogs.Add(activityLog);
            }

            _activityLogRepository.CreateActivityLogs(activityLogs);

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
                var modifiedUserDetails = new ModifiedUserDetails { ModifiedBy = agentScheduleDetails.ModifiedBy };
                _agentScheduleRepository.ImportAgentScheduleChart(importAgentScheduleChart, modifiedUserDetails);

                var activityLog = GetActivityLogForSchedulingChart(importAgentScheduleChart.AgentScheduleCharts, importAgentScheduleChart.EmployeeId,
                                                                   agentScheduleDetails.ModifiedBy, agentScheduleDetails.ActivityOrigin);
                activityLogs.Add(activityLog);
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
        public async Task<CSSResponse> CopyAgentSchedule(AgentScheduleIdDetails agentScheduleIdDetails, CopyAgentSchedule agentScheduleDetails)
        {
            var agentSchedule = await _agentScheduleRepository.GetAgentSchedule(agentScheduleIdDetails);
            if (agentSchedule == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            _agentScheduleRepository.CopyAgentSchedules(agentSchedule, agentScheduleDetails);

            if (!agentScheduleDetails.EmployeeIds.Any())
            {
                AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails = new AgentSchedulingGroupIdDetails { AgentSchedulingGroupId = agentScheduleDetails.AgentSchedulingGroupId };
                agentScheduleDetails.EmployeeIds = await _agentScheduleRepository.GetEmployeeIdsByAgentScheduleGroupId(agentSchedulingGroupIdDetails);
                agentScheduleDetails.EmployeeIds = agentScheduleDetails.EmployeeIds.FindAll(x => x != agentSchedule.EmployeeId);
            }

            var activityLogs = new List<ActivityLog>();

            foreach (var employeeId in agentScheduleDetails.EmployeeIds)
            {
                if (agentScheduleDetails.AgentScheduleType == AgentScheduleType.SchedulingTab)
                {
                    var activityLog = GetActivityLogForSchedulingChart(agentSchedule.AgentScheduleCharts, employeeId, agentScheduleDetails.ModifiedBy,
                                                                       agentScheduleDetails.ActivityOrigin);
                    activityLogs.Add(activityLog);
                }
                else if (agentScheduleDetails.AgentScheduleType == AgentScheduleType.SchedulingMangerTab)
                {
                    var activityLog = GetActivityLogForSchedulingChart(agentSchedule.AgentScheduleManagerCharts, employeeId, agentScheduleDetails.ModifiedBy,
                                                                       agentScheduleDetails.ActivityOrigin);
                    activityLogs.Add(activityLog);
                }
            }

            _activityLogRepository.CreateActivityLogs(activityLogs);

            await _uow.Commit();

            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>Gets the agent my schedule.</summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <param name="myScheduleQueryParameter">My schedule query parameter.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<CSSResponse> GetAgentMySchedule(EmployeeIdDetails employeeIdDetails, MyScheduleQueryParameter myScheduleQueryParameter)
        {
            AgentMyScheduleDetailsDTO agentMyScheduleDetailsDTO;

            var agentSchedule = await _agentScheduleRepository.GetAgentScheduleByEmployeeId(employeeIdDetails);
            if (agentSchedule == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            if (agentSchedule.DateFrom == null || agentSchedule.DateTo == null
                || agentSchedule.AgentScheduleCharts == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            agentMyScheduleDetailsDTO = new AgentMyScheduleDetailsDTO();
            agentMyScheduleDetailsDTO.Id = agentSchedule.Id.ToString();
            agentMyScheduleDetailsDTO.AgentMySchedules = new List<AgentMyScheduleDay>();

            AgentMyScheduleDay schedule;

            foreach (DateTime date in EachDay(myScheduleQueryParameter.StartDate, myScheduleQueryParameter.EndDate))
            {
                if (date.Date >= agentSchedule.DateFrom.Value.Date
                    && date.Date.AddDays(1) <= agentSchedule.DateTo.Value.Date.AddDays(1))
                {
                    bool isChartAvailableForDay =
                        agentSchedule.AgentScheduleCharts.Any(chart => chart.Day == (int)date.DayOfWeek);
                    if (isChartAvailableForDay)
                    {
                        var chartsOfDay = agentSchedule.AgentScheduleCharts
                            .Where(chart => chart.Day == (int)date.DayOfWeek)
                            .Select(chart => chart.Charts)
                            .FirstOrDefault();

                        var firstStartTime = chartsOfDay.Min(chart => DateTime.
                        ParseExact(chart.StartTime, "hh:mm tt", CultureInfo.InvariantCulture)).ToString("hh:mm tt");
                        var lastEndTime = chartsOfDay.Max(chart => DateTime.
                        ParseExact(chart.EndTime, "hh:mm tt", CultureInfo.InvariantCulture)).ToString("hh:mm tt");

                        schedule = new AgentMyScheduleDay
                        {
                            Day = (int)date.DayOfWeek,
                            Date = date,
                            Charts = chartsOfDay,
                            FirstStartTime = firstStartTime,
                            LastEndTime = lastEndTime
                        };
                    }
                    else
                    {
                        schedule = CreateMyScheduleDayWithNoChart(date);
                    }
                }
                else
                {
                    schedule = CreateMyScheduleDayWithNoChart(date);

                }
                agentMyScheduleDetailsDTO.AgentMySchedules.Add(schedule);
            }

            return new CSSResponse(agentMyScheduleDetailsDTO, HttpStatusCode.OK);
        }

        /// <summary>
        /// Gets the activity log for scheduling chart.
        /// </summary>
        /// <param name="scheduleCharts">The schedule charts.</param>
        /// <param name="employeeId">The employee identifier.</param>
        /// <param name="executedBy">The executed by.</param>
        /// <param name="activityOrigin">The activity origin.</param>
        /// <returns></returns>
        private ActivityLog GetActivityLogForSchedulingChart(object scheduleCharts, int employeeId, string executedBy, ActivityOrigin activityOrigin)
        {
            var activityLog = new ActivityLog()
            {
                EmployeeId = employeeId,
                ExecutedBy = executedBy,
                TimeStamp = DateTimeOffset.UtcNow,
                ActivityOrigin = activityOrigin,
                ActivityStatus = ActivityStatus.Updated,
                SchedulingFieldDetails = new SchedulingFieldDetails()
            };

            if (scheduleCharts is List<AgentScheduleChart>)
            {
                var charts = scheduleCharts as List<AgentScheduleChart>;
                activityLog.ActivityType = ActivityType.SchedulingGrid;
                activityLog.SchedulingFieldDetails.AgentScheduleCharts = charts;
            }
            else if (scheduleCharts is List<AgentScheduleManagerChart>)
            {
                var charts = scheduleCharts as List<AgentScheduleManagerChart>;
                activityLog.ActivityType = ActivityType.SchedulingmanagerGrid;
                activityLog.SchedulingFieldDetails.AgentScheduleManagerCharts = charts;
            }
            else if (scheduleCharts is AgentScheduleManagerChart)
            {
                var chart = scheduleCharts as AgentScheduleManagerChart;
                activityLog.ActivityType = ActivityType.SchedulingmanagerGrid;
                activityLog.SchedulingFieldDetails.AgentScheduleManagerCharts = new List<AgentScheduleManagerChart>
                {
                    chart
                };
            }

            return activityLog;
        }

        /// <summary>
        /// Eaches the day.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        private IEnumerable<DateTime> EachDay(DateTime from, DateTime to)
        {
            for (var day = from.Date; day.Date <= to.Date; day = day.AddDays(1))
                yield return day;
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
            else if (agentScheduleDetails is UpdateAgentScheduleManagerChart)
            {
                var details = agentScheduleDetails as UpdateAgentScheduleManagerChart;
                foreach (var agentScheduleManager in details.AgentScheduleManagers)
                {
                    var scheduleManagerCodes = agentScheduleManager.AgentScheduleManagerChart.Charts.Select(x => x.SchedulingCodeId).ToList().ToList();
                    codes.AddRange(scheduleManagerCodes);
                }
            }
            else if (agentScheduleDetails is ImportAgentSchedule)
            {

                var details = agentScheduleDetails as ImportAgentSchedule;
                foreach (var importAgentScheduleChart in details.ImportAgentScheduleCharts)
                {
                    foreach (var agentScheduleChart in importAgentScheduleChart.AgentScheduleCharts)
                    {
                        var scheduleCodes = agentScheduleChart.Charts.Select(x => x.SchedulingCodeId).ToList();
                        codes.AddRange(scheduleCodes);
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

        /// <summary>Creates my schedule day with no chart.</summary>
        /// <param name="date">The date.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        private AgentMyScheduleDay CreateMyScheduleDayWithNoChart(DateTime date)
        {
            return new AgentMyScheduleDay
            {
                Day = (int)date.DayOfWeek,
                Date = date
            };
        }
    }
}