using Css.Api.Core.Models.DTO.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Admin.Models.DTO.Request.LanguageTranslation
{
    public class CssVariableQueryParameters : QueryStringParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CssVariableQueryParameters"/> class.
        /// </summary>
        public CssVariableQueryParameters()
        {
            OrderBy = "CreatedDate";
        }

        /// <summary>
        /// Gets or sets the menu identifier.
        /// </summary>
        /// <value>
        /// The menu identifier.
        /// </value>
        public int MenuId { get; set; }
    }
}
