using AutoMapper;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Models.DTO.Response;
using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.Client;
using Css.Api.Scheduling.Models.DTO.Request.ClientLOBGroup;
using Css.Api.Scheduling.Models.DTO.Response.ClientLOBGroup;
using Css.Api.Scheduling.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Business
{
    /// <summary>Service for Client lob group part</summary>
    public class ClientLOBGroupService : IClientLOBGroupService
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
        /// Initializes a new instance of the <see cref="ClientLOBGroupService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="mapper">The mapper.</param>
        public ClientLOBGroupService(IRepositoryWrapper repository, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the client lob groups.
        /// </summary>
        /// <param name="clientLOBGroupParameters">The client lob group parameters.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<CSSResponse> GetClientLOBGroups(ClientLOBGroupQueryParameter clientLOBGroupParameters)
        {
            var clientLOBGroups = await _repository.ClientLOBGroups.GetClientLOBGroups(clientLOBGroupParameters);
            _httpContextAccessor.HttpContext.Response.Headers.Add("X-Pagination", PagedList<Entity>.ToJson(clientLOBGroups));

            return new CSSResponse(clientLOBGroups, HttpStatusCode.OK);
        }

        /// <summary>
        /// Gets the client lob group.
        /// </summary>
        /// <param name="clientLOBGroupIdDetails">The client lob group identifier details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<CSSResponse> GetClientLOBGroup(ClientLOBGroupIdDetails clientLOBGroupIdDetails)
        {
            var clientLOBGroup = await _repository.ClientLOBGroups.GetClientLOBGroup(clientLOBGroupIdDetails);
            if (clientLOBGroup == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var mappedClientLOBGroup = _mapper.Map<ClientLOBGroupDTO>(clientLOBGroup);
            return new CSSResponse(mappedClientLOBGroup, HttpStatusCode.OK);
        }

        /// <summary>
        /// Creates the client lob group.
        /// </summary>
        /// <param name="clientLOBGroupDetails">The client lob group details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<CSSResponse> CreateClientLOBGroup(CreateClientLOBGroup clientLOBGroupDetails)
        {
            var clientLOBGroups = await _repository.ClientLOBGroups.GetClientLOBGroupsIdByClientIdAndGroupName(
                new ClientIdDetails { ClientId = clientLOBGroupDetails.ClientId }, new ClientLOBGroupNameDetails { Name = clientLOBGroupDetails.Name });
            if (clientLOBGroups?.Count > 0)
            {
                return new CSSResponse($"Client LOB Group with name '{clientLOBGroupDetails.Name}' already exists.", HttpStatusCode.Conflict);
            }

            var clientLOBGroupRequest = _mapper.Map<ClientLobGroup>(clientLOBGroupDetails);
            _repository.ClientLOBGroups.CreateClientLOBGroup(clientLOBGroupRequest);

            await _repository.SaveAsync();

            return new CSSResponse(new ClientLOBGroupIdDetails { ClientLOBGroupId = clientLOBGroupRequest.Id }, HttpStatusCode.Created);
        }

        /// <summary>Updates the client lob group.</summary>
        /// <param name="clientLOBGroupIdDetails">The client lob group identifier details.</param>
        /// <param name="clientLOBGroupDetails">The client lob group details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<CSSResponse> UpdateClientLOBGroup(ClientLOBGroupIdDetails clientLOBGroupIdDetails, UpdateClientLOBGroup clientLOBGroupDetails)
        {
            var clientLOBGroup = await _repository.ClientLOBGroups.GetClientLOBGroup(clientLOBGroupIdDetails);
            if (clientLOBGroup == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var clientLOBGroups = await _repository.ClientLOBGroups.GetClientLOBGroupsIdByClientIdAndGroupName(
               new ClientIdDetails { ClientId = clientLOBGroupDetails.ClientId }, new ClientLOBGroupNameDetails { Name = clientLOBGroupDetails.Name });
            if (clientLOBGroups?.Count > 0 && clientLOBGroups.IndexOf(clientLOBGroupIdDetails.ClientLOBGroupId) == -1)
            {
                return new CSSResponse($"Client LOB Group with name '{clientLOBGroupDetails.Name}' already exists.", HttpStatusCode.Conflict);
            }

            var clientLOBGroupRequest = _mapper.Map(clientLOBGroupDetails, clientLOBGroup);
            _repository.ClientLOBGroups.UpdateClientLOBGroup(clientLOBGroupRequest);

            await _repository.SaveAsync();

            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>Deletes the client lob group.</summary>
        /// <param name="clientLOBGroupIdDetails">The client lob group identifier details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<CSSResponse> DeleteClientLOBGroup(ClientLOBGroupIdDetails clientLOBGroupIdDetails)
        {
            var clientLOBGroup = await _repository.ClientLOBGroups.GetClientLOBGroup(clientLOBGroupIdDetails);
            if (clientLOBGroup == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var hasDependency = await _repository.SkillGroups.GetSkillGroupsCountByClientLobId(clientLOBGroupIdDetails) > 0;
            if (hasDependency)
            {
                return new CSSResponse($"The Client LOB Group {clientLOBGroup.Name} has dependency with other modules", HttpStatusCode.FailedDependency);
            }

            clientLOBGroup.IsDeleted = true;

            _repository.ClientLOBGroups.UpdateClientLOBGroup(clientLOBGroup);
            await _repository.SaveAsync();

            return new CSSResponse(HttpStatusCode.NoContent);
        }
    }
}