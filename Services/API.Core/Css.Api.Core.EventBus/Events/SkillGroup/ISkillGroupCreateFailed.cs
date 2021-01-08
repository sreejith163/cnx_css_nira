using System;

namespace Css.Api.Core.EventBus.Events.SkillGroup
{
    /// <summary>
    /// An interface event for any failure in creating client
    /// </summary>
    public interface ISkillGroupCreateFailed
    {
        /// <summary>Gets or sets the identifier.</summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>Gets or sets the name.</summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>Gets or sets the client identifier.</summary>
        /// <value>The client identifier.</value>
        public int ClientId { get; set; }

        /// <summary>Gets or sets the client lob group identifier.</summary>
        /// <value>The client lob group identifier.</value>
        public int ClientLobGroupId { get; set; }

        /// <summary>Gets or sets the timezone identifier.</summary>
        /// <value>The timezone identifier.</value>
        public int TimezoneId { get; set; }

        /// <summary>Gets or sets the first day of week.</summary>
        /// <value>The first day of week.</value>
        public int FirstDayOfWeek { get; set; }

        /// <summary>Gets or sets the operation hour.</summary>
        /// <value>The operation hour.</value>
        public string OperationHour { get; set; }

        /// <summary>Gets or sets the modified date.</summary>
        /// <value>The modified date.</value>
        public DateTimeOffset? ModifiedDate { get; set; }

    }
}
