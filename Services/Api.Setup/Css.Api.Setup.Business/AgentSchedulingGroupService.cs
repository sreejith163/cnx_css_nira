using AutoMapper;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Models.DTO.Response;
using Css.Api.Setup.Business.Interfaces;
using Css.Api.Setup.Models.Domain;
using Css.Api.Setup.Models.DTO.Request.AgentSchedulingGroup;
using Css.Api.Setup.Models.DTO.Request.SkillTag;
using Css.Api.Setup.Models.DTO.Response.AgentSchedulingGroup;
using Css.Api.Setup.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentSchedulingGroupService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="mapper">The mapper.</param>
        public AgentSchedulingGroupService(IRepositoryWrapper repository, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
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
            var agentSchedulingGroupNameDetails = new AgentSchedulingGroupNameDetails { Name = agentSchedulingGroupDetails.Name };

            var skillTag = await _repository.SkillTags.GetSkillTag(skillTagIdDetails);
            if (skillTag == null)
            {
                return new CSSResponse($"Skill Tag with id '{skillTagIdDetails.SkillTagId}' not found", HttpStatusCode.NotFound);
            }

            var agentSchedulingGroups = await _repository.AgentSchedulingGroups.GetAgentSchedulingGroupIdBySkillTagIdAndTagName(skillTagIdDetails, agentSchedulingGroupNameDetails);

            if (agentSchedulingGroups?.Count > 0)
            {
                return new CSSResponse($"Agent Scheduling Group with name '{agentSchedulingGroupNameDetails.Name}' already exists.", HttpStatusCode.Conflict);
            }

            var agentSchedulingGroupRequest = _mapper.Map<AgentSchedulingGroup>(agentSchedulingGroupDetails);

            agentSchedulingGroupRequest.ClientId = skillTag.ClientId;
            agentSchedulingGroupRequest.ClientLobGroupId = skillTag.ClientLobGroupId;
            agentSchedulingGroupRequest.SkillGroupId = skillTag.SkillGroupId;

            _repository.AgentSchedulingGroups.CreateAgentSchedulingGroup(agentSchedulingGroupRequest);

            await _repository.SaveAsync();

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
            var agentSchedulingGroupNameDetails = new AgentSchedulingGroupNameDetails { Name = agentSchedulingGroupDetails.Name };

            var skillTag = await _repository.SkillTags.GetSkillTag(skillTagIdDetails);
            if (skillTag == null)
            {
                return new CSSResponse($"Skill Tag with id '{skillTagIdDetails.SkillTagId}' not found", HttpStatusCode.NotFound);
            }

            var agentSchedulingGroups = await _repository.AgentSchedulingGroups.GetAgentSchedulingGroupIdBySkillTagIdAndTagName(skillTagIdDetails, agentSchedulingGroupNameDetails);
            if (agentSchedulingGroups?.Count > 0 && agentSchedulingGroups.IndexOf(agentSchedulingGroupIdDetails.AgentSchedulingGroupId) == -1)
            {
                return new CSSResponse($"Agent Scheduling Group with name '{agentSchedulingGroupDetails.Name}' already exists.", HttpStatusCode.Conflict);
            }

            _repository.OperationHours.RemoveOperatingHours(agentSchedulingGroup.OperationHour.ToList());

            var agentSchedulingGroupRequest = _mapper.Map(agentSchedulingGroupDetails, agentSchedulingGroup);

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

            agentSchedulingGroup.IsDeleted = true;

            _repository.AgentSchedulingGroups.UpdateAgentSchedulingGroup(agentSchedulingGroup);
            await _repository.SaveAsync();

            return new CSSResponse(HttpStatusCode.NoContent);
        }
    }
}