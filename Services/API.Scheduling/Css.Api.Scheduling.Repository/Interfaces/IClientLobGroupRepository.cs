using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.ClientLobGroup;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Css.Api.Scheduling.Repository.Interfaces
{
    public interface IClientLobGroupRepository
    {
        Task<ClientLobCollection> GetClientLobGroup(ClientLobGroupIdDetails clientLobGroupIdDetails);
        void CreateClientLobGroups(ICollection<ClientLobCollection> clientLobGroupRequestCollection);
        Task<int> GetClientLobGroupsCount();
    }
}
