using AutoMapper;
using Css.Api.Core.DataAccess.Repository.UnitOfWork.Interfaces;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Core.Models.DTO.Response;
using Css.Api.Core.Models.Enums;
using Css.Api.Core.Utilities.Extensions;
using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Models.DTO.Request.ActivityLog;
using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedule;
using Css.Api.Scheduling.Models.DTO.Request.AgentScheduleManager;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedulingGroup;
using Css.Api.Scheduling.Models.DTO.Request.Client;
using Css.Api.Scheduling.Models.DTO.Request.ClientLobGroup;
using Css.Api.Scheduling.Models.DTO.Request.SkillGroup;
using Css.Api.Scheduling.Models.DTO.Request.SkillTag;
using Css.Api.Scheduling.Models.DTO.Request.Timezone;
using Css.Api.Scheduling.Models.DTO.Response.AgentAdmin;
using Css.Api.Scheduling.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
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
        /// The agent schedule manager repository
        /// </summary>
        private readonly IAgentScheduleManagerRepository _agentScheduleManagerRepository;

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

        /// <summary>The timezone repository</summary>
        private readonly ITimezoneRepository _timezoneRepository;

        /// <summary>
        /// The activity log repository
        /// </summary>
        private readonly IActivityLogRepository _activityLogRepository;

        /// <summary>The agent scheduling group history repository</summary>
        private readonly IAgentSchedulingGroupHistoryRepository _agentSchedulingGroupHistoryRepository;

        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// The uow
        /// </summary>
        private readonly IUnitOfWork _uow;


        /// <summary>Initializes a new instance of the <see cref="AgentAdminService" /> class.</summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="agentAdminRepository">The agent admin repository.</param>
        /// <param name="agentScheduleRepository">The agent schedule repository.</param>
        /// <param name="agentScheduleManagerRepository">The agent schedule manager repository.</param>
        /// <param name="clientRepository">The client repository.</param>
        /// <param name="clientLobGroupRepository">The client lob group repository.</param>
        /// <param name="skillGroupRepository">The skill group repository.</param>
        /// <param name="skillTagRepository">The skill tag repository.</param>
        /// <param name="agentSchedulingGroupRepository">The agent scheduling group repository.</param>
        /// <param name="timezoneRepository">The timezone repository.</param>
        /// <param name="activityLogRepository">The activity log repository.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="uow">The uow.</param>
        public AgentAdminService(
            IHttpContextAccessor httpContextAccessor,
            IAgentAdminRepository agentAdminRepository,
            IAgentScheduleRepository agentScheduleRepository,
            IAgentScheduleManagerRepository agentScheduleManagerRepository,
            IClientRepository clientRepository,
            IClientLobGroupRepository clientLobGroupRepository,
            ISkillGroupRepository skillGroupRepository,
            ISkillTagRepository skillTagRepository,
            IAgentSchedulingGroupRepository agentSchedulingGroupRepository,
            ITimezoneRepository timezoneRepository,
            IActivityLogRepository activityLogRepository,
            IAgentSchedulingGroupHistoryRepository agentSchedulingGroupHistoryRepository,
            IMapper mapper,
            IUnitOfWork uow)
        {
            _httpContextAccessor = httpContextAccessor;
            _agentAdminRepository = agentAdminRepository;
            _agentScheduleRepository = agentScheduleRepository;
            _agentScheduleManagerRepository = agentScheduleManagerRepository;
            _clientRepository = clientRepository;
            _clientLobGroupRepository = clientLobGroupRepository;
            _skillGroupRepository = skillGroupRepository;
            _skillTagRepository = skillTagRepository;
            _agentSchedulingGroupRepository = agentSchedulingGroupRepository;
            _timezoneRepository = timezoneRepository;
            _activityLogRepository = activityLogRepository;
            _agentSchedulingGroupHistoryRepository = agentSchedulingGroupHistoryRepository;
            _mapper = mapper;
            _uow = uow;
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

            var agentSchedulingGroup = await _agentSchedulingGroupRepository.GetAgentSchedulingGroup(new AgentSchedulingGroupIdDetails
            {
                AgentSchedulingGroupId = agentAdmin.AgentSchedulingGroupId
            });

            if (agentSchedulingGroup == null)
            {
                return new CSSResponse("Agent Scheduling Group not found", HttpStatusCode.NotFound);
            }

            var skillTag = await _skillTagRepository.GetSkillTag(new SkillTagIdDetails
            {
                SkillTagId = agentAdmin.SkillTagId
            });

            if (skillTag == null)
            {
                return new CSSResponse("Skill Tag not found", HttpStatusCode.NotFound);
            }

            var skillGroup = await _skillGroupRepository.GetSkillGroup(new SkillGroupIdDetails
            {
                SkillGroupId = skillTag.SkillGroupId
            });

            if (skillGroup == null)
            {
                return new CSSResponse("Skill Group not found", HttpStatusCode.NotFound);
            }

            var clientLobGroup = await _clientLobGroupRepository.GetClientLobGroup(new ClientLobGroupIdDetails
            {
                ClientLobGroupId = skillGroup.ClientLobGroupId
            });

            if (clientLobGroup == null)
            {
                return new CSSResponse("Client Lob Group not found", HttpStatusCode.NotFound);
            }

            var clientName = await _clientRepository.GetClient(new ClientIdDetails
            {
                ClientId = clientLobGroup.ClientId
            });

            if (clientName == null)
            {
                return new CSSResponse("Client Name not found", HttpStatusCode.NotFound);
            }

            var mappedAgentAdmin = _mapper.Map<AgentAdminDetailsDTO>(agentAdmin);
            mappedAgentAdmin.AgentSchedulingGroupName = agentSchedulingGroup.Name;
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
        /// Gets the agent admin by employee identifier.
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> GetAgentAdminByEmployeeId(EmployeeIdDetails employeeIdDetails)
        {
            var agentAdmin = await _agentAdminRepository.GetAgentAdminByEmployeeId(employeeIdDetails);
            if (agentAdmin == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var mappedAgentAdmin = _mapper.Map<AgentAdminDetailsDTO>(agentAdmin);
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
            var agentSchedulingGroupIdDetails = new AgentSchedulingGroupIdDetails { AgentSchedulingGroupId = agentAdminDetails.AgentSchedulingGroupId };

            var agentAdminIdsByEmployeeIdAndSso = await _agentAdminRepository.GetAgentAdminIdsByEmployeeIdAndSso(agentAdminEmployeeIdDetails, agentAdminSsoDetails);

            if (agentAdminIdsByEmployeeIdAndSso != null)
            {
                return new CSSResponse($"Agent Admin with Employee ID '{agentAdminEmployeeIdDetails.Id}' and SSO '{agentAdminDetails.Sso}' already exists.", HttpStatusCode.Conflict);
            }

            var agentAdminsBasedOnEmployeeId = await _agentAdminRepository.GetAgentAdminByEmployeeId(agentAdminEmployeeIdDetails);

            if (agentAdminsBasedOnEmployeeId != null)
            {
                return new CSSResponse($"Agent Admin with Employee ID '{agentAdminEmployeeIdDetails.Id}' already exists.", HttpStatusCode.Conflict);
            }

            var agentAdminsBasedonSSO = await _agentAdminRepository.GetAgentAdminBySso(agentAdminSsoDetails);

            if (agentAdminsBasedonSSO != null)
            {
                return new CSSResponse($"Agent Admin with SSO '{agentAdminDetails.Sso}' already exists.", HttpStatusCode.Conflict);
            }

            if (agentAdminDetails.Sso == agentAdminDetails.SupervisorSso)
            {
                return new CSSResponse($"Please enter a unique email address for SSO and Team Lead SSO.", HttpStatusCode.Conflict);
            }

            var agentSchedulingGroup = await _agentSchedulingGroupRepository.GetAgentSchedulingGroup(agentSchedulingGroupIdDetails);

            if (agentSchedulingGroup == null)
            {
                return new CSSResponse($"No Scheduling Group match to this record. Please create before you proceed.", HttpStatusCode.NotFound);
            }

            var agentAdminRequest = _mapper.Map<Agent>(agentAdminDetails);

            agentAdminRequest.ClientId = agentSchedulingGroup.ClientId;
            agentAdminRequest.ClientLobGroupId = agentSchedulingGroup.ClientLobGroupId;
            agentAdminRequest.SkillGroupId = agentSchedulingGroup.SkillGroupId;
            agentAdminRequest.SkillTagId = agentSchedulingGroup.SkillTagId;

            _agentAdminRepository.CreateAgentAdmin(agentAdminRequest);

            var agentScheduleRequest = _mapper.Map<AgentSchedule>(agentAdminRequest);
            _agentScheduleRepository.CreateAgentSchedule(agentScheduleRequest);

            // get the preUpdated details and compare it with the updated details to check changes
            var fieldDetails = addActivityLogFields(null, agentAdminRequest, "");

            // create activity log based on the changed fields
            var activityLog = new ActivityLog()
            {
                ActivityType = ActivityType.AgentAdmin,
                FieldDetails = fieldDetails,
                ActivityStatus = ActivityStatus.Created,
                EmployeeId = agentAdminDetails.EmployeeId,
                ExecutedBy = agentAdminDetails.CreatedBy,
                ExecutedUser = agentAdminDetails.EmployeeId,
                TimeStamp = DateTimeOffset.UtcNow,
                ActivityOrigin = agentAdminDetails.ActivityOrigin,
            };

            _activityLogRepository.CreateActivityLog(activityLog);

            AgentSchedulingGroupHistory AgentSchedulingGroupHistory = new AgentSchedulingGroupHistory
            {
                EmployeeId = agentAdminDetails.EmployeeId,
                AgentSchedulingGroupId = agentAdminRequest.AgentSchedulingGroupId,
                StartDate = await GetCurrentDateOfTimezone(agentSchedulingGroup.TimezoneId),
                EndDate = null,
                CreatedBy = agentAdminDetails.CreatedBy,
                //CreatedDate = await GetCurrentTimeOfTimezone(agentSchedulingGroupBasedonSkillTag.TimezoneId),
                CreatedDate = DateTime.UtcNow,
                ActivityOrigin = ActivityOrigin.CSS
            };

            _agentSchedulingGroupHistoryRepository.UpdateAgentSchedulingGroupHistory(AgentSchedulingGroupHistory);

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

            // get preupdated details
            var preUpdateAgentAdminHireDate = agentAdmin.AgentData.Find(x => x.Group.Description == "Hire Date")?.Group?.Value?.ToString();
            var preUpdateAsgId = agentAdmin.AgentSchedulingGroupId;
            var preUpdateAgentAdmin = new PreUpdateAgentAdmin()
            {
                EmployeeId = agentAdmin.Ssn,
                FirstName = agentAdmin.FirstName,
                LastName = agentAdmin.LastName,
                ClientId = agentAdmin.ClientId,
                ClientLobGroupId = agentAdmin.ClientLobGroupId,
                SkillGroupId = agentAdmin.SkillGroupId,
                SkillTagId = agentAdmin.SkillTagId,
                AgentSchedulingGroupId = agentAdmin.AgentSchedulingGroupId,
                Sso = agentAdmin.Sso,
                SupervisorId = agentAdmin.SupervisorId,
                SupervisorSso = agentAdmin.SupervisorSso,
                SupervisorName = agentAdmin.SupervisorName
            };

            var agentAdminEmployeeIdDetails = new EmployeeIdDetails { Id = agentAdminDetails.EmployeeId };
            var agentAdminSsoDetails = new AgentAdminSsoDetails { Sso = agentAdminDetails.Sso };
            var agentSchedulingGroupIdDetails = new AgentSchedulingGroupIdDetails { AgentSchedulingGroupId = agentAdminDetails.AgentSchedulingGroupId };
            var employeeIdDetails = new EmployeeIdDetails { Id = agentAdmin.Ssn };
            var newEmployeeIdDetails = new EmployeeIdDetails { Id = agentAdminDetails.EmployeeId };

            var agentAdminsBasedOnEmployeeId = await _agentAdminRepository.GetAgentAdminByEmployeeId(agentAdminEmployeeIdDetails);

            if (agentAdminsBasedOnEmployeeId != null &&
                !string.Equals(agentAdminsBasedOnEmployeeId.Id.ToString(), agentAdminIdDetails.AgentAdminId))
            {
                return new CSSResponse($"Agent Admin with Employee ID '{agentAdminEmployeeIdDetails.Id}' already exists.", HttpStatusCode.Conflict);
            }

            var agentAdminsBasedonSSO = await _agentAdminRepository.GetAgentAdminBySso(agentAdminSsoDetails);

            if (agentAdminsBasedonSSO != null &&
                !string.Equals(agentAdminsBasedonSSO.Id.ToString(), agentAdminIdDetails.AgentAdminId))
            {
                return new CSSResponse($"Agent Admin with SSO '{agentAdminDetails.Sso}' already exists.", HttpStatusCode.Conflict);
            }

            if (agentAdminDetails.Sso == agentAdminDetails.SupervisorSso)
            {
                return new CSSResponse($"Please enter a unique email address for SSO and Team Lead SSO.", HttpStatusCode.Conflict);
            }

            var agentSchedulingGroup = await _agentSchedulingGroupRepository.GetAgentSchedulingGroup(agentSchedulingGroupIdDetails);

            if (agentSchedulingGroup == null)
            {
                return new CSSResponse($"No Scheduling Group match to this record. Please create before you proceed.", HttpStatusCode.NotFound);
            }

            var agentAdminRequest = _mapper.Map(agentAdminDetails, agentAdmin);
            agentAdminRequest.ClientId = agentSchedulingGroup.ClientId;
            agentAdminRequest.ClientLobGroupId = agentSchedulingGroup.ClientLobGroupId;
            agentAdminRequest.SkillGroupId = agentSchedulingGroup.SkillGroupId;
            agentAdminRequest.SkillTagId = agentSchedulingGroup.SkillTagId;

            _agentAdminRepository.UpdateAgentAdmin(agentAdminRequest);

            bool isASGIdDifferent = preUpdateAsgId != agentAdminRequest.AgentSchedulingGroupId;
            bool isEmployeeIdDifferent = preUpdateAgentAdmin.EmployeeId != agentAdminDetails.EmployeeId;
            bool isFirstNameDifferent = !(preUpdateAgentAdmin.FirstName.Equals(agentAdminDetails.FirstName));
            bool isLastNameDifferent = !(preUpdateAgentAdmin.LastName.Equals(agentAdminDetails.LastName));

            if (isASGIdDifferent || isEmployeeIdDifferent || isFirstNameDifferent || isLastNameDifferent)
            {
                DateTime movingDate = await FindMovingDateBasedonTimezone(preUpdateAsgId);

                List<AgentScheduleRange> updatedRanges = null;

                AgentSchedule agentSchedule = await _agentScheduleRepository.GetAgentScheduleByEmployeeId(employeeIdDetails);
                if (agentSchedule != null && agentSchedule.Ranges != null && agentSchedule.Ranges.Count > 0)
                {
                    updatedRanges = ScheduleHelper.GenerateAgentScheduleRanges(movingDate, agentAdminRequest.AgentSchedulingGroupId, agentSchedule.Ranges);

                    var updateAgentScheduleEmployeeDetails = new UpdateAgentScheduleEmployeeDetails
                    {
                        EmployeeId = agentAdminDetails.EmployeeId,
                        FirstName = agentAdminDetails.FirstName,
                        LastName = agentAdminDetails.LastName,
                        AgentSchedulingGroupId = agentAdminRequest.AgentSchedulingGroupId,
                        Ranges = updatedRanges,
                        ModifiedBy = agentAdminDetails.ModifiedBy
                    };

                    _agentScheduleRepository.UpdateAgentScheduleWithRanges(employeeIdDetails, updateAgentScheduleEmployeeDetails);
                }

                var updateAgentScheduleManagerEmployeeDetails = new UpdateAgentScheduleManagerEmployeeDetails
                {
                    EmployeeId = agentAdminDetails.EmployeeId,
                    AgentSchedulingGroupId = agentAdminRequest.AgentSchedulingGroupId,
                    MovingDate = movingDate,
                    ModifiedBy = agentAdminDetails.ModifiedBy
                };

                _agentScheduleManagerRepository.UpdateAgentScheduleManagerFromMovingDate(employeeIdDetails, updateAgentScheduleManagerEmployeeDetails);

                AgentSchedulingGroupHistory AgentSchedulingGroupHistory = new AgentSchedulingGroupHistory
                {
                    EmployeeId = agentAdminDetails.EmployeeId,
                    AgentSchedulingGroupId = agentAdminRequest.AgentSchedulingGroupId,
                    StartDate = movingDate,
                    EndDate = null,
                    CreatedBy = agentAdminDetails.ModifiedBy,
                    //CreatedDate = await FindCurrentTimeOfSchedulingGroup(movingAgent.AgentSchedulingGroupId),
                    CreatedDate = DateTime.UtcNow,
                    ActivityOrigin = ActivityOrigin.CSS
                };

                _agentSchedulingGroupHistoryRepository.UpdateAgentSchedulingGroupHistory(AgentSchedulingGroupHistory);
            }

            if (employeeIdDetails.Id != newEmployeeIdDetails.Id)
            {
                _activityLogRepository.UpdateActivityLogsEmployeeId(employeeIdDetails, newEmployeeIdDetails);
            }

            var fieldDetails = addActivityLogFields(preUpdateAgentAdmin, agentAdminRequest, preUpdateAgentAdminHireDate);

            var activityLog = new ActivityLog()
            {
                ActivityType = ActivityType.AgentAdmin,
                FieldDetails = fieldDetails,
                ActivityStatus = ActivityStatus.Updated,
                EmployeeId = agentAdminDetails.EmployeeId,
                ExecutedBy = agentAdminDetails.ModifiedBy,
                ExecutedUser = agentAdminDetails.EmployeeId,
                TimeStamp = DateTimeOffset.UtcNow,
                ActivityOrigin = agentAdminDetails.ActivityOrigin,
            };

            _activityLogRepository.CreateActivityLog(activityLog);

            await _uow.Commit();

            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Adds the activity log fields.
        /// </summary>
        /// <param name="preUpdateDetails">The pre update details.</param>
        /// <param name="updatedDetails">The updated details.</param>
        /// <param name="preUpdateAgentAdminHireDate">The pre update agent admin hire date.</param>
        /// <returns></returns>
        private List<FieldDetail> addActivityLogFields(PreUpdateAgentAdmin preUpdateDetails, Agent updatedDetails, string preUpdateAgentAdminHireDate)
        {
            var fielDetails = new List<FieldDetail>();
            var agentUpdatedHireDate = updatedDetails.AgentData.Find(x => x.Group.Description == "Hire Date")?.Group?.Value;

            if (!string.IsNullOrWhiteSpace(agentUpdatedHireDate))
            {
                if (preUpdateDetails != null)
                {
                    if (preUpdateAgentAdminHireDate != agentUpdatedHireDate)
                    {
                        var field = new FieldDetail()
                        {
                            Name = "Hire Date",
                            OldValue = preUpdateAgentAdminHireDate != null ? preUpdateAgentAdminHireDate.ToString() : "",
                            NewValue = agentUpdatedHireDate.ToString()
                        };
                        fielDetails.Add(field);
                    }

                    if (preUpdateDetails.EmployeeId != updatedDetails.Ssn)
                    {
                        var field = new FieldDetail()
                        {
                            Name = "EmployeeId",
                            OldValue = preUpdateDetails != null ? preUpdateDetails.EmployeeId.ToString() : "",
                            NewValue = updatedDetails.Ssn.ToString()
                        };
                        fielDetails.Add(field);
                    }

                    if (preUpdateDetails.FirstName != updatedDetails.FirstName)
                    {
                        var field = new FieldDetail()
                        {
                            Name = "FirstName",
                            OldValue = preUpdateDetails != null ? preUpdateDetails.FirstName.ToString() : "",
                            NewValue = updatedDetails.FirstName.ToString()
                        };
                        fielDetails.Add(field);
                    }

                    if (preUpdateDetails.LastName != updatedDetails.LastName)
                    {
                        var field = new FieldDetail()
                        {
                            Name = "LastName",
                            OldValue = preUpdateDetails != null ? preUpdateDetails.LastName.ToString() : "",
                            NewValue = updatedDetails.LastName.ToString()
                        };
                        fielDetails.Add(field);
                    }


                    if (preUpdateDetails.ClientId != updatedDetails.ClientId)
                    {
                        var field = new FieldDetail()
                        {
                            Name = "ClientId",
                            OldValue = preUpdateDetails != null ? preUpdateDetails.ClientId.ToString() : "",
                            NewValue = updatedDetails.ClientId.ToString()
                        };
                        fielDetails.Add(field);
                    }

                    if (preUpdateDetails.ClientLobGroupId != updatedDetails.ClientLobGroupId)
                    {
                        var field = new FieldDetail()
                        {
                            Name = "ClientLobGroupId",
                            OldValue = preUpdateDetails != null ? preUpdateDetails.ClientLobGroupId.ToString() : "",
                            NewValue = updatedDetails.ClientLobGroupId.ToString()
                        };
                        fielDetails.Add(field);
                    }

                    if (preUpdateDetails.SkillGroupId != updatedDetails.SkillGroupId)
                    {
                        var field = new FieldDetail()
                        {
                            Name = "SkillGroupId",
                            OldValue = preUpdateDetails != null ? preUpdateDetails.SkillGroupId.ToString() : "",
                            NewValue = updatedDetails.SkillGroupId.ToString()
                        };
                        fielDetails.Add(field);
                    }

                    if (preUpdateDetails.SkillTagId != updatedDetails.SkillTagId)
                    {
                        var field = new FieldDetail()
                        {
                            Name = "SkillTagId",
                            OldValue = preUpdateDetails != null ? preUpdateDetails.SkillTagId.ToString() : "",
                            NewValue = updatedDetails.SkillTagId.ToString()
                        };
                        fielDetails.Add(field);
                    }

                    if (preUpdateDetails.AgentSchedulingGroupId != updatedDetails.AgentSchedulingGroupId)
                    {
                        var field = new FieldDetail()
                        {
                            Name = "AgentSchedulingGroupId",
                            OldValue = preUpdateDetails != null ? preUpdateDetails.AgentSchedulingGroupId.ToString() : "",
                            NewValue = updatedDetails.AgentSchedulingGroupId.ToString()
                        };
                        fielDetails.Add(field);
                    }

                    if (preUpdateDetails.Sso != updatedDetails.Sso)
                    {
                        var field = new FieldDetail()
                        {
                            Name = "Sso",
                            OldValue = preUpdateDetails != null ? preUpdateDetails.Sso.ToString() : "",
                            NewValue = updatedDetails.Sso.ToString()
                        };
                        fielDetails.Add(field);
                    }

                    if (preUpdateDetails.SupervisorId != updatedDetails.SupervisorId)
                    {
                        var field = new FieldDetail()
                        {
                            Name = "SupervisorId",
                            OldValue = preUpdateDetails != null ? preUpdateDetails.SupervisorId.ToString() : "",
                            NewValue = updatedDetails.SupervisorId.ToString()
                        };
                        fielDetails.Add(field);
                    }

                    if (preUpdateDetails.SupervisorSso != updatedDetails.SupervisorSso)
                    {
                        var field = new FieldDetail()
                        {
                            Name = "SupervisorSso",
                            OldValue = preUpdateDetails != null ? preUpdateDetails.SupervisorSso.ToString() : "",
                            NewValue = updatedDetails.SupervisorSso.ToString()
                        };
                        fielDetails.Add(field);
                    }

                    if (preUpdateDetails.SupervisorName != updatedDetails.SupervisorName)
                    {
                        var field = new FieldDetail()
                        {
                            Name = "SupervisorName",
                            OldValue = preUpdateDetails != null ? preUpdateDetails.SupervisorName.ToString() : "",
                            NewValue = updatedDetails.SupervisorName.ToString()
                        };
                        fielDetails.Add(field);
                    }
                }
                else if (preUpdateDetails == null)
                {
                    var createFieldDetails = new List<FieldDetail>()
                    {
                        new FieldDetail()
                        {
                            Name = "Hire Date",
                            OldValue = "",
                            NewValue = agentUpdatedHireDate.ToString()
                        },
                         new FieldDetail()
                        {
                            Name = "EmployeeId",
                            OldValue = "",
                            NewValue = updatedDetails.Ssn.ToString()
                        },
                        new FieldDetail()
                        {
                            Name = "FirstName",
                            OldValue = "",
                            NewValue = updatedDetails.FirstName.ToString()
                        },
                        new FieldDetail()
                        {
                            Name = "LastName",
                            OldValue = "",
                            NewValue = updatedDetails.LastName.ToString()
                        },
                        new FieldDetail()
                        {
                            Name = "ClientId",
                            OldValue = "",
                            NewValue = updatedDetails.ClientId.ToString()
                        },
                        new FieldDetail()
                        {
                            Name = "ClientLobGroupId",
                            OldValue = "",
                            NewValue = updatedDetails.ClientLobGroupId.ToString()
                        },
                        new FieldDetail()
                        {
                            Name = "SkillGroupId",
                            OldValue = "",
                            NewValue = updatedDetails.SkillGroupId.ToString()
                        },
                        new FieldDetail()
                        {
                            Name = "SkillTagId",
                            OldValue = "",
                            NewValue = updatedDetails.SkillTagId.ToString()
                        },
                        new FieldDetail()
                        {
                            Name = "AgentSchedulingGroupId",
                            OldValue = "",
                            NewValue = updatedDetails.AgentSchedulingGroupId.ToString()
                        },
                        new FieldDetail()
                        {
                            Name = "Sso",
                            OldValue = "",
                            NewValue = updatedDetails.Sso.ToString()
                        },

                        new FieldDetail()
                        {
                            Name = "SupervisorId",
                            OldValue = "",
                            NewValue = updatedDetails.SupervisorId.ToString()
                        },
                        new FieldDetail()
                        {
                            Name = "SupervisorSso",
                            OldValue = "",
                            NewValue = updatedDetails.SupervisorSso.ToString()
                        },
                        new FieldDetail()
                        {
                            Name = "SupervisorName",
                            OldValue = "",
                            NewValue = updatedDetails.SupervisorName.ToString()
                        }

                };

                    fielDetails = createFieldDetails;
                }
            }

            return fielDetails;
        }

        /// <summary>Moves the agent admins.</summary>
        /// <param name="moveAgentAdminsDetails">The move agent admins details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<CSSResponse> MoveAgentAdmins(MoveAgentAdminsDetails moveAgentAdminsDetails)
        {
            var sourceSchedulingGroup = await _agentSchedulingGroupRepository.GetAgentSchedulingGroup(
                new AgentSchedulingGroupIdDetails { AgentSchedulingGroupId = moveAgentAdminsDetails.SourceSchedulingGroupId });

            if (sourceSchedulingGroup == null)
            {
                return new CSSResponse($"Source Scheduling Group does not exists.", HttpStatusCode.NotFound);
            }

            var destinationSchedulingGroup = await _agentSchedulingGroupRepository.GetAgentSchedulingGroup(
                new AgentSchedulingGroupIdDetails { AgentSchedulingGroupId = moveAgentAdminsDetails.DestinationSchedulingGroupId });

            if (destinationSchedulingGroup == null)
            {
                return new CSSResponse($"Destination Scheduling Group does not exists.", HttpStatusCode.NotFound);
            }

            List<ObjectId> agentIds = moveAgentAdminsDetails.AgentAdminIds.Select(id => new ObjectId(id)).ToList();

            var agentAdminsInSourceSchedulingGroup
                = await _agentAdminRepository.GetAgentAdminsByIds(agentIds, moveAgentAdminsDetails.SourceSchedulingGroupId);

            if (agentAdminsInSourceSchedulingGroup == null)
            {
                return new CSSResponse($"Agent admins does not exists in source scheduling group.", HttpStatusCode.NotFound);
            }

            if (agentAdminsInSourceSchedulingGroup.Count != moveAgentAdminsDetails.AgentAdminIds.Count)
            {
                return new CSSResponse("One of the agent admins does not exists in source scheduling group.", HttpStatusCode.NotFound);
            }

            foreach (Agent movingAgent in agentAdminsInSourceSchedulingGroup)
            {
                movingAgent.AgentSchedulingGroupId = moveAgentAdminsDetails.DestinationSchedulingGroupId;
                movingAgent.SkillTagId = destinationSchedulingGroup.SkillTagId;
                movingAgent.ModifiedBy = moveAgentAdminsDetails.ModifiedBy;
                movingAgent.ModifiedDate = DateTime.Now;

                _agentAdminRepository.UpdateAgentAdmin(movingAgent);

                DateTime movingDate = await FindMovingDateBasedonTimezone(moveAgentAdminsDetails.SourceSchedulingGroupId);

                List<AgentScheduleRange> updatedRanges = null;
                EmployeeIdDetails employeeIdDetails = new EmployeeIdDetails
                {
                    Id = movingAgent.Ssn
                };

                AgentSchedule agentSchedule = await _agentScheduleRepository.GetAgentScheduleByEmployeeId(employeeIdDetails);
                if (agentSchedule != null && agentSchedule.Ranges != null && agentSchedule.Ranges.Count > 0)
                {
                    updatedRanges = ScheduleHelper.GenerateAgentScheduleRanges(movingDate, moveAgentAdminsDetails.DestinationSchedulingGroupId, agentSchedule.Ranges);

                    var updateAgentScheduleEmployeeDetails = new UpdateAgentScheduleEmployeeDetails
                    {
                        EmployeeId = movingAgent.Ssn,
                        FirstName = movingAgent.FirstName,
                        LastName = movingAgent.LastName,
                        AgentSchedulingGroupId = movingAgent.AgentSchedulingGroupId,
                        Ranges = updatedRanges,
                        ModifiedBy = movingAgent.ModifiedBy
                    };

                    _agentScheduleRepository.UpdateAgentScheduleWithRanges(employeeIdDetails, updateAgentScheduleEmployeeDetails);
                }



                var updateAgentScheduleManagerEmployeeDetails = new UpdateAgentScheduleManagerEmployeeDetails
                {
                    EmployeeId = movingAgent.Ssn,
                    AgentSchedulingGroupId = movingAgent.AgentSchedulingGroupId,
                    MovingDate = movingDate,
                    ModifiedBy = movingAgent.ModifiedBy
                };

                _agentScheduleManagerRepository.UpdateAgentScheduleManagerFromMovingDate(employeeIdDetails, updateAgentScheduleManagerEmployeeDetails);

                AgentSchedulingGroupHistory AgentSchedulingGroupHistory = new AgentSchedulingGroupHistory
                {
                    EmployeeId = movingAgent.Ssn,
                    AgentSchedulingGroupId = movingAgent.AgentSchedulingGroupId,
                    StartDate = movingDate,
                    EndDate = null,
                    CreatedBy = moveAgentAdminsDetails.ModifiedBy,
                    //CreatedDate = await FindCurrentTimeOfSchedulingGroup(movingAgent.AgentSchedulingGroupId),
                    CreatedDate = DateTime.UtcNow,
                    ActivityOrigin = ActivityOrigin.CSS
                };

                _agentSchedulingGroupHistoryRepository.UpdateAgentSchedulingGroupHistory(AgentSchedulingGroupHistory);
            }

            await _uow.Commit();

            return new CSSResponse(HttpStatusCode.NoContent);

        }

        /// <summary>Finds the moving date basedon timezone.</summary>
        /// <param name="schedulingGroupId">The scheduling group identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        private async Task<DateTime> FindMovingDateBasedonTimezone(int schedulingGroupId)
        {
            AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails = new AgentSchedulingGroupIdDetails
            {
                AgentSchedulingGroupId = schedulingGroupId
            };
            AgentSchedulingGroup asg = await _agentSchedulingGroupRepository.GetAgentSchedulingGroup(agentSchedulingGroupIdDetails);
            int sourceASGTimezoneId = asg.TimezoneId;
            return await GetCurrentDateOfTimezone(sourceASGTimezoneId);
        }

        /// <summary>Gets the current date of timezone.</summary>
        /// <param name="timezoneId">The timezone identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        private async Task<DateTime> GetCurrentDateOfTimezone(int timezoneId)
        {
            TimezoneIdDetails timezoneIdDetails = new TimezoneIdDetails
            {
                TimezoneId = timezoneId
            };
            Timezone timezone = await _timezoneRepository.GetTimeZone(timezoneIdDetails);

            DateTime currentTime = DateTime.UtcNow.Add(timezone.UtcOffset);
            DateTime currentDateOfTimezone = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 0, 0, 0, DateTimeKind.Utc);
            return currentDateOfTimezone;
        }

        /// <summary>Finds the current time of scheduling group.</summary>
        /// <param name="schedulingGroupId">The scheduling group identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        private async Task<DateTime> FindCurrentTimeOfSchedulingGroup(int schedulingGroupId)
        {
            AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails = new AgentSchedulingGroupIdDetails
            {
                AgentSchedulingGroupId = schedulingGroupId
            };
            AgentSchedulingGroup asg = await _agentSchedulingGroupRepository.GetAgentSchedulingGroup(agentSchedulingGroupIdDetails);
            int timezoneId = asg.TimezoneId;
            return await GetCurrentTimeOfTimezone(timezoneId);
        }

        /// <summary>Gets the current time of timezone.</summary>
        /// <param name="timezoneId">The timezone identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        private async Task<DateTime> GetCurrentTimeOfTimezone(int timezoneId)
        {
            TimezoneIdDetails timezoneIdDetails = new TimezoneIdDetails
            {
                TimezoneId = timezoneId
            };
            Timezone timezone = await _timezoneRepository.GetTimeZone(timezoneIdDetails);

            DateTime currentTime = DateTime.UtcNow.Add(timezone.UtcOffset);
            return currentTime;
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

            var agentAdmins = await _agentAdminRepository.GetAgentAdminByEmployeeId(agentAdminEmployeeIdDetails);

            if (agentAdmins == null)
            {
                return new CSSResponse($"Agent Admin with Employee id '{agentAdminEmployeeIdDetails.Id}' does not exists.", HttpStatusCode.NotFound);
            }

            var activityLogRequest = _mapper.Map<ActivityLog>(agentActivityLogDetails);
            activityLogRequest.ActivityType = ActivityType.AgentAdmin;

            _activityLogRepository.CreateActivityLog(activityLogRequest);

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