using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.ClientName;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Css.Api.Scheduling.Repository.Interfaces
{
    public interface IClientNameRepository
    {
        Task<ClientCollection> GetClientName(ClientNameIdDetails clientNameIdDetails);
        void CreateClientNames(ICollection<ClientCollection> clientNameRequestCollection);
        Task<int> GetClientNamesCount();
    }
}

