using Css.Api.Core.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.AgentCategoryValueView;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Repository.Interfaces
{
    public interface IAgentCategoryValueRepository 
    {
        /// <summary>
        /// Gets the agent admins.
        /// </summary>
        /// <param name="agentAdminQueryParameter">The agent admin query parameter.</param>
        /// <returns></returns>
        Task<PagedList<Entity>> GetAgentCategoryValues(AgentCategoryValueQueryParameter agentCategoryValueQueryParameter);
    }
}
