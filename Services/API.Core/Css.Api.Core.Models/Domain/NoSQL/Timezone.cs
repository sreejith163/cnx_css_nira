using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Core.Models.Domain.NoSQL
{
    [BsonCollection("timezone")]
    public class Timezone : BaseDocument
    {
        /// <summary>
        /// Gets or sets the timezone identifier
        /// </summary>
        public int TimezoneId { get; set; }

        /// <summary>
        /// Gets or sets the timezone name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the UTC Offset
        /// </summary>
        public TimeSpan UtcOffset { get; set; }
    }
}
