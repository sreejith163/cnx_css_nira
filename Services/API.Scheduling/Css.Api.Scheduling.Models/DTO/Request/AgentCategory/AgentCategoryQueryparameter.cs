using Css.Api.Core.Models.DTO.Request;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentCategory
{
    public class AgentCategoryQueryparameter : QueryStringParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AgentCategoryQueryparameter"/> class.
        /// </summary>
        public AgentCategoryQueryparameter()
        {
            OrderBy = "CreatedDate";
        }
    }
}
