using Css.Api.Core.Models.DTO.Request;

namespace Css.Api.Admin.Models.DTO.Request.AgentCategory
{
    public class AgentCategoryQueryParameter : QueryStringParameters
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentCategoryQueryParameter"/> class.
        /// </summary>
        public AgentCategoryQueryParameter()
        {
            OrderBy = "CreatedDate";
        }
    }
}
