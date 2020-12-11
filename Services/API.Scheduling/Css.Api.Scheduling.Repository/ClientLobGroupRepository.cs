using AutoMapper;
using Css.Api.Core.DataAccess.Repository.NoSQL;
using Css.Api.Core.DataAccess.Repository.NoSQL.Interfaces;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.ClientLobGroup;
using Css.Api.Scheduling.Repository.Interfaces;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Repository
{
    public class ClientLobGroupRepository : GenericRepository<ClientLobCollection>, IClientLobGroupRepository
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


        /// <summary>Gets the client lob group.</summary>
        /// <param name="clientLobGroupIdDetails">The client lob group identifier details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<ClientLobCollection> GetClientLobGroup(ClientLobGroupIdDetails clientLobGroupIdDetails)
        {
            var query = Builders<ClientLobCollection>.Filter.Eq(i => i.ClientLobGroupId, clientLobGroupIdDetails.ClientLobGroupId);

            return await FindByIdAsync(query);
        }


        public void CreateClientLobGroups(ICollection<ClientLobCollection> clientLobGroupRequestCollection)
        {
            InsertManyAsync(clientLobGroupRequestCollection);
        }

        public async Task<int> GetClientLobGroupsCount()
        {
            var count = FilterBy(x => true)
                .Count();
            return await Task.FromResult(count);
        }
    }
}

