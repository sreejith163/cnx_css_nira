using Css.Api.Core.Models.DTO.Request;

namespace Css.Api.Scheduling.Models.DTO.Request.ClientLobGroup
{
    public class ClientLobGroupQueryparameter : QueryStringParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientLobGroupQueryparameter"/> class.
        /// </summary>
        public ClientLobGroupQueryparameter()
        {
            OrderBy = "CreatedDate";
        }
    }
}
