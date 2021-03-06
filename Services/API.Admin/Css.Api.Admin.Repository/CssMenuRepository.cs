﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Css.Api.Admin.Models.Domain;
using Css.Api.Admin.Models.DTO.Request.Menu;
using Css.Api.Admin.Repository.DatabaseContext;
using Css.Api.Admin.Repository.Interfaces;
using Css.Api.Core.DataAccess.Repository.SQL;
using Css.Api.Core.Models.DTO.Response;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Css.Api.Admin.Repository
{
    public class CssMenuRepository : GenericRepository<CssMenu>, ICssMenuRepository
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
        public CssMenuRepository(
            AdminContext repositoryContext,
            IMapper mapper)
            : base(repositoryContext)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the menus.
        /// </summary>
        /// <param name="menuQueryParameters">The menu query parameters.</param>
        /// <returns></returns>
        public async Task<List<KeyValue>> GetCssMenus()
        {
            var menus = FindAll()
                .ProjectTo<KeyValue>(_mapper.ConfigurationProvider).ToList();

            return await Task.FromResult(menus);
        }

        /// <summary>
        /// Gets the CSS menu.
        /// </summary>
        /// <param name="menuIdDetails">The menu identifier details.</param>
        /// <returns></returns>
        public async Task<KeyValue> GetCssMenu(MenuIdDetails menuIdDetails)
        {
            var menu = FindByCondition(x =>x.Id == menuIdDetails.MenuId)
                .ProjectTo<KeyValue>(_mapper.ConfigurationProvider).SingleOrDefault();

            return await Task.FromResult(menu);
        }
    }
}
