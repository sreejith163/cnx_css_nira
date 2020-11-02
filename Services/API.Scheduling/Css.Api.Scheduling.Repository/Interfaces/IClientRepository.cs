﻿using Css.Api.Core.Models.Domain;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.Client;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Repository.Interfaces
{
    public interface IClientRepository
    {
        /// <summary>
        /// Gets the clients.
        /// </summary>
        /// <param name="clientParameters">The client parameters.</param>
        /// <returns></returns>
        Task<PagedList<Entity>> GetClients(ClientQueryParameters clientParameters);

        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <param name="clientIdDetails">The client identifier details.</param>
        /// <returns></returns>
        Task<Client> GetClient(ClientIdDetails clientIdDetails);

        /// <summary>
        /// Creates the client.
        /// </summary>
        /// <param name="client">The client.</param>
        void CreateClient(Client client);

        /// <summary>
        /// Updates the client.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <returns></returns>
        void UpdateClient(Client client);

        /// <summary>
        /// Deletes the client.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <returns></returns>
        void DeleteClient(Client client);
    }
}
