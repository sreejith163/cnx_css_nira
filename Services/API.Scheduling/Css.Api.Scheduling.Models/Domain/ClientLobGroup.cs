using Css.Api.Core.Models.Domain;

namespace Css.Api.Scheduling.Models.Domain
{
    [BsonCollection("client_lob_group")]
    public class ClientLobGroup : BaseDocument
    {
        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        public int ClientId { get; set; }

        /// <summary>
        /// Gets or sets the client lob group identifier.
        /// </summary>
        public int ClientLobGroupId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }
    }
}