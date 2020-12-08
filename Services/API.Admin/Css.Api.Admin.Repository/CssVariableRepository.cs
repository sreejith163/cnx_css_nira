using AutoMapper;
using AutoMapper.QueryableExtensions;
using Css.Api.Admin.Models.Domain;
using Css.Api.Admin.Models.DTO.Request.Menu;
using Css.Api.Admin.Models.DTO.Request.Variable;
using Css.Api.Admin.Models.DTO.Response.SchedulingCode;
using Css.Api.Admin.Repository.DatabaseContext;
using Css.Api.Admin.Repository.Interfaces;
using Css.Api.Core.DataAccess.Repository.SQL;
using Css.Api.Core.Models.DTO.Response;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Css.Api.Admin.Repository
{
    public class CssVariableRepository : GenericRepository<CssVariable>, ICssVariableRepository
    {
        // <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulingCodeRepository" /> class.
        /// </summary>
        /// <param name="repositoryContext">The repository context.</param>
        /// <param name="mapper">The mapper.</param>
        public CssVariableRepository(
            AdminContext repositoryContext,
            IMapper mapper)
            : base(repositoryContext)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the CSS variables.
        /// </summary>
        /// <param name="variableQueryParams">The variable query parameters.</param>
        /// <returns></returns>
        public async Task<List<KeyValue>> GetCssVariables(VariableQueryParams variableQueryParams)
        {
            var variables = FindAll();

            var pagedVariables = variables
                .Skip((variableQueryParams.PageNumber - 1) * variableQueryParams.PageSize)
                .Take(variableQueryParams.PageSize)
                .Include(x => x.Menu);

            var mappedVariables = pagedVariables
                .ProjectTo<KeyValue>(_mapper.ConfigurationProvider).ToList();

            return await Task.FromResult(mappedVariables);
        }

        /// <summary>
        /// Gets the CSS variable.
        /// </summary>
        /// <param name="variableIdDetails">The variable identifier details.</param>
        /// <returns></returns>
        public async Task<VariableDTO> GetCssVariable(VariableIdDetails variableIdDetails)
        {
            var variable = FindByCondition(x => x.Id == variableIdDetails.VariableId)
                .Include(x => x.Menu);

            var mappedVariable = variable
                .ProjectTo<VariableDTO>(_mapper.ConfigurationProvider).SingleOrDefault();

            return await Task.FromResult(mappedVariable);
        }

        /// <summary>
        /// Gets the CSS variablesby menu identifier.
        /// </summary>
        /// <param name="menuIdDetails">The menu identifier details.</param>
        /// <returns></returns>
        public async Task<List<VariableDTO>> GetCssVariablesbyMenuId(MenuIdDetails menuIdDetails)
        {
            var variables = FindByCondition(x => x.MenuId == menuIdDetails.MenuId)
                .ProjectTo<VariableDTO>(_mapper.ConfigurationProvider).ToList();

            return await Task.FromResult(variables);
        }
    }
}
