﻿namespace Css.Api.Core.EventBus.Events.AgentSchedulingGroup
{
    public interface IAgentSchedulingGroupCreateSuccess
    {
        /// <summary>Gets or sets the identifier.</summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>Gets or sets the name.</summary>
        /// <value>The name.</value>
        public string Name { get; set; }
    }
}