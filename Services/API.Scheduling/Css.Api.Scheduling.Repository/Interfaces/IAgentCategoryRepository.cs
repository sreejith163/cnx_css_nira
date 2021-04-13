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
        /// <param name="schedulingCodeQueryparameter">The scheduling code queryparameter.</param>
        /// <returns></returns>
        Task<PagedList<Entity>> GetAgentCategorys(AgentCategoryQueryparameter schedulingCodeQueryparameter);

        /// <summary>
        /// Gets the scheduling code.
        /// </summary>
        /// <param name="schedulingCodeIdDetails">The scheduling code identifier details.</param>
        /// <returns></returns>
        Task<AgentCategory> GetAgentCategory(AgentCategoryIdDetails schedulingCodeIdDetails);

        /// <summary>
        /// Finds the agent categorys.
        /// </summary>
        /// <returns></returns>
        Task<List<AgentCategory>> FindAgentCategorys();

        /// <summary>
        /// Gets the scheduling codes count by ids.
        /// </summary>
        /// <param name="schedulingCodes">The scheduling codes.</param>
        /// <returns></returns>
        Task<long> GetAgentCategorysCountByIds(List<int> schedulingCodes);

        /// <summary>
        /// Gets the scheduling codes count.
        /// </summary>
        /// <returns></returns>
        Task<int> GetAgentCategorysCount();

        /// <summary>
        /// Creates the scheduling code.
        /// </summary>
        /// <param name="schedulingCodeRequest">The scheduling code request.</param>
        void CreateAgentCategory(AgentCategory schedulingCodeRequest);

        /// <summary>
        /// Updates the scheduling code.
        /// </summary>
        /// <param name="schedulingCodeRequest">The scheduling code request.</param>
        void UpdateAgentCategory(AgentCategory schedulingCodeRequest);
    }
}

