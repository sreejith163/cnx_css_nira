using Css.Api.Core.Models.Domain;

namespace Css.Api.Scheduling.Models.Domain
{
    [BsonCollection("scheduling_code")]
    public class SchedulingCode : BaseDocument
    {
        /// <summary>
        /// Gets or sets the scheduling code identifier.
        /// </summary>
        public int SchedulingCodeId { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public string value { get; set; }

        /// <summary>
        /// Gets or sets the icon identifier.
        /// </summary>
        public int IconId { get; set; }

        /// <summary>
        /// Gets or sets the icon value.
        /// </summary>
        public string IconValue { get; set; }
    }
}
