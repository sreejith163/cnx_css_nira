using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.ClientName;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Repository.Interfaces
{

    /// <summary>
    /// Repository interface for Client name
    /// </summary>
    public interface IClientNameRepository
    {
        /// <summary>
        /// Gets the name of the client.
        /// </summary>
        /// <param name="clientNameIdDetails">The client name identifier details.</param>
        /// <returns></returns>
        Task<Client> GetClientName(ClientNameIdDetails clientNameIdDetails);

        /// <summary>
        /// Creates the client names.
        /// </summary>
        /// <param name="clientNameRequestCollection">The client name request collection.</param>
        void CreateClientNames(ICollection<Client> clientNameRequestCollection);

        /// <summary>
        /// Gets the client names count.
        /// </summary>
        /// <returns></returns>
        Task<int> GetClientNamesCount();
    }
}

