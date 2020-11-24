using System;

namespace Css.Api.SetupMenu.Models.DTO.Response.ClientLOBGroup
{
    public class ClientLOBGroupDTO
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the reference identifier.
        /// </summary>
        public int? RefId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        public int ClientId { get; set; }

        /// <summary>
        /// Gets or sets the name of the client.
        /// </summary>
        /// <value>
        /// The name of the client.
        /// </value>
        public string ClientName { get; set; }

        /// <summary>
        /// Gets or sets the first day of week.
        /// </summary>
        public int FirstDayOfWeek { get; set; }

        /// <summary>
        /// Gets or sets the timezone identifier.
        /// </summary>
        public int TimezoneId { get; set; }

        /// <summary>
        /// Gets or sets the timezone label.
        /// </summary>
        public string TimezoneLabel { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        public DateTimeOffset CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the modified by.
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the modified date.
        /// </summary>
        public DateTimeOffset? ModifiedDate { get; set; }
    }
}
