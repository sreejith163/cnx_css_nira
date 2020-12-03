using Css.Api.Core.Models.DTO.Response;
using System.Threading.Tasks;

namespace Css.Api.Admin.Business.Interfaces
{
    public interface ICssVariableService
    {
        /// <summary>
        /// Gets css variables
        /// </summary>
        /// <returns></returns>
        Task<CSSResponse> GetCssVariables();
    }
}
