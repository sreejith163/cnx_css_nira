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
        /// Get or sets the timezone Iana timezone abbreviation
        /// </summary>
        public string Abbreviation { get; set; }

    }
}
