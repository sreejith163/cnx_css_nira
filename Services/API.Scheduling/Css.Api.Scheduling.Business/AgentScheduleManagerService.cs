using AutoMapper;
using Css.Api.Core.DataAccess.Repository.UnitOfWork.Interfaces;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Core.Models.DTO.Response;
using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedule;
using Css.Api.Scheduling.Models.DTO.Request.AgentScheduleManager;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedulingGroup;
using Css.Api.Scheduling.Models.DTO.Response.AgentScheduleManager;
using Css.Api.Scheduling.Models.Enums;
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
            IMapper mapper,
            IUnitOfWork uow)
        {
            _httpContextAccessor = httpContextAccessor;
            _activityLogRepository = activityLogRepository;
            _agentScheduleManagerRepository = agentScheduleManagerRepository;
            _agentAdminRepository = agentAdminRepository;
            _schedulingCodeRepository = schedulingCodeRepository;
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
            var agentScheduleManagers = await _agentScheduleManagerRepository.GetAgentScheduleManagerCharts(agentScheduleManagerChartQueryparameter);
            _httpContextAccessor.HttpContext.Response.Headers.Add("X-Pagination", PagedList<Entity>.ToJson(agentScheduleManagers));

            if (agentScheduleManagerChartQueryparameter.Date.HasValue && agentScheduleManagerChartQueryparameter.Date != default(DateTime))
            {
                var mappedAgentScheduleManagers = JsonConvert.DeserializeObject<List<AgentScheduleManagerChartDetailsDTO>>(JsonConvert.SerializeObject(agentScheduleManagers));
                foreach (var mappedAgentScheduleManager in mappedAgentScheduleManagers)
                {
                    var date = agentScheduleManagerChartQueryparameter.Date.Value;
                    var dateTimeWithZeroTimeSpan = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);

                    mappedAgentScheduleManager.AgentScheduleManagerCharts = mappedAgentScheduleManager.AgentScheduleManagerCharts
                        .Where(x => x.Date == dateTimeWithZeroTimeSpan).ToList();
                }

                return new CSSResponse(mappedAgentScheduleManagers, HttpStatusCode.OK);
            }

            return new CSSResponse(agentScheduleManagers, HttpStatusCode.OK);
        }

        /// <summary>
        /// Gets the agent schedule manager chart.
        /// </summary>
        /// <param name="agentScheduleManagerIdDetails">The agent schedule manager identifier details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> GetAgentScheduleManagerChart(AgentScheduleManagerIdDetails agentScheduleManagerIdDetails)
        {
            var agentScheduleManager = await _agentScheduleManagerRepository.GetAgentScheduleManagerChart(agentScheduleManagerIdDetails);
            if (agentScheduleManager == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var mappedAgentScheduleManager = _mapper.Map<AgentScheduleManagerChartDetailsDTO>(agentScheduleManager);
            return new CSSResponse(mappedAgentScheduleManager, HttpStatusCode.OK);
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
                // return new CSSResponse("One of the scheduling code does not exists", HttpStatusCode.NotFound);
            }

            var activityLogs = new List<ActivityLog>();

            foreach (var agentScheduleManager in agentScheduleManagerChartDetails.AgentScheduleManagers)
            {
                var employeeIdDetails = new EmployeeIdDetails { Id = agentScheduleManager.EmployeeId };
                var modifiedUserDetails = new ModifiedUserDetails { ModifiedBy = agentScheduleManagerChartDetails.ModifiedBy };

                agentScheduleManager.AgentScheduleManagerChart.Date = new DateTime(agentScheduleManager.AgentScheduleManagerChart.Date.Year, 
                                                                                   agentScheduleManager.AgentScheduleManagerChart.Date.Month, 
                                                                                   agentScheduleManager.AgentScheduleManagerChart.Date.Day, 0, 0, 0);

                var agentScheduleMangerSchedule = await _agentScheduleManagerRepository.GetAgentScheduleManagerChartByEmployeeId(employeeIdDetails);
                if (agentScheduleMangerSchedule != null)
                {
                    var agentAdmin = await _agentAdminRepository.GetAgentAdminIdsByEmployeeId(employeeIdDetails);
                    if (agentAdmin != null)
                    {
                        var agentScheduleManagerChart = new AgentScheduleManagerChart
                        {
                            AgentSchedulingGroupId = agentAdmin.AgentSchedulingGroupId,
                            Date = agentScheduleManager.AgentScheduleManagerChart.Date,
                            Charts = agentScheduleManager.AgentScheduleManagerChart.Charts,
                            CreatedBy = modifiedUserDetails.ModifiedBy,
                            CreatedDate = DateTimeOffset.UtcNow
                        };

                        _agentScheduleManagerRepository.UpdateAgentScheduleMangerChart(employeeIdDetails, agentScheduleManagerChart);
                        var activityLog = GetActivityLogForSchedulingManager(agentScheduleManagerChart, employeeIdDetails.Id,
                                                                             agentScheduleManagerChartDetails.ModifiedBy, agentScheduleManagerChartDetails.ModifiedUser,
                                                                             agentScheduleManagerChartDetails.ActivityOrigin);
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
        /// <param name="agentScheduleManagerIdDetails">The agent schedule manager identifier details.</param>
        /// <param name="agentScheduleDetails">The agent schedule details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> CopyAgentScheduleManagerChart(AgentScheduleManagerIdDetails agentScheduleManagerIdDetails, CopyAgentScheduleManager agentScheduleDetails)
        {
            var agentScheduleManager = await _agentScheduleManagerRepository.GetAgentScheduleManagerChart(agentScheduleManagerIdDetails);
            if (agentScheduleManager == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            if (!agentScheduleDetails.EmployeeIds.Any())
            {
                AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails = new AgentSchedulingGroupIdDetails { AgentSchedulingGroupId = agentScheduleDetails.AgentSchedulingGroupId };
                agentScheduleDetails.EmployeeIds = await _agentScheduleManagerRepository.GetEmployeeIdsByAgentScheduleGroupId(agentSchedulingGroupIdDetails);
                agentScheduleDetails.EmployeeIds = agentScheduleDetails.EmployeeIds.FindAll(x => x != agentScheduleManager.EmployeeId);
            }

            var activityLogs = new List<ActivityLog>();

            foreach (var employeeId in agentScheduleDetails.EmployeeIds)
            {
                var employeeIdDetails = new EmployeeIdDetails { Id = employeeId };
                var modifiedUserDetails = new ModifiedUserDetails { ModifiedBy = agentScheduleDetails.ModifiedBy };

                var employeeSchedule = await _agentScheduleManagerRepository.GetAgentScheduleManagerChartByEmployeeId(employeeIdDetails);
                if (employeeSchedule != null)
                {

                    agentScheduleDetails.Date = new DateTime(agentScheduleDetails.Date.Year, agentScheduleDetails.Date.Month, agentScheduleDetails.Date.Day, 0, 0, 0);
                    var agentScheduleManagerChartExist = employeeSchedule.AgentScheduleManagerCharts.Exists(x => x.Date == agentScheduleDetails.Date);
                    
                    if (!agentScheduleManagerChartExist)
                    {
                        var copiedAgentScheduleManagerChart = agentScheduleManager.AgentScheduleManagerCharts
                           .FirstOrDefault(x => x.Date == agentScheduleDetails.Date);

                        if (copiedAgentScheduleManagerChart != null && copiedAgentScheduleManagerChart.Charts.Any())
                        {
                            var agentAdmin = await _agentAdminRepository.GetAgentAdminIdsByEmployeeId(employeeIdDetails);
                            if (agentAdmin != null)
                            {
                                var agentScheduleManagerChart = new AgentScheduleManagerChart
                                {
                                    AgentSchedulingGroupId = agentAdmin.AgentSchedulingGroupId,
                                    Date = agentScheduleDetails.Date,
                                    Charts = copiedAgentScheduleManagerChart.Charts,
                                    CreatedBy = modifiedUserDetails.ModifiedBy,
                                    CreatedDate = DateTimeOffset.UtcNow
                                };

                                _agentScheduleManagerRepository.CopyAgentScheduleManagerChart(employeeIdDetails, agentScheduleManagerChart);

                                var activityLog = GetActivityLogForSchedulingManager(agentScheduleManagerChart, employeeId, agentScheduleDetails.ModifiedBy,
                                                                                     agentScheduleDetails.ModifiedUser, agentScheduleDetails.ActivityOrigin);
                                activityLogs.Add(activityLog);
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
        /// Gets the activity log for scheduling manager.
        /// </summary>
        /// <param name="agentScheduleManagerChart">The agent schedule manager chart.</param>
        /// <param name="employeeId">The employee identifier.</param>
        /// <param name="executedBy">The executed by.</param>
        /// <param name="executedUser">The executed user.</param>
        /// <param name="activityOrigin">The activity origin.</param>
        /// <returns></returns>
        private ActivityLog GetActivityLogForSchedulingManager(AgentScheduleManagerChart agentScheduleManagerChart, int employeeId, string executedBy, int executedUser,
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
                SchedulingFieldDetails = new SchedulingFieldDetails() { ActivityLogScheduleManager = _mapper.Map<ActivityLogScheduleManager>(agentScheduleManagerChart) }
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
    }
}