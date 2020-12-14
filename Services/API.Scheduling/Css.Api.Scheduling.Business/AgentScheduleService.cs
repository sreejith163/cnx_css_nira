using AutoMapper;
using Css.Api.Core.DataAccess.Repository.UnitOfWork.Interfaces;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Models.DTO.Response;
using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedule;
using Css.Api.Scheduling.Models.DTO.Response.AgentSchedule;
using Css.Api.Scheduling.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
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
        /// <param name="mapper">The mapper.</param>
        /// <param name="uow">The uow.</param>
        public AgentScheduleService(
            IHttpContextAccessor httpContextAccessor,
            IAgentScheduleRepository agentScheduleRepository,
            IAgentAdminRepository agentAdminRepository,
            IMapper mapper,
            IUnitOfWork uow)
        {
            _httpContextAccessor = httpContextAccessor;
            _agentScheduleRepository = agentScheduleRepository;
            _agentAdminRepository = agentAdminRepository;
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
            var agentAdmins = await _agentScheduleRepository.GetAgentSchedules(agentScheduleQueryparameter);
            _httpContextAccessor.HttpContext.Response.Headers.Add("X-Pagination", PagedList<Entity>.ToJson(agentAdmins));

            return new CSSResponse(agentAdmins, HttpStatusCode.OK);
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
            if (agentProfile == null)
            {
                return new CSSResponse($"Employee Id '{agentSchedule.EmployeeId}' not found", HttpStatusCode.NotFound);
            }

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

            _agentScheduleRepository.UpdateAgentScheduleChart(agentScheduleIdDetails, agentScheduleDetails);

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
    }
}