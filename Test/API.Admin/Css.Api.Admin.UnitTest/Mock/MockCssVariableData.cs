using Css.Api.Admin.Models.Domain;
using Css.Api.Admin.Models.DTO.Request.Menu;
using Css.Api.Admin.Models.DTO.Request.Variable;
using Css.Api.Core.Models.DTO.Response;
using System.Collections.Generic;
using System.Net;

namespace Css.Api.Admin.UnitTest.Mock
{
    public class MockCssVariableData
    {
        /// <summary>
        /// The CSS menus database
        /// </summary>
        public readonly List<CssMenu> cssMenusDB = new List<CssMenu>()
        {
            new CssMenu{ Id = 1, Name = "menu1", Description = "menu1"},
            new CssMenu{ Id = 2, Name = "menu2", Description = "menu2"},
            new CssMenu{ Id = 3, Name = "menu3", Description = "menu3"}
        };

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
        public CSSResponse GetVariables(VariableQueryParams variableQueryParams)
        {
            return new CSSResponse(cssVariableDB, HttpStatusCode.OK);
        }

        /// <summary>
        /// Gets the menu variables.
        /// </summary>
        /// <param name="menuIdDetails">The menu identifier details.</param>
        /// <returns></returns>
        public CSSResponse GetMenuVariables(MenuIdDetails menuIdDetails)
        {
            var menu = cssMenusDB.Find(x => x.Id == menuIdDetails.MenuId);
            if (menu == null)
            {
                return new CSSResponse($"Menu with id '{menuIdDetails.MenuId}' not found", HttpStatusCode.NotFound);
            }

            return new CSSResponse(cssVariableDB.FindAll(x => x.MenuId == menuIdDetails.MenuId), HttpStatusCode.OK);
        }
    }
}
