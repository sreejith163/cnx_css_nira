using Css.Api.Core.DataAccess.Repository.NoSQL;
using Css.Api.Core.DataAccess.Repository.NoSQL.Interfaces;
using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Scheduling.Repository.Interfaces;
using MongoDB.Driver;
using System.Linq;

namespace Css.Api.Scheduling.Repository
{
    /// <summary>
    /// The agent scheduling group repository
    /// </summary>
    public class AgentSchedulingGroupHistoryRepository : GenericRepository<AgentSchedulingGroupHistory>, IAgentSchedulingGroupHistoryRepository
    {
        /// <summary>Initializes a new instance of the <see cref="AgentSchedulingGroupHistoryRepository" /> class.</summary>
        /// <param name="mongoContext">The mongo context.</param>
        public AgentSchedulingGroupHistoryRepository(IMongoContext mongoContext) : base(mongoContext)
        {

        }

        /// <summary>
        /// The method to update the history for the input agents and their current scheduling group details
        /// </summary>
        /// <param name="agentSchedulingGroupHistories">The details of updated agent and their scheduling groups</param>
        /// <returns></returns>
        public void UpdateAgentSchedulingGroupHistory(AgentSchedulingGroupHistory agentSchedulingGroupHistory)
        {
            var query = Builders<AgentSchedulingGroupHistory>.Filter.Eq(i => i.EmployeeId, agentSchedulingGroupHistory.EmployeeId) &
                        Builders<AgentSchedulingGroupHistory>.Filter.Eq(i => i.EndDate, null);

            var currentRecord = FilterBy(query).FirstOrDefault();

            if (currentRecord == null)
            {
                InsertOneAsync(agentSchedulingGroupHistory);
            }
            else if (currentRecord.AgentSchedulingGroupId != agentSchedulingGroupHistory.AgentSchedulingGroupId)
            {
                var update = Builders<AgentSchedulingGroupHistory>.Update.Set(x => x.EndDate, agentSchedulingGroupHistory.StartDate)
                               .Set(x => x.ModifiedBy, agentSchedulingGroupHistory.CreatedBy)
                               .Set(x => x.ModifiedDate, agentSchedulingGroupHistory.CreatedDate);

                UpdateOneAsync(query, update);
                InsertOneAsync(agentSchedulingGroupHistory);
            }
        }
    }
}
