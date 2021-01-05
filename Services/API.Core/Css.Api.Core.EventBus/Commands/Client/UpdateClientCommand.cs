using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Core.EventBus.Commands.Client
{
    /// <summary>
    /// A command class for updating the client
    /// </summary>
    public class UpdateClientCommand
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
