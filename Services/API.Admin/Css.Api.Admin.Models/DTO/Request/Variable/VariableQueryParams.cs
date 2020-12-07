using Css.Api.Core.Models.DTO.Request;

namespace Css.Api.Admin.Models.DTO.Request.Variable
{
    public class VariableQueryParams : QueryStringParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TranslationQueryParameters"/> class.
        /// </summary>
        public VariableQueryParams()
        {
            OrderBy = "CreatedDate";
        }
    }
}
