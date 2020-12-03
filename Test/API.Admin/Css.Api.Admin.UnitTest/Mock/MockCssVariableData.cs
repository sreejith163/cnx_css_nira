using Css.Api.Admin.Models.Domain;
using Css.Api.Admin.Models.DTO.Request.LanguageTranslation;
using Css.Api.Core.Models.DTO.Response;
using System.Collections.Generic;
using System.Net;

namespace Css.Api.Admin.UnitTest.Mock
{
    public class MockCssVariableData
    {
        /// <summary>
        /// The CSS variable database
        /// </summary>
        public readonly List<CssVariable> cssVariableDB = new List<CssVariable>()
        {
            new CssVariable{ Id = 1, Name = "variable1", Description = "variable1", MenuId = 1},
            new CssVariable{ Id = 2, Name = "variable2", Description = "variable2", MenuId = 2},
            new CssVariable{ Id = 3, Name = "variable3", Description = "variable3", MenuId = 3}
        };

        /// <summary>
        /// Gets the variables.
        /// </summary>
        /// <param name="queryParameters">The query parameters.</param>
        /// <returns></returns>
        public CSSResponse GetVariables(CssVariableQueryParameters queryParameters)
        {
            return new CSSResponse(cssVariableDB, HttpStatusCode.OK);
        }
    }
}
