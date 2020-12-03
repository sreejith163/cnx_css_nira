using Css.Api.Core.Models.DTO.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Admin.Models.DTO.Request.LanguageTranslation
{
    public class TranslationQueryParameters : QueryStringParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TranslationQueryParameters"/> class.
        /// </summary>
        public TranslationQueryParameters()
        {
            OrderBy = "CreatedDate";
        }
    }
}
