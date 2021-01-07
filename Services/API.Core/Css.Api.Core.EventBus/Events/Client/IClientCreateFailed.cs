﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Core.EventBus.Events.Client
{
    /// <summary>
    /// An interface event for any failure in creating client
    /// </summary>
    public interface IClientCreateFailed
    {
        /// <summary>Gets or sets the identifier.</summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>Gets or sets the name.</summary>
        /// <value>The name.</value>
        public string Name { get; set; }

    }
}
