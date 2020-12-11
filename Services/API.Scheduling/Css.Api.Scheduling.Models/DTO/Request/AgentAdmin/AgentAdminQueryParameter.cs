using Css.Api.Core.Models.DTO.Request;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentAdmin
{
    public class AgentAdminQueryParameter : QueryStringParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AgentAdminQueryParameter"/> class.
        /// </summary>
        public AgentAdminQueryParameter()
        {
            OrderBy = "CreatedDate";
        }
    }
}