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
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedulingGroup;
using Css.Api.Scheduling.Models.DTO.Request.Client;
using Css.Api.Scheduling.Models.DTO.Request.ClientLobGroup;
using Css.Api.Scheduling.Models.DTO.Request.SkillGroup;
using Css.Api.Scheduling.Models.DTO.Request.SkillTag;
using Css.Api.Scheduling.Models.DTO.Response.AgentAdmin;
using Css.Api.Scheduling.Models.Enums;
using Css.Api.Scheduling.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
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

        /// <summary>
        /// The activity log repository
        /// </summary>
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
        /// Gets the agent admin by employee identifier.
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> GetAgentAdminByEmployeeId(EmployeeIdDetails employeeIdDetails)
        {
            var agentAdmin = await _agentAdminRepository.GetAgentAdminIdsByEmployeeId(employeeIdDetails);
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

            if (agentAdminDetails.Sso == agentAdminDetails.SupervisorSso)
            {
                return new CSSResponse($"Please enter a unique email address for SSO and Team Lead SSO.", HttpStatusCode.Conflict);
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
            var preUpdateAgentAdminHireDate = agentAdmin.AgentData.Find(x => x.Group.Description == "Hire Date").Group.Value.ToString();
            var preUpdateAgentAdmin = new UpdateAgentAdmin()
            {
                EmployeeId = agentAdmin.Ssn,
                FirstName = agentAdmin.FirstName,
                LastName = agentAdmin.LastName,
                ClientId = agentAdmin.ClientId,
                ClientLobGroupId = agentAdmin.ClientLobGroupId,
                SkillGroupId = agentAdmin.SkillGroupId,
                SkillTagId = agentAdmin.SkillTagId,
                Sso = agentAdmin.Sso,
                SupervisorId = agentAdmin.SupervisorId,
                SupervisorSso = agentAdmin.SupervisorSso,
                SupervisorName = agentAdmin.SupervisorName
            };

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

            if (agentAdminDetails.Sso == agentAdminDetails.SupervisorSso)
            {
                return new CSSResponse($"Please enter a unique email address for SSO and Team Lead SSO.", HttpStatusCode.Conflict);
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
                FirstName = agentAdminDetails.FirstName,
                LastName = agentAdminDetails.LastName,
                AgentSchedulingGroupId = agentSchedulingGroupBasedonSkillTag.AgentSchedulingGroupId,
                ModifiedBy = agentAdminDetails.ModifiedBy
            };

            _agentScheduleRepository.UpdateAgentSchedule(employeeIdDetails, updateAgentScheduleEmployeeDetails);


            // get the preUpdated details and compare it with the updated details to check changes
            var fieldDetails = addActivityLogFields(preUpdateAgentAdmin, agentAdminRequest, preUpdateAgentAdminHireDate);

            // create activity log based on the changed fields
            var activityLog = new ActivityLog() {
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

        private List<FieldDetail> addActivityLogFields(UpdateAgentAdmin preUpdateDetails, Agent updatedDetails, string preUpdateAgentAdminHireDate)
        {
            var fielDetails = new List<FieldDetail>();
            var agentUpdatedHireDate = updatedDetails.AgentData.Find(x => x.Group.Description == "Hire Date").Group.Value;

            if (preUpdateDetails != null){
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
            }else if(preUpdateDetails == null)
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
                movingAgent.MovedDate = DateTime.Now;

                var updateAgentScheduleEmployeeDetails = new UpdateAgentScheduleEmployeeDetails
                {
                    EmployeeId = movingAgent.Ssn,
                    FirstName = movingAgent.FirstName,
                    LastName = movingAgent.LastName,
                    AgentSchedulingGroupId = movingAgent.AgentSchedulingGroupId,
                    ModifiedBy = movingAgent.ModifiedBy
                };

                var employeeIdDetails = new EmployeeIdDetails { Id = movingAgent.Ssn };

                _agentAdminRepository.UpdateAgentAdmin(movingAgent);
                _agentScheduleRepository.UpdateAgentSchedule(employeeIdDetails, updateAgentScheduleEmployeeDetails);
            }

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