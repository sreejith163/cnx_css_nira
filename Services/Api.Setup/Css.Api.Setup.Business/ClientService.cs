using AutoMapper;
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
        private IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="mapper">The mapper.</param>
        public ClientService(
            IRepositoryWrapper repository, 
            IHttpContextAccessor httpContextAccessor, 
            IMapper mapper)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
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
            var clients = await _repository.Clients.GetClientsByName(new ClientNameDetails { Name = clientDetails.Name });
            if (clients?.Count > 0)
            {
                return new CSSResponse($"Client with name '{clientDetails.Name}' already exists.", HttpStatusCode.Conflict);
            }

            var clientRequest = _mapper.Map<Client>(clientDetails);
            _repository.Clients.CreateClient(clientRequest);

            await _repository.SaveAsync();

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
            var client = await _repository.Clients.GetClient(clientIdDetails);
            if (client == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var clients = await _repository.Clients.GetClientsByName(new ClientNameDetails { Name = clientDetails.Name });
            if (clients?.Count > 0 && clients.IndexOf(clientIdDetails.ClientId) == -1)
            {
                return new CSSResponse($"Client with name '{clientDetails.Name}' already exists.", HttpStatusCode.Conflict);
            }

            var clientRequest = _mapper.Map(clientDetails, client);
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

            client.IsDeleted = true;

            _repository.Clients.UpdateClient(client);
            await _repository.SaveAsync();

            return new CSSResponse(HttpStatusCode.NoContent);
        }
    }
}
