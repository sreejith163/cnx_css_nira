using Css.Api.Core.Models.Domain;
using Css.Api.Setup.Models.Domain;
using Css.Api.Setup.Models.DTO.Request.Client;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Css.Api.Setup.Repository.Interfaces
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
        /// Gets the name of the clients by.
        /// </summary>
        /// <param name="clientNameDetails">The client name details.</param>
        /// <returns></returns>
        Task<List<int>> GetClientsByName(ClientNameDetails clientNameDetails);

        /// <summary>Gets the name of all clients by.</summary>
        /// <param name="clientNameDetails">The client name details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<List<int>> GetAllClientsByName(ClientNameDetails clientNameDetails);

        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <param name="clientIdDetails">The client identifier details.</param>
        /// <returns></returns>
        Task<Client> GetClient(ClientIdDetails clientIdDetails);

        /// <summary>Gets all client.</summary>
        /// <param name="clientIdDetails">The client identifier details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<Client> GetAllClient(ClientIdDetails clientIdDetails);

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
