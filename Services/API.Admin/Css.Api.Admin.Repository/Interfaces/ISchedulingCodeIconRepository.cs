using Css.Api.Core.Models.DTO.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Css.Api.Admin.Repository.Interfaces
{
    public interface ISchedulingCodeIconRepository
    {
        /// <summary>
        /// Gets the scheduling code icons.
        /// </summary>
        /// <returns></returns>
        Task<List<KeyValue>> GetSchedulingCodeIcons();
    }
}
