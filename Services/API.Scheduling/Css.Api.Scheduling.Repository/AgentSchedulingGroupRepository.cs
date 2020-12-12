using AutoMapper;
using Css.Api.Core.DataAccess.Repository.NoSQL;
using Css.Api.Core.DataAccess.Repository.NoSQL.Interfaces;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Utilities.Extensions;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedulingGroup;
using Css.Api.Scheduling.Repository.Interfaces;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Repository
{
    public class AgentSchedulingGroupRepository : GenericRepository<AgentSchedulingGroup>, IAgentSchedulingGroupRepository
    {
        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>Initializes a new instance of the <see cref="AgentSchedulingGroupRepository" /> class.</summary>
        /// <param name="mongoContext">The mongo context.</param>
        /// <param name="mapper">The mapper.</param>
        public AgentSchedulingGroupRepository(IMongoContext mongoContext,
            IMapper mapper) : base(mongoContext)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the agent scheduling groups.
        /// </summary>
        /// <param name="agentSchedulingGroupQueryparameter">The agent scheduling group queryparameter.</param>
        /// <returns></returns>
        public async Task<PagedList<Entity>> GetAgentSchedulingGroups(AgentSchedulingGroupQueryparameter agentSchedulingGroupQueryparameter)
        {
            var agentSchedulingGroups = FilterBy(x => true);

            var filteredAgentSchedulingGroups = FilterAgentSchedulingGroups(agentSchedulingGroups, agentSchedulingGroupQueryparameter);

            var sortedAgentSchedulingGroups = SortHelper.ApplySort(filteredAgentSchedulingGroups, agentSchedulingGroupQueryparameter.OrderBy);

            var pagedAgentSchedulingGroups = sortedAgentSchedulingGroups
                .Skip((agentSchedulingGroupQueryparameter.PageNumber - 1) * agentSchedulingGroupQueryparameter.PageSize)
                .Take(agentSchedulingGroupQueryparameter.PageSize);

            var shapedAgentSchedulingGroups = DataShaper.ShapeData(pagedAgentSchedulingGroups, agentSchedulingGroupQueryparameter.Fields);

            return await PagedList<Entity>
                .ToPagedList(shapedAgentSchedulingGroups, filteredAgentSchedulingGroups.Count(), agentSchedulingGroupQueryparameter.PageNumber, agentSchedulingGroupQueryparameter.PageSize);
        }

        /// <summary>
        /// Gets the agent scheduling group.
        /// </summary>
        /// <param name="agentSchedulingGroupIdDetails">The agent scheduling group identifier details.</param>
        /// <returns></returns>
        public async Task<AgentSchedulingGroup> GetAgentSchedulingGroup(AgentSchedulingGroupIdDetails agentSchedulingGroupIdDetails)
        {
            var query = Builders<AgentSchedulingGroup>.Filter.Eq(i => i.AgentSchedulingGroupId, agentSchedulingGroupIdDetails.AgentSchedulingGroupId);

            return await FindByIdAsync(query);
        }

        /// <summary>
        /// Gets the agent scheduling groups count.
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetAgentSchedulingGroupsCount()
        {
            var count = FilterBy(x => true)
                .Count();

            return await Task.FromResult(count);
        }

        /// <summary>
        /// Creates the agent scheduling group.
        /// </summary>
        /// <param name="agentSchedulingGroupRequest">The agent scheduling group request.</param>
        public void CreateAgentSchedulingGroup(AgentSchedulingGroup agentSchedulingGroupRequest)
        {
            InsertOneAsync(agentSchedulingGroupRequest);
        }

        /// <summary>
        /// Updates the agent scheduling group.
        /// </summary>
        /// <param name="agentSchedulingGroupRequest">The agent scheduling group request.</param>
        public void UpdateAgentSchedulingGroup(AgentSchedulingGroup agentSchedulingGroupRequest)
        {
            ReplaceOneAsync(agentSchedulingGroupRequest);
        }

        /// <summary>
        /// Filters the agent scheduling groups.
        /// </summary>
        /// <param name="agentSchedulingGroups">The agent scheduling groups.</param>
        /// <param name="agentSchedulingGroupQueryparameter">The agent scheduling group queryparameter.</param>
        /// <returns></returns>
        private IQueryable<AgentSchedulingGroup> FilterAgentSchedulingGroups(IQueryable<AgentSchedulingGroup> agentSchedulingGroups, AgentSchedulingGroupQueryparameter agentSchedulingGroupQueryparameter)
        {
            if (!agentSchedulingGroups.Any())
            {
                return agentSchedulingGroups;
            }

            return agentSchedulingGroups;
        }
    }
}