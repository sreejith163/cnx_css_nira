using Css.Api.Admin.Business.Interfaces;
using Css.Api.Admin.Models.DTO.Request.Menu;
using Css.Api.Admin.Repository.Interfaces;
using Css.Api.Core.Models.DTO.Response;
using System.Net;
using System.Threading.Tasks;

namespace Css.Api.Admin.Business
{
    public class CssMenuService : ICssMenuService
    {
        /// <summary>
        /// The repository
        /// </summary>
        private readonly IRepositoryWrapper _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CssMenuService"/> class.
        /// </summary>
        /// <param name="repository"></param>
        public CssMenuService(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets the css menus
        /// </summary>
        /// <returns></returns>
        public async Task<CSSResponse> GetCssMenus()
        {
            var menus = await _repository.CssMenu.GetCssMenus();
            return new CSSResponse(menus, HttpStatusCode.OK);
        }

        /// <summary>
        /// Gets the CSS menu variables.
        /// </summary>
        /// <param name="menuId">The menu identifier.</param>
        /// <returns></returns>
        public async Task<CSSResponse> GetCssMenuVariables(MenuIdDetails menuIdDetails)
        {
            var menu = await _repository.CssMenu.GetCssMenu(menuIdDetails);
            if (menu == null)
            {
                return new CSSResponse($"Menu with id '{menuIdDetails.MenuId}' not found", HttpStatusCode.NotFound);
            }

            var menus = await _repository.CssVariable.GetCssVariablesbyMenuId(menuIdDetails);
            return new CSSResponse(menus, HttpStatusCode.OK);
        }
    }
}
