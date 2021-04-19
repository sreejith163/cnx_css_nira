using Css.Api.Core.DataAccess.Repository.NoSQL;
using Css.Api.Core.DataAccess.Repository.NoSQL.Interfaces;
using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Reporting.Repository.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Reporting.Repository
{
    /// <summary>
    /// The agent category repository
    /// </summary>
    public class AgentCategoryRepository : GenericRepository<AgentCategory>, IAgentCategoryRepository
    {

        /// <summary>Initializes a new instance of the <see cref="AgentCategoryRepository" /> class.</summary>
        /// <param name="mongoContext">The mongo context.</param>
        public AgentCategoryRepository(IMongoContext mongoContext) : base(mongoContext)
        {

        }

        /// <summary>
        /// Get the agent categories.
        /// </summary>
        /// <returns>List of instances of AgentCategory</returns>
        public async Task<List<AgentCategory>> GetAgentCategories()
        {
            var query = Builders<AgentCategory>.Filter.Eq(i => i.IsDeleted, false);

            var result = FilterBy(query);
            return await Task.FromResult(result.ToList());
        }

        /// <summary>
        /// Get the agent categories based on input agent category ids
        /// </summary>
        /// <param name="agentCategoryIds">The ids of agent categories</param>
        /// <returns>List of instances of AgentCategory</returns>
        public async Task<List<AgentCategory>> GetAgentCategories(List<int> agentCategoryIds)
        {
            var query = Builders<AgentCategory>.Filter.Eq(i => i.IsDeleted, false)
                    & Builders<AgentCategory>.Filter.In(i => i.AgentCategoryId, agentCategoryIds);

            var result = FilterBy(query);
            return await Task.FromResult(result.ToList());
        }

        /// <summary>
        /// Get the agent categories based on input names
        /// </summary>
        /// <param name="names">The names of agent categories</param>
        /// <returns>List of instances of AgentCategory</returns>
        public async Task<List<AgentCategory>> GetAgentCategories(List<string> names)
        {
            var query = Builders<AgentCategory>.Filter.Eq(i => i.IsDeleted, false) 
                    & Builders<AgentCategory>.Filter.In(i => i.Name, names);

            var result = FilterBy(query);
            return await Task.FromResult(result.ToList());
        }

        /// <summary>
        /// Get the agent category based on input agent category id
        /// </summary>
        /// <param name="agentCategoryId">The id of the agent category</param>
        /// <returns>An instance of AgentCategory</returns>
        public async Task<AgentCategory> GetAgentCategory(int agentCategoryId)
        {
            var query = Builders<AgentCategory>.Filter.Eq(i => i.IsDeleted, false)
                    & Builders<AgentCategory>.Filter.Eq(i => i.AgentCategoryId, agentCategoryId);

            var result = FilterBy(query);
            return await Task.FromResult(result.FirstOrDefault());
        }

        /// <summary>
        /// Get the agent category based on input name
        /// </summary>
        /// <param name="name">The name of the agent category</param>
        /// <returns>An instance of AgentCategory</returns>
        public async Task<AgentCategory> GetAgentCategory(string name)
        {
            var query = Builders<AgentCategory>.Filter.Eq(i => i.IsDeleted, false)
                    & Builders<AgentCategory>.Filter.Eq(i => i.Name, name);

            var result = FilterBy(query);
            return await Task.FromResult(result.FirstOrDefault());
        }
    }
}
