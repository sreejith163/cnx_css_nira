namespace Css.Api.SetupMenu.Models.DTO.Request.SkillTag
{
    public class CreateSkillTag : SkillTagAttribute
    {
        /// <summary>
        /// Gets or sets the reference identifier.
        /// </summary>
        public int? RefId { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        public string CreatedBy { get; set; }
    }
}

