using Css.Api.Core.Models.Domain;
using Css.Api.SetupMenu.Models.Domain;
using Css.Api.SetupMenu.Models.DTO.Request.Client;
using Css.Api.SetupMenu.Models.DTO.Request.ClientLOBGroup;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Css.Api.SetupMenu.Repository.Interfaces
{
    public interface IClientLOBGroupRepository
    {
        /// <summary>
        /// Gets the client lob groups.
        /// </summary>
        /// <param name="clientLOBGroupParameters">The client lob group parameters.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<PagedList<Entity>> GetClientLOBGroups(ClientLOBGroupQueryParameter clientLOBGroupParameters);

        /// <summary>
        /// Gets the client lob group.
        /// </summary>
        /// <param name="clientLOBGroupIdDetails">The client lob group identifier details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<ClientLobGroup> GetClientLOBGroup(ClientLOBGroupIdDetails clientLOBGroupIdDetails);

        /// <summary>
        /// Gets the client lob groups count by client identifier.
        /// </summary>
        /// <param name="clientIdDetails">The client identifier details.</param>
        /// <returns></returns>
        Task<int> GetClientLOBGroupsCountByClientId(ClientIdDetails clientIdDetails);

        /// <summary>
        /// Gets the name of the client lob groups identifier by client identifier and group.
        /// </summary>
        /// <param name="clientIdDetails">The client identifier details.</param>
        /// <param name="clientLOBGroupNameDetails">The client lob group name details.</param>
        /// <returns></returns>
        Task<List<int>> GetClientLOBGroupsIdByClientIdAndGroupName(ClientIdDetails clientIdDetails, ClientLOBGroupNameDetails clientLOBGroupNameDetails);

        /// <summary>
        /// Creates the client lob group.
        /// </summary>
        /// <param name="clientLOBGroupDetails">The client lob group details.</param>
        void CreateClientLOBGroup(ClientLobGroup clientLOBGroupDetails);

        /// <summary>
        /// Updates the client lob group.
        /// </summary>
        /// <param name="clientLobGroup">The client lob group.</param>
        void UpdateClientLOBGroup(ClientLobGroup clientLobGroup);

        /// <summary>
        /// Deletes the client lob group.
        /// </summary>
        /// <param name="clientLobGroup">The client lob group.</param>
        void DeleteClientLOBGroup(ClientLobGroup clientLobGroup);
    }
}
