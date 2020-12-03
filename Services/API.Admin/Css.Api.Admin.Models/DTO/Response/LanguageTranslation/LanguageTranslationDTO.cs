using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Admin.Models.DTO.Response.LanguageTranslation
{
    public class LanguageTranslationDTO
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the language identifier.
        /// </summary>
        /// <value>
        /// The language identifier.
        /// </value>
        public int LanguageId { get; set; }

        /// <summary>
        /// Gets or sets the name of the language.
        /// </summary>
        /// <value>
        /// The name of the language.
        /// </value>
        public string LanguageName { get; set; }

        /// <summary>
        /// Gets or sets the menu identifier.
        /// </summary>
        /// <value>
        /// The menu identifier.
        /// </value>
        public int MenuId { get; set; }

        /// <summary>
        /// Gets or sets the name of the menu.
        /// </summary>
        /// <value>
        /// The name of the menu.
        /// </value>
        public string MenuName { get; set; }

        /// <summary>
        /// Gets or sets the variable identifier.
        /// </summary>
        /// <value>
        /// The variable identifier.
        /// </value>
        public int VariableId { get; set; }

        /// <summary>
        /// Gets or sets the name of the variable.
        /// </summary>
        /// <value>
        /// The name of the variable.
        /// </value>
        public string VariableName { get; set; }

        /// <summary>
        /// Gets or sets the translation.
        /// </summary>
        /// <value>
        /// The translation.
        /// </value>
        public string Translation { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>
        /// The created by.
        /// </value>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>
        /// The created date.
        /// </value>
        public DateTimeOffset CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the modified by.
        /// </summary>
        /// <value>
        /// The modified by.
        /// </value>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the modified date.
        /// </summary>
        /// <value>
        /// The modified date.
        /// </value>
        public DateTimeOffset? ModifiedDate { get; set; }
    }
}
