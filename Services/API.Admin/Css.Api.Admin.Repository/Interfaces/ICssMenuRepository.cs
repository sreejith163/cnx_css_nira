using Css.Api.Core.Models.Domain;
using Css.Api.Core.Models.DTO.Response;
using System;
using System.Collections.Generic;
using System.Text;
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
