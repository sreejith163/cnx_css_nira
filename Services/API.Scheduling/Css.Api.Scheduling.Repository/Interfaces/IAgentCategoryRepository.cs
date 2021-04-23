using Css.Api.Core.Models.Domain;
using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Scheduling.Models.DTO.Request.AgentCategory;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Repository.Interfaces
{
    /// <summary>
    /// Repository interface for Agent Category
    /// </summary>
    public interface IAgentCategoryRepository
    {
        /// <summary>
        /// Gets the scheduling codes.
        /// </summary>
        /// <param name="agentCategoryQueryparameter">The scheduling code queryparameter.</param>
        /// <returns></returns>
        Task<PagedList<Entity>> GetAgentCategories(AgentCategoryQueryparameter agentCategoryQueryparameter);

        /// <summary>
        /// Gets the scheduling code.
        /// </summary>
        /// <param name="agentCategoryIdDetails">The scheduling code identifier details.</param>
        /// <returns></returns>
        Task<AgentCategory> GetAgentCategory(AgentCategoryIdDetails agentCategoryIdDetails);

        /// <summary>
        /// Finds the agent categorys.
        /// </summary>
        /// <returns></returns>
        Task<List<AgentCategory>> FindAgentCategorys();

        /// <summary>
        /// Get the agent category list.
        /// </summary>
        /// <returns></returns>
        Task<List<AgentCategory>> GetAgentCategoryList();

        /// <summary>
        /// Gets the scheduling codes count by ids.
        /// </summary>
        /// <param name="agentCategorys">The scheduling codes.</param>
        /// <returns></returns>
        Task<long> GetAgentCategorysCountByIds(List<int> agentCategorys);

        /// <summary>
        /// Gets the scheduling codes count.
        /// </summary>
        /// <returns></returns>
        Task<int> GetAgentCategorysCount();

        /// <summary>
        /// Creates the scheduling code.
        /// </summary>
        /// <param name="agentCategoryRequest">The scheduling code request.</param>
        void CreateAgentCategory(AgentCategory agentCategoryRequest);

        /// <summary>
        /// Updates the scheduling code.
        /// </summary>
        /// <param name="agentCategoryRequest">The scheduling code request.</param>
        void UpdateAgentCategory(AgentCategory agentCategoryRequest);
    }
}

