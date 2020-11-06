using Css.Api.Core.Models.Domain;
using Css.Api.Core.Utilities.Interfaces;
using Css.Api.Core.DataAccess.Repository;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.Client;
using Css.Api.Scheduling.Repository.DatabaseContext;
using Css.Api.Scheduling.Repository.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Css.Api.Scheduling.Models.DTO.Response.Client;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Css.Api.Scheduling.Repository
{
    public class ClientRepository : GenericRepository<Client>, IClientRepository
    {
        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// The sort helper
        /// </summary>
        private readonly ISortHelper<ClientDTO> _sortHelper;

        /// <summary>
        /// The data shaper
        /// </summary>
        private readonly IDataShaper<ClientDTO> _dataShaper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientRepository" /> class.
        /// </summary>
        /// <param name="repositoryContext">The repository context.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="sortHelper">The sort helper.</param>
        /// <param name="dataShaper">The data shaper.</param>
        public ClientRepository(
            SchedulingContext repositoryContext,
            IMapper mapper,
            ISortHelper<ClientDTO> sortHelper,
            IDataShaper<ClientDTO> dataShaper)
            : base(repositoryContext)
        {
            _mapper = mapper;
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

            var filteredClients = FilterClients(clients, clientParameters.SearchKeyword);

            var pagedClients = filteredClients
                .Skip((clientParameters.PageNumber - 1) * clientParameters.PageSize)
                .Take(clientParameters.PageSize)
                .ToList();

            var mappedClients = pagedClients
                .AsQueryable()
                .ProjectTo<ClientDTO>(_mapper.ConfigurationProvider);

            var sortedClients = _sortHelper.ApplySort(mappedClients, clientParameters.OrderBy);
            var shapedClients = _dataShaper.ShapeData(sortedClients, clientParameters.Fields);

            return await PagedList<Entity>
                .ToPagedList(shapedClients, filteredClients.Count(), clientParameters.PageNumber, clientParameters.PageSize);
        }

        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <param name="clientIdDetails">The client identifier details.</param>
        /// <returns></returns>
        public async Task<Client> GetClient(ClientIdDetails clientIdDetails)
        {
            var client = FindByCondition(x => x.Id == clientIdDetails.ClientId && x.IsDeleted == false)
                .SingleOrDefault();

            return await Task.FromResult(client);
        }

        /// <summary>
        /// Gets the name of the client by.
        /// </summary>
        /// <param name="clientNameDetails">The client name details.</param>
        /// <returns></returns>
        public async Task<List<int>> GetClientsByName(ClientNameDetails clientNameDetails)
        {
            var client = FindByCondition(x => x.Name == clientNameDetails.Name && x.IsDeleted == false)
                .Select(x => x.Id)
                .ToList();

            return await Task.FromResult(client);
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
        /// Filters the clients.
        /// </summary>
        /// <param name="clients">The clients.</param>
        /// <param name="clientName">Name of the client.</param>
        /// <returns></returns>
        private IQueryable<Client> FilterClients(IQueryable<Client> clients, string clientName)
        {
            if (!clients.Any() || string.IsNullOrWhiteSpace(clientName))
            {
                return clients;
            }

            return clients.Where(o => o.Name.ToLower().Contains(clientName.Trim().ToLower()));
        }
    }
}
