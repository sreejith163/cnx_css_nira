using Css.Api.Core.Models.Domain;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.Client;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Repository.Interfaces
{
    /// <summary>
    /// Repository interface for Client
    /// </summary>
    public interface IClientRepository
    {
        //// <summary>
        /// Gets the clients.
        /// </summary>
        /// <param name="clientQueryparameter">The client queryparameter.</param>
        /// <returns></returns>
        Task<PagedList<Entity>> GetClients(ClientQueryparameter clientQueryparameter);

        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <param name="clientIdDetails">The client identifier details.</param>
        /// <returns></returns>
        Task<Client> GetClient(ClientIdDetails clientIdDetails);

        /// <summary>
        /// Gets the clients count.
        /// </summary>
        /// <returns></returns>
        Task<int> GetClientsCount();

        /// <summary>
        /// Creates the client.
        /// </summary>
        /// <param name="clientRequest">The client request.</param>
        void CreateClient(Client clientRequest);

        /// <summary>
        /// Updates the client.
        /// </summary>
        /// <param name="clientRequest">The client request.</param>
        void UpdateClient(Client clientRequest);
    }
}

