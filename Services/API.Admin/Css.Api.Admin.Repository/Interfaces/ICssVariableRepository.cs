﻿using Css.Api.Admin.Models.DTO.Request.Menu;
using Css.Api.Admin.Models.DTO.Request.Variable;
using Css.Api.Admin.Models.DTO.Response.SchedulingCode;
using Css.Api.Core.Models.DTO.Response;
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
        /// Gets the CSS variablesby menu identifier.
        /// </summary>
        /// <param name="menuIdDetails">The menu identifier details.</param>
        /// <returns></returns>
        Task<List<KeyValue>> GetCssVariablesbyMenuId(MenuIdDetails menuIdDetails);
    }
}
