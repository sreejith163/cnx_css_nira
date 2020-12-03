using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Admin.Models.DTO.Request.LanguageTranslation
{
    public class TranslationData
    {
        /// <summary>
        /// Gets or sets the language identifier.
        /// </summary>
        /// <value>
        /// The language identifier.
        /// </value>
        public int LanguageId { get; set; }

        /// <summary>
        /// Gets or sets the menu identifier.
        /// </summary>
        /// <value>
        /// The menu identifier.
        /// </value>
        public int MenuId { get; set; }

        /// <summary>
        /// Gets or sets the variable identifier.
        /// </summary>
        /// <value>
        /// The variable identifier.
        /// </value>
        public int VariableId { get; set; }
    }
}
