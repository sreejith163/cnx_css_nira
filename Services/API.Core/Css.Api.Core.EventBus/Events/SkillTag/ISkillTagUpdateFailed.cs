using System;

namespace Css.Api.Core.EventBus.Events.SkillTag
{
    public interface ISkillTagUpdateFailed
    {
        /// <summary>Gets or sets the identifier.</summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>Gets or sets the name old value.</summary>
        /// <value>The name old value.</value>
        public string NameOldValue { get; set; }

        /// <summary>Gets or sets the client identifier oldvalue.</summary>
        /// <value>The client identifier oldvalue.</value>
        public int ClientIdOldValue { get; set; }

        /// <summary>Gets or sets the client lob group identifier oldvalue.</summary>
        /// <value>The client lob group identifier oldvalue.</value>
        public int ClientLobGroupIdOldvalue { get; set; }

        /// <summary>Gets or sets the skill group identifier old value.</summary>
        /// <value>The skill group identifier old value.</value>
        public int SkillGroupIdOldValue { get; set; }

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
    }
}