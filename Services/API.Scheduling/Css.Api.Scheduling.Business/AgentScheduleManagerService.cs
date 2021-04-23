using AutoMapper;
using Css.Api.Core.DataAccess.Repository.UnitOfWork.Interfaces;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Core.Models.DTO.Response;
using Css.Api.Core.Models.Enums;
using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedule;
using Css.Api.Scheduling.Models.DTO.Request.AgentScheduleManager;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedulingGroup;
using Css.Api.Scheduling.Models.DTO.Request.MySchedule;
using Css.Api.Scheduling.Models.DTO.Request.SkillGroup;
using Css.Api.Scheduling.Models.DTO.Response.AgentAdmin;
using Css.Api.Scheduling.Models.DTO.Response.AgentScheduleManager;
using Css.Api.Scheduling.Models.DTO.Response.MySchedule;
using Css.Api.Scheduling.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Business
{
    /// <summary>
    /// Service for agent admin part
    /// </summary>
    /// <seealso cref="Css.Api.Setup.Business.Interfaces.AgentScheduleManagerService" />
    public class AgentScheduleManagerService : IAgentScheduleManagerService
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
        private readonly IAgentScheduleManagerRepository _agentScheduleManagerRepository;

        /// <summary>
        /// The agent schedule group repository
        /// </summary>
        private readonly IAgentSchedulingGroupRepository _agentSchedulingGroupRepository;

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
        /// <param name="agentScheduleManagerRepository">The agent schedule manager repository.</param>
        /// <param name="agentAdminRepository">The agent admin repository.</param>
        /// <param name="schedulingCodeRepository">The scheduling code repository.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="uow">The uow.</param>
        public AgentScheduleManagerService(
            IHttpContextAccessor httpContextAccessor,
            IActivityLogRepository activityLogRepository,
            IAgentScheduleManagerRepository agentScheduleManagerRepository,
            IAgentAdminRepository agentAdminRepository,
            ISchedulingCodeRepository schedulingCodeRepository,
            IAgentSchedulingGroupRepository agentSchedulingGroupRepository,
            IMapper mapper,
            IUnitOfWork uow)
        {
            _httpContextAccessor = httpContextAccessor;
            _activityLogRepository = activityLogRepository;
            _agentScheduleManagerRepository = agentScheduleManagerRepository;
            _agentAdminRepository = agentAdminRepository;
            _schedulingCodeRepository = schedulingCodeRepository;
            _agentSchedulingGroupRepository = agentSchedulingGroupRepository;
            _mapper = mapper;
            _uow = uow;
        }

        /// <summary>
        /// Gets the agent schedule manager charts.
        /// </summary>
        /// <param name="agentScheduleManagerChartQueryparameter">The agent schedule manager chart queryparameter.</param>
        /// <returns></returns>
        public async Task<CSSResponse> GetAgentScheduleManagerCharts(AgentScheduleManagerChartQueryparameter agentScheduleManagerChartQueryparameter)
        {
            //// get all agents first from the selected agent scheduling group
            //// change the skip page size parameter to true for Agent Query
            //// this is to prevent the mismatch of data pages from the agent repository query and schedule manager query
            //// we are fetching ALL the data from the agent repository with the given parameters to loop through them and check with mappedAgentScheduleManagers if they have a schedule
            //agentScheduleManagerChartQueryparameter.SkipPageSize = true;
            //var agents = await GetAgents(agentScheduleManagerChartQueryparameter);
            //var mappedAgents = JsonConvert.DeserializeObject<List<AgentAdminDTO>>(JsonConvert.SerializeObject(agents));


            //var query = agentScheduleManagerChartQueryparameter;
            //query.SkipPageSize = true;

            //var agentScheduleManagers = await _agentScheduleManagerRepository.GetAgentScheduleManagerCharts(query);

            //var mappedAgentScheduleManagers = JsonConvert.DeserializeObject<List<AgentScheduleManagerChartDetailsDTO>>(JsonConvert.SerializeObject(agentScheduleManagers));

            //// loop through the agent admins and check from the schedule manager list if the agent has schedule
            //foreach (var agent in mappedAgents)
            //{
            //    var mappedAgentScheduleManager = mappedAgentScheduleManagers.Find(x => x.EmployeeId == agent.EmployeeId);

            //    if (mappedAgentScheduleManager == null)
            //    {
            //        // make a schedule placeholder if it doesn't exist
            //        var scheduleManagerExists = await _agentScheduleManagerRepository.HasAgentScheduleManagerChartByEmployeeId(new EmployeeIdDetails { Id = agent.EmployeeId });
            //        if (!scheduleManagerExists || !agentScheduleManagerChartQueryparameter.ExcludeConflictSchedule)
            //        {
            //            var agentScheduleManager = new AgentScheduleManagerChartDetailsDTO
            //            {
            //                EmployeeId = agent.EmployeeId,
            //                FirstName = agent.FirstName,
            //                LastName = agent.LastName,
            //                AgentSchedulingGroupId = agent.AgentSchedulingGroupId,
            //                ChartsCount = 0,
            //            };
            //            mappedAgentScheduleManagers.Add(agentScheduleManager);
            //        }
            //    }
            //    else
            //    {
            //        // just map the name if it has a schedule
            //        mappedAgentScheduleManager.FirstName = agent?.FirstName;
            //        mappedAgentScheduleManager.LastName = agent?.LastName;
            //        mappedAgentScheduleManager.ChartsCount = mappedAgentScheduleManager.Charts.Count;
            //    }
            //}

            // var schedules = mappedAgentScheduleManagers.AsQueryable().Where(x => x.FirstName != null && x.LastName != null)
            //    .OrderByDescending(x => x.ChartsCount)
            //    .Skip((agentScheduleManagerChartQueryparameter.PageNumber - 1) * agentScheduleManagerChartQueryparameter.PageSize)
            //    .Take(agentScheduleManagerChartQueryparameter.PageSize).ToList();

            var agentScheduleManagers = await _agentScheduleManagerRepository.GetAgentScheduleManagerCharts(agentScheduleManagerChartQueryparameter);

            _httpContextAccessor.HttpContext.Response.Headers.Add("X-Pagination", PagedList<Entity>.ToJson(agentScheduleManagers));

            return new CSSResponse(agentScheduleManagers, HttpStatusCode.OK);
        }


        /// <summary>Gets the agent my schedule.</summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <param name="myScheduleQueryParameter">My schedule query parameter.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<CSSResponse> GetAgentMySchedule(EmployeeIdDetails employeeIdDetails, MyScheduleQueryParameter myScheduleQueryParameter)
        {
            // Gets agent scheduling group Id, if the AgentSchedulingGroupId is not specified
            if (myScheduleQueryParameter.AgentSchedulingGroupId == 0)
            {
                var agentAdmin = await _agentAdminRepository.GetAgentAdminByEmployeeId(employeeIdDetails);
                myScheduleQueryParameter.AgentSchedulingGroupId = agentAdmin.AgentSchedulingGroupId;
            }

            var agentSchedules = await _agentScheduleManagerRepository.GetAgentScheduleManagerChartByEmployeeId(employeeIdDetails, myScheduleQueryParameter);
            //if (agentSchedules == null || agentSchedules.Count < 1)
            //{
            //    return new CSSResponse(HttpStatusCode.NotFound);
            //}

            var agentMyScheduleDetailsDTO = new AgentMyScheduleDetailsDTO
            {
                AgentMySchedules = new List<AgentMyScheduleDay>()
            };

            AgentMyScheduleDay schedule;

            foreach (DateTime date in EachDay(myScheduleQueryParameter.StartDate, myScheduleQueryParameter.EndDate))
            {
                AgentScheduleManager agentSchedule = agentSchedules.Where(s => s.Date == date).FirstOrDefault();
                if (agentSchedule != null)
                {

                    bool isChartAvailableForDay =
                        agentSchedule.Charts.Any();
                    if (isChartAvailableForDay)
                    {
                        var chartsOfDay = agentSchedule.Charts;

                        //var firstStartTime = chartsOfDay.Min(chart => DateTime.
                        //ParseExact(chart.StartDateTime.ToString(), "hh:mm tt", CultureInfo.InvariantCulture)).ToString("hh:mm tt");
                        //var lastEndTime = chartsOfDay.Max(chart => DateTime.
                        //ParseExact(chart.EndDateTime.ToString(), "hh:mm tt", CultureInfo.InvariantCulture)).ToString("hh:mm tt");

                        var firstStartTime = chartsOfDay.Min(chart => chart.StartDateTime);
                        var lastEndTime = chartsOfDay.Max(chart => chart.EndDateTime);

                        schedule = new AgentMyScheduleDay
                        {
                            Day = (int)agentSchedule.Date.DayOfWeek,
                            Date = agentSchedule.Date,
                            Charts = chartsOfDay,
                            FirstStartTime = firstStartTime.ToString(),
                            LastEndTime = lastEndTime.ToString()
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
        /// Updates the agent schedule manger chart.
        /// </summary>
        /// <param name="agentScheduleDetails">The agent schedule manager details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> UpdateAgentScheduleMangerChart(UpdateAgentScheduleManager agentScheduleDetails)
        {
            var hasValidCodes = await HasValidSchedulingCodes(agentScheduleDetails);
            if (!hasValidCodes)
            {
                return new CSSResponse("One of the scheduling code does not exists", HttpStatusCode.NotFound);
            }

            var activityLogs = new List<ActivityLog>();

            foreach (var scheduleManager in agentScheduleDetails.ScheduleManagers)
            {
                var employeeIdDetails = new EmployeeIdDetails { Id = scheduleManager.EmployeeId };

                foreach (var agentScheduleManagerChart in scheduleManager.AgentScheduleManagerCharts)
                {
                    var dateDetails = new DateDetails { Date = agentScheduleManagerChart.Date };
                    var modifiedUserDetails = new ModifiedUserDetails { ModifiedBy = agentScheduleDetails.ModifiedBy };

                    agentScheduleManagerChart.Date = new DateTime(agentScheduleManagerChart.Date.Year, agentScheduleManagerChart.Date.Month,
                                                                  agentScheduleManagerChart.Date.Day, 0, 0, 0, DateTimeKind.Utc);
                    if (agentScheduleDetails.IsImport)
                    {
                        var scheduleExists = await _agentScheduleManagerRepository.IsAgentScheduleManagerChartExists(employeeIdDetails, dateDetails);
                        if (scheduleExists)
                        {
                            continue;
                        }
                    }

                    var agentAdmin = await _agentAdminRepository.GetAgentAdminByEmployeeId(employeeIdDetails);
                    if (agentAdmin != null)
                    {
                        var agentScheduleManager = new AgentScheduleManager
                        {
                            EmployeeId = agentAdmin.Ssn,
                            AgentSchedulingGroupId = agentAdmin.AgentSchedulingGroupId,
                            Date = agentScheduleManagerChart.Date,
                            Charts = agentScheduleManagerChart.Charts,
                            CreatedBy = modifiedUserDetails.ModifiedBy,
                            CreatedDate = DateTimeOffset.UtcNow
                        };

                        _agentScheduleManagerRepository.UpdateAgentScheduleMangerChart(employeeIdDetails, agentScheduleManager);

                        var activityLog = GetActivityLogForSchedulingManager(agentScheduleManager, employeeIdDetails.Id,
                                                                             agentScheduleDetails.ModifiedBy, agentScheduleDetails.ModifiedUser,
                                                                             agentScheduleDetails.ActivityOrigin);
                        activityLogs.Add(activityLog);
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
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <param name="agentScheduleDetails">The agent schedule details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> CopyAgentScheduleManagerChart(EmployeeIdDetails employeeIdDetails, CopyAgentScheduleManager agentScheduleDetails)
        {
            var dateDetails = new DateDetails { Date = agentScheduleDetails.Date };
            var agentScheduleManager = await _agentScheduleManagerRepository.GetAgentScheduleManagerChart(employeeIdDetails, dateDetails);
            if (agentScheduleManager == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            if (!agentScheduleDetails.EmployeeIds.Any())
            {
                AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails = new AgentSchedulingGroupIdDetails { AgentSchedulingGroupId = agentScheduleDetails.AgentSchedulingGroupId };
                agentScheduleDetails.EmployeeIds = await _agentAdminRepository.GetEmployeeIdsByAgentSchedulingGroup(agentSchedulingGroupIdDetails);
                agentScheduleDetails.EmployeeIds = agentScheduleDetails.EmployeeIds.FindAll(x => x != agentScheduleManager.EmployeeId);
            }

            var activityLogs = new List<ActivityLog>();

            agentScheduleDetails.Date = new DateTime(agentScheduleDetails.Date.Year, agentScheduleDetails.Date.Month, agentScheduleDetails.Date.Day,
                                                     0, 0, 0, DateTimeKind.Utc);

            foreach (var employeeId in agentScheduleDetails.EmployeeIds)
            {
                var employeeDetails = new EmployeeIdDetails { Id = employeeId };
                var modifiedUserDetails = new ModifiedUserDetails { ModifiedBy = agentScheduleDetails.ModifiedBy };

                var scheduleExists = await _agentScheduleManagerRepository.IsAgentScheduleManagerChartExists(employeeDetails, dateDetails);
                if (!scheduleExists)
                {
                    var agentAdmin = await _agentAdminRepository.GetAgentAdminByEmployeeId(employeeDetails);
                    if (agentAdmin != null)
                    {
                        var agentScheduleManagerChart = new AgentScheduleManager
                        {
                            EmployeeId = agentAdmin.Ssn,
                            AgentSchedulingGroupId = agentAdmin.AgentSchedulingGroupId,
                            Date = agentScheduleDetails.Date,
                            Charts = agentScheduleManager.Charts,
                            CreatedBy = modifiedUserDetails.ModifiedBy,
                            CreatedDate = DateTimeOffset.UtcNow
                        };

                        _agentScheduleManagerRepository.UpdateAgentScheduleMangerChart(employeeDetails, agentScheduleManagerChart);

                        var activityLog = GetActivityLogForSchedulingManager(agentScheduleManagerChart, employeeId, agentScheduleDetails.ModifiedBy,
                                                                             agentScheduleDetails.ModifiedUser, agentScheduleDetails.ActivityOrigin);
                        activityLogs.Add(activityLog);
                    }
                }
            }

            _activityLogRepository.CreateActivityLogs(activityLogs);

            await _uow.Commit();

            return new CSSResponse(HttpStatusCode.NoContent);
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
        private ActivityLog GetActivityLogForSchedulingManager(AgentScheduleManager agentScheduleManagerChart, string employeeId, string executedBy, string executedUser,
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
        /// Determines whether [has valid scheduling codes] [the specified manager].
        /// </summary>
        /// <param name="manager">The manager.</param>
        /// <returns></returns>
        private async Task<bool> HasValidSchedulingCodes(UpdateAgentScheduleManager manager)
        {
            bool isValid = true;
            List<int> codes = new List<int>();

            foreach (var agentScheduleManager in manager.ScheduleManagers)
            {
                foreach (var agentScheduleManagerChart in agentScheduleManager.AgentScheduleManagerCharts)
                {
                    var scheduleManagerCodes = agentScheduleManagerChart.Charts.Select(x => x.SchedulingCodeId).ToList().ToList();
                    codes.AddRange(scheduleManagerCodes);
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

        /// <summary>
        /// Gets the agents.
        /// </summary>
        /// <param name="agentScheduleManagerChartQueryparameter">The agent schedule manager chart queryparameter.</param>
        /// <returns></returns>
        private async Task<PagedList<Entity>> GetAgents(AgentScheduleManagerChartQueryparameter agentScheduleManagerChartQueryparameter)
        {
            var agentAdminQueryParameter = _mapper.Map<AgentAdminQueryParameter>(agentScheduleManagerChartQueryparameter);
            agentAdminQueryParameter.Fields = "EmployeeId, FirstName, LastName";

            return await _agentAdminRepository.GetAgentAdmins(agentAdminQueryParameter);
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
