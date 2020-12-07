using Css.Api.Core.Models.DTO.Request;

namespace Css.Api.Admin.Models.DTO.Request.LanguageTranslation
{
    public class LanguageTranslationQueryParameters : QueryStringParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageTranslationQueryParameters"/> class.
        /// </summary>
        public LanguageTranslationQueryParameters()
        {
            OrderBy = "CreatedDate";
        }

        /// <summary>
        /// Gets or sets the language identifier.
        /// </summary>
        public int? LanguageId { get; set; }

        /// <summary>
        /// Gets or sets the menu identifier.
        /// </summary>
        public int? MenuId { get; set; }

        /// <summary>
        /// Gets or sets the variable identifier.
        /// </summary>
        public int? VariableId { get; set; }
    }
}
