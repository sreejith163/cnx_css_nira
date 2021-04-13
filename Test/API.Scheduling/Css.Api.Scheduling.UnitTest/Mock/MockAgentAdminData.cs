using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Core.Models.DTO.Response;
using Css.Api.Core.Models.Enums;
using Css.Api.Core.Utilities.Extensions;
using Css.Api.Scheduling.Business.UnitTest.Mocks;
using Css.Api.Scheduling.Models.Domain;
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
using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.UnitTest.Mock
{
    public class MockAgentAdminData
    {

        /// <summary>Gets the agent admins.</summary>
        /// <param name="agentAdminQueryParameter">The agent admin query parameter.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public CSSResponse GetAgentAdmins(AgentAdminQueryParameter agentAdminQueryParameter)
        {
            var agentAdmins = new MockDataContext().GetAgentAdmins(agentAdminQueryParameter);
            return new CSSResponse(agentAdmins, HttpStatusCode.OK);
        }

        /// <summary>Gets the agent admin.</summary>
        /// <param name="agentAdminIdDetails">The agent admin identifier details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public CSSResponse GetAgentAdmin(AgentAdminIdDetails agentAdminIdDetails)
        {
            var agentAdmin = new MockDataContext().GetAgentAdmin(agentAdminIdDetails);
            if (agentAdmin == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var skillTag = new MockDataContext().GetSkillTag(new SkillTagIdDetails
            {
                SkillTagId = agentAdmin.SkillTagId
            });

            if (skillTag == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var skillGroup = new MockDataContext().GetSkillGroup(new SkillGroupIdDetails
            {
                SkillGroupId = skillTag.SkillGroupId
            });

            if (skillGroup == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var clientLobGroup = new MockDataContext().GetClientLobGroup(new ClientLobGroupIdDetails
            {
                ClientLobGroupId = skillGroup.ClientLobGroupId
            });

            if (clientLobGroup == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var clientName = new MockDataContext().GetClient(new ClientIdDetails
            {
                ClientId = clientLobGroup.ClientId
            });

            if (clientName == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var mappedAgentAdmin = JsonConvert.DeserializeObject<AgentAdminDetailsDTO>(JsonConvert.SerializeObject(agentAdmin));

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

        /// <summary>Gets the agent admin by employee identifier.</summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public CSSResponse GetAgentAdminByEmployeeId(EmployeeIdDetails employeeIdDetails)
        {
            var agentAdmin = new MockDataContext().GetAgentAdminByEmployeeId(employeeIdDetails);
            if (agentAdmin == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }
            var mappedAgentAdmin = JsonConvert.DeserializeObject<AgentAdminDetailsDTO>(JsonConvert.SerializeObject(agentAdmin));

            return new CSSResponse(mappedAgentAdmin, HttpStatusCode.OK);
        }

        /// <summary>Creates the agent admin.</summary>
        /// <param name="agentAdminDetails">The agent admin details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public CSSResponse CreateAgentAdmin(CreateAgentAdmin agentAdminDetails)
        {
            var agentAdminEmployeeIdDetails = new EmployeeIdDetails { Id = agentAdminDetails.EmployeeId };
            var agentAdminSsoDetails = new AgentAdminSsoDetails { Sso = agentAdminDetails.Sso };
            var agentSchedulingGroupIdDetails = new AgentSchedulingGroupIdDetails { AgentSchedulingGroupId = agentAdminDetails.AgentSchedulingGroupId };

            var agentAdminsBasedOnEmployeeId = new MockDataContext().
                GetAgentAdminIdsByEmployeeIdAndSso(agentAdminEmployeeIdDetails, agentAdminSsoDetails);

            if (agentAdminsBasedOnEmployeeId != null)
            {
                return new CSSResponse($"Agent Admin with Employee ID '{agentAdminEmployeeIdDetails.Id}' and SSO '{agentAdminDetails.Sso}' already exists.", HttpStatusCode.Conflict);
            }

            var agentAdminsBasedonEmpId = new MockDataContext().GetAgentAdminByEmployeeId(agentAdminEmployeeIdDetails);

            if (agentAdminsBasedonEmpId != null)
            {
                return new CSSResponse($"Agent Admin with Employee ID '{agentAdminEmployeeIdDetails.Id}' already exists.", HttpStatusCode.Conflict);
            }
            var agentAdminsBasedonSSO = new MockDataContext().GetAgentAdminBySso(agentAdminSsoDetails);

            if (agentAdminsBasedonSSO != null)
            {
                return new CSSResponse($"Agent Admin with SSO '{agentAdminDetails.Sso}' already exists.", HttpStatusCode.Conflict);
            }
            if (agentAdminDetails.Sso == agentAdminDetails.SupervisorSso)
            {
                return new CSSResponse($"Please enter a unique email address for SSO and Team Lead SSO.", HttpStatusCode.Conflict);
            }

            var agentSchedulingGroup = new MockDataContext().GetAgentSchedulingGroup(agentSchedulingGroupIdDetails);

            if (agentSchedulingGroup == null)
            {
                return new CSSResponse($"No Scheduling Group match to this record. Please create before you proceed.", HttpStatusCode.NotFound);
            }

            var mappedAgentAdmin = JsonConvert.DeserializeObject<Agent>(JsonConvert.SerializeObject(agentAdminDetails));


            mappedAgentAdmin.ClientId = agentSchedulingGroup.ClientId;
            mappedAgentAdmin.ClientLobGroupId = agentSchedulingGroup.ClientLobGroupId;
            mappedAgentAdmin.SkillGroupId = agentSchedulingGroup.SkillGroupId;
            mappedAgentAdmin.SkillTagId = agentSchedulingGroup.SkillTagId;


            new MockDataContext().CreateAgentAdmin(mappedAgentAdmin);
            var agentScheduleRequest = JsonConvert.DeserializeObject<AgentSchedule>(JsonConvert.SerializeObject(mappedAgentAdmin));

            new MockDataContext().CreateAgentSchedule(agentScheduleRequest);

            var activityLog = new ActivityLog()
            {
                ActivityType = ActivityType.AgentAdmin,
                ActivityStatus = ActivityStatus.Created,
                EmployeeId = agentAdminDetails.EmployeeId,
                ExecutedBy = agentAdminDetails.CreatedBy,
                ExecutedUser = agentAdminDetails.EmployeeId,
                TimeStamp = DateTimeOffset.UtcNow,
                ActivityOrigin = agentAdminDetails.ActivityOrigin,
            };

            new MockDataContext().CreateActivityLog(activityLog);

            AgentSchedulingGroupHistory AgentSchedulingGroupHistory = new AgentSchedulingGroupHistory
            {
                EmployeeId = agentAdminDetails.EmployeeId,
                AgentSchedulingGroupId = mappedAgentAdmin.AgentSchedulingGroupId,
                StartDate = DateTime.UtcNow.Date,
                EndDate = null,
                CreatedBy = agentAdminDetails.CreatedBy,
                //CreatedDate = await GetCurrentTimeOfTimezone(agentSchedulingGroupBasedonSkillTag.TimezoneId),
                CreatedDate = DateTime.UtcNow,
                ActivityOrigin = ActivityOrigin.CSS
            };

            new MockDataContext().UpdateAgentSchedulingGroupHistory(AgentSchedulingGroupHistory);


            return new CSSResponse(new AgentAdminIdDetails { AgentAdminId = mappedAgentAdmin.Id.ToString() }, HttpStatusCode.Created);

        }

        /// <summary>
        /// Updates the agent admin.
        /// </summary>
        /// <param name="agentAdminIdDetails">The agent admin identifier details.</param>
        /// <param name="agentAdminDetails">The agent admin details.</param>
        /// <returns></returns>
        public CSSResponse UpdateAgentAdmin(AgentAdminIdDetails agentAdminIdDetails, UpdateAgentAdmin agentAdminDetails)
        {
            var agentAdmin = new MockDataContext().GetAgentAdmin(agentAdminIdDetails);
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

            var agentAdminsBasedOnEmployeeId = new MockDataContext().GetAgentAdminByEmployeeId(agentAdminEmployeeIdDetails);

            if (agentAdminsBasedOnEmployeeId != null &&
                !string.Equals(agentAdminsBasedOnEmployeeId.Id.ToString(), agentAdminIdDetails.AgentAdminId))
            {
                return new CSSResponse($"Agent Admin with Employee ID '{agentAdminEmployeeIdDetails.Id}' already exists.", HttpStatusCode.Conflict);
            }

            var agentAdminsBasedonSSO = new MockDataContext().GetAgentAdminBySso(agentAdminSsoDetails);

            if (agentAdminsBasedonSSO != null &&
                !string.Equals(agentAdminsBasedonSSO.Id.ToString(), agentAdminIdDetails.AgentAdminId))
            {
                return new CSSResponse($"Agent Admin with SSO '{agentAdminDetails.Sso}' already exists.", HttpStatusCode.Conflict);
            }

            if (agentAdminDetails.Sso == agentAdminDetails.SupervisorSso)
            {
                return new CSSResponse($"Please enter a unique email address for SSO and Team Lead SSO.", HttpStatusCode.Conflict);
            }

            var agentSchedulingGroup = new MockDataContext().GetAgentSchedulingGroup(agentSchedulingGroupIdDetails);

            if (agentSchedulingGroup == null)
            {
                return new CSSResponse($"No Scheduling Group match to this record. Please create before you proceed.", HttpStatusCode.NotFound);
            }

            var agentAdminRequest = JsonConvert.DeserializeObject<Agent>(JsonConvert.SerializeObject(agentAdminDetails));

            agentAdminRequest.ClientId = agentSchedulingGroup.ClientId;
            agentAdminRequest.ClientLobGroupId = agentSchedulingGroup.ClientLobGroupId;
            agentAdminRequest.SkillGroupId = agentSchedulingGroup.SkillGroupId;
            agentAdminRequest.SkillTagId = agentSchedulingGroup.SkillTagId;

            new MockDataContext().UpdateAgentAdmin(agentAdminRequest);

            if (preUpdateAsgId != agentAdminRequest.AgentSchedulingGroupId)
            {

                DateTime movingDate = DateTime.UtcNow.Date;

                List<AgentScheduleRange> updatedRanges = null;

                AgentSchedule agentSchedule = new MockDataContext().GetAgentScheduleByEmployeeId(employeeIdDetails);
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

                    new MockDataContext().UpdateAgentScheduleWithRanges(employeeIdDetails, updateAgentScheduleEmployeeDetails);
                }

                var updateAgentScheduleManagerEmployeeDetails = new UpdateAgentScheduleManagerEmployeeDetails
                {
                    EmployeeId = agentAdminDetails.EmployeeId,
                    AgentSchedulingGroupId = agentAdminRequest.AgentSchedulingGroupId,
                    MovingDate = movingDate,
                    ModifiedBy = agentAdminDetails.ModifiedBy
                };

                new MockDataContext().UpdateAgentScheduleManagerFromMovingDate(employeeIdDetails, updateAgentScheduleManagerEmployeeDetails);

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

                new MockDataContext().UpdateAgentSchedulingGroupHistory(AgentSchedulingGroupHistory);
            }

            if (employeeIdDetails.Id != newEmployeeIdDetails.Id)
            {
                new MockDataContext().UpdateActivityLogsEmployeeId(employeeIdDetails, newEmployeeIdDetails);
            }

            var activityLog = new ActivityLog()
            {
                ActivityType = ActivityType.AgentAdmin,
                ActivityStatus = ActivityStatus.Updated,
                EmployeeId = agentAdminDetails.EmployeeId,
                ExecutedBy = agentAdminDetails.ModifiedBy,
                ExecutedUser = agentAdminDetails.EmployeeId,
                TimeStamp = DateTimeOffset.UtcNow,
                ActivityOrigin = agentAdminDetails.ActivityOrigin,
            };

            new MockDataContext().CreateActivityLog(activityLog);

            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>Moves the agent admins.</summary>
        /// <param name="moveAgentAdminsDetails">The move agent admins details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public CSSResponse MoveAgentAdmins(MoveAgentAdminsDetails moveAgentAdminsDetails)
        {
            var sourceSchedulingGroup = new MockDataContext().GetAgentSchedulingGroup(
                new AgentSchedulingGroupIdDetails { AgentSchedulingGroupId = moveAgentAdminsDetails.SourceSchedulingGroupId });

            if (sourceSchedulingGroup == null)
            {
                return new CSSResponse($"Source Scheduling Group does not exists.", HttpStatusCode.NotFound);
            }

            var destinationSchedulingGroup = new MockDataContext().GetAgentSchedulingGroup(
                new AgentSchedulingGroupIdDetails { AgentSchedulingGroupId = moveAgentAdminsDetails.DestinationSchedulingGroupId });

            if (destinationSchedulingGroup == null)
            {
                return new CSSResponse($"Destination Scheduling Group does not exists.", HttpStatusCode.NotFound);
            }

            List<ObjectId> agentIds = moveAgentAdminsDetails.AgentAdminIds.Select(id => new ObjectId(id)).ToList();

            var agentAdminsInSourceSchedulingGroup
                = new MockDataContext().GetAgentAdminsByIds(agentIds, moveAgentAdminsDetails.SourceSchedulingGroupId);

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

                new MockDataContext().UpdateAgentAdmin(movingAgent);

                DateTime movingDate = DateTime.UtcNow.Date;

                List<AgentScheduleRange> updatedRanges = null;
                EmployeeIdDetails employeeIdDetails = new EmployeeIdDetails
                {
                    Id = movingAgent.Ssn
                };

                AgentSchedule agentSchedule = new MockDataContext().GetAgentScheduleByEmployeeId(employeeIdDetails);
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

                    new MockDataContext().UpdateAgentScheduleWithRanges(employeeIdDetails, updateAgentScheduleEmployeeDetails);
                }



                var updateAgentScheduleManagerEmployeeDetails = new UpdateAgentScheduleManagerEmployeeDetails
                {
                    EmployeeId = movingAgent.Ssn,
                    AgentSchedulingGroupId = movingAgent.AgentSchedulingGroupId,
                    MovingDate = movingDate,
                    ModifiedBy = movingAgent.ModifiedBy
                };

                new MockDataContext().UpdateAgentScheduleManagerFromMovingDate(employeeIdDetails, updateAgentScheduleManagerEmployeeDetails);

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

                new MockDataContext().UpdateAgentSchedulingGroupHistory(AgentSchedulingGroupHistory);
            }

            return new CSSResponse(HttpStatusCode.NoContent);

        }


        /// <summary>
        /// Deletes the agent admin.
        /// </summary>
        /// <param name="agentAdminIdDetails">The agent admin identifier details.</param>
        /// <returns></returns>
        public CSSResponse DeleteAgentAdmin(AgentAdminIdDetails agentAdminIdDetails)
        {
            var agentAdmin = new MockDataContext().GetAgentAdmin(agentAdminIdDetails);
            if (agentAdmin == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            agentAdmin.IsDeleted = true;

            new MockDataContext().UpdateAgentAdmin(agentAdmin);

            new MockDataContext().DeleteAgentSchedule(new EmployeeIdDetails { Id = agentAdmin.Ssn });

            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>Creates the agent activity log.</summary>
        /// <param name="agentActivityLogDetails">The agent activity log details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public CSSResponse CreateAgentActivityLog(CreateAgentActivityLog agentActivityLogDetails)
        {
            var agentAdminEmployeeIdDetails = new EmployeeIdDetails { Id = agentActivityLogDetails.EmployeeId };

            var agentAdmins = new MockDataContext().GetAgentAdminByEmployeeId(agentAdminEmployeeIdDetails);

            if (agentAdmins == null)
            {
                return new CSSResponse($"Agent Admin with Employee id '{agentAdminEmployeeIdDetails.Id}' does not exists.", HttpStatusCode.NotFound);
            }

            var activityLogRequest = JsonConvert.DeserializeObject<ActivityLog>(JsonConvert.SerializeObject(agentActivityLogDetails));
            activityLogRequest.ActivityType = ActivityType.AgentAdmin;


            new MockDataContext().CreateActivityLog(activityLogRequest);

            return new CSSResponse(new AgentActivityLogIdDetails { AgentActivityLogId = activityLogRequest.Id }, HttpStatusCode.Created);
        }

        /// <summary>Gets the agent activity logs.</summary>
        /// <param name="activityLogQueryParameter">The activity log query parameter.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public CSSResponse GetAgentActivityLogs(ActivityLogQueryParameter agentActivityLogQueryParameter)
        {
            agentActivityLogQueryParameter.ActivityType = ActivityType.AgentAdmin;
            var agentActivityLogs = new MockDataContext().GetActivityLogs(agentActivityLogQueryParameter);

            return new CSSResponse(agentActivityLogs, HttpStatusCode.OK);
        }
    }
}
