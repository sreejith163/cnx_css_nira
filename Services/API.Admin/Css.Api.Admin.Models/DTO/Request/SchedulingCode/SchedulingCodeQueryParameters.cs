using Css.Api.Core.Models.DTO.Request;

namespace Css.Api.Admin.Models.DTO.Request.SchedulingCode
{
    public class SchedulingCodeQueryParameters: QueryStringParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulingCodeQueryParameters"/> class.
        /// </summary>
        public SchedulingCodeQueryParameters()
        {
            OrderBy = "CreatedDate";
        }

        /// <summary>
        /// Gets or sets a value indicating whether [skip page size].
        /// </summary>
        public bool SkipPageSize { get; set; }
    }
}
