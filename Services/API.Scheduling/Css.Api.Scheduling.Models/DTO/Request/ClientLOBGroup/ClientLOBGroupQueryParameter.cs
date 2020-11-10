using Css.Api.Core.Models.DTO.Request;

namespace Css.Api.Scheduling.Models.DTO.Request.ClientLOBGroup
{
    public class ClientLOBGroupQueryParameter : QueryStringParameters
    {

        /// <summary>Initializes a new instance of the <see cref="ClientLOBGroupQueryParameter" /> class.</summary>
        public ClientLOBGroupQueryParameter()
        {
            OrderBy = "CreatedDate";
        }

        /// <summary>Gets or sets the client identifier.</summary>
        /// <value>The client identifier.</value>
        public int? ClientId { get; set; }
    }
}