using AutoMapper;
using Css.Api.Core.EventBus;
using Css.Api.Core.EventBus.Commands.Client;
using Css.Api.Core.EventBus.Services;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Models.DTO.Response;
using Css.Api.Setup.Business.Interfaces;
using Css.Api.Setup.Models.Domain;
using Css.Api.Setup.Models.DTO.Request.Client;
using Css.Api.Setup.Models.DTO.Response.Client;
using Css.Api.Setup.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;

namespace Css.Api.Setup.Business
{
    public class ClientService : IClientService
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

        private readonly IBusService _bus;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="mapper">The mapper.</param>
        public ClientService(
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
        /// Gets the clients.
        /// </summary>
        /// <param name="clientParameters">The client parameters.</param>
        /// <returns></returns>
        public async Task<CSSResponse> GetClients(ClientQueryParameters clientParameters)
        {
            var clients = await _repository.Clients.GetClients(clientParameters);
            _httpContextAccessor.HttpContext.Response.Headers.Add("X-Pagination", PagedList<Entity>.ToJson(clients));

            return new CSSResponse(clients, HttpStatusCode.OK);
        }

        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <param name="clientIdDetails">The client identifier details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> GetClient(ClientIdDetails clientIdDetails)
        {
            var client = await _repository.Clients.GetClient(clientIdDetails);
            if (client == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var mappedClient = _mapper.Map<ClientDTO>(client);
            return new CSSResponse(mappedClient, HttpStatusCode.OK);
        }

        /// <summary>
        /// Creates the client.
        /// </summary>
        /// <param name="clientDetails">The client details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> CreateClient(CreateClient clientDetails)
        {
            var client = await _repository.Clients.GetClientsByNameOrRefId(new ClientAttributes { Name = clientDetails.Name, RefId = clientDetails.RefId });

            if (client?.Count > 0 && client[0].RefId != null && client[0].RefId == clientDetails.RefId)
            {
                return new CSSResponse($"Client with id '{clientDetails.RefId}' already exists.", HttpStatusCode.Conflict);
            }
            else if (client?.Count > 0 && client[0].Name == clientDetails.Name) 
            {
                return new CSSResponse($"Client with name '{clientDetails.Name}' already exists.", HttpStatusCode.Conflict);
            }

            var clientRequest = _mapper.Map<Client>(clientDetails);
            _repository.Clients.CreateClient(clientRequest);

            await _repository.SaveAsync();

            await _bus.SendCommand<CreateClientCommand>(MassTransitConstants.ClientCreateCommandRouteKey, 
                new {
                    clientRequest.Id, 
                    clientRequest.Name,
                    clientRequest.RefId,
                    clientRequest.ModifiedDate
                });

            return new CSSResponse(new ClientIdDetails { ClientId = clientRequest.Id }, HttpStatusCode.Created);
        }

        /// <summary>
        /// Updates the client.
        /// </summary>
        /// <param name="clientIdDetails">The client identifier details.</param>
        /// <param name="clientDetails">The client details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> UpdateClient(ClientIdDetails clientIdDetails, UpdateClient clientDetails)
        {
            Client client = await _repository.Clients.GetClient(clientIdDetails);

            if (client == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var clients = await _repository.Clients.GetClientsByNameOrRefId(new ClientAttributes { Name = clientDetails.Name, RefId = clientDetails.RefId });
            var result = clients.Find(x => x.Id != clientIdDetails.ClientId);
            if (result != null && result.RefId != null && result.RefId == clientDetails.RefId)
            {
                return new CSSResponse($"Client with id '{clientDetails.RefId}' already exists.", HttpStatusCode.Conflict);
            }
            else if (result != null && result.Name == clientDetails.Name)
            {
                return new CSSResponse($"Client with name '{clientDetails.Name}' already exists.", HttpStatusCode.Conflict);
            }

            var clientDetailsPreUpdate = new Client
            {
                RefId = client.RefId,
                Name = client.Name,
                ModifiedBy = client.ModifiedBy,
                IsDeleted = client.IsDeleted,
                ModifiedDate = client.ModifiedDate
            };

            var clientRequest = _mapper.Map(clientDetails, client);
            if (clientDetails.IsUpdateRevert)
            {
                clientRequest.ModifiedDate = clientDetails.ModifiedDate;
            }
            _repository.Clients.UpdateClient(clientRequest);

            await _repository.SaveAsync();

            if (!clientDetails.IsUpdateRevert)
            {
                await _bus.SendCommand<UpdateClientCommand>(
                    MassTransitConstants.ClientUpdateCommandRouteKey,
                    new
                    {
                        clientRequest.Id,
                        NameOldValue = clientDetailsPreUpdate.Name,
                        RefIdOldValue = clientDetailsPreUpdate.RefId,
                        ModifiedByOldValue = clientDetailsPreUpdate.ModifiedBy,
                        IsDeletedOldValue = clientDetailsPreUpdate.IsDeleted,
                        ModifiedDateOldValue = clientDetailsPreUpdate.ModifiedDate,
                        NameNewValue = clientRequest.Name,
                        RefIdNewValue = clientRequest.RefId,
                        IsDeletedNewValue = clientRequest.IsDeleted
                    });
            }

            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>Reverts the client.</summary>
        /// <param name="clientIdDetails">The client identifier details.</param>
        /// <param name="clientDetails">The client details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<CSSResponse> RevertClient(ClientIdDetails clientIdDetails, UpdateClient clientDetails)
        {
            Client client = await _repository.Clients.GetAllClient(clientIdDetails);

            if (client == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var clients = await _repository.Clients.GetAllClientsByName(new ClientNameDetails { Name = clientDetails.Name });
            if (clients?.Count > 0 && clients.IndexOf(clientIdDetails.ClientId) == -1)
            {
                return new CSSResponse($"Client with name '{clientDetails.Name}' already exists.", HttpStatusCode.Conflict);
            }

            var clientRequest = _mapper.Map(clientDetails, client);
            clientRequest.ModifiedDate = clientDetails.ModifiedDate;
            _repository.Clients.UpdateClient(clientRequest);

            await _repository.SaveAsync();

            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Deletes the client.
        /// </summary>
        /// <param name="clientIdDetails">The client identifier details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> DeleteClient(ClientIdDetails clientIdDetails)
        {
            var client = await _repository.Clients.GetClient(clientIdDetails);
            if (client == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var hasDependency = await _repository.ClientLOBGroups.GetClientLOBGroupsCountByClientId(clientIdDetails) > 0;
            if (hasDependency)
            {
                return new CSSResponse($"The Client {client.Name} has dependency with other modules", HttpStatusCode.FailedDependency);
            }

            var clientDetailsPreUpdate = new Client
            {
                Name = client.Name,
                RefId = client.RefId,
                ModifiedBy = client.ModifiedBy,
                IsDeleted = client.IsDeleted,
                ModifiedDate = client.ModifiedDate
            };

            client.IsDeleted = true;

            _repository.Clients.UpdateClient(client);
            await _repository.SaveAsync();

            await _bus.SendCommand<DeleteClientCommand>(
                MassTransitConstants.ClientDeleteCommandRouteKey,
                new
                {
                    client.Id,
                    client.Name,
                    client.RefId,
                    ModifiedByOldValue = clientDetailsPreUpdate.ModifiedBy,
                    IsDeletedOldValue = clientDetailsPreUpdate.IsDeleted,
                    ModifiedDateOldValue = clientDetailsPreUpdate.ModifiedDate,
                    IsDeletedNewValue = client.IsDeleted
                });

            return new CSSResponse(HttpStatusCode.NoContent);
        }
    }
}
