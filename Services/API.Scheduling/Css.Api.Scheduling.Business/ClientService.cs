using AutoMapper;
using Css.Api.Core.Common.Models.DTO.Responses;
using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Requests.Client;
using Css.Api.Scheduling.Repository.Interface;
using Microsoft.Extensions.Logging;
using System;
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
        /// The logger
        /// </summary>
        private readonly ILogger<ClientService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        public ClientService(IRepositoryWrapper repository, IMapper mapper, ILogger<ClientService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }


        /// <summary>
        /// Gets the clients.
        /// </summary>
        /// <param name="clientParameters">The client parameters.</param>
        /// <returns></returns>
        public async Task<CSSResponse> GetClients(ClientQueryParameters clientParameters)
        {
            try
            {
                var clients = await _repository.Clients.GetClients(clientParameters);
                return new CSSResponse(clients, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in method 'GetClients()'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return new CSSResponse(exMessage, HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Gets the client names.
        /// </summary>
        /// <returns></returns>
        public async Task<CSSResponse> GetClientNames()
        {
            try
            {
                var clients = await _repository.Clients.GetClientNames();
                return new CSSResponse(clients, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in method 'GetClientNames()'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return new CSSResponse(exMessage, HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <param name="clientIdDetails">The client identifier details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> GetClient(ClientIdDetails clientIdDetails)
        {
            try
            {
                var client = await _repository.Clients.GetClient(clientIdDetails);
                if (client == null)
                {
                    return new CSSResponse(HttpStatusCode.NotFound);
                }

                return new CSSResponse(client, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in method 'GetClient()'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return new CSSResponse(exMessage, HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Creates the client.
        /// </summary>
        /// <param name="clientDetails">The client details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> CreateClient(CreateClient clientDetails)
        {
            try
            {
                var clientRequest = _mapper.Map<Client>(clientDetails);
                _repository.Clients.CreateClient(clientRequest);

                await _repository.SaveAsync();

                return new CSSResponse(clientRequest, HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in method 'CreateClient()'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return new CSSResponse(exMessage, HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Updates the client.
        /// </summary>
        /// <param name="clientIdDetails">The client identifier details.</param>
        /// <param name="clientDetails">The client details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> UpdateClient(ClientIdDetails clientIdDetails, UpdateClient clientDetails)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in method 'UpdateClient()'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return new CSSResponse(exMessage, HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Deletes the client.
        /// </summary>
        /// <param name="clientIdDetails">The client identifier details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> DeleteClient(ClientIdDetails clientIdDetails)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in method 'DeleteClient()'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return new CSSResponse(exMessage, HttpStatusCode.InternalServerError);
            }
        }
    }
}
