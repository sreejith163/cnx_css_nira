namespace Css.Api.Scheduling.Models.DTO.Request.Client
{
    public class CreateClient: ClientAttributes
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
