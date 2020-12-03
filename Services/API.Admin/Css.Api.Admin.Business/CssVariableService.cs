using AutoMapper;
using Css.Api.Admin.Business.Interfaces;
using Css.Api.Admin.Models.DTO.Request.LanguageTranslation;
using Css.Api.Admin.Repository.Interfaces;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Models.DTO.Response;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
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
        /// <param name="cssVariableQueryParameters"></param>
        /// <returns></returns>
        public async Task<CSSResponse> GetCssVariables(CssVariableQueryParameters cssVariableQueryParameters)
        {
            var menus = await _repository.CssVariable.GetCssVariables(cssVariableQueryParameters);
            return new CSSResponse(menus, HttpStatusCode.OK);
        }
    }
}
