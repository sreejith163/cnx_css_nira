using System;

namespace Css.Api.Core.EventBus.Commands.SchedulingCode
{
    public class CreateSchedulingCodeCommand
    {
        /// <summary>Gets or sets the identifier.</summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>Gets or sets the name.</summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>Gets or sets the priority number.</summary>
        /// <value>The priority number.</value>
        public int PriorityNumber { get; set; }

        /// <summary>Gets or sets the icon identifier.</summary>
        /// <value>The icon identifier.</value>
        public int IconId { get; set; }

        /// <summary>Gets or sets the scheduling type code.</summary>
        /// <value>The scheduling type code.</value>
        public string SchedulingTypeCode { get; set; }

        /// <summary>Gets or sets the modified date.</summary>
        /// <value>The modified date.</value>
        public DateTimeOffset? ModifiedDate { get; set; }
    }
}

