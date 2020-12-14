using AutoMapper;
using AutoMapper.QueryableExtensions;
using Css.Api.Core.DataAccess.Repository.NoSQL;
using Css.Api.Core.DataAccess.Repository.NoSQL.Interfaces;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Utilities.Extensions;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using Css.Api.Scheduling.Models.DTO.Response.AgentAdmin;
using Css.Api.Scheduling.Repository.Interfaces;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Repository
{
    public class AgentAdminRepository : GenericRepository<Agent>, IAgentAdminRepository
    {
        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;


        /// <summary>Initializes a new instance of the <see cref="AgentAdminRepository" /> class.</summary>
        /// <param name="mongoContext">The mongo context.</param>
        /// <param name="mapper">The mapper.</param>
        public AgentAdminRepository(IMongoContext mongoContext,
            IMapper mapper) : base(mongoContext)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the agent admins.
        /// </summary>
        /// <param name="agentAdminQueryParameter">The agent admin query parameter.</param>
        /// <returns></returns>
        public async Task<PagedList<Entity>> GetAgentAdmins(AgentAdminQueryParameter agentAdminQueryParameter)
        {
            var agentAdmins = FilterBy(x => x.IsDeleted == false);

            var filteredAgentAdmins = FilterAgentAdmin(agentAdmins, agentAdminQueryParameter.SearchKeyword);

            var sortedAgentAdmins = SortHelper.ApplySort(filteredAgentAdmins, agentAdminQueryParameter.OrderBy);

            var pagedAgentAdmins = sortedAgentAdmins
                .Skip((agentAdminQueryParameter.PageNumber - 1) * agentAdminQueryParameter.PageSize)
                .Take(agentAdminQueryParameter.PageSize);

            var mappedAgentAdmins = pagedAgentAdmins
                .ProjectTo<AgentAdminDTO>(_mapper.ConfigurationProvider);

            var shapedAgentAdmins = DataShaper.ShapeData(mappedAgentAdmins, agentAdminQueryParameter.Fields);

            return await PagedList<Entity>
                .ToPagedList(shapedAgentAdmins, filteredAgentAdmins.Count(), agentAdminQueryParameter.PageNumber, agentAdminQueryParameter.PageSize);
        }

        /// <summary>
        /// Gets the agent admin.
        /// </summary>
        /// <param name="agentAdminIdDetails">The agent admin identifier details.</param>
        /// <returns></returns>
        public async Task<Agent> GetAgentAdmin(AgentAdminIdDetails agentAdminIdDetails)
        {
            var query = 
                Builders<Agent>.Filter.Eq(i => i.IsDeleted, false) & 
                Builders<Agent>.Filter.Eq(i => i.AgentAdminId, agentAdminIdDetails.AgentAdminId);

            return await FindByIdAsync(query);
        }

        //To be changed
        /// <summary>
        /// Gets the agent admins by employee identifier.
        /// </summary>
        /// <param name="agentAdminEmployeeIdDetails">The agent admin employee identifier details.</param>
        /// <param name="agentAdminSsoDetails">The agent admin sso details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<Agent> GetAgentAdminIdsByEmployeeIdAndSso(AgentAdminEmployeeIdDetails agentAdminEmployeeIdDetails, AgentAdminSsoDetails agentAdminSsoDetails)
        {

            var query = 
                Builders<Agent>.Filter.Eq(i => i.IsDeleted, false) &
                Builders<Agent>.Filter.Eq(i => i.Ssn, agentAdminEmployeeIdDetails.Id) |
                Builders<Agent>.Filter.Eq(i => i.Sso, agentAdminSsoDetails.Sso);

            return await FindByIdAsync(query);
        }

        /// <summary>
        /// Gets the agent admin ids by employee identifier.
        /// </summary>
        /// <param name="agentAdminEmployeeIdDetails">The agent admin employee identifier details.</param>
        /// <returns></returns>
        public async Task<Agent> GetAgentAdminIdsByEmployeeId(AgentAdminEmployeeIdDetails agentAdminEmployeeIdDetails)
        {

            var query =
                Builders<Agent>.Filter.Eq(i => i.IsDeleted, false) &
                Builders<Agent>.Filter.Eq(i => i.Ssn, agentAdminEmployeeIdDetails.Id)

            return await FindByIdAsync(query);
        }

        /// <summary>
        /// Gets the agent admins count.
        /// </summary>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<int> GetAgentAdminsCount()
        {
            var count = FilterBy(x => true)
                .Count();

            return await Task.FromResult(count);
        }

        /// <summary>
        /// Creates the agent admin.
        /// </summary>
        /// <param name="agentAdminRequest">The agent admin request.</param>
        public void CreateAgentAdmin(Agent agentAdminRequest)
        {
            InsertOneAsync(agentAdminRequest);
        }

        /// <summary>
        /// Updates the agent admin.
        /// </summary>
        /// <param name="agentAdminRequest">The agent admin request.</param>
        public void UpdateAgentAdmin(Agent agentAdminRequest)
        {
            ReplaceOneAsync(agentAdminRequest);
        }

        /// <summary>
        /// Deletes the agent admin.
        /// </summary>
        /// <param name="agentAdminRequest">The agent admin request.</param>
        public void DeleteAgentAdmin(Agent agentAdminRequest)
        {
            DeleteOneAsync(x => x.Id == agentAdminRequest.Id);
        }

        /// <summary>Filters the agent admin.</summary>
        /// <param name="agentAdmins">The agent admins.</param>
        /// <param name="agentAdminName">Name of the agent admin.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        private IQueryable<Agent> FilterAgentAdmin(IQueryable<Agent> agentAdmins, string agentAdminName)
        {
            if (!agentAdmins.Any())
            {
                return agentAdmins;
            }

            if (!string.IsNullOrWhiteSpace(agentAdminName))
            {
                agentAdmins = agentAdmins.Where(o => string.Equals(o.FirstName, agentAdminName, StringComparison.OrdinalIgnoreCase) ||
                                                     string.Equals(o.FirstName, agentAdminName, StringComparison.OrdinalIgnoreCase) );
            }

            return agentAdmins;
        }
    }
}

