using System.Net;

namespace Css.Api.Core.Common.Models.DTO.Responses
{
    public class CSSResponse
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        public HttpStatusCode Code { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Domain.JsonResult" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="code">The code.</param>
        public CSSResponse(object value, HttpStatusCode code)
        {
            Value = value;
            Code = code;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CSSResponse"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        public CSSResponse(HttpStatusCode code)
        {
            Code = code;
        }
    }
}