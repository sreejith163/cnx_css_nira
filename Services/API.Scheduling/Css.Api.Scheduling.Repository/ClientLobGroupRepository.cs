using AutoMapper;
using Css.Api.Core.DataAccess.Repository.NoSQL;
using Css.Api.Core.DataAccess.Repository.NoSQL.Interfaces;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Utilities.Extensions;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.ClientLobGroup;
using Css.Api.Scheduling.Repository.Interfaces;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Repository
{
    public class ClientLobGroupRepository : GenericRepository<ClientLobGroup>, IClientLobGroupRepository
    {
        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;


        /// <summary>Initializes a new instance of the <see cref="ClientLobGroupRepository" /> class.</summary>
        /// <param name="mongoContext">The mongo context.</param>
        /// <param name="mapper">The mapper.</param>
        public ClientLobGroupRepository(IMongoContext mongoContext,
            IMapper mapper) : base(mongoContext)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the client lob groups.
        /// </summary>
        /// <param name="clientLobGroupQueryparameter">The client lob group queryparameter.</param>
        /// <returns></returns>
        public async Task<PagedList<Entity>> GetClientLobGroups(ClientLobGroupQueryparameter clientLobGroupQueryparameter)
        {
            var clientLobGroups = FilterBy(x => x.IsDeleted == false);

            var filteredClientLobGroups = FilterClientLobGroups(clientLobGroups, clientLobGroupQueryparameter);

            var sortedClientLobGroups = SortHelper.ApplySort(filteredClientLobGroups, clientLobGroupQueryparameter.OrderBy);

            var pagedClientLobGroups = sortedClientLobGroups
                .Skip((clientLobGroupQueryparameter.PageNumber - 1) * clientLobGroupQueryparameter.PageSize)
                .Take(clientLobGroupQueryparameter.PageSize);

            var shapedClientLobGroups = DataShaper.ShapeData(pagedClientLobGroups, clientLobGroupQueryparameter.Fields);

            return await PagedList<Entity>
                .ToPagedList(shapedClientLobGroups, filteredClientLobGroups.Count(), clientLobGroupQueryparameter.PageNumber, clientLobGroupQueryparameter.PageSize);
        }

        /// <summary>
        /// Gets the client lob group.
        /// </summary>
        /// <param name="clientLobGroupIdDetails">The client lob group identifier details.</param>
        /// <returns></returns>
        public async Task<ClientLobGroup> GetClientLobGroup(ClientLobGroupIdDetails clientLobGroupIdDetails)
        {
            var query = Builders<ClientLobGroup>.Filter.Eq(i => i.ClientLobGroupId, clientLobGroupIdDetails.ClientLobGroupId) &
               Builders<ClientLobGroup>.Filter.Eq(i => i.IsDeleted, false); 

            return await FindByIdAsync(query);
        }

        /// <summary>
        /// Gets the client lob groups count.
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetClientLobGroupsCount()
        {
            var count = FilterBy(x => true)
                .Count();

            return await Task.FromResult(count);
        }

        /// <summary>
        /// Creates the client lob group.
        /// </summary>
        /// <param name="clientLobGroupRequest">The client lob group request.</param>
        public void CreateClientLobGroup(ClientLobGroup clientLobGroupRequest)
        {
            InsertOneAsync(clientLobGroupRequest);
        }

        /// <summary>
        /// Updates the client lob group.
        /// </summary>
        /// <param name="clientLobGroupRequest">The client lob group request.</param>
        public void UpdateClientLobGroup(ClientLobGroup clientLobGroupRequest)
        {
            ReplaceOneAsync(clientLobGroupRequest);
        }

        /// <summary>
        /// Filters the client lob groups.
        /// </summary>
        /// <param name="clientLobGroups">The client lob groups.</param>
        /// <param name="clientLobGroupQueryparameter">The client lob group queryparameter.</param>
        /// <returns></returns>
        private IQueryable<ClientLobGroup> FilterClientLobGroups(IQueryable<ClientLobGroup> clientLobGroups, ClientLobGroupQueryparameter clientLobGroupQueryparameter)
        {
            if (!clientLobGroups.Any())
            {
                return clientLobGroups;
            }

            if (!string.IsNullOrWhiteSpace(clientLobGroupQueryparameter.SearchKeyword))
            {
                clientLobGroups = clientLobGroups.Where(o => o.Name.ToLower().Contains(clientLobGroupQueryparameter.SearchKeyword.Trim().ToLower()));
            }

            return clientLobGroups;
        }
    }
}

