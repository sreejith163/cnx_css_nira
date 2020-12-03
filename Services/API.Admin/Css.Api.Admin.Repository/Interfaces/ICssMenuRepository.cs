using Css.Api.Core.Models.DTO.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Css.Api.Admin.Repository.Interfaces
{
    public interface ICssMenuRepository
    {
        /// <summary>
        /// Gets the CSS menus.
        /// </summary>
        /// <returns></returns>
        Task<List<KeyValue>> GetCssMenus();
    }
}
