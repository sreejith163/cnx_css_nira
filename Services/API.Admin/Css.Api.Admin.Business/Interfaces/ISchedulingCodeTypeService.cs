using Css.Api.Core.Models.DTO.Response;
using System.Threading.Tasks;

namespace Css.Api.Admin.Business.Interfaces
{
    public interface ISchedulingCodeTypeService
    {
        /// <summary>
        /// Gets the scheduling code types.
        /// </summary>
        /// <returns></returns>
        Task<CSSResponse> GetSchedulingCodeTypes();
    }
}
