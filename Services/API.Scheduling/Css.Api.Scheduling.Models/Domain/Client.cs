using Css.Api.Core.Models.Domain;

namespace Css.Api.Scheduling.Models.Domain
{
    [BsonCollection("client")]
    public class Client : BaseDocument
    {
        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        public int ClientId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }
    }
}