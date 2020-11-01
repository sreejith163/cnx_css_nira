using Css.Api.Core.Models.Domain;
using Css.Api.Core.Utilities.Interfaces;
using Css.Api.Core.DataAccess.Repository;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.Client;
using Css.Api.Scheduling.Repository.DatabaseContext;
using Css.Api.Scheduling.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Repository
{
    public class ClientRepository : GenericRepository<Client>, IClientRepository
    {
        /// <summary>
        /// The sort helper
        /// </summary>
        private readonly ISortHelper<Client> _sortHelper;

        /// <summary>
        /// The data shaper
        /// </summary>
        private readonly IDataShaper<Client> _dataShaper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientRepository" /> class.
        /// </summary>
        /// <param name="repositoryContext">The repository context.</param>
        /// <param name="sortHelper">The sort helper.</param>
        /// <param name="dataShaper">The data shaper.</param>
        public ClientRepository(
            SchedulingContext repositoryContext,
            ISortHelper<Client> sortHelper,
            IDataShaper<Client> dataShaper)
            : base(repositoryContext)
        {
            _sortHelper = sortHelper;
            _dataShaper = dataShaper;
        }

        /// <summary>
        /// Gets the clients.
        /// </summary>
        /// <param name="clientParameters">The client parameters.</param>
        /// <returns></returns>
        public async Task<PagedList<Entity>> GetClients(ClientQueryParameters clientParameters)
        {
            var clients = FindByCondition(x => x.IsDeleted == false);
            SearchByName(ref clients, clientParameters.SearchKeyword);
            var sortedClients = _sortHelper.ApplySort(clients, clientParameters.OrderBy);
            var shapedClients = _dataShaper.ShapeData(sortedClients, clientParameters.Fields);

            return await PagedList<Entity>.ToPagedList(shapedClients, clientParameters.PageNumber, clientParameters.PageSize);
        }

        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <param name="clientIdDetails">The client identifier details.</param>
        /// <returns></returns>
        public async Task<Client> GetClient(ClientIdDetails clientIdDetails)
        {
            return await FindByCondition(x => x.Id == clientIdDetails.ClientId && x.IsDeleted == false).SingleOrDefaultAsync();
        }

        /// <summary>
        /// Creates the client.
        /// </summary>
        /// <param name="client">The client.</param>
        public void CreateClient(Client client)
        {
            Create(client);
        }

        /// <summary>
        /// Updates the client.
        /// </summary>
        /// <param name="client">The client.</param>
        public void UpdateClient(Client client)
        {
            Update(client);
        }

        /// <summary>
        /// Deletes the client.
        /// </summary>
        /// <param name="client">The client.</param>
        public void DeleteClient(Client client)
        {
            Delete(client);
        }

        /// <summary>
        /// Searches the name of the by.
        /// </summary>
        /// <param name="clients">The clients.</param>
        /// <param name="clientName">Name of the client.</param>
        private void SearchByName(ref IQueryable<Client> clients, string clientName)
        {
            if (!clients.Any() || string.IsNullOrWhiteSpace(clientName))
                return;

            if (string.IsNullOrEmpty(clientName))
                return;

            clients = clients.Where(o => o.Name.ToLower().Contains(clientName.Trim().ToLower()));
        }
    }
}
