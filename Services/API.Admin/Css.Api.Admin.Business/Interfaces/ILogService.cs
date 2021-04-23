using Css.Api.Admin.Models.DTO.Request.Log;
using Css.Api.Core.Models.DTO.Response;
using System.Threading.Tasks;

namespace Css.Api.Admin.Business.Interfaces
{


    public interface ILogService
    {
        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <param name="createLogDTO">The role parameters.</param>
        /// <returns></returns>
        Task<CSSResponse> CreateLog(CreateLogDTO createLogDTO);

    }
}
