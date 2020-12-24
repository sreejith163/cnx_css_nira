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
        /// Gets or sets the anme.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
