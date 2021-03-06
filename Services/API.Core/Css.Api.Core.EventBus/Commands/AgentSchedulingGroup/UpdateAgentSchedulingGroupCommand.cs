﻿using System;

namespace Css.Api.Core.EventBus.Commands.AgentSchedulingGroup
{
    /// <summary>
    ///   <br />
    /// </summary>
    public class UpdateAgentSchedulingGroupCommand
    {
        /// <summary>Gets or sets the identifier.</summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>Gets or sets the name old value.</summary>
        /// <value>The name old value.</value>
        public string NameOldValue { get; set; }

        /// <summary>Gets or sets the reference identifier old value.</summary>
        /// <value>The reference identifier old value.</value>
        public int? RefIdOldValue { get; set; }

        /// <summary>Gets or sets the client identifier oldvalue.</summary>
        /// <value>The client identifier oldvalue.</value>
        public int ClientIdOldValue { get; set; }

        /// <summary>Gets or sets the client lob group identifier oldvalue.</summary>
        /// <value>The client lob group identifier oldvalue.</value>
        public int ClientLobGroupIdOldvalue { get; set; }

        /// <summary>Gets or sets the skill group identifier old value.</summary>
        /// <value>The skill group identifier old value.</value>
        public int SkillGroupIdOldValue { get; set; }

        /// <summary>Gets or sets the skill tag identifier old value.</summary>
        /// <value>The skill tag identifier old value.</value>
        public int SkillTagIdOldValue { get; set; }

        /// <summary>Gets or sets the timezone identifier oldvalue.</summary>
        /// <value>The timezone identifier oldvalue.</value>
        public int TimezoneIdOldValue { get; set; }

        /// <summary>Gets or sets the old estart provisioning flag.</summary>
        /// <value>The old estart provisioning flag.</value>
        public bool EstartProvisionOldValue { get; set; }

        /// <summary>Gets or sets the first day of week oldvalue.</summary>
        /// <value>The first day of week oldvalue.</value>
        public int FirstDayOfWeekOldValue { get; set; }

        /// <summary>Gets or sets the operation hour.</summary>
        /// <value>The operation hour.</value>
        public string OperationHourOldValue { get; set; }

        /// <summary>Gets or sets the modified by old value.</summary>
        /// <value>The modified by old value.</value>
        public string ModifiedByOldValue { get; set; }

        /// <summary>Gets or sets the modified date old value.</summary>
        /// <value>The modified date old value.</value>
        public DateTimeOffset? ModifiedDateOldValue { get; set; }

        /// <summary>Gets or sets a value indicating whether this instance is deleted old value.</summary>
        /// <value>
        ///   <c>true</c> if this instance is deleted old value; otherwise, <c>false</c>.</value>
        public bool IsDeletedOldValue { get; set; }

        /// <summary>Gets or sets the name new value.</summary>
        /// <value>The name new value.</value>
        public string NameNewValue { get; set; }

        /// <summary>Gets or sets the reference identifier new value.</summary>
        /// <value>The reference identifier new value.</value>
        public int? RefIdNewValue { get; set; }

        /// <summary>Gets or sets the client identifier new value.</summary>
        /// <value>The client identifier new value.</value>
        public int ClientIdNewValue { get; set; }

        /// <summary>Gets or sets the client lob group identifier new value.</summary>
        /// <value>The client lob group identifier new value.</value>
        public int ClientLobGroupIdNewValue { get; set; }

        /// <summary>Gets or sets the skill group identifier new value.</summary>
        /// <value>The skill group identifier new value.</value>
        public int SkillGroupIdNewValue { get; set; }

        /// <summary>Gets or sets the skill tag identifier new value.</summary>
        /// <value>The skill tag identifier new value.</value>
        public int SkillTagIdNewValue { get; set; }

        /// <summary>Gets or sets the timezone identifier new value.</summary>
        /// <value>The timezone identifier new value.</value>
        public int TimezoneIdNewValue { get; set; }

        /// <summary>Gets or sets a value indicating whether this instance is deleted new value.</summary>
        /// <value>
        ///   <c>true</c> if this instance is deleted new value; otherwise, <c>false</c>.</value>
        public bool IsDeletedNewValue { get; set; }

        /// <summary>Gets or sets the new estart provisioning flag.</summary>
        /// <value>The new estart provisioning flag.</value>
        public bool EstartProvisionNewValue { get; set; }
    }
}