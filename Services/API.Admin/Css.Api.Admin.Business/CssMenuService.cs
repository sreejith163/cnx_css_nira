using AutoMapper;
using Css.Api.Admin.Business.Interfaces;
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
    public class CssMenuService : ICssMenuService
    {
        /// <summary>
        /// The repository
        /// </summary>
        private readonly IRepositoryWrapper _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CssMenuService"/> class.
        /// </summary>
        /// <param name="repository"></param>
        public CssMenuService(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets the css menus
        /// </summary>
        /// <returns></returns>
        public async Task<CSSResponse> GetCssMenus()
        {
            var menus = await _repository.CssMenu.GetCssMenus();
            return new CSSResponse(menus, HttpStatusCode.OK);
        }
    }
}
