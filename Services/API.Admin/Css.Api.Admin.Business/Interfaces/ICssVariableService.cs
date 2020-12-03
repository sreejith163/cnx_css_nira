using Css.Api.Admin.Models.DTO.Request.Variable;
using Css.Api.Core.Models.DTO.Response;
using System.Threading.Tasks;

namespace Css.Api.Admin.Business.Interfaces
{
    public interface ICssVariableService
    {
        /// <summary>
        /// Gets css variables
        /// </summary>
        /// <param name="variableQueryParams">The variable query parameters.</param>
        /// <returns></returns>
        Task<CSSResponse> GetCssVariables(VariableQueryParams variableQueryParams);
    }
}
