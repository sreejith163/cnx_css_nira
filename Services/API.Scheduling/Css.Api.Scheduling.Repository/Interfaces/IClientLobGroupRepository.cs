using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.ClientLobGroup;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Repository.Interfaces
{
    /// <summary>
    /// Repository interface for Client LOB group
    /// </summary>
    public interface IClientLobGroupRepository
    {
        /// <summary>
        /// Gets the client lob group.
        /// </summary>
        /// <param name="clientLobGroupIdDetails">The client lob group identifier details.</param>
        /// <returns></returns>
        Task<ClientLobGroup> GetClientLobGroup(ClientLobGroupIdDetails clientLobGroupIdDetails);

        /// <summary>
        /// Creates the client lob groups.
        /// </summary>
        /// <param name="clientLobGroupRequestCollection">The client lob group request collection.</param>
        void CreateClientLobGroups(ICollection<ClientLobGroup> clientLobGroupRequestCollection);

        /// <summary>
        /// Gets the client lob groups count.
        /// </summary>
        /// <returns></returns>
        Task<int> GetClientLobGroupsCount();
    }
}
