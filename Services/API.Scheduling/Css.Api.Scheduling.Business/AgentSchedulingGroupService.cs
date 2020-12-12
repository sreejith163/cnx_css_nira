using Css.Api.Core.Models.Domain;
using Css.Api.Core.Models.DTO.Response;
using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedulingGroup;

namespace Css.Api.Scheduling.Business
{
    public class AgentSchedulingGroupService : IAgentSchedulingGroupService
    {
        /// <summary>
        /// The HTTP context accessor
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// The repository
        /// </summary>
        private readonly IAgentSchedulingGroupRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimezoneService" /> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="repository">The repository.</param>
        public AgentSchedulingGroupService(
            IHttpContextAccessor httpContextAccessor,
            IAgentSchedulingGroupRepository repository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = repository;
        }

        /// <summary>
        /// Gets the agent scheduling groups.
        /// </summary>
        /// <param name="agentSchedulingGroupQueryparameter">The agent scheduling group queryparameter.</param>
        /// <returns></returns>
        public async Task<CSSResponse> GetAgentSchedulingGroups(AgentSchedulingGroupQueryparameter agentSchedulingGroupQueryparameter)
        {
            var agentSchedulingGroups = await _repository.GetAgentSchedulingGroups(agentSchedulingGroupQueryparameter);
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
            var agentSchedulingGroup = await _repository.GetAgentSchedulingGroup(agentSchedulingGroupIdDetails);
            if (agentSchedulingGroup == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            return new CSSResponse(agentSchedulingGroup, HttpStatusCode.OK);
        }
    }
}
