using AutoMapper;
using Css.Api.Core.DataAccess.Repository.UnitOfWork.Interfaces;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Core.Models.DTO.Response;
using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.ActivityLog;
using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedule;
using Css.Api.Scheduling.Models.DTO.Request.Client;
using Css.Api.Scheduling.Models.DTO.Request.ClientLobGroup;
using Css.Api.Scheduling.Models.DTO.Request.SkillGroup;
using Css.Api.Scheduling.Models.DTO.Request.SkillTag;
using Css.Api.Scheduling.Models.DTO.Response.AgentAdmin;
using Css.Api.Scheduling.Models.Enums;
using Css.Api.Scheduling.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Business
{
    /// <summary>
    /// Service for agent admin part
    /// </summary>
    /// <seealso cref="Css.Api.Setup.Business.Interfaces.IAgentAdminService" />
    public class AgentAdminService : IAgentAdminService
    {
        /// <summary>
        /// The HTTP context accessor
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// The repository
        /// </summary>
        private readonly IAgentAdminRepository _agentAdminRepository;

        /// <summary>
        /// The agent schedule repository
        /// </summary>
        private readonly IAgentScheduleRepository _agentScheduleRepository;

        /// <summary>
        /// The client name repository
        /// </summary>
        private readonly IClientRepository _clientRepository;

        /// <summary>
        /// The client lob group repository
        /// </summary>
        private readonly IClientLobGroupRepository _clientLobGroupRepository;

        /// <summary>
        /// The skill group repository
        /// </summary>
        private readonly ISkillGroupRepository _skillGroupRepository;

        /// <summary>
        /// The skill tag repository
        /// </summary>
        private readonly ISkillTagRepository _skillTagRepository;

        /// <summary>
        /// The agent scheduling group repository
        /// </summary>
        private readonly IAgentSchedulingGroupRepository _agentSchedulingGroupRepository;

        /// <summary>The activity log repository</summary>
        private readonly IActivityLogRepository _activityLogRepository;

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
        /// <param name="agentAdminRepository">The agent admin repository.</param>
        /// <param name="_agentScheduleRepository">The agent schedule repository.</param>
        /// <param name="clientRepository">The client repository.</param>
        /// <param name="clientLobGroupRepository">The client lob group repository.</param>
        /// <param name="skillGroupRepository">The skill group repository.</param>
        /// <param name="skillTagRepository">The skill tag repository.</param>
        /// <param name="agentSchedulingGroupRepository">The agent scheduling group repository.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="uow">The uow.</param>
        public AgentAdminService(
            IHttpContextAccessor httpContextAccessor,
            IAgentAdminRepository agentAdminRepository,
            IAgentScheduleRepository _agentScheduleRepository,
            IClientRepository clientRepository,
            IClientLobGroupRepository clientLobGroupRepository,
            ISkillGroupRepository skillGroupRepository,
            ISkillTagRepository skillTagRepository,
            IAgentSchedulingGroupRepository agentSchedulingGroupRepository,
            IActivityLogRepository activityLogRepository,
        IMapper mapper,
            IUnitOfWork uow)
        {
            _httpContextAccessor = httpContextAccessor;
            _agentAdminRepository = agentAdminRepository;
            this._agentScheduleRepository = _agentScheduleRepository;
            _clientRepository = clientRepository;
            _clientLobGroupRepository = clientLobGroupRepository;
            _skillGroupRepository = skillGroupRepository;
            _skillTagRepository = skillTagRepository;
            _agentSchedulingGroupRepository = agentSchedulingGroupRepository;
            _activityLogRepository = activityLogRepository;
            _mapper = mapper;
            _uow = uow;
        }

        //To be changed
        /// <summary>
        /// Seeds the data.
        /// </summary>
        public async void SeedData()
        {
            var isClientSeeded = await _clientRepository.GetClientsCount() > 0;
            if (!isClientSeeded)
            {
                _clientRepository.CreateClient(
                    new Client { ClientId = 1, Name = "CName1ForAgentAdmin" });
                _clientRepository.CreateClient(
                    new Client { ClientId = 2, Name = "CName2ForAgentAdmin" });
            }

            var isClientLobSeeded = await _clientLobGroupRepository.GetClientLobGroupsCount() > 0;
            if (!isClientLobSeeded)
            {
                _clientLobGroupRepository.CreateClientLobGroup(
                    new ClientLobGroup { ClientId = 1, ClientLobGroupId = 1, Name = "Lob1ForAgentAdmin" });
                _clientLobGroupRepository.CreateClientLobGroup(
                    new ClientLobGroup { ClientId = 2, ClientLobGroupId = 2, Name = "Lob2ForAgentAdmin" });
            }

            var isSkillGroupSeeded = await _skillGroupRepository.GetSkillGroupsCount() > 0;
            if (!isSkillGroupSeeded)
            {
                _skillGroupRepository.CreateSkillGroup(
                    new SkillGroup { ClientId = 1, ClientLobGroupId = 1, SkillGroupId = 1, Name = "SG1ForAgentAdmin" }
                );
                _skillGroupRepository.CreateSkillGroup(
                                   new SkillGroup { ClientId = 2, ClientLobGroupId = 2, SkillGroupId = 2, Name = "SG2ForAgentAdmin" }
               );
            }

            var isSkillTagSeeded = await _skillTagRepository.GetSkillTagsCount() > 0;
            if (!isSkillTagSeeded)
            {
                _skillTagRepository.CreateSkillTag(
                    new SkillTag { ClientId = 1, ClientLobGroupId = 1, SkillGroupId = 1, SkillTagId = 1, Name = "ST1ForAgentAdmin" }
                );
                _skillTagRepository.CreateSkillTag(
                new SkillTag { ClientId = 2, ClientLobGroupId = 2, SkillGroupId = 2, SkillTagId = 2, Name = "ST2ForAgentAdmin" }
                );
            }

            var isagentSchedulingGroupsSeeded = await _agentSchedulingGroupRepository.GetAgentSchedulingGroupsCount() > 0;
            if (!isagentSchedulingGroupsSeeded)
            {
                _agentSchedulingGroupRepository.CreateAgentSchedulingGroup(new AgentSchedulingGroup
                {
                    ClientId = 1,
                    ClientLobGroupId = 1,
                    SkillGroupId = 1,
                    SkillTagId = 1,
                    AgentSchedulingGroupId = 1,
                    Name = "Agent Scheduliung Group 1",
                    IsDeleted = false
                });

                _agentSchedulingGroupRepository.CreateAgentSchedulingGroup(new AgentSchedulingGroup
                {
                    ClientId = 2,
                    ClientLobGroupId = 2,
                    SkillGroupId = 2,
                    SkillTagId = 2,
                    AgentSchedulingGroupId = 2,
                    Name = "Agent Scheduliung Group 2",
                    IsDeleted = false
                });
            }

            await _uow.Commit();
        }

        /// <summary>
        /// Gets the agent admins.
        /// </summary>
        /// <param name="agentAdminQueryParameter">The agent admin query parameter.</param>
        /// <returns></returns>
        public async Task<CSSResponse> GetAgentAdmins(AgentAdminQueryParameter agentAdminQueryParameter)
        {
            var agentAdmins = await _agentAdminRepository.GetAgentAdmins(agentAdminQueryParameter);
            _httpContextAccessor.HttpContext.Response.Headers.Add("X-Pagination", PagedList<Entity>.ToJson(agentAdmins));

            return new CSSResponse(agentAdmins, HttpStatusCode.OK);
        }

        /// <summary>
        /// Gets the agent admin.
        /// </summary>
        /// <param name="agentAdminIdDetails">The agent admin identifier details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> GetAgentAdmin(AgentAdminIdDetails agentAdminIdDetails)
        {
            var agentAdmin = await _agentAdminRepository.GetAgentAdmin(agentAdminIdDetails);
            if (agentAdmin == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var skillTag = await _skillTagRepository.GetSkillTag(new SkillTagIdDetails
            {
                SkillTagId = agentAdmin.SkillTagId
            });

            if (skillTag == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var skillGroup = await _skillGroupRepository.GetSkillGroup(new SkillGroupIdDetails
            {
                SkillGroupId = skillTag.SkillGroupId
            });

            if (skillGroup == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var clientLobGroup = await _clientLobGroupRepository.GetClientLobGroup(new ClientLobGroupIdDetails
            {
                ClientLobGroupId = skillGroup.ClientLobGroupId
            });

            if (clientLobGroup == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var clientName = await _clientRepository.GetClient(new ClientIdDetails
            {
                ClientId = clientLobGroup.ClientId
            });

            if (clientName == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var mappedAgentAdmin = _mapper.Map<AgentAdminDetailsDTO>(agentAdmin);
            mappedAgentAdmin.SkillTagId = skillTag.SkillTagId;
            mappedAgentAdmin.SkillTagName = skillTag.Name;
            mappedAgentAdmin.SkillGroupId = skillGroup.SkillGroupId;
            mappedAgentAdmin.SkillGroupName = skillGroup.Name;
            mappedAgentAdmin.ClientLobGroupId = clientLobGroup.ClientLobGroupId;
            mappedAgentAdmin.ClientLobGroupName = clientLobGroup.Name;
            mappedAgentAdmin.ClientId = clientName.ClientId;
            mappedAgentAdmin.ClientName = clientName.Name;

            return new CSSResponse(mappedAgentAdmin, HttpStatusCode.OK);
        }

        /// <summary>
        /// Creates the agent admin.
        /// </summary>
        /// <param name="agentAdminDetails">The agent admin details</param>
        /// <returns></returns>
        public async Task<CSSResponse> CreateAgentAdmin(CreateAgentAdmin agentAdminDetails)
        {
            var agentAdminEmployeeIdDetails = new EmployeeIdDetails { Id = agentAdminDetails.EmployeeId };
            var agentAdminSsoDetails = new AgentAdminSsoDetails { Sso = agentAdminDetails.Sso };
            var skillTagIdDetails = new SkillTagIdDetails { SkillTagId = agentAdminDetails.SkillTagId };

            var agentAdminIdsByEmployeeIdAndSso = await _agentAdminRepository.GetAgentAdminIdsByEmployeeIdAndSso(agentAdminEmployeeIdDetails, agentAdminSsoDetails);

            if (agentAdminIdsByEmployeeIdAndSso != null)
            {
                return new CSSResponse($"Agent Admin with Employee ID '{agentAdminEmployeeIdDetails.Id}' and SSO '{agentAdminDetails.Sso}' already exists.", HttpStatusCode.Conflict);
            }            

            var agentAdminsBasedOnEmployeeId = await _agentAdminRepository.GetAgentAdminIdsByEmployeeId(agentAdminEmployeeIdDetails);

            if (agentAdminsBasedOnEmployeeId != null)
            {
                return new CSSResponse($"Agent Admin with Employee ID '{agentAdminEmployeeIdDetails.Id}' already exists.", HttpStatusCode.Conflict);
            }

            var agentAdminsBasedonSSO = await _agentAdminRepository.GetAgentAdminIdsBySso(agentAdminSsoDetails);

            if (agentAdminsBasedonSSO != null)
            {
                return new CSSResponse($"Agent Admin with SSO '{agentAdminDetails.Sso}' already exists.", HttpStatusCode.Conflict);
            }

            var agentSchedulingGroupBasedonSkillTag = await _agentSchedulingGroupRepository.GetAgentSchedulingGroupBasedonSkillTag(skillTagIdDetails);

            if (agentSchedulingGroupBasedonSkillTag == null)
            {
                return new CSSResponse($"No Scheduling Group match to this record. Please create before you proceed.", HttpStatusCode.NotFound);
            }

            var agentAdminRequest = _mapper.Map<Agent>(agentAdminDetails);
            
            agentAdminRequest.AgentSchedulingGroupId = agentSchedulingGroupBasedonSkillTag.AgentSchedulingGroupId;

            _agentAdminRepository.CreateAgentAdmin(agentAdminRequest);

            var agentScheduleRequest = _mapper.Map<AgentSchedule>(agentAdminRequest);
            _agentScheduleRepository.CreateAgentSchedule(agentScheduleRequest);

            await _uow.Commit();

            return new CSSResponse(new AgentAdminIdDetails { AgentAdminId = agentAdminRequest.Id.ToString() }, HttpStatusCode.Created);
        }

        /// <summary>
        /// Updates the agent admin.
        /// </summary>
        /// <param name="agentAdminIdDetails">The agent admin identifier details.</param>
        /// <param name="agentAdminDetails">The agent admin details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> UpdateAgentAdmin(AgentAdminIdDetails agentAdminIdDetails, UpdateAgentAdmin agentAdminDetails)
        {
            var agentAdmin = await _agentAdminRepository.GetAgentAdmin(agentAdminIdDetails);
            if (agentAdmin == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var agentAdminEmployeeIdDetails = new EmployeeIdDetails { Id = agentAdminDetails.EmployeeId };
            var agentAdminSsoDetails = new AgentAdminSsoDetails { Sso = agentAdminDetails.Sso };
            var skillTagIdDetails = new SkillTagIdDetails { SkillTagId = agentAdminDetails.SkillTagId };
            var employeeIdDetails = new EmployeeIdDetails { Id = agentAdmin.Ssn };

            var agentAdminsBasedOnEmployeeId = await _agentAdminRepository.GetAgentAdminIdsByEmployeeId(agentAdminEmployeeIdDetails);

            if (agentAdminsBasedOnEmployeeId != null && 
                !string.Equals(agentAdminsBasedOnEmployeeId.Id.ToString(), agentAdminIdDetails.AgentAdminId))
            {
                return new CSSResponse($"Agent Admin with Employee ID '{agentAdminEmployeeIdDetails.Id}' already exists.", HttpStatusCode.Conflict);
            }

            var agentAdminsBasedonSSO = await _agentAdminRepository.GetAgentAdminIdsBySso(agentAdminSsoDetails);

            if (agentAdminsBasedonSSO != null &&
                !string.Equals(agentAdminsBasedonSSO.Id.ToString(), agentAdminIdDetails.AgentAdminId))
            {
                return new CSSResponse($"Agent Admin with SSO '{agentAdminDetails.Sso}' already exists.", HttpStatusCode.Conflict);
            }

            var agentSchedulingGroupBasedonSkillTag = await _agentSchedulingGroupRepository.GetAgentSchedulingGroupBasedonSkillTag(skillTagIdDetails);

            if (agentSchedulingGroupBasedonSkillTag == null)
            {
                return new CSSResponse($"No Scheduling Group match to this record. Please create before you proceed.", HttpStatusCode.NotFound);
            }

            var agentAdminRequest = _mapper.Map(agentAdminDetails, agentAdmin);
            agentAdminRequest.AgentSchedulingGroupId = agentSchedulingGroupBasedonSkillTag.AgentSchedulingGroupId;

            _agentAdminRepository.UpdateAgentAdmin(agentAdminRequest);

            var updateAgentScheduleEmployeeDetails = new UpdateAgentScheduleEmployeeDetails
            {
                EmployeeId = agentAdminDetails.EmployeeId,
                AgentSchedulingGroupId = agentSchedulingGroupBasedonSkillTag.AgentSchedulingGroupId,
                ModifiedBy = agentAdminDetails.ModifiedBy
            };

            _agentScheduleRepository.UpdateAgentSchedule(employeeIdDetails, updateAgentScheduleEmployeeDetails);

            await _uow.Commit();

            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Deletes the agent admin.
        /// </summary>
        /// <param name="agentAdminIdDetails">The agent admin identifier details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> DeleteAgentAdmin(AgentAdminIdDetails agentAdminIdDetails)
        {
            var agentAdmin = await _agentAdminRepository.GetAgentAdmin(agentAdminIdDetails);
            if (agentAdmin == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            agentAdmin.IsDeleted = true;

            _agentAdminRepository.UpdateAgentAdmin(agentAdmin);

            _agentScheduleRepository.DeleteAgentSchedule(new EmployeeIdDetails { Id = agentAdmin.Ssn });

            await _uow.Commit();

            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>Creates the agent activity log.</summary>
        /// <param name="agentActivityLogDetails">The agent activity log details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<CSSResponse> CreateAgentActivityLog(CreateAgentActivityLog agentActivityLogDetails)
        {
            var agentAdminEmployeeIdDetails = new EmployeeIdDetails { Id = agentActivityLogDetails.EmployeeId };

            var agentAdmins = await _agentAdminRepository.GetAgentAdminIdsByEmployeeId(agentAdminEmployeeIdDetails);

            if (agentAdmins == null)
            {
                return new CSSResponse($"Agent Admin with Employee id '{agentAdminEmployeeIdDetails.Id}' does not exists.", HttpStatusCode.NotFound);
            }

            var activityLogRequest = _mapper.Map<ActivityLog>(agentActivityLogDetails);
            activityLogRequest.ActivityType = ActivityType.AgentAdmin;

            _activityLogRepository.CreateActivityLogs(activityLogRequest);

            await _uow.Commit();

            return new CSSResponse(new AgentActivityLogIdDetails { AgentActivityLogId = activityLogRequest.Id }, HttpStatusCode.Created);
        }

        /// <summary>Gets the agent activity logs.</summary>
        /// <param name="agentActivityLogQueryParameter">The agent activity log query parameter.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<CSSResponse> GetAgentActivityLogs(ActivityLogQueryParameter agentActivityLogQueryParameter)
        {
            agentActivityLogQueryParameter.ActivityType = ActivityType.AgentAdmin;
            var agentActivityLogs = await _activityLogRepository.GetActivityLogs(agentActivityLogQueryParameter);
            _httpContextAccessor.HttpContext.Response.Headers.Add("X-Pagination", PagedList<Entity>.ToJson(agentActivityLogs));

            return new CSSResponse(agentActivityLogs, HttpStatusCode.OK);
        }
    }
}