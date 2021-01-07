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
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
