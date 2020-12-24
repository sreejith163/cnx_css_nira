using AutoMapper;
using Css.Api.Core.DataAccess.Repository.UnitOfWork.Interfaces;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Models.DTO.Response;
using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedule;
using Css.Api.Scheduling.Models.DTO.Response.AgentAdmin;
using Css.Api.Scheduling.Models.DTO.Response.AgentSchedule;
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
    /// <seealso cref="Css.Api.Setup.Business.Interfaces.AgentScheduleService" />
    public class AgentScheduleService : IAgentScheduleService
    {
        /// <summary>
        /// The HTTP context accessor
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// The agent schedule repository
        /// </summary>
        private readonly IAgentScheduleRepository _agentScheduleRepository;

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
        /// <param name="agentScheduleRepository">The agent schedule repository.</param>
        /// <param name="agentAdminRepository">The agent admin repository.</param>
        /// <param name="_schedulingCodeRepository">The scheduling code repository.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="uow">The uow.</param>
        public AgentScheduleService(
            IHttpContextAccessor httpContextAccessor,
            IAgentScheduleRepository agentScheduleRepository,
            IAgentAdminRepository agentAdminRepository,
            ISchedulingCodeRepository schedulingCodeRepository,
            IMapper mapper,
            IUnitOfWork uow)
        {
            _httpContextAccessor = httpContextAccessor;
            _agentScheduleRepository = agentScheduleRepository;
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
            return await GetAgentSchedulesData(agentScheduleQueryparameter);
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

            var agentProfile = await _agentAdminRepository.GetAgentAdminIdsByEmployeeId(new EmployeeIdDetails { Id = agentSchedule.EmployeeId });

            var mappedAgentSchedule = _mapper.Map<AgentScheduleDetailsDTO>(agentSchedule);
            mappedAgentSchedule.EmployeeName = $"{agentProfile?.FirstName} {agentProfile?.LastName}";

            return new CSSResponse(mappedAgentSchedule, HttpStatusCode.OK);
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

            await _uow.Commit();

            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Imports the agent schedule chart.
        /// </summary>
        /// <param name="agentScheduleIdDetails">The agent schedule identifier details.</param>
        /// <param name="agentScheduleDetails">The agent schedule details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> ImportAgentScheduleChart(AgentScheduleIdDetails agentScheduleIdDetails, ImportAgentScheduleChart agentScheduleDetails)
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

            _agentScheduleRepository.ImportAgentScheduleChart(agentScheduleIdDetails, agentScheduleDetails);

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

            await _uow.Commit();

            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Gets the agent schedules.
        /// </summary>
        /// <param name="agentScheduleQueryparameter">The agent schedule queryparameter.</param>
        /// <returns></returns>
        private async Task<CSSResponse> GetAgentSchedulesData(AgentScheduleQueryparameter agentScheduleQueryparameter)
        {
            var agentSchedulesData = new List<AgentScheduleDTO>();
            var fields = agentScheduleQueryparameter.Fields?.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToString().ToLower());
            var orderBy = agentScheduleQueryparameter.OrderBy.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToString().ToLower());
            var orderKeywords = orderBy.Select(x => x.Split(" ")[0]);
            bool needEmployeeName = fields == null || fields.Contains("employeename");
            bool needEmployeeNameSort = orderBy != null && orderKeywords != null && orderKeywords.Contains("employeename");

            if (needEmployeeNameSort)
            {
                var orderByType = "asc";
                var existingOrderType = orderBy.Where(x => x.Contains("employeename")).FirstOrDefault()?.Split(" ");
                if (existingOrderType.Count() > 1)
                {
                    orderByType = existingOrderType[1];
                }

                var agentAdminQueryParameter = new AgentAdminQueryParameter
                {
                    PageNumber = agentScheduleQueryparameter.PageNumber,
                    PageSize = agentScheduleQueryparameter.PageSize,
                    OrderBy = $"FirstName {orderByType}",
                    Fields = "EmployeeId, FirstName, LastName"
                };

                var agents = await _agentAdminRepository.GetAgentAdmins(agentAdminQueryParameter);
                var mappedAgents = JsonConvert.DeserializeObject<List<AgentAdminDTO>>(JsonConvert.SerializeObject(agents));
                var employeeIds = mappedAgents.Select(x => x.EmployeeId).ToList();

                agentScheduleQueryparameter.EmployeeIds = employeeIds;
                var agentSchedules = await _agentScheduleRepository.GetAgentSchedules(agentScheduleQueryparameter);

                _httpContextAccessor.HttpContext.Response.Headers.Add("X-Pagination", PagedList<Entity>.ToJson(agentSchedules));

                var mappedAgentSchedules = JsonConvert.DeserializeObject<List<AgentScheduleDTO>>(JsonConvert.SerializeObject(agentSchedules));
                foreach (var mappedAgentSchedule in mappedAgentSchedules)
                {
                    var agentAdmin = mappedAgents.Find(x => x.EmployeeId == mappedAgentSchedule.EmployeeId);
                    if (needEmployeeName)
                    {
                        mappedAgentSchedule.EmployeeName = $"{agentAdmin?.FirstName} {agentAdmin?.LastName}";
                    }
                }

                var sortedAgentSchedules = orderByType == "desc" ? mappedAgentSchedules.OrderByDescending(x => x.EmployeeName) : mappedAgentSchedules.OrderBy(x => x.EmployeeName);
                agentSchedulesData = sortedAgentSchedules.ToList();
            }
            else
            {
                var agentSchedules = await _agentScheduleRepository.GetAgentSchedules(agentScheduleQueryparameter);

                _httpContextAccessor.HttpContext.Response.Headers.Add("X-Pagination", PagedList<Entity>.ToJson(agentSchedules));

                var mappedAgentSchedules = JsonConvert.DeserializeObject<List<AgentScheduleDTO>>(JsonConvert.SerializeObject(agentSchedules));

                if (needEmployeeName)
                {
                    var employeeIds = mappedAgentSchedules.Select(x => x.EmployeeId).Distinct().ToList();

                    var agentAdmins = await _agentAdminRepository.GetAgentAdminsByEmployeeIds(employeeIds);

                    foreach (var mappedAgentSchedule in mappedAgentSchedules)
                    {
                        var agentAdmin = agentAdmins.Find(x => x.Ssn == mappedAgentSchedule.EmployeeId);
                        mappedAgentSchedule.EmployeeName = $"{agentAdmin?.FirstName} {agentAdmin?.LastName}";
                    }
                }

                agentSchedulesData = mappedAgentSchedules;
            }

            return new CSSResponse(agentSchedulesData, HttpStatusCode.OK);
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
                if (details.AgentScheduleType == Models.Enums.AgentScheduleType.SchedulingTab)
                {
                    foreach (var agentScheduleChart in details.AgentScheduleCharts)
                    {
                        var scheduleCodes = agentScheduleChart.Charts.Select(x => x.SchedulingCodeId).ToList();
                        codes.AddRange(scheduleCodes);
                    }
                }
                else
                {
                    var scheduleManagerCodes = details.AgentScheduleManagerChart.Charts.Select(x => x.SchedulingCodeId).ToList().ToList();
                    codes.AddRange(scheduleManagerCodes);
                }
            }
            else if (agentScheduleDetails is ImportAgentScheduleChart)
            {

                var details = agentScheduleDetails as ImportAgentScheduleChart;
                if (details.AgentScheduleType == Models.Enums.AgentScheduleType.SchedulingTab)
                {
                    foreach (var agentScheduleChart in details.AgentScheduleCharts)
                    {
                        var scheduleCodes = agentScheduleChart.Charts.Select(x => x.SchedulingCodeId).ToList();
                        codes.AddRange(scheduleCodes);
                    }

                }
                else
                {
                    foreach (var agentScheduleManagerChart in details.AgentScheduleManagerCharts)
                    {
                        var scheduleManagerCodes = agentScheduleManagerChart.Charts.Select(x => x.SchedulingCodeId).ToList();
                        codes.AddRange(scheduleManagerCodes);
                    }
                }
            }

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