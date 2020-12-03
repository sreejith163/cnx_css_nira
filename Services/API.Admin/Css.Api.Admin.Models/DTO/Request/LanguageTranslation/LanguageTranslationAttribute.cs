namespace Css.Api.Admin.Models.DTO.Request.LanguageTranslation
{
    public class LanguageTranslationAttribute
    {
        /// <summary>
        /// Gets or sets the language identifier.
        /// </summary>
        public int LanguageId { get; set; }

        /// <summary>
        /// Gets or sets the menu identifier.
        /// </summary>
        public int MenuId { get; set; }

        /// <summary>
        /// Gets or sets the variable identifier.
        /// </summary>
        public int VariableId { get; set; }

        /// <summary>
        /// Gets or sets the translation.
        /// </summary>
        public string Translation { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }
    }
}
