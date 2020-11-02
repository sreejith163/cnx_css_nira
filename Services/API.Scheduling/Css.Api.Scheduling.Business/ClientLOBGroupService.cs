using AutoMapper;
using Css.Api.Core.Models.DTO.Response;
using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.ClientLOBGroup;
using Css.Api.Scheduling.Repository.Interfaces;
using Microsoft.Extensions.Logging;
using System;
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
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<ClientService> _logger;

        /// <summary>Initializes a new instance of the <see cref="ClientLOBGroupService" /> class.</summary>
        /// <param name="repository">The repository.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        public ClientLOBGroupService(IRepositoryWrapper repository, IMapper mapper, ILogger<ClientService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
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
            try
            {
                var clientLOBGroups = await _repository.ClientLOBGroups.GetClientLOBGroups(clientLOBGroupParameters);
                return new CSSResponse(clientLOBGroups, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in method 'GetClientLOBGroups()'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return new CSSResponse(exMessage, HttpStatusCode.InternalServerError);
            }
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
            try
            {
                var clientLOBGroup = await _repository.ClientLOBGroups.GetClientLOBGroup(clientLOBGroupIdDetails);
                if (clientLOBGroup == null)
                {
                    return new CSSResponse(HttpStatusCode.NotFound);
                }

                return new CSSResponse(clientLOBGroup, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in method 'GetClientLOBGroup()'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return new CSSResponse(exMessage, HttpStatusCode.InternalServerError);
            }
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
            try
            {
                var clientLOBGroupRequest = _mapper.Map<ClientLobGroup>(clientLOBGroupDetails);
                _repository.ClientLOBGroups.CreateClientLOBGroup(clientLOBGroupRequest);

                await _repository.SaveAsync();

                return new CSSResponse(clientLOBGroupRequest, HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in method 'CreateClientLOBGroup()'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return new CSSResponse(exMessage, HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>Updates the client lob group.</summary>
        /// <param name="clientLOBGroupIdDetails">The client lob group identifier details.</param>
        /// <param name="clientLOBGroupDetails">The client lob group details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<CSSResponse> UpdateClientLOBGroup(ClientLOBGroupIdDetails clientLOBGroupIdDetails, UpdateClientLOBGroup clientLOBGroupDetails)
        {
            try
            {
                var clientLOBGroup = await _repository.ClientLOBGroups.GetClientLOBGroup(clientLOBGroupIdDetails);
                if (clientLOBGroup == null)
                {
                    return new CSSResponse(HttpStatusCode.NotFound);
                }

                var clientLOBGroupRequest = _mapper.Map(clientLOBGroupDetails, clientLOBGroup);
                _repository.ClientLOBGroups.UpdateClientLOBGroup(clientLOBGroupRequest);

                await _repository.SaveAsync();

                return new CSSResponse(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in method 'UpdateClientLOBGroup()'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return new CSSResponse(exMessage, HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>Deletes the client lob group.</summary>
        /// <param name="clientLOBGroupIdDetails">The client lob group identifier details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<CSSResponse> DeleteClientLOBGroup(ClientLOBGroupIdDetails clientLOBGroupIdDetails)
        {
            try
            {
                var clientLOBGroup = await _repository.ClientLOBGroups.GetClientLOBGroup(clientLOBGroupIdDetails);
                if (clientLOBGroup == null)
                {
                    return new CSSResponse(HttpStatusCode.NotFound);
                }

                _repository.ClientLOBGroups.DeleteClientLOBGroup(clientLOBGroup);
                await _repository.SaveAsync();

                return new CSSResponse(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in method 'DeleteClientLOBGroup()'");
                var exMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return new CSSResponse(exMessage, HttpStatusCode.InternalServerError);
            }
        }
    }
}