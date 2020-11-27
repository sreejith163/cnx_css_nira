using Css.Api.Core.Models.DTO.Response;
using Css.Api.Setup.Models.Domain;
using Css.Api.Setup.Models.DTO.Request.AgentSchedulingGroup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Css.Api.Setup.UnitTest.Mock
{
    public class MockAgentSchedulingGroupData
    {
        /// <summary>
        /// The AgentSchedulingGroups
        /// </summary>
        private List<AgentSchedulingGroup> agentSchedulingGroupsDB = new List<AgentSchedulingGroup>()
        {
             new AgentSchedulingGroup { Id = 1, RefId = 1, Name = "agentSchedulingGroup1", ClientId=1, ClientLobGroupId=1,
                  FirstDayOfWeek=1, TimezoneId=1, CreatedBy = "admin", CreatedDate = DateTime.UtcNow },
             new AgentSchedulingGroup { Id = 2, RefId = 1, Name = "agentSchedulingGroup2",  ClientId=1, ClientLobGroupId=1,
                  FirstDayOfWeek=1, TimezoneId=1, CreatedBy = "admin", CreatedDate = DateTime.UtcNow },
             new AgentSchedulingGroup { Id = 3, RefId = 1, Name = "agentSchedulingGroup3",  ClientId=1, ClientLobGroupId=1,
                  FirstDayOfWeek=1, TimezoneId=1, CreatedBy = "admin", CreatedDate = DateTime.UtcNow }
        };

        /// <summary>
        /// Gets the AgentSchedulingGroups.
        /// </summary>
        /// <param name="AgentSchedulingGroupParameters">The AgentSchedulingGroup parameters.</param>
        /// <returns></returns>
        public CSSResponse GetAgentSchedulingGroups(AgentSchedulingGroupQueryParameter queryParameters)
        {
            var agentSchedulingGroups = agentSchedulingGroupsDB.Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize).Take(queryParameters.PageSize);
            return new CSSResponse(agentSchedulingGroups, HttpStatusCode.OK);
        }

        /// <summary>
        /// Gets the agent scheduling group.
        /// </summary>
        /// <param name="agentSchedulingGroupId">The agent scheduling group identifier.</param>
        /// <returns></returns>
        public CSSResponse GetAgentSchedulingGroup(AgentSchedulingGroupIdDetails agentSchedulingGroupId)
        {
            var agentSchedulingGroup = agentSchedulingGroupsDB.Where(x => x.Id == agentSchedulingGroupId.AgentSchedulingGroupId && x.IsDeleted == false).FirstOrDefault();
            return agentSchedulingGroup != null ? new CSSResponse(agentSchedulingGroup, HttpStatusCode.OK) : new CSSResponse(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Creates the agent scheduling group.
        /// </summary>
        /// <param name="createAgentSchedulingGroup">The create agent scheduling group.</param>
        /// <returns></returns>
        public CSSResponse CreateAgentSchedulingGroup(CreateAgentSchedulingGroup createAgentSchedulingGroup)
        {
            if (agentSchedulingGroupsDB.Exists(x => x.IsDeleted == false && x.Name == createAgentSchedulingGroup.Name))
            {
                return new CSSResponse($"Agent Scheduling Group with name '{createAgentSchedulingGroup.Name}' already exists.", HttpStatusCode.Conflict);
            }

            AgentSchedulingGroup agentSchedulingGroup = new AgentSchedulingGroup()
            {
                Id = 4,
                RefId = createAgentSchedulingGroup.RefId,
                CreatedBy = createAgentSchedulingGroup.CreatedBy,
                Name = createAgentSchedulingGroup.Name,
                SkillTagId = createAgentSchedulingGroup.SkillTagId,
                FirstDayOfWeek = createAgentSchedulingGroup.FirstDayOfWeek,
                TimezoneId = createAgentSchedulingGroup.TimezoneId,
                CreatedDate = DateTime.UtcNow
            };

            agentSchedulingGroupsDB.Add(agentSchedulingGroup);

            return new CSSResponse(new AgentSchedulingGroupIdDetails { AgentSchedulingGroupId = agentSchedulingGroup.Id }, HttpStatusCode.Created);
        }

        /// <summary>
        /// Updates the agent scheduling group.
        /// </summary>
        /// <param name="agentSchedulingGroupIdDetails">The agent scheduling group identifier details.</param>
        /// <param name="updateAgentSchedulingGroup">The update agent scheduling group.</param>
        /// <returns></returns>
        public CSSResponse UpdateAgentSchedulingGroup(AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails, UpdateAgentSchedulingGroup updateAgentSchedulingGroup)
        {
            if (!agentSchedulingGroupsDB.Exists(x => x.IsDeleted == false && x.Id == agentSchedulingGroupIdDetails.AgentSchedulingGroupId))
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            if (agentSchedulingGroupsDB.Exists(x => x.IsDeleted == false && x.Name == updateAgentSchedulingGroup.Name && x.Id != agentSchedulingGroupIdDetails.AgentSchedulingGroupId))
            {
                return new CSSResponse($"Agent Scheduling Group with name '{updateAgentSchedulingGroup.Name}' already exists.", HttpStatusCode.Conflict);
            }

            var agentSchedulingGroup = agentSchedulingGroupsDB.Where(x => x.IsDeleted == false && x.Id == agentSchedulingGroupIdDetails.AgentSchedulingGroupId).FirstOrDefault();
            agentSchedulingGroup.Name = updateAgentSchedulingGroup.Name;
            agentSchedulingGroup.SkillTagId = updateAgentSchedulingGroup.SkillTagId;
            agentSchedulingGroup.ModifiedBy = updateAgentSchedulingGroup.ModifiedBy;
            agentSchedulingGroup.ModifiedDate = DateTime.UtcNow;
            agentSchedulingGroup.FirstDayOfWeek = updateAgentSchedulingGroup.FirstDayOfWeek;
            agentSchedulingGroup.TimezoneId = updateAgentSchedulingGroup.TimezoneId;

            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Deletes the agent scheduling groups.
        /// </summary>
        /// <param name="agentSchedulingGroupIdDetails">The agent scheduling group identifier details.</param>
        /// <returns></returns>
        public CSSResponse DeleteAgentSchedulingGroups(AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails)
        {
            if (!agentSchedulingGroupsDB.Exists(x => x.IsDeleted == false && x.Id == agentSchedulingGroupIdDetails.AgentSchedulingGroupId))
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var agentSchedulingGroup = agentSchedulingGroupsDB.Where(x => x.IsDeleted == false && x.Id == agentSchedulingGroupIdDetails.AgentSchedulingGroupId).FirstOrDefault();
            agentSchedulingGroupsDB.Remove(agentSchedulingGroup);
            return new CSSResponse(HttpStatusCode.NoContent);
        }
    }
}
