using Css.Api.Scheduling.Models.DTO.Request.Client;

namespace Css.Api.Scheduling.Models.DTO.Request.SchedulingCode
{
    public class UpdateSchedulingCode : ClientAttributes
    {
        /// <summary>
        /// Gets or sets the modified by.
        /// </summary>
        public string ModifiedBy { get; set; }
    }
}
