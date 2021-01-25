using Css.Api.Core.Models.Domain;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.Client;
using Css.Api.Scheduling.Models.DTO.Request.ClientLobGroup;
using System.Linq;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Repository.Interfaces
{
    /// <summary>
    /// Repository interface for ClientLobGroup LOB group
    /// </summary>
    public interface IClientLobGroupRepository
    {
        //// <summary>
        /// Gets the client lob groups.
        /// </summary>
        /// <param name="clientLobGroupQueryparameter">The client lob group queryparameter.</param>
        /// <returns></returns>
        Task<PagedList<Entity>> GetClientLobGroups(ClientLobGroupQueryparameter clientLobGroupQueryparameter);

        /// <summary>Gets the client lob groups of client.</summary>
        /// <param name="clientIdDetails">The client identifier details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<IQueryable<ClientLobGroup>> GetClientLobGroupsOfClient(ClientIdDetails clientIdDetails);

        /// <summary>
        /// Gets the client lob group.
        /// </summary>
        /// <param name="clientLobGroupIdDetails">The client lob group identifier details.</param>
        /// <returns></returns>
        Task<ClientLobGroup> GetClientLobGroup(ClientLobGroupIdDetails clientLobGroupIdDetails);

        /// <summary>
        /// Gets the client lob groups count.
        /// </summary>
        /// <returns></returns>
        Task<int> GetClientLobGroupsCount();

        /// <summary>
        /// Creates the client lob group.
        /// </summary>
        /// <param name="clientLobGroupRequest">The client lob group request.</param>
        void CreateClientLobGroup(ClientLobGroup clientLobGroupRequest);

        /// <summary>
        /// Updates the client lob group.
        /// </summary>
        /// <param name="clientLobGroupRequest">The client lob group request.</param>
        void UpdateClientLobGroup(ClientLobGroup clientLobGroupRequest);
    }
}
