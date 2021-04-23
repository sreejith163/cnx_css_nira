using Css.Api.Core.Models.Domain.NoSQL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Reporting.Repository.Interfaces
{
    /// <summary>
    /// The interface for agent category repository
    /// </summary>
    public interface IAgentCategoryRepository
    {
        /// <summary>
        /// Get the agent categories.
        /// </summary>
        /// <returns>List of instances of AgentCategory</returns>
        Task<List<AgentCategory>> GetAgentCategories();

        /// <summary>
        /// Get the agent categories based on input agent category ids
        /// </summary>
        /// <param name="agentCategoryIds">The ids of agent categories</param>
        /// <returns>List of instances of AgentCategory</returns>
        Task<List<AgentCategory>> GetAgentCategories(List<int> agentCategoryIds);

        /// <summary>
        /// Get the agent categories based on input names
        /// </summary>
        /// <param name="names">The names of agent categories</param>
        /// <returns>List of instances of AgentCategory</returns>
        Task<List<AgentCategory>> GetAgentCategories(List<string> names);

        /// <summary>
        /// Get the agent category based on input agent category id
        /// </summary>
        /// <param name="agentCategoryId">The id of the agent category</param>
        /// <returns>An instance of AgentCategory</returns>
        Task<AgentCategory> GetAgentCategory(int agentCategoryId);

        /// <summary>
        /// Get the agent category based on input name
        /// </summary>
        /// <param name="name">The name of the agent category</param>
        /// <returns>An instance of AgentCategory</returns>
        Task<AgentCategory> GetAgentCategory(string name);
    }
}
