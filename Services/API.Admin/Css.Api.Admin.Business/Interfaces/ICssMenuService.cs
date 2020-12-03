using Css.Api.Core.Models.DTO.Response;
using System.Threading.Tasks;

namespace Css.Api.Admin.Business.Interfaces
{
    public interface ICssMenuService
    {
        /// <summary>
        /// Gets the CSS menus.
        /// </summary>
        /// <returns></returns>
        Task<CSSResponse> GetCssMenus();

        /// <summary>
        /// Gets the CSS menu variables.
        /// </summary>
        /// <param name="menuId">The menu identifier.</param>
        /// <returns></returns>
        Task<CSSResponse> GetCssMenuVariables(int menuId);
    }
}
