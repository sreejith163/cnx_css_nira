using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Admin.Models.DTO.Request.LanguageTranslation
{
    public class CreateLanguageTranslation : LanguageTranslationAttribute
    {
        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>
        /// The created by.
        /// </value>
        public string CreatedBy { get; set; }
    }
}
