using AutoMapper;
using Css.Api.Core.DataAccess.Repository.NoSQL;
using Css.Api.Core.DataAccess.Repository.NoSQL.Interfaces;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.ClientName;
using Css.Api.Scheduling.Repository.Interfaces;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Repository
{
    public class ClientNameRepository : GenericRepository<Client>, IClientNameRepository
    {
        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>Initializes a new instance of the <see cref="ClientNameRepository" /> class.</summary>
        /// <param name="mongoContext">The mongo context.</param>
        /// <param name="mapper">The mapper.</param>
        public ClientNameRepository(IMongoContext mongoContext,
            IMapper mapper) : base(mongoContext)
        {
            _mapper = mapper;
        }

        /// <summary>Gets the client name group.</summary>
        /// <param name="clientNameGroupIdDetails">The client name group identifier details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<Client> GetClientName(ClientNameIdDetails clientNameGroupIdDetails)
        {
            var query = Builders<Client>.Filter.Eq(i => i.ClientId, clientNameGroupIdDetails.ClientNameId);

            return await FindByIdAsync(query);
        }

        /// <summary>
        /// Creates the client names.
        /// </summary>
        /// <param name="clientNameGroupRequestCollection">The client name group request collection.</param>
        public void CreateClientNames(ICollection<Client> clientNameGroupRequestCollection)
        {
            InsertManyAsync(clientNameGroupRequestCollection);
        }

        /// <summary>
        /// Gets the client names count.
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetClientNamesCount()
        {
            var count = FilterBy(x => true)
                .Count();
            return await Task.FromResult(count);
        }
    }
}


