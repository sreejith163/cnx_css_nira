using Css.Api.Core.Models.DTO.Response;
using System.Threading.Tasks;

namespace Css.Api.AdminOps.Business.Interfaces
{
    public interface ISchedulingCodeIconService
    {
        /// <summary>
        /// Gets the scheduling code icons.
        /// </summary>
        /// <returns></returns>
        Task<CSSResponse> GetSchedulingCodeIcons();
    }
}
