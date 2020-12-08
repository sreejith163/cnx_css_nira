using Css.Api.Admin.Models.DTO.Request.Menu;
using Css.Api.Admin.Models.DTO.Request.Variable;
using Css.Api.Admin.Models.DTO.Response.SchedulingCode;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Css.Api.Admin.Repository.Interfaces
{
    public interface ICssVariableRepository
    {
        /// <summary>
        /// Gets the CSS variables.
        /// </summary>
        /// <param name="variableQueryParams">The variable query parameters.</param>
        /// <returns></returns>
        Task<List<VariableDTO>> GetCssVariables(VariableQueryParams variableQueryParams);

        /// <summary>
        /// Gets the CSS variable.
        /// </summary>
        /// <param name="variableIdDetails">The variable identifier details.</param>
        /// <returns></returns>
        Task<VariableDTO> GetCssVariable(VariableIdDetails variableIdDetails);

        /// <summary>
        /// Gets the CSS variablesby menu identifier.
        /// </summary>
        /// <param name="menuIdDetails">The menu identifier details.</param>
        /// <returns></returns>
        Task<List<VariableDTO>> GetCssVariablesbyMenuId(MenuIdDetails menuIdDetails);
    }
}
