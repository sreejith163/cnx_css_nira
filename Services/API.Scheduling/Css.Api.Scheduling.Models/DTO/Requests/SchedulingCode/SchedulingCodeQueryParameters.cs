using Css.Api.Core.Models.DTO.Requests;

namespace Css.Api.Scheduling.Models.DTO.Requests.SchedulingCode
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
