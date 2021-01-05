using Css.Api.Core.Models.DTO.Request;

namespace Css.Api.Scheduling.Models.DTO.Request.Timezone
{
    public class TimezoneQueryParameters: QueryStringParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimezoneQueryParameters"/> class.
        /// </summary>
        public TimezoneQueryParameters()
        {
            OrderBy = "CreatedDate";
        }
    }
}
