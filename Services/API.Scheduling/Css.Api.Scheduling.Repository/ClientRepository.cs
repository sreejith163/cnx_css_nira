using AutoMapper;
using Css.Api.Core.DataAccess.Repository.NoSQL;
using Css.Api.Core.DataAccess.Repository.NoSQL.Interfaces;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Utilities.Extensions;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.Client;
using Css.Api.Scheduling.Repository.Interfaces;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Repository
{
    public class ClientRepository : GenericRepository<Client>, IClientRepository
    {
        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>Initializes a new instance of the <see cref="ClientRepository" /> class.</summary>
        /// <param name="mongoContext">The mongo context.</param>
        /// <param name="mapper">The mapper.</param>
        public ClientRepository(IMongoContext mongoContext,
            IMapper mapper) : base(mongoContext)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the clients.
        /// </summary>
        /// <param name="clientQueryparameter">The client queryparameter.</param>
        /// <returns></returns>
        public async Task<PagedList<Entity>> GetClients(ClientQueryparameter clientQueryparameter)
        {
            var clients = FilterBy(x => true);

            var filteredClients = FilterClients(clients, clientQueryparameter);

            var sortedClients = SortHelper.ApplySort(filteredClients, clientQueryparameter.OrderBy);

            var pagedClients = sortedClients
                .Skip((clientQueryparameter.PageNumber - 1) * clientQueryparameter.PageSize)
                .Take(clientQueryparameter.PageSize);

            var shapedClients = DataShaper.ShapeData(pagedClients, clientQueryparameter.Fields);

            return await PagedList<Entity>
                .ToPagedList(shapedClients, filteredClients.Count(), clientQueryparameter.PageNumber, clientQueryparameter.PageSize);
        }

        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <param name="clientIdDetails">The client identifier details.</param>
        /// <returns></returns>
        public async Task<Client> GetClient(ClientIdDetails clientIdDetails)
        {
            var query = Builders<Client>.Filter.Eq(i => i.ClientId, clientIdDetails.ClientId);

            return await FindByIdAsync(query);
        }

        /// <summary>
        /// Gets the clients count.
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetClientsCount()
        {
            var count = FilterBy(x => true)
                .Count();

            return await Task.FromResult(count);
        }

        /// <summary>
        /// Creates the client.
        /// </summary>
        /// <param name="clientRequest">The client request.</param>
        public void CreateClient(Client clientRequest)
        {
            InsertOneAsync(clientRequest);
        }

        /// <summary>
        /// Updates the client.
        /// </summary>
        /// <param name="clientRequest">The client request.</param>
        public void UpdateClient(Client clientRequest)
        {
            ReplaceOneAsync(clientRequest);
        }

        /// <summary>
        /// Filters the clients.
        /// </summary>
        /// <param name="clients">The clients.</param>
        /// <param name="clientQueryparameter">The client queryparameter.</param>
        /// <returns></returns>
        private IQueryable<Client> FilterClients(IQueryable<Client> clients, ClientQueryparameter clientQueryparameter)
        {
            if (!clients.Any())
            {
                return clients;
            }

            if (!string.IsNullOrWhiteSpace(clientQueryparameter.SearchKeyword))
            {
                clients = clients.Where(o => o.Name.ToLower().Contains(clientQueryparameter.SearchKeyword.ToLower()));
            }

            return clients;
        }
    }
}


