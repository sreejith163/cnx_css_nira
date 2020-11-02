using AutoMapper;
using Css.Api.Core.Models.DTO.Response;
using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.Client;
using Css.Api.Scheduling.Repository.Interfaces;
using System.Net;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Business
{
    public class ClientService : IClientService
    {
        /// <summary>
        /// The repository
        /// </summary>
        private readonly IRepositoryWrapper _repository;

        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="mapper">The mapper.</param>
        public ClientService(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
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

            return new CSSResponse(client, HttpStatusCode.OK);
        }

        /// <summary>
        /// Creates the client.
        /// </summary>
        /// <param name="clientDetails">The client details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> CreateClient(CreateClient clientDetails)
        {
            var clientRequest = _mapper.Map<Client>(clientDetails);
            _repository.Clients.CreateClient(clientRequest);

            await _repository.SaveAsync();

            return new CSSResponse(clientRequest, HttpStatusCode.Created);
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

            _repository.Clients.DeleteClient(client);
            await _repository.SaveAsync();

            return new CSSResponse(HttpStatusCode.NoContent);
        }
    }
}
