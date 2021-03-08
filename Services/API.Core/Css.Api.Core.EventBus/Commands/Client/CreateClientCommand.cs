using System;

namespace Css.Api.Core.EventBus.Commands.Client
{
    /// <summary>
    /// A command class for creating a client
    /// </summary>
    public class CreateClientCommand
    {
        /// <summary>Gets or sets the identifier.</summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>Gets or sets the name.</summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the reference identifier.
        /// </summary>
        public int? RefId { get; set; }

        /// <summary>Gets or sets the modified date.</summary>
        /// <value>The modified date.</value>
        public DateTimeOffset? ModifiedDate { get; set; }
    }
}
