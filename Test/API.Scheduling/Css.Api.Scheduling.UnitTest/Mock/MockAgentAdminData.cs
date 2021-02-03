using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Core.Models.DTO.Response;
using Css.Api.Scheduling.Business.UnitTest.Mocks;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.ActivityLog;
using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedule;
using Css.Api.Scheduling.Models.DTO.Request.Client;
using Css.Api.Scheduling.Models.DTO.Request.ClientLobGroup;
using Css.Api.Scheduling.Models.DTO.Request.SkillGroup;
using Css.Api.Scheduling.Models.DTO.Request.SkillTag;
using Css.Api.Scheduling.Models.DTO.Response.AgentAdmin;
using System.Net;

namespace Css.Api.Scheduling.UnitTest.Mock
{
    public class MockAgentAdminData
    {
        public CSSResponse GetAgentAdmins(AgentAdminQueryParameter agentAdminQueryParameter)
        {
            var agentAdmins = new MockDataContext().GetAgentAdmins(agentAdminQueryParameter);

            return new CSSResponse(agentAdmins, HttpStatusCode.OK);
        }

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

            var mappedAgentAdmin = new AgentAdminDetailsDTO
            {
                SkillTagId = skillTag.SkillTagId,
                SkillTagName = skillTag.Name,
                SkillGroupId = skillGroup.SkillGroupId,
                SkillGroupName = skillGroup.Name,
                ClientLobGroupId = clientLobGroup.ClientLobGroupId,
                ClientLobGroupName = clientLobGroup.Name,
                ClientId = clientName.ClientId,
                ClientName = clientName.Name
            };

            return new CSSResponse(mappedAgentAdmin, HttpStatusCode.OK);
        }

        public CSSResponse GetAgentAdminByEmployeeId(EmployeeIdDetails employeeIdDetails)
        {
            var agentAdmin = new MockDataContext().GetAgentAdminIdsByEmployeeId(employeeIdDetails);
            if (agentAdmin == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            return new CSSResponse(agentAdmin, HttpStatusCode.OK);
        }

        public CSSResponse CreateAgentAdmin(CreateAgentAdmin agentAdminDetails)
        {
            var agentAdminEmployeeIdDetails = new EmployeeIdDetails { Id = agentAdminDetails.EmployeeId };
            var agentAdminSsoDetails = new AgentAdminSsoDetails { Sso = agentAdminDetails.Sso };
            var skillTagIdDetails = new SkillTagIdDetails { SkillTagId = agentAdminDetails.SkillTagId };

            var agentAdminsBasedOnEmployeeId = new MockDataContext().GetAgentAdminIdsByEmployeeId(agentAdminEmployeeIdDetails);

            if (agentAdminsBasedOnEmployeeId != null)
            {
                return new CSSResponse($"Agent Admin with Employee ID '{agentAdminEmployeeIdDetails.Id}' already exists.", HttpStatusCode.Conflict);
            }

            var agentAdminsBasedonSSO = new MockDataContext().GetAgentAdminIdsBySso(agentAdminSsoDetails);

            if (agentAdminsBasedonSSO != null)
            {
                return new CSSResponse($"Agent Admin with SSO '{agentAdminDetails.Sso}' already exists.", HttpStatusCode.Conflict);
            }

            var agentSchedulingGroupBasedonSkillTag = new MockDataContext().GetAgentSchedulingGroupBasedonSkillTag(skillTagIdDetails);

            if (agentSchedulingGroupBasedonSkillTag == null)
            {
                return new CSSResponse($"No Scheduling Group match to this record. Please create before you proceed.", HttpStatusCode.NotFound);
            }
            var mappedAgentAdmin = new Agent
            {
                SkillTagId = agentAdminDetails.SkillTagId,
                AgentSchedulingGroupId = agentSchedulingGroupBasedonSkillTag.AgentSchedulingGroupId,
                CreatedBy = agentAdminDetails.CreatedBy,
                FirstName = agentAdminDetails.FirstName,
                LastName = agentAdminDetails.LastName,
                Ssn = agentAdminDetails.EmployeeId,
                Sso = agentAdminDetails.Sso,
                SupervisorId = agentAdminDetails.SupervisorId,
                SupervisorName = agentAdminDetails.SupervisorName,
                SupervisorSso = agentAdminDetails.SupervisorSso,
            };

            new MockDataContext().CreateAgentAdmin(mappedAgentAdmin);

            var mappedAgentSchedule = new AgentSchedule
            {
                AgentSchedulingGroupId = mappedAgentAdmin.AgentSchedulingGroupId,
                CreatedBy = mappedAgentAdmin.CreatedBy,
                EmployeeId = mappedAgentAdmin.Ssn
            };

            new MockDataContext().CreateAgentSchedule(mappedAgentSchedule);

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

            var agentAdminEmployeeIdDetails = new EmployeeIdDetails { Id = agentAdminDetails.EmployeeId };
            var agentAdminSsoDetails = new AgentAdminSsoDetails { Sso = agentAdminDetails.Sso };
            var skillTagIdDetails = new SkillTagIdDetails { SkillTagId = agentAdminDetails.SkillTagId };
            var employeeIdDetails = new EmployeeIdDetails { Id = agentAdmin.Ssn };

            var agentAdminsBasedOnEmployeeId = new MockDataContext().GetAgentAdminIdsByEmployeeId(agentAdminEmployeeIdDetails);

            if (agentAdminsBasedOnEmployeeId != null &&
                !string.Equals(agentAdminsBasedOnEmployeeId.Id.ToString(), agentAdminIdDetails.AgentAdminId))
            {
                return new CSSResponse($"Agent Admin with Employee ID '{agentAdminEmployeeIdDetails.Id}' already exists.", HttpStatusCode.Conflict);
            }

            var agentAdminsBasedonSSO = new MockDataContext().GetAgentAdminIdsBySso(agentAdminSsoDetails);

            if (agentAdminsBasedonSSO != null &&
                !string.Equals(agentAdminsBasedonSSO.Id.ToString(), agentAdminIdDetails.AgentAdminId))
            {
                return new CSSResponse($"Agent Admin with SSO '{agentAdminDetails.Sso}' already exists.", HttpStatusCode.Conflict);
            }

            var agentSchedulingGroupBasedonSkillTag = new MockDataContext().GetAgentSchedulingGroupBasedonSkillTag(skillTagIdDetails);

            if (agentSchedulingGroupBasedonSkillTag == null)
            {
                return new CSSResponse($"No Scheduling Group match to this record. Please create before you proceed.", HttpStatusCode.NotFound);
            }

            var agentAdminRequest = new Agent
            {
                SkillTagId = agentAdminDetails.SkillTagId,
                AgentSchedulingGroupId = agentSchedulingGroupBasedonSkillTag.AgentSchedulingGroupId,
                ModifiedBy = agentAdminDetails.ModifiedBy,
                FirstName = agentAdminDetails.FirstName,
                LastName = agentAdminDetails.LastName,
                Ssn = agentAdminDetails.EmployeeId,
                Sso = agentAdminDetails.Sso,
                SupervisorId = agentAdminDetails.SupervisorId,
                SupervisorName = agentAdminDetails.SupervisorName,
                SupervisorSso = agentAdminDetails.SupervisorSso,
            };

            new MockDataContext().UpdateAgentAdmin(agentAdminRequest);

            var updateAgentScheduleEmployeeDetails = new UpdateAgentScheduleEmployeeDetails
            {
                EmployeeId = agentAdminDetails.EmployeeId,
                AgentSchedulingGroupId = agentSchedulingGroupBasedonSkillTag.AgentSchedulingGroupId,
                ModifiedBy = agentAdminDetails.ModifiedBy
            };

            new MockDataContext().UpdateAgentSchedule(employeeIdDetails, updateAgentScheduleEmployeeDetails);

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

            var agentAdmins = new MockDataContext().GetAgentAdminIdsByEmployeeId(agentAdminEmployeeIdDetails);

            if (agentAdmins == null)
            {
                return new CSSResponse($"Agent Admin with Employee id '{agentAdminEmployeeIdDetails.Id}' does not exists.", HttpStatusCode.NotFound);
            }

            var activityLog = new ActivityLog
            {
                ActivityOrigin = agentActivityLogDetails.ActivityOrigin,
                ActivityStatus = agentActivityLogDetails.ActivityStatus,
                ActivityType = Models.Enums.ActivityType.AgentAdmin,
                ExecutedBy = agentActivityLogDetails.ExecutedBy,
                TimeStamp = agentActivityLogDetails.TimeStamp,
                FieldDetails = agentActivityLogDetails.FieldDetails
            };

            new MockDataContext().CreateActivityLog(activityLog);

            return new CSSResponse(new AgentActivityLogIdDetails { AgentActivityLogId = activityLog.Id }, HttpStatusCode.Created);
        }

        /// <summary>Gets the agent activity logs.</summary>
        /// <param name="activityLogQueryParameter">The activity log query parameter.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public CSSResponse GetAgentActivityLogs(ActivityLogQueryParameter activityLogQueryParameter)
        {
            var activityLogs = new MockDataContext().GetActivityLogs(activityLogQueryParameter);

            return new CSSResponse(activityLogs, HttpStatusCode.OK);
        }
    }
}
