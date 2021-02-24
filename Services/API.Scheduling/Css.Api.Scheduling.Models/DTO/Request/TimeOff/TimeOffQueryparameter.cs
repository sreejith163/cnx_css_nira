using Css.Api.Core.Models.DTO.Request;

namespace Css.Api.Scheduling.Models.DTO.Request.TimeOff
{
    public class TimeOffQueryparameter : QueryStringParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeOffQueryparameter"/> class.
        /// </summary>
        public TimeOffQueryparameter()
        {
            OrderBy = "CreatedDate";
        }
    }
}
