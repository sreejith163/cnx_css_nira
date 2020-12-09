using Newtonsoft.Json;
using System;

namespace Css.Api.Admin.Models.DTO.Response.LanguageTranslation
{
    public class LanguageTranslationDTO
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the language identifier.
        /// </summary>
        public int LanguageId { get; set; }

        /// <summary>
        /// Gets or sets the name of the language.
        /// </summary>
        public string LanguageName { get; set; }

        /// <summary>
        /// Gets or sets the menu identifier.
        /// </summary>
        public int MenuId { get; set; }

        /// <summary>
        /// Gets or sets the name of the menu.
        /// </summary>
        public string MenuName { get; set; }

        /// <summary>
        /// Gets or sets the variable identifier.
        /// </summary>
        public int VariableId { get; set; }

        /// <summary>
        /// Gets or sets the name of the variable.
        /// </summary>
        public string VariableName { get; set; }

        /// <summary>
        /// Gets or sets the variable description.
        /// </summary>
        public string VariableDescription { get; set; }

        /// <summary>
        /// Gets or sets the translation.
        /// </summary>
        public string Translation { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        public DateTimeOffset CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the modified by.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the modified date.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? ModifiedDate { get; set; }
    }
}
