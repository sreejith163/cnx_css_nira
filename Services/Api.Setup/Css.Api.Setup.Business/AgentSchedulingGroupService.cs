using AutoMapper;
using Css.Api.Core.EventBus;
using Css.Api.Core.EventBus.Commands.AgentSchedulingGroup;
using Css.Api.Core.EventBus.Services;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Models.DTO.Response;
using Css.Api.Setup.Business.Interfaces;
using Css.Api.Setup.Models.Domain;
using Css.Api.Setup.Models.DTO.Request.AgentSchedulingGroup;
using Css.Api.Setup.Models.DTO.Request.SkillTag;
using Css.Api.Setup.Models.DTO.Response.AgentSchedulingGroup;
using Css.Api.Setup.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Css.Api.Setup.Business
{
    /// <summary>
    /// Service for agent scheduling group part
    /// </summary>
    /// <seealso cref="Css.Api.Setup.Business.Interfaces.IAgentSchedulingGroupService" />
    public class AgentSchedulingGroupService : IAgentSchedulingGroupService
    {
        /// <summary>
        /// The repository
        /// </summary>
        private readonly IRepositoryWrapper _repository;

        /// <summary>
        /// The HTTP context accessor
        /// </summary>
        private IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>The bus</summary>
        private readonly IBusService _bus;

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentSchedulingGroupService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="mapper">The mapper.</param>
        public AgentSchedulingGroupService(IRepositoryWrapper repository, IHttpContextAccessor httpContextAccessor,
            IMapper mapper, IBusService bus)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _bus = bus;
        }

        /// <summary>
        /// Gets the agent scheduling groups.
        /// </summary>
        /// <param name="agentSchedulingGroupQueryParameter">The agent scheduling group query parameter.</param>
        /// <returns></returns>
        public async Task<CSSResponse> GetAgentSchedulingGroups(AgentSchedulingGroupQueryParameter agentSchedulingGroupQueryParameter)
        {
            var agentSchedulingGroups = await _repository.AgentSchedulingGroups.GetAgentSchedulingGroups(agentSchedulingGroupQueryParameter);
            _httpContextAccessor.HttpContext.Response.Headers.Add("X-Pagination", PagedList<Entity>.ToJson(agentSchedulingGroups));

            return new CSSResponse(agentSchedulingGroups, HttpStatusCode.OK);
        }

        /// <summary>
        /// Gets the agent scheduling group.
        /// </summary>
        /// <param name="agentSchedulingGroupIdDetails">The agent scheduling group identifier details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> GetAgentSchedulingGroup(AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails)
        {
            var agentSchedulingGroup = await _repository.AgentSchedulingGroups.GetAgentSchedulingGroup(agentSchedulingGroupIdDetails);
            if (agentSchedulingGroup == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var mappedAgentSchedulingGroup = _mapper.Map<AgentSchedulingGroupDetailsDTO>(agentSchedulingGroup);
            return new CSSResponse(mappedAgentSchedulingGroup, HttpStatusCode.OK);
        }

        /// <summary>
        /// Creates the agent scheduling group.
        /// </summary>
        /// <param name="agentSchedulingGroupDetails">The agent scheduling group details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> CreateAgentSchedulingGroup(CreateAgentSchedulingGroup agentSchedulingGroupDetails)
        {
            var skillTagIdDetails = new SkillTagIdDetails { SkillTagId = agentSchedulingGroupDetails.SkillTagId };

            var skillTag = await _repository.SkillTags.GetSkillTag(skillTagIdDetails);
            if (skillTag == null)
            {
                return new CSSResponse($"Skill Tag with id '{skillTagIdDetails.SkillTagId}' not found", HttpStatusCode.NotFound);
            }

            var agentSchedulingGroups = await _repository.AgentSchedulingGroups.GetAgentSchedulingGroupsCountBySkillTagIdOrRefId(new AgentSchedulingGroupAttribute 
            { 
                SkillTagId = agentSchedulingGroupDetails.SkillTagId,
                Name = agentSchedulingGroupDetails.Name,
                RefId = agentSchedulingGroupDetails.RefId
            });


            if (agentSchedulingGroups?.Count > 0 && agentSchedulingGroups[0].SkillTagId == agentSchedulingGroupDetails.SkillTagId)
            {
                return new CSSResponse($"This entry has existing record from other Scheduling Group. Please try again.", HttpStatusCode.Conflict);
            }
            else if (agentSchedulingGroups?.Count > 0 && agentSchedulingGroups[0].RefId != null && agentSchedulingGroups[0].RefId == agentSchedulingGroupDetails.RefId)
            {
                return new CSSResponse($"Agent Scheduling Group with id '{agentSchedulingGroupDetails.RefId}' already exists.", HttpStatusCode.Conflict);
            }
            else if (agentSchedulingGroups?.Count > 0 && agentSchedulingGroups[0].Name == agentSchedulingGroupDetails.Name)
            {
                return new CSSResponse($"Agent Scheduling Group with name '{agentSchedulingGroupDetails.Name}' already exists.", HttpStatusCode.Conflict);
            }

            var agentSchedulingGroupRequest = _mapper.Map<AgentSchedulingGroup>(agentSchedulingGroupDetails);

            agentSchedulingGroupRequest.ClientId = skillTag.ClientId;
            agentSchedulingGroupRequest.ClientLobGroupId = skillTag.ClientLobGroupId;
            agentSchedulingGroupRequest.SkillGroupId = skillTag.SkillGroupId;

            _repository.AgentSchedulingGroups.CreateAgentSchedulingGroup(agentSchedulingGroupRequest);

            await _repository.SaveAsync();

            await _bus.SendCommand<CreateAgentSchedulingGroupCommand>(MassTransitConstants.AgentSchedulingGroupCreateCommandRouteKey,
              new
              {
                  Id = agentSchedulingGroupRequest.Id,
                  Name = agentSchedulingGroupRequest.Name,
                  RefId = agentSchedulingGroupRequest.RefId,
                  ClientId = agentSchedulingGroupRequest.ClientId,
                  ClientLobGroupId = agentSchedulingGroupRequest.ClientLobGroupId,
                  SkillGroupId = agentSchedulingGroupRequest.SkillGroupId,
                  SkillTagId = agentSchedulingGroupRequest.SkillTagId,
                  TimezoneId = agentSchedulingGroupRequest.TimezoneId,
                  FirstDayOfWeek = agentSchedulingGroupRequest.FirstDayOfWeek,
                  OperationHour = JsonConvert.SerializeObject(agentSchedulingGroupDetails.OperationHour),
                  ModifiedDate = agentSchedulingGroupRequest.ModifiedDate
              });

            return new CSSResponse(new AgentSchedulingGroupIdDetails { AgentSchedulingGroupId = agentSchedulingGroupRequest.Id }, HttpStatusCode.Created);
        }

        /// <summary>
        /// Updates the agent scheduling group.
        /// </summary>
        /// <param name="agentSchedulingGroupIdDetails">The agent scheduling group identifier details.</param>
        /// <param name="agentSchedulingGroupDetails">The agent scheduling group details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> UpdateAgentSchedulingGroup(AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails, UpdateAgentSchedulingGroup agentSchedulingGroupDetails)
        {
            var agentSchedulingGroup = await _repository.AgentSchedulingGroups.GetAgentSchedulingGroup(agentSchedulingGroupIdDetails);
            if (agentSchedulingGroup == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var skillTagIdDetails = new SkillTagIdDetails { SkillTagId = agentSchedulingGroupDetails.SkillTagId };

            var skillTag = await _repository.SkillTags.GetSkillTag(skillTagIdDetails);
            if (skillTag == null)
            {
                return new CSSResponse($"Skill Tag with id '{skillTagIdDetails.SkillTagId}' not found", HttpStatusCode.NotFound);
            }

            //var agentSchedulingGroups = await _repository.AgentSchedulingGroups.GetAgentSchedulingGroupsBySkillTagId(skillTagIdDetails);
            //int matchingSchedulingGroupCount = agentSchedulingGroups.Count;
            //var isSchedulingGroupExistsForSkillTag = matchingSchedulingGroupCount > 0;
            //if (isSchedulingGroupExistsForSkillTag)
            //{
            //    if (!agentSchedulingGroupDetails.IsUpdateRevert 
            //        && agentSchedulingGroups.FirstOrDefault().Id != agentSchedulingGroupIdDetails.AgentSchedulingGroupId)
            //    {
            //        return new CSSResponse($"This entry has existing record from other Scheduling Group. Please try again.", HttpStatusCode.Conflict);
            //    }
            //    else
            //    {
            //        if (matchingSchedulingGroupCount > 1)
            //        {
            //            return new CSSResponse($"This entry has existing record from other Scheduling Group. Please try again.", HttpStatusCode.Conflict);
            //        }
            //    }
            //}

            var agentSchedulingGroups = await _repository.AgentSchedulingGroups.GetAgentSchedulingGroupsCountBySkillTagIdOrRefId(new AgentSchedulingGroupAttribute
            {
                SkillTagId = agentSchedulingGroupDetails.SkillTagId,
                Name = agentSchedulingGroupDetails.Name,
                RefId = agentSchedulingGroupDetails.RefId
            });

            var result = agentSchedulingGroups.Find(x => x.Id != agentSchedulingGroupIdDetails.AgentSchedulingGroupId);
            if (result != null && result.SkillTagId == agentSchedulingGroupDetails.SkillTagId)
            {
                return new CSSResponse($"This entry has existing record from other Scheduling Group. Please try again.", HttpStatusCode.Conflict);
            }
            else if (result != null && result.RefId != null && result.RefId == agentSchedulingGroupDetails.RefId)
                {
                return new CSSResponse($"Agent Scheduling Group with id '{agentSchedulingGroupDetails.RefId}' already exists.", HttpStatusCode.Conflict);
            }
            else if (result != null && result.Name == agentSchedulingGroupDetails.Name)
                {
                return new CSSResponse($"Agent Scheduling Group with name '{agentSchedulingGroupDetails.Name}' already exists.", HttpStatusCode.Conflict);
            }

            AgentSchedulingGroup agentSchedulingGroupDetailsPreUpdate = null;
            if (!agentSchedulingGroupDetails.IsUpdateRevert)
            {
                List<OperationHour> operationHourPreUpdated =
                    new List<OperationHour>(agentSchedulingGroup.OperationHour);
                agentSchedulingGroupDetailsPreUpdate = new AgentSchedulingGroup
                {
                    Name = agentSchedulingGroup.Name,
                    RefId = agentSchedulingGroup.RefId,
                    ClientId = agentSchedulingGroup.ClientId,
                    ClientLobGroupId = agentSchedulingGroup.ClientLobGroupId,
                    SkillGroupId = agentSchedulingGroup.SkillGroupId,
                    SkillTagId = agentSchedulingGroup.SkillTagId,
                    FirstDayOfWeek = agentSchedulingGroup.FirstDayOfWeek,
                    TimezoneId = agentSchedulingGroup.TimezoneId,
                    OperationHour = operationHourPreUpdated,
                    ModifiedBy = agentSchedulingGroup.ModifiedBy,
                    IsDeleted = agentSchedulingGroup.IsDeleted,
                    ModifiedDate = agentSchedulingGroup.ModifiedDate
                };
            }

            _repository.OperationHours.RemoveOperatingHours(agentSchedulingGroup.OperationHour.ToList());

            var agentSchedulingGroupRequest = _mapper.Map(agentSchedulingGroupDetails, agentSchedulingGroup);

            if (agentSchedulingGroupDetails.IsUpdateRevert)
            {
                agentSchedulingGroupRequest.ModifiedDate = agentSchedulingGroupDetails.ModifiedDate;
            }

            agentSchedulingGroupRequest.ClientId = skillTag.ClientId;
            agentSchedulingGroupRequest.ClientLobGroupId = skillTag.ClientLobGroupId;
            agentSchedulingGroupRequest.SkillGroupId = skillTag.SkillGroupId;

            _repository.AgentSchedulingGroups.UpdateAgentSchedulingGroup(agentSchedulingGroupRequest);

            await _repository.SaveAsync();

            if (!agentSchedulingGroupDetails.IsUpdateRevert)
            {
                UpdateAgentSchedulingGroup agentSchedulingGroupPreUpdate = null;
                var agentSchedulingGroupPreRequest = _mapper.Map(agentSchedulingGroupDetailsPreUpdate, agentSchedulingGroupPreUpdate);

                await _bus.SendCommand<UpdateAgentSchedulingGroupCommand>(
                    MassTransitConstants.AgentSchedulingGroupUpdateCommandRouteKey,
                    new
                    {
                        Id = agentSchedulingGroupRequest.Id,
                        NameOldValue = agentSchedulingGroupDetailsPreUpdate.Name,
                        RefIdOldValue = agentSchedulingGroupDetailsPreUpdate.RefId,
                        ClientIdOldValue = agentSchedulingGroupDetailsPreUpdate.ClientId,
                        ClientLobGroupIdOldvalue = agentSchedulingGroupDetailsPreUpdate.ClientLobGroupId,
                        SkillGroupIdOldValue = agentSchedulingGroupDetailsPreUpdate.SkillGroupId,
                        SkillTagIdOldValue = agentSchedulingGroupDetailsPreUpdate.SkillTagId,
                        TimezoneIdOldValue = agentSchedulingGroupDetailsPreUpdate.TimezoneId,
                        FirstDayOfWeekOldValue = agentSchedulingGroupDetailsPreUpdate.FirstDayOfWeek,
                        OperationHourOldValue =
                            JsonConvert.SerializeObject(agentSchedulingGroupPreRequest.OperationHour),
                        ModifiedByOldValue = agentSchedulingGroupDetailsPreUpdate.ModifiedBy,
                        ModifiedDateOldValue = agentSchedulingGroupDetailsPreUpdate.ModifiedDate,
                        IsDeletedOldValue = agentSchedulingGroupDetailsPreUpdate.IsDeleted,
                        NameNewValue = agentSchedulingGroupRequest.Name,
                        RefIdNewValue = agentSchedulingGroupRequest.RefId,
                        ClientIdNewValue = agentSchedulingGroupRequest.ClientId,
                        ClientLobGroupIdNewValue = agentSchedulingGroupRequest.ClientLobGroupId,
                        SkillGroupIdNewValue = agentSchedulingGroupRequest.SkillGroupId,
                        SkillTagIdNewValue = agentSchedulingGroupRequest.SkillTagId,
                        IsDeletedNewValue = agentSchedulingGroupRequest.IsDeleted
                    });
            }

            return new CSSResponse(HttpStatusCode.NoContent);
        }

        public async Task<CSSResponse> RevertAgentSchedulingGroup(AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails, UpdateAgentSchedulingGroup agentSchedulingGroupDetails)
        {
            var agentSchedulingGroup = await _repository.AgentSchedulingGroups.GetAllAgentSchedulingGroup(agentSchedulingGroupIdDetails);
            if (agentSchedulingGroup == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var skillTagIdDetails = new SkillTagIdDetails { SkillTagId = agentSchedulingGroupDetails.SkillTagId };
            var agentSchedulingGroupNameDetails = new AgentSchedulingGroupNameDetails { Name = agentSchedulingGroupDetails.Name };

            var skillTag = await _repository.SkillTags.GetSkillTag(skillTagIdDetails);
            if (skillTag == null)
            {
                return new CSSResponse($"Skill Tag with id '{skillTagIdDetails.SkillTagId}' not found", HttpStatusCode.NotFound);
            }

            var isSchedulingGroupExistsForSkillTag = await _repository.AgentSchedulingGroups.GetAllAgentSchedulingGroupsCountBySkillTagId(skillTagIdDetails, agentSchedulingGroupIdDetails) > 1;
            if (isSchedulingGroupExistsForSkillTag)
            {
                return new CSSResponse($"This entry has existing record from other Scheduling Group. Please try again.", HttpStatusCode.Conflict);
            }

            _repository.OperationHours.RemoveOperatingHours(agentSchedulingGroup.OperationHour.ToList());

            var agentSchedulingGroupRequest = _mapper.Map(agentSchedulingGroupDetails, agentSchedulingGroup);


            agentSchedulingGroupRequest.ModifiedDate = agentSchedulingGroupDetails.ModifiedDate;


            agentSchedulingGroupRequest.ClientId = skillTag.ClientId;
            agentSchedulingGroupRequest.ClientLobGroupId = skillTag.ClientLobGroupId;
            agentSchedulingGroupRequest.SkillGroupId = skillTag.SkillGroupId;

            _repository.AgentSchedulingGroups.UpdateAgentSchedulingGroup(agentSchedulingGroupRequest);

            await _repository.SaveAsync();

            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Deletes the agent scheduling group.
        /// </summary>
        /// <param name="agentSchedulingGroupIdDetails">The agent scheduling group identifier details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> DeleteAgentSchedulingGroup(AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails)
        {
            var agentSchedulingGroup = await _repository.AgentSchedulingGroups.GetAgentSchedulingGroup(agentSchedulingGroupIdDetails);
            if (agentSchedulingGroup == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            AgentSchedulingGroup agentSchedulingGroupDetailsPreUpdate = null;

            agentSchedulingGroupDetailsPreUpdate = new AgentSchedulingGroup
            {
                Name = agentSchedulingGroup.Name,
                RefId = agentSchedulingGroup.RefId,
                ClientId = agentSchedulingGroup.ClientId,
                ClientLobGroupId = agentSchedulingGroup.ClientLobGroupId,
                SkillGroupId = agentSchedulingGroup.SkillGroupId,
                SkillTagId = agentSchedulingGroup.SkillTagId,
                FirstDayOfWeek = agentSchedulingGroup.FirstDayOfWeek,
                TimezoneId = agentSchedulingGroup.TimezoneId,
                OperationHour = agentSchedulingGroup.OperationHour,
                ModifiedBy = agentSchedulingGroup.ModifiedBy,
                IsDeleted = agentSchedulingGroup.IsDeleted,
                ModifiedDate = agentSchedulingGroup.ModifiedDate
            };

            agentSchedulingGroup.IsDeleted = true;

            _repository.AgentSchedulingGroups.UpdateAgentSchedulingGroup(agentSchedulingGroup);
            await _repository.SaveAsync();

            UpdateAgentSchedulingGroup agentSchedulingGroupPreUpdate = null;
            var agentSchedulingGroupPreRequest = _mapper.Map(agentSchedulingGroupDetailsPreUpdate, agentSchedulingGroupPreUpdate);

            await _bus.SendCommand<DeleteAgentSchedulingGroupCommand>(
               MassTransitConstants.AgentSchedulingGroupDeleteCommandRouteKey,
               new
               {
                   Id = agentSchedulingGroup.Id,
                   Name = agentSchedulingGroupDetailsPreUpdate.Name,
                   RefId = agentSchedulingGroupDetailsPreUpdate.RefId,
                   ClientId = agentSchedulingGroupDetailsPreUpdate.ClientId,
                   ClientLobGroupId = agentSchedulingGroupDetailsPreUpdate.ClientLobGroupId,
                   SkillGroupId = agentSchedulingGroupDetailsPreUpdate.SkillGroupId,
                   SkillTagId = agentSchedulingGroupDetailsPreUpdate.SkillTagId,
                   TimezoneId = agentSchedulingGroupDetailsPreUpdate.TimezoneId,
                   FirstDayOfWeek = agentSchedulingGroupDetailsPreUpdate.FirstDayOfWeek,
                   OperationHour = JsonConvert.SerializeObject(agentSchedulingGroupPreRequest.OperationHour),
                   ModifiedByOldValue = agentSchedulingGroupDetailsPreUpdate.ModifiedBy,
                   IsDeletedOldValue = agentSchedulingGroupDetailsPreUpdate.IsDeleted,
                   ModifiedDateOldValue = agentSchedulingGroupDetailsPreUpdate.ModifiedDate,
                   IsDeletedNewValue = agentSchedulingGroup.IsDeleted
               });

            return new CSSResponse(HttpStatusCode.NoContent);
        }
    }
}