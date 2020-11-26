using AutoMapper;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Models.DTO.Response;
using Css.Api.Setup.Business.Interfaces;
using Css.Api.Setup.Models.Domain;
using Css.Api.Setup.Models.DTO.Request.ClientLOBGroup;
using Css.Api.Setup.Models.DTO.Request.SkillGroup;
using Css.Api.Setup.Models.DTO.Response.SkillGroup;
using Css.Api.Setup.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Css.Api.Setup.Business
{
    public class SkillGroupService : ISkillGroupService
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

        /// <summary>Initializes a new instance of the <see cref="SkillGroupService" /> class.</summary>
        /// <param name="repository">The repository.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="mapper">The mapper.</param>
        public SkillGroupService(
            IRepositoryWrapper repository, 
            IHttpContextAccessor httpContextAccessor, 
            IMapper mapper)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        /// <summary>Gets the skill groups.</summary>
        /// <param name="skillGroupParameters">The skill group parameters.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<CSSResponse> GetSkillGroups(SkillGroupQueryParameter skillGroupParameters)
        {
            var skillGroups = await _repository.SkillGroups.GetSkillGroups(skillGroupParameters);
            _httpContextAccessor.HttpContext.Response.Headers.Add("X-Pagination", PagedList<Entity>.ToJson(skillGroups));

            return new CSSResponse(skillGroups, HttpStatusCode.OK);
        }

        /// <summary>Gets the skill group.</summary>
        /// <param name="skillGroupIdDetails">The skill group identifier details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<CSSResponse> GetSkillGroup(SkillGroupIdDetails skillGroupIdDetails)
        {
            var skillGroup = await _repository.SkillGroups.GetSkillGroup(skillGroupIdDetails);
            if (skillGroup == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var mappedSkillGroup = _mapper.Map<SkillGroupDetailsDTO>(skillGroup);
            return new CSSResponse(mappedSkillGroup, HttpStatusCode.OK);
        }

        /// <summary>Creates the skill group.</summary>
        /// <param name="skillGroupDetails">The skill group details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<CSSResponse> CreateSkillGroup(CreateSkillGroup skillGroupDetails)
        {
            var clientLOBGroupIdDetails = new ClientLOBGroupIdDetails { ClientLOBGroupId = skillGroupDetails.ClientLobGroupId };
            var skillGroupNameDetails = new SkillGroupNameDetails { Name = skillGroupDetails.Name };

            var clientLobGroup = await _repository.ClientLOBGroups.GetClientLOBGroup(clientLOBGroupIdDetails);
            if (clientLobGroup == null)
            {
                return new CSSResponse($"Client LOB Group with id '{skillGroupDetails.ClientLobGroupId}' not found", HttpStatusCode.NotFound);
            }

            var skillGroups = await _repository.SkillGroups.GetSkillGroupIdsByClientLobIdAndSkillGroupName(clientLOBGroupIdDetails, skillGroupNameDetails);
            if (skillGroups?.Count > 0)
            {
                return new CSSResponse($"Skill Group with name '{skillGroupDetails.Name}' already exists.", HttpStatusCode.Conflict);
            }

            var skillGroupRequest = _mapper.Map<SkillGroup>(skillGroupDetails);

            skillGroupRequest.ClientId = clientLobGroup.ClientId;

            _repository.SkillGroups.CreateSkillGroup(skillGroupRequest);

            await _repository.SaveAsync();

            return new CSSResponse(new SkillGroupIdDetails { SkillGroupId = skillGroupRequest.Id }, HttpStatusCode.Created);
        }

        /// <summary>Updates the skill group.</summary>
        /// <param name="skillGroupIdDetails">The skill group identifier details.</param>
        /// <param name="skillGroupDetails">The skill group details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<CSSResponse> UpdateSkillGroup(SkillGroupIdDetails skillGroupIdDetails, UpdateSkillGroup skillGroupDetails)
        {
            var skillGroup = await _repository.SkillGroups.GetSkillGroup(skillGroupIdDetails);
            if (skillGroup == null)
            {
                return new CSSResponse($"Skill Group with id '{skillGroupIdDetails.SkillGroupId}' not found", HttpStatusCode.NotFound);
            }

            var clientLOBGroupIdDetails = new ClientLOBGroupIdDetails { ClientLOBGroupId = skillGroupDetails.ClientLobGroupId };
            var skillGroupNameDetails = new SkillGroupNameDetails { Name = skillGroupDetails.Name };

            var clientLobGroup = await _repository.ClientLOBGroups.GetClientLOBGroup(clientLOBGroupIdDetails);
            if (clientLobGroup == null)
            {
                return new CSSResponse($"Client LOB Group with id '{skillGroupDetails.ClientLobGroupId}' not found", HttpStatusCode.NotFound);
            }

            var skillGroups = await _repository.SkillGroups.GetSkillGroupIdsByClientLobIdAndSkillGroupName(clientLOBGroupIdDetails, skillGroupNameDetails);
            if (skillGroups?.Count > 0 && skillGroups.IndexOf(skillGroupIdDetails.SkillGroupId) == -1)
            {
                return new CSSResponse($"Skill Group with name '{skillGroupDetails.Name}' already exists.", HttpStatusCode.Conflict);
            }

            _repository.OperationHours.RemoveOperatingHours(skillGroup.OperationHour.ToList());

            var skillGroupRequest = _mapper.Map(skillGroupDetails, skillGroup);

            skillGroupRequest.ClientId = clientLobGroup.ClientId;

            _repository.SkillGroups.UpdateSkillGroup(skillGroupRequest);

            await _repository.SaveAsync();

            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>Deletes the skill group.</summary>
        /// <param name="skillGroupIdDetails">The skill group identifier details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<CSSResponse> DeleteSkillGroup(SkillGroupIdDetails skillGroupIdDetails)
        {
            var skillGroup = await _repository.SkillGroups.GetSkillGroup(skillGroupIdDetails);
            if (skillGroup == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var hasDependency = await _repository.SkillTags.GetSkillTagsCountBySkillGroupId(skillGroupIdDetails) > 0;
            if (hasDependency)
            {
                return new CSSResponse($"The Skill Group {skillGroup.Name} has dependency with other modules", HttpStatusCode.FailedDependency);
            }

            skillGroup.IsDeleted = true;

            _repository.SkillGroups.UpdateSkillGroup(skillGroup);
            await _repository.SaveAsync();

            return new CSSResponse(HttpStatusCode.NoContent);
        }
    }
}
