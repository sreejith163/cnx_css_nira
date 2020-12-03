using Css.Api.Admin.Business.Interfaces;
using Css.Api.Admin.Models.DTO.Request.Variable;
using Css.Api.Admin.Repository.Interfaces;
using Css.Api.Core.Models.DTO.Response;
using System.Net;
using System.Threading.Tasks;

namespace Css.Api.Admin.Business
{
    public class CssVariableService : ICssVariableService
    {
        /// <summary>
        /// The repository
        /// </summary>
        private readonly IRepositoryWrapper _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CssVariableService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public CssVariableService(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets css variables
        /// </summary>
        /// <param name="variableQueryParams">The variable query parameters.</param>
        /// <returns></returns>
        public async Task<CSSResponse> GetCssVariables(VariableQueryParams variableQueryParams)
        {
            var menus = await _repository.CssVariable.GetCssVariables(variableQueryParams);
            return new CSSResponse(menus, HttpStatusCode.OK);
        }
    }
}
