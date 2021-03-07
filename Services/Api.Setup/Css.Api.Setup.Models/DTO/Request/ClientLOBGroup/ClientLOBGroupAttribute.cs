namespace Css.Api.Setup.Models.DTO.Request.ClientLOBGroup
{
    public class ClientLOBGroupAttribute
    {
        /// <summary>Gets or sets the name.</summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>Gets or sets the client identifier.</summary>
        /// <value>The client identifier.</value>
        public int ClientId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public int? RefId { get; set; }

        /// <summary>Gets or sets the timezone identifier.</summary>
        /// <value>The timezone identifier.</value>
        public int TimezoneId { get; set; }

        /// <summary>Gets or sets the first day of week.</summary>
        /// <value>The first day of week.</value>
        public int FirstDayOfWeek { get; set; }
    }
}