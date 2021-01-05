using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Core.EventBus.Events.Client
{
    /// <summary>
    /// An interface event for the successful completion of the client creation
    /// </summary>
    
    public interface IClientCreateSuccess
    {
        public int Id { get; set; }
    }
}
