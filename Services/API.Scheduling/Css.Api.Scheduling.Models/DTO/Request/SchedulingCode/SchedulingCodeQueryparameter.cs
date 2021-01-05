using Css.Api.Core.Models.DTO.Request;

namespace Css.Api.Scheduling.Models.DTO.Request.SchedulingCode
{
    public class SchedulingCodeQueryparameter : QueryStringParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulingCodeQueryparameter"/> class.
        /// </summary>
        public SchedulingCodeQueryparameter()
        {
            OrderBy = "CreatedDate";
        }

        /// <summary>
        /// Gets or sets a value indicating whether [skip page size].
        /// </summary>
        public bool SkipPageSize { get; set; }
    }
}
