using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Core.EventBus.Events.Client
{
    /// <summary>
    /// An interface event for any failure in creating client
    /// </summary>
    public interface IClientCreateFailed
    {
        public int Id { get; set; }
    }
}
