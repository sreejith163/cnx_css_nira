﻿using Css.Api.Core.Models.DTO.Response;
using Css.Api.Setup.Models.DTO.Request.Client;
using System.Threading.Tasks;

namespace Css.Api.Setup.Business.Interfaces
{
    public interface IClientService
    {
        /// <summary>
        /// Gets the clients.
        /// </summary>
        /// <param name="clientParameters">The client parameters.</param>
        /// <returns></returns>
        Task<CSSResponse> GetClients(ClientQueryParameters clientParameters);

        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <param name="clientIdDetails">The client identifier details.</param>
        /// <returns></returns>
        Task<CSSResponse> GetClient(ClientIdDetails clientIdDetails);

        /// <summary>
        /// Creates the client.
        /// </summary>
        /// <param name="clientDetails">The client details.</param>
        /// <returns></returns>
        Task<CSSResponse> CreateClient(CreateClient clientDetails);

        /// <summary>
        /// Updates the client.
        /// </summary>
        /// <param name="clientIdDetails">The client identifier details.</param>
        /// <param name="clientDetails">The client details.</param>
        /// <returns></returns>
        Task<CSSResponse> UpdateClient(ClientIdDetails clientIdDetails, UpdateClient clientDetails);

        /// <summary>Reverts the client.</summary>
        /// <param name="clientIdDetails">The client identifier details.</param>
        /// <param name="clientDetails">The client details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<CSSResponse> RevertClient(ClientIdDetails clientIdDetails, UpdateClient clientDetails);

        /// <summary>
        /// Deletes the client.
        /// </summary>
        /// <param name="clientIdDetails">The client identifier details.</param>
        /// <returns></returns>
        Task<CSSResponse> DeleteClient(ClientIdDetails clientIdDetails);
    }
}
