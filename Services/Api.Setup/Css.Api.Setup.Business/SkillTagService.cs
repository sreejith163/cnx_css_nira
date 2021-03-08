using AutoMapper;
using Css.Api.Core.EventBus;
using Css.Api.Core.EventBus.Commands.SkillTag;
using Css.Api.Core.EventBus.Services;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Models.DTO.Response;
using Css.Api.Setup.Business.Interfaces;
using Css.Api.Setup.Models.Domain;
using Css.Api.Setup.Models.DTO.Request.SkillGroup;
using Css.Api.Setup.Models.DTO.Request.SkillTag;
using Css.Api.Setup.Models.DTO.Response.SkillTag;
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
    /// Service for skill tag part
    /// </summary>
    /// <seealso cref="Css.Api.Setup.Business.Interfaces.ISkillTagService" />
    public class SkillTagService : ISkillTagService
    {
        /// <summary>
        /// The repository
        /// </summary>
        private readonly IRepositoryWrapper _repository;

        /// <summary>
        /// The HTTP context accessor
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>The bus</summary>
        private readonly IBusService _bus;

        /// <summary>
        /// Initializes a new instance of the <see cref="SkillTagService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="mapper">The mapper.</param>
        public SkillTagService(
            IRepositoryWrapper repository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            IBusService bus)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _bus = bus;
        }

        /// <summary>
        /// Gets the skill tags.
        /// </summary>
        /// <param name="skillTagQueryParameter">The skill tag query parameter.</param>
        /// <returns></returns>
        public async Task<CSSResponse> GetSkillTags(SkillTagQueryParameter skillTagQueryParameter)
        {
            var skillTags = await _repository.SkillTags.GetSkillTags(skillTagQueryParameter);
            _httpContextAccessor.HttpContext.Response.Headers.Add("X-Pagination", PagedList<Entity>.ToJson(skillTags));

            return new CSSResponse(skillTags, HttpStatusCode.OK);
        }

        /// <summary>
        /// Gets the skill tag.
        /// </summary>
        /// <param name="skillTagIdDetails">The skill tag identifier details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> GetSkillTag(SkillTagIdDetails skillTagIdDetails)
        {
            var skillTag = await _repository.SkillTags.GetSkillTag(skillTagIdDetails);
            if (skillTag == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var mappedSkillTag = _mapper.Map<SkillTagDetailsDTO>(skillTag);
            return new CSSResponse(mappedSkillTag, HttpStatusCode.OK);
        }

        /// <summary>
        /// Creates the skill tag.
        /// </summary>
        /// <param name="skillTagDetails">The skill tag details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> CreateSkillTag(CreateSkillTag skillTagDetails)
        {
            var skillGroupIdDetails = new SkillGroupIdDetails { SkillGroupId = skillTagDetails.SkillGroupId };

            var skillGroup = await _repository.SkillGroups.GetSkillGroup(skillGroupIdDetails);
            if (skillGroup == null)
            {
                return new CSSResponse($"Skill Group with id '{skillGroupIdDetails.SkillGroupId}' not found", HttpStatusCode.NotFound);
            }

            var skillTags = await _repository.SkillTags.GetSkillTagIdBySkillGroupIdAndGroupNameOrRefId(new SkillTagAttribute
            {
                SkillGroupId = skillTagDetails.SkillGroupId,
                Name = skillTagDetails.Name,
                RefId = skillTagDetails.RefId
            });

            if (skillTags?.Count > 0 && skillTags[0].RefId != null && skillTags[0].RefId == skillTagDetails.RefId)
            {
                return new CSSResponse($"Skill Tag with id '{skillTagDetails.RefId}' already exists.", HttpStatusCode.Conflict);
            }
            else if (skillTags?.Count > 0 && skillTags[0].Name == skillTagDetails.Name)
            {
                return new CSSResponse($"Skill Tag with name '{skillTagDetails.Name}' already exists.", HttpStatusCode.Conflict);
            }

            var skillTagRequest = _mapper.Map<SkillTag>(skillTagDetails);

            skillTagRequest.ClientId = skillGroup.ClientId;
            skillTagRequest.ClientLobGroupId = skillGroup.ClientLobGroupId;

            _repository.SkillTags.CreateSkillTag(skillTagRequest);

            await _repository.SaveAsync();

            await _bus.SendCommand<CreateSkillTagCommand>(MassTransitConstants.SkillTagCreateCommandRouteKey,
               new
               {
                   skillTagRequest.Id,
                   skillTagRequest.Name,
                   skillTagRequest.RefId,
                   skillTagRequest.ClientId,
                   skillTagRequest.ClientLobGroupId,
                   skillTagRequest.SkillGroupId,
                   OperationHour = JsonConvert.SerializeObject(skillTagDetails.OperationHour),
                   skillTagRequest.ModifiedDate
               });

            return new CSSResponse(new SkillTagIdDetails { SkillTagId = skillTagRequest.Id }, HttpStatusCode.Created);
        }

        /// <summary>
        /// Updates the skill tag.
        /// </summary>
        /// <param name="skillTagIdDetails">The skill tag identifier details.</param>
        /// <param name="skillTagDetails">The skill tag details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> UpdateSkillTag(SkillTagIdDetails skillTagIdDetails, UpdateSkillTag skillTagDetails)
        {
            var skillTag = await _repository.SkillTags.GetSkillTag(skillTagIdDetails);
            if (skillTag == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var skillGroupIdDetails = new SkillGroupIdDetails { SkillGroupId = skillTagDetails.SkillGroupId };
            var skillTagNameDetails = new SkillTagNameDetails { Name = skillTagDetails.Name };

            var skillGroup = await _repository.SkillGroups.GetSkillGroup(skillGroupIdDetails);
            if (skillGroup == null)
            {
                return new CSSResponse($"Skill Group with id '{skillGroupIdDetails.SkillGroupId}' not found", HttpStatusCode.NotFound);
            }

            var skillTags = await _repository.SkillTags.GetSkillTagIdBySkillGroupIdAndGroupNameOrRefId(new SkillTagAttribute
            {
                SkillGroupId = skillTagDetails.SkillGroupId,
                Name = skillTagDetails.Name,
                RefId = skillTagDetails.RefId
            });
            var result = skillTags.Find(x => x.Id != skillTagIdDetails.SkillTagId);
            if (result != null && result.RefId != null && result.RefId == skillTagDetails.RefId)
            {
                return new CSSResponse($"Skill Tag with id '{skillTagDetails.RefId}' already exists.", HttpStatusCode.Conflict);
            }
            else if (result != null && result.Name == skillTagDetails.Name)
            {
                return new CSSResponse($"Skill Tag with name '{skillTagDetails.Name}' already exists.", HttpStatusCode.Conflict);
            }

            SkillTag skillTagDetailsPreUpdate = null;
            if (!skillTagDetails.IsUpdateRevert)
            {
                List<OperationHour> operationHourPreUpdated =
                    new List<OperationHour>(skillTag.OperationHour);
                skillTagDetailsPreUpdate = new SkillTag
                {
                    RefId = skillTag.RefId,
                    Name = skillTag.Name,
                    ClientId = skillTag.ClientId,
                    ClientLobGroupId = skillTag.ClientLobGroupId,
                    SkillGroupId = skillTag.SkillGroupId,
                    OperationHour = operationHourPreUpdated,
                    ModifiedBy = skillTag.ModifiedBy,
                    IsDeleted = skillTag.IsDeleted,
                    ModifiedDate = skillTag.ModifiedDate
                };
            }

            _repository.OperationHours.RemoveOperatingHours(skillTag.OperationHour.ToList());

            var skillTagRequest = _mapper.Map(skillTagDetails, skillTag);

            if (skillTagDetails.IsUpdateRevert)
            {
                skillTagRequest.ModifiedDate = skillTagDetails.ModifiedDate;
            }

            skillTagRequest.ClientId = skillGroup.ClientId;
            skillTagRequest.ClientLobGroupId = skillGroup.ClientLobGroupId;

            _repository.SkillTags.UpdateSkillTag(skillTagRequest);

            await _repository.SaveAsync();

            if (!skillTagDetails.IsUpdateRevert)
            {
                UpdateSkillTag skillTagPreUpdate = null;
                var skillTagPreRequest = _mapper.Map(skillTagDetailsPreUpdate, skillTagPreUpdate);

                await _bus.SendCommand<UpdateSkillTagCommand>(
                    MassTransitConstants.SkillTagUpdateCommandRouteKey,
                    new
                    {
                        skillTagRequest.Id,
                        NameOldValue = skillTagDetailsPreUpdate.Name,
                        RefIdOldValue = skillTagDetailsPreUpdate.RefId,
                        ClientIdOldValue = skillTagDetailsPreUpdate.ClientId,
                        ClientLobGroupIdOldvalue = skillTagDetailsPreUpdate.ClientLobGroupId,
                        SkillGroupIdOldValue = skillTagDetailsPreUpdate.SkillGroupId,
                        OperationHourOldValue = JsonConvert.SerializeObject(skillTagPreRequest.OperationHour),
                        ModifiedByOldValue = skillTagDetailsPreUpdate.ModifiedBy,
                        ModifiedDateOldValue = skillTagDetailsPreUpdate.ModifiedDate,
                        IsDeletedOldValue = skillTagDetailsPreUpdate.IsDeleted,
                        NameNewValue = skillTagRequest.Name,
                        RefIdNewValue = skillTagRequest.RefId,
                        ClientIdNewValue = skillTagRequest.ClientId,
                        ClientLobGroupIdNewValue = skillTagRequest.ClientLobGroupId,
                        SkillGroupIdNewValue = skillTagRequest.SkillGroupId,
                        IsDeletedNewValue = skillTagRequest.IsDeleted
                    });
            }

            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Reverts the skill tag.
        /// </summary>
        /// <param name="skillTagIdDetails">The skill tag identifier details.</param>
        /// <param name="skillTagDetails">The skill tag details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<CSSResponse> RevertSkillTag(SkillTagIdDetails skillTagIdDetails, UpdateSkillTag skillTagDetails)
        {
            var skillTag = await _repository.SkillTags.GetAllSkillTag(skillTagIdDetails);
            if (skillTag == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var skillGroupIdDetails = new SkillGroupIdDetails { SkillGroupId = skillTagDetails.SkillGroupId };
            var skillTagNameDetails = new SkillTagNameDetails { Name = skillTagDetails.Name };

            var skillGroup = await _repository.SkillGroups.GetSkillGroup(skillGroupIdDetails);
            if (skillGroup == null)
            {
                return new CSSResponse($"Skill Group with id '{skillGroupIdDetails.SkillGroupId}' not found", HttpStatusCode.NotFound);
            }

            var skillTags = await _repository.SkillTags.GetAllSkillTagIdBySkillGroupIdAndGroupName(skillGroupIdDetails, skillTagNameDetails);
            if (skillTags?.Count > 0 && skillTags.IndexOf(skillTagIdDetails.SkillTagId) == -1)
            {
                return new CSSResponse($"Skill tag with name '{skillTagDetails.Name}' already exists.", HttpStatusCode.Conflict);
            }

            _repository.OperationHours.RemoveOperatingHours(skillTag.OperationHour.ToList());

            var skillTagRequest = _mapper.Map(skillTagDetails, skillTag);


            skillTagRequest.ModifiedDate = skillTagDetails.ModifiedDate;


            skillTagRequest.ClientId = skillGroup.ClientId;
            skillTagRequest.ClientLobGroupId = skillGroup.ClientLobGroupId;

            _repository.SkillTags.UpdateSkillTag(skillTagRequest);

            await _repository.SaveAsync();

            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Deletes the skill tag.
        /// </summary>
        /// <param name="skillTagIdDetails">The skill tag identifier details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> DeleteSkillTag(SkillTagIdDetails skillTagIdDetails)
        {
            var skillTag = await _repository.SkillTags.GetSkillTag(skillTagIdDetails);
            if (skillTag == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var hasDependency = await _repository.AgentSchedulingGroups.GetAgentSchedulingGroupsCountBySkillTagId(skillTagIdDetails) > 0;
            if (hasDependency)
            {
                return new CSSResponse($"The Skill Group {skillTag.Name} has dependency with other modules", HttpStatusCode.FailedDependency);
            }

            SkillTag skillTagDetailsPreUpdate = null;

            skillTagDetailsPreUpdate = new SkillTag
            {
                Name = skillTag.Name,
                RefId = skillTag.RefId,
                ClientId = skillTag.ClientId,
                ClientLobGroupId = skillTag.ClientLobGroupId,
                SkillGroupId = skillTag.SkillGroupId,
                OperationHour = skillTag.OperationHour,
                ModifiedBy = skillTag.ModifiedBy,
                IsDeleted = skillTag.IsDeleted,
                ModifiedDate = skillTag.ModifiedDate
            };

            skillTag.IsDeleted = true;

            _repository.SkillTags.UpdateSkillTag(skillTag);
            await _repository.SaveAsync();

            UpdateSkillTag skillTagPreUpdate = null;
            var skillTagPreRequest = _mapper.Map(skillTagDetailsPreUpdate, skillTagPreUpdate);


            await _bus.SendCommand<DeleteSkillTagCommand>(
               MassTransitConstants.SkillTagDeleteCommandRouteKey,
               new
               {
                   skillTag.Id,
                   skillTagDetailsPreUpdate.Name,
                   skillTagDetailsPreUpdate.RefId,
                   skillTagDetailsPreUpdate.ClientId,
                   skillTagDetailsPreUpdate.ClientLobGroupId,
                   skillTagDetailsPreUpdate.SkillGroupId,
                   OperationHour = JsonConvert.SerializeObject(skillTagPreRequest.OperationHour),
                   ModifiedByOldValue = skillTagDetailsPreUpdate.ModifiedBy,
                   IsDeletedOldValue = skillTagDetailsPreUpdate.IsDeleted,
                   ModifiedDateOldValue = skillTagDetailsPreUpdate.ModifiedDate,
                   IsDeletedNewValue = skillTag.IsDeleted
               });

            return new CSSResponse(HttpStatusCode.NoContent);
        }
    }
}
