namespace Css.Api.Admin.Models.DTO.Request.SchedulingCode
{
    public class CreateSchedulingCode: SchedulingCodeAttributes
    {
        /// <summary>
        /// Gets or sets the ref Id.
        /// </summary>
        public int? RefId { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        public string CreatedBy { get; set; }
    }
}
