using Css.Api.Admin.Models.DTO.Request.AgentCategory;
using Css.Api.Core.Models.DTO.Response;
using System.Threading.Tasks;

namespace Css.Api.Admin.Business.Interfaces
{
    /// <summary>Interface for agent category service</summary>
    public interface IAgentCategoryService
    {
        /// <summary>
        /// Gets the agent categories.
        /// </summary>
        /// <param name="agentCategoryQueryParameter">The agent category query parameter.</param>
        /// <returns></returns>
        Task<CSSResponse> GetAgentCategories(AgentCategoryQueryParameter agentCategoryQueryParameter);

        /// <summary>
        /// Gets the agent category.
        /// </summary>
        /// <param name="agentCategoryIdDetails">The agent category identifier details.</param>
        /// <returns></returns>
        Task<CSSResponse> GetAgentCategory(AgentCategoryIdDetails agentCategoryIdDetails);

        /// <summary>
        /// Creates the agent category.
        /// </summary>
        /// <param name="agentCategoryDetails">The agent category details.</param>
        /// <returns></returns>
        Task<CSSResponse> CreateAgentCategory(CreateAgentCategory agentCategoryDetails);

        /// <summary>
        /// Updates the agent category.
        /// </summary>
        /// <param name="agentCategoryIdDetails">The agent category identifier details.</param>
        /// <param name="agentCategoryDetails">The agent category details.</param>
        /// <returns></returns>
        Task<CSSResponse> UpdateAgentCategory(AgentCategoryIdDetails agentCategoryIdDetails, UpdateAgentCategory agentCategoryDetails);

        /// <summary>
        /// Deletes the agent category.
        /// </summary>
        /// <param name="agentCategoryIdDetails">The agent category identifier details.</param>
        /// <returns></returns>
        Task<CSSResponse> DeleteAgentCategory(AgentCategoryIdDetails agentCategoryIdDetails);
    }
}