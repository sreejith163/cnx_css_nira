using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
