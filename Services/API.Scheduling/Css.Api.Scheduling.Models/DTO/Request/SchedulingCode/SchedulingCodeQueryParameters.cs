using Css.Api.Core.Models.DTO.Request;

namespace Css.Api.Scheduling.Models.DTO.Request.SchedulingCode
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
    }
}
