using Css.Api.Core.Common.Models.DTO.Requests;

namespace Css.Api.Scheduling.Models.DTO.Requests.Client
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

        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        public int? ClientId { get; set; }
    }
}
