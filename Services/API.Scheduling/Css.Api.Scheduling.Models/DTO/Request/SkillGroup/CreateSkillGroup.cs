namespace Css.Api.Scheduling.Models.DTO.Request.SkillGroup
{
    public class CreateSkillGroup : SkillGroupAttribute
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
