using Css.Api.Core.Models.DTO.Request;

namespace Css.Api.Scheduling.Models.DTO.Request.Client
{
    public class ClientQueryParameters: QueryStringParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientQueryParameters"/> class.
        /// </summary>
        public ClientQueryParameters()
        {
            OrderBy = "CreatedDate";
        }
    }
}
