using Css.Api.Scheduling.Models.DTO.Requests.Client;

namespace Css.Api.Scheduling.Models.DTO.Requests.SchedulingCode
{
    public class UpdateSchedulingCode : ClientAttributes
    {
        /// <summary>
        /// Gets or sets the modified by.
        /// </summary>
        public string ModifiedBy { get; set; }
    }
}
