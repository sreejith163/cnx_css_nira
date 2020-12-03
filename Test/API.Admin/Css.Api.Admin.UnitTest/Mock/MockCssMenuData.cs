using Css.Api.Admin.Models.Domain;
using Css.Api.Core.Models.DTO.Response;
using System.Collections.Generic;
using System.Net;

namespace Css.Api.Admin.UnitTest.Mock
{
    public class MockCssMenuData
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
        /// Gets the menus.
        /// </summary>
        /// <returns></returns>
        public CSSResponse GetCssMenus()
        {
            return new CSSResponse(cssMenusDB, HttpStatusCode.OK);
        }
    }
}
