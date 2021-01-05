using Css.Api.Core.Models.DTO.Request;

namespace Css.Api.Scheduling.Models.DTO.Request.Client
{
    public class ClientQueryparameter : QueryStringParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientQueryparameter"/> class.
        /// </summary>
        public ClientQueryparameter()
        {
            OrderBy = "CreatedDate";
        }
    }
}
