namespace Css.Api.SetupMenu.Models.DTO.Request.ClientLOBGroup
{
    public class CreateClientLOBGroup : ClientLOBGroupAttribute
    {
        /// <summary>
        /// Gets or sets the ref Id.
        /// </summary>
        public int? RefId { get; set; }


        /// <summary>Gets or sets the created by.</summary>
        /// <value>The created by.</value>
        public string CreatedBy { get; set; }
    }
}

