using AutoMapper;
using Css.Api.Core.EventBus;
using Css.Api.Core.EventBus.Commands.ClientLOB;
using Css.Api.Core.EventBus.Services;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Models.DTO.Response;
using Css.Api.Setup.Business.Interfaces;
using Css.Api.Setup.Models.Domain;
using Css.Api.Setup.Models.DTO.Request.Client;
using Css.Api.Setup.Models.DTO.Request.ClientLOBGroup;
using Css.Api.Setup.Models.DTO.Response.ClientLOBGroup;
using Css.Api.Setup.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;

namespace Css.Api.Setup.Business
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>The bus</summary>
        private readonly IBusService _bus;


        /// <summary>
        /// Initializes a new instance of the <see cref="ClientLOBGroupService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="mapper">The mapper.</param>
        public ClientLOBGroupService(
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
            var clientLOBGroups = await _repository.ClientLOBGroups.GetClientLOBGroupsIdByClientIdAndGroupNameOrRefId(
                new ClientLOBGroupAttribute { ClientId = clientLOBGroupDetails.ClientId, Name = clientLOBGroupDetails.Name, RefId = clientLOBGroupDetails.RefId});

            if (clientLOBGroups?.Count > 0 && clientLOBGroups[0].RefId != null && clientLOBGroups[0].RefId == clientLOBGroupDetails.RefId)
            {
                return new CSSResponse($"Client LOB Group with id '{clientLOBGroupDetails.RefId}' already exists.", HttpStatusCode.Conflict);
            }
            else if (clientLOBGroups?.Count > 0 && clientLOBGroups[0].Name == clientLOBGroupDetails.Name)
            {
                return new CSSResponse($"Client LOB Group with name '{clientLOBGroupDetails.Name}' already exists.", HttpStatusCode.Conflict);
            }

                var clientLOBGroupRequest = _mapper.Map<ClientLobGroup>(clientLOBGroupDetails);
            _repository.ClientLOBGroups.CreateClientLOBGroup(clientLOBGroupRequest);

            await _repository.SaveAsync();

            await _bus.SendCommand<CreateClientLOBCommand>(MassTransitConstants.ClientLOBCreateCommandRouteKey,
                new
                {
                    clientLOBGroupRequest.Id,
                    clientLOBGroupRequest.Name,
                    clientLOBGroupRequest.RefId,
                    clientLOBGroupRequest.ClientId,
                    clientLOBGroupRequest.TimezoneId,
                    clientLOBGroupRequest.ModifiedDate
                });

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

            var clientLOBGroups = await _repository.ClientLOBGroups.GetClientLOBGroupsIdByClientIdAndGroupNameOrRefId(
                new ClientLOBGroupAttribute { ClientId = clientLOBGroupDetails.ClientId, Name = clientLOBGroupDetails.Name, RefId = clientLOBGroupDetails.RefId });
            var result = clientLOBGroups.Find(x => x.Id != clientLOBGroupIdDetails.ClientLOBGroupId);
            if (result != null && result.RefId != null && result.RefId == clientLOBGroupDetails.RefId)
            {
                return new CSSResponse($"Client LOB Group with id '{clientLOBGroupDetails.RefId}' already exists.", HttpStatusCode.Conflict);
            }
            else if (result != null && result.Name == clientLOBGroup.Name)
            {
                return new CSSResponse($"Client LOB Group with name '{clientLOBGroupDetails.Name}' already exists.", HttpStatusCode.Conflict);
            }

            ClientLobGroup clientLOBDetailsPreUpdate = null;
            if (!clientLOBGroupDetails.IsUpdateRevert)
            {
                clientLOBDetailsPreUpdate = new ClientLobGroup
                {   
                    RefId = clientLOBGroup.RefId,
                    Name = clientLOBGroup.Name,
                    ClientId = clientLOBGroup.ClientId,
                    FirstDayOfWeek = clientLOBGroup.FirstDayOfWeek,
                    TimezoneId = clientLOBGroup.TimezoneId,
                    ModifiedBy = clientLOBGroup.ModifiedBy,
                    IsDeleted = clientLOBGroup.IsDeleted,
                    ModifiedDate = clientLOBGroup.ModifiedDate
                };
            }

            var clientLOBGroupRequest = _mapper.Map(clientLOBGroupDetails, clientLOBGroup);
            if (clientLOBGroupDetails.IsUpdateRevert)
            {
                clientLOBGroupRequest.ModifiedDate = clientLOBGroupDetails.ModifiedDate;
            }
            _repository.ClientLOBGroups.UpdateClientLOBGroup(clientLOBGroupRequest);

            await _repository.SaveAsync();

            if (!clientLOBGroupDetails.IsUpdateRevert)
            {
                await _bus.SendCommand<UpdateClientLOBCommand>(
                    MassTransitConstants.ClientLOBUpdateCommandRouteKey,
                    new
                    {
                        clientLOBGroupRequest.Id,
                        NameOldValue = clientLOBDetailsPreUpdate.Name,
                        RefIdOldValue = clientLOBDetailsPreUpdate.RefId,
                        ClientIdOldValue = clientLOBDetailsPreUpdate.ClientId,
                        TimezoneIdOldValue = clientLOBDetailsPreUpdate.TimezoneId,
                        FirstDayOfWeekOldValue = clientLOBDetailsPreUpdate.FirstDayOfWeek,
                        ModifiedByOldValue = clientLOBDetailsPreUpdate.ModifiedBy,
                        ModifiedDateOldValue = clientLOBDetailsPreUpdate.ModifiedDate,
                        IsDeletedOldValue = clientLOBDetailsPreUpdate.IsDeleted,
                        NameNewValue = clientLOBGroupRequest.Name,
                        RefIdNewValue = clientLOBGroupRequest.RefId,
                        ClientIdNewValue = clientLOBGroupRequest.ClientId,
                        IsDeletedNewValue = clientLOBGroupRequest.IsDeleted
                    });
            }

            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Reverts the client lob group.
        /// </summary>
        /// <param name="clientLOBGroupIdDetails">The client lob group identifier details.</param>
        /// <param name="clientLOBGroupDetails">The client lob group details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<CSSResponse> RevertClientLOBGroup(ClientLOBGroupIdDetails clientLOBGroupIdDetails, UpdateClientLOBGroup clientLOBGroupDetails)
        {
            var clientLOBGroup = await _repository.ClientLOBGroups.GetAllClientLOBGroup(clientLOBGroupIdDetails);
            if (clientLOBGroup == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var clientLOBGroups = await _repository.ClientLOBGroups.GetAllClientLOBGroupsIdByClientIdAndGroupName(
               new ClientIdDetails { ClientId = clientLOBGroupDetails.ClientId }, new ClientLOBGroupNameDetails { Name = clientLOBGroupDetails.Name });
            if (clientLOBGroups?.Count > 0 && clientLOBGroups.IndexOf(clientLOBGroupIdDetails.ClientLOBGroupId) == -1)
            {
                return new CSSResponse($"Client LOB Group with name '{clientLOBGroupDetails.Name}' already exists.", HttpStatusCode.Conflict);
            }

            var clientLOBGroupRequest = _mapper.Map(clientLOBGroupDetails, clientLOBGroup);
            clientLOBGroupRequest.ModifiedDate = clientLOBGroupDetails.ModifiedDate;

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

            var clientLOBDetailsPreUpdate = new ClientLobGroup
            {
                Name = clientLOBGroup.Name,
                RefId = clientLOBGroup.RefId,
                ClientId = clientLOBGroup.ClientId,
                FirstDayOfWeek = clientLOBGroup.FirstDayOfWeek,
                TimezoneId = clientLOBGroup.TimezoneId,
                ModifiedBy = clientLOBGroup.ModifiedBy,
                IsDeleted = clientLOBGroup.IsDeleted,
                ModifiedDate = clientLOBGroup.ModifiedDate
            };

            clientLOBGroup.IsDeleted = true;

            _repository.ClientLOBGroups.UpdateClientLOBGroup(clientLOBGroup);
            await _repository.SaveAsync();

            await _bus.SendCommand<DeleteClientLOBCommand>(
                MassTransitConstants.ClientLOBDeleteCommandRouteKey,
                new
                {
                    clientLOBGroup.Id,
                    clientLOBDetailsPreUpdate.Name,
                    clientLOBDetailsPreUpdate.RefId,
                    clientLOBDetailsPreUpdate.ClientId,
                    clientLOBDetailsPreUpdate.TimezoneId,
                    clientLOBDetailsPreUpdate.FirstDayOfWeek,
                    ModifiedByOldValue = clientLOBDetailsPreUpdate.ModifiedBy,
                    IsDeletedOldValue = clientLOBDetailsPreUpdate.IsDeleted,
                    ModifiedDateOldValue = clientLOBDetailsPreUpdate.ModifiedDate,
                    IsDeletedNewValue = clientLOBGroup.IsDeleted
                });

            return new CSSResponse(HttpStatusCode.NoContent);
        }
    }
}