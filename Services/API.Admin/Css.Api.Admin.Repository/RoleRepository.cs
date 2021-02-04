using AutoMapper;
using AutoMapper.QueryableExtensions;
using Css.Api.Core.DataAccess.Repository.SQL;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Utilities.Extensions;
using Css.Api.Admin.Models.Domain;
using Css.Api.Admin.Models.DTO.Request.Role;
using Css.Api.Admin.Models.DTO.Response.Role;
using Css.Api.Admin.Repository.DatabaseContext;
using Css.Api.Admin.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Admin.Repository
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleRepository" /> class.
        /// </summary>
        /// <param name="repositoryContext">The repository context.</param>
        /// <param name="mapper">The mapper.</param>
        public RoleRepository(
            AdminContext repositoryContext,
            IMapper mapper)
            : base(repositoryContext)
        {
            _mapper = mapper;
        }

        public void CreateRole(Role role)
        {
            Create(role);
        }

        public void DeleteRole(Role role)
        {
            Delete(role);
        }

        public async Task<Role> GetRole(RoleIdDetails roleIdDetails)
        {
            var role = FindByCondition(x => x.RoleId == roleIdDetails.RoleId && x.IsDeleted == false)
                .SingleOrDefault();

            return await Task.FromResult(role);
        }

        public async Task<PagedList<Entity>> GetRoles(RoleQueryParameters roleParameters)
        {
            var roles = FindAll();

            var filteredRoles = FilterRoles(roles, roleParameters);

            var sortedRoles = SortHelper.ApplySort(filteredRoles, roleParameters.OrderBy);

            var pagedRoles = sortedRoles;

            if (!roleParameters.SkipPageSize)
            {
                pagedRoles = sortedRoles
                .Skip((roleParameters.PageNumber - 1) * roleParameters.PageSize)
                .Take(roleParameters.PageSize);
            }

            var mappedRoles = pagedRoles
                .ProjectTo<RoleDTO>(_mapper.ConfigurationProvider);

            var shapedRoles = DataShaper.ShapeData(mappedRoles, roleParameters.Fields);

            return await PagedList<Entity>
                .ToPagedList(shapedRoles, filteredRoles.Count(), roleParameters.PageNumber, roleParameters.PageSize);

        }

        public async Task<Role> GetAllRole(RoleIdDetails roleIdDetails)
        {
            var role = FindByCondition(x => x.RoleId == roleIdDetails.RoleId)
                .SingleOrDefault();

            return await Task.FromResult(role);
        }

        public async Task<List<int>> GetRolesByName(RoleNameDetails roleNameDetails)
        {
            var roles = FindByCondition(x => string.Equals(x.Name.Trim(), roleNameDetails.Name.Trim(), StringComparison.OrdinalIgnoreCase) && x.IsDeleted == false)
                .Select(x => x.RoleId)
                .ToList();

            return await Task.FromResult(roles);
        }


        public async Task<List<int>> GetAllRolesByName(RoleNameDetails roleNameDetails)
        {
            var roles = FindByCondition(x => string.Equals(x.Name.Trim(), roleNameDetails.Name.Trim(), StringComparison.OrdinalIgnoreCase))
                .Select(x => x.RoleId)
                .ToList();

            return await Task.FromResult(roles);
        }


        public void UpdateRole(Role role)
        {
            Update(role);
        }

        private IQueryable<Role> FilterRoles(IQueryable<Role> roles, RoleQueryParameters roleQueryParameters)
        {
            if (!roles.Any() || string.IsNullOrWhiteSpace(roleQueryParameters.SearchKeyword))
            {
                return roles;
            }

            return roles.Where(o => o.Name.ToLower().Contains(roleQueryParameters.SearchKeyword.Trim().ToLower()));
        }
    }
}
