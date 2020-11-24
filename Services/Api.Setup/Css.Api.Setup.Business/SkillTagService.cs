using AutoMapper;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Css.Api.Core.Models.Domain;
using Css.Api.Setup.Models.Domain;
using Css.Api.Core.Models.DTO.Response;
using Css.Api.Setup.Business.Interfaces;
using Css.Api.Setup.Repository.Interfaces;
using Css.Api.Setup.Models.DTO.Request.SkillTag;
using Css.Api.Setup.Models.DTO.Request.SkillGroup;
using System.Linq;
using Css.Api.Setup.Models.DTO.Response.SkillTag;

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
        private IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="SkillTagService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="mapper">The mapper.</param>
        public SkillTagService(IRepositoryWrapper repository, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
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
            var skillTagNameDetails = new SkillTagNameDetails { Name = skillTagDetails.Name };

            var skillGroup = await _repository.SkillGroups.GetSkillGroup(skillGroupIdDetails);
            if (skillGroup == null)
            {
                return new CSSResponse($"Skill Group with id '{skillGroupIdDetails.SkillGroupId}' not found", HttpStatusCode.NotFound);
            }

            var skillTags = await _repository.SkillTags.GetSkillTagIdBySkillGroupIdAndGroupName(skillGroupIdDetails, skillTagNameDetails);

            if (skillTags?.Count > 0)
            {
                return new CSSResponse($"Skill Tag with name '{skillTagNameDetails.Name}' already exists.", HttpStatusCode.Conflict);
            }

            var skillTagRequest = _mapper.Map<SkillTag>(skillTagDetails);

            skillTagRequest.ClientId = skillGroup.ClientId;
            skillTagRequest.ClientLobGroupId = skillGroup.ClientLobGroupId;

            _repository.SkillTags.CreateSkillTag(skillTagRequest);

            await _repository.SaveAsync();

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

            var skillTags = await _repository.SkillTags.GetSkillTagIdBySkillGroupIdAndGroupName(skillGroupIdDetails, skillTagNameDetails);
            if (skillTags?.Count > 0 && skillTags.IndexOf(skillTagIdDetails.SkillTagId) == -1)
            {
                return new CSSResponse($"Skill tag with name '{skillTagDetails.Name}' already exists.", HttpStatusCode.Conflict);
            }

            _repository.OperationHours.RemoveOperatingHours(skillTag.OperationHour.ToList());

            var skillTagRequest = _mapper.Map(skillTagDetails, skillTag);

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

            skillTag.IsDeleted = true;

            _repository.SkillTags.UpdateSkillTag(skillTag);
            await _repository.SaveAsync();

            return new CSSResponse(HttpStatusCode.NoContent);
        }
    }
}
