using Css.Api.Admin.Models.Domain;
using Css.Api.Admin.Models.DTO.Request.AgentCategory;
using Css.Api.Core.Models.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Css.Api.Admin.Repository.Interfaces
{
    public interface IAgentCategoryRepository
    {
        /// <summary>
        /// Gets the agentCategories.
        /// </summary>
        /// <param name="agentCategoryParameters">The agentCategory parameters.</param>
        /// <returns></returns>
        Task<PagedList<Entity>> GetAgentCategories(AgentCategoryQueryParameter agentCategoryParameters);

        /// <summary>
        /// Gets the name of the agentCategories by.
        /// </summary>
        /// <param name="agentCategoryNameDetails">The agentCategory name details.</param>
        /// <returns></returns>
        Task<List<int>> GetAgentCategoriesByName(AgentCategoryNameDetails agentCategoryNameDetails);

        /// <summary>
        /// Gets the agentCategory.
        /// </summary>
        /// <param name="agentCategoryIdDetails">The agentCategory identifier details.</param>
        /// <returns></returns>
        Task<AgentCategory> GetAgentCategory(AgentCategoryIdDetails agentCategoryIdDetails);

        /// <summary>
        /// Creates the agentCategory.
        /// </summary>
        /// <param name="agentCategory">The agentCategory.</param>
        void CreateAgentCategory(AgentCategory agentCategory);

        /// <summary>
        /// Updates the agentCategory.
        /// </summary>
        /// <param name="agentCategory">The agentCategory.</param>
        /// <returns></returns>
        void UpdateAgentCategory(AgentCategory agentCategory);

        /// <summary>
        /// Deletes the agentCategory.
        /// </summary>
        /// <param name="agentCategory">The agentCategory.</param>
        /// <returns></returns>
        void DeleteAgentCategory(AgentCategory agentCategory);
    }
}

