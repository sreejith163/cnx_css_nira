using Css.Api.Core.Models.DTO.Response;
using Css.Api.Scheduling.Models.DTO.Request.AgentCategoryValueView;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Business.Interfaces
{
    public interface IAgentCategoryValueService
    {
        /// <summary>
        /// Gets the agent category values.
        /// </summary>
        /// <param name="agentCategoryValueQueryParameter">The agentCategoryValue query parameters.</param>
        /// <returns></returns>
        Task<CSSResponse> GetAgentCategoryValues(AgentCategoryValueQueryParameter agentCategoryValueQueryParameter);

        Task<CSSResponse> ImportAgentCategoryValue(List<ImportAgentCategoryValue> importAgentCategoryValue, string modifiedBy);
    }
}