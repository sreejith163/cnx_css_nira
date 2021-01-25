using AutoMapper;
using AutoMapper.QueryableExtensions;
using Css.Api.Core.DataAccess.Repository.SQL;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Utilities.Extensions;
using Css.Api.Admin.Models.Domain;
using Css.Api.Admin.Models.DTO.Request.UserPermission;
using Css.Api.Admin.Models.DTO.Response.UserPermission;
using Css.Api.Admin.Repository.DatabaseContext;
using Css.Api.Admin.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Admin.Repository
{
    public class UserPermissionRepository : GenericRepository<UserPermission>, IUserPermissionRepository
    {
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserPermissionRepository" /> class.
        /// </summary>
        /// <param name="repositoryContext">The repository context.</param>
        /// <param name="mapper">The mapper.</param>
        public UserPermissionRepository(
            AdminContext repositoryContext,
            IMapper mapper)
            : base(repositoryContext)
        {
            _mapper = mapper;
        }

        public void CreateUserPermission(UserPermission userPermission)
        {
            Create(userPermission);
        }

        public void DeleteUserPermission(UserPermission userPermission)
        {
            Delete(userPermission);
        }

        public async Task<UserPermission> GetUserPermission(UserPermissionEmployeeIdDetails userPermissionEmployeeIdDetails)
        {
            var userPermission = FindByCondition(x => x.EmployeeId == userPermissionEmployeeIdDetails.EmployeeId && x.IsDeleted == false)
                .Include(x => x.Role)
                .SingleOrDefault();

            return await Task.FromResult(userPermission);
        }

        public async Task<PagedList<Entity>> GetUserPermissions(UserPermissionQueryParameters userPermissionParameters)
        {
            var userPermissions = FindByCondition(x => x.IsDeleted == false);

            var filteredAgents = FilterUserPermissions(userPermissions, userPermissionParameters)
                .Include(x => x.Role);

            var sortedAgents = SortHelper.ApplySort(filteredAgents, userPermissionParameters.OrderBy);

            var pagedAgents = sortedAgents
                .Skip((userPermissionParameters.PageNumber - 1) * userPermissionParameters.PageSize)
                .Take(userPermissionParameters.PageSize)
                .Include(x => x.Role);

            var mappedAgents = pagedAgents
                .ProjectTo<UserPermissionDTO>(_mapper.ConfigurationProvider);

            var shapedAgents = DataShaper.ShapeData(mappedAgents, userPermissionParameters.Fields);

            return await PagedList<Entity>
                .ToPagedList(shapedAgents, filteredAgents.Count(), userPermissionParameters.PageNumber, userPermissionParameters.PageSize);

        }

        public async Task<List<int>> GetUserPermissionsByEmployeeId(UserPermissionEmployeeIdDetails userPermissionEmployeeIdDetails)
        {
            var userPermissions = FindByCondition(x => string.Equals(x.EmployeeId.Trim(), userPermissionEmployeeIdDetails.EmployeeId.Trim(), StringComparison.OrdinalIgnoreCase) && x.IsDeleted == false)
                .Select(x => x.Id)
                .ToList();

            return await Task.FromResult(userPermissions);
        }   

        public async Task<List<int>> GetUserPermissionsBySso(UserPermissionSsoDetails userPermissionSsoDetails)
        {
            var userPermissions = FindByCondition(x => string.Equals(x.Sso.Trim(), userPermissionSsoDetails.Sso.Trim(), StringComparison.OrdinalIgnoreCase) && x.IsDeleted == false)
                .Select(x => x.Id)
                .ToList();

            return await Task.FromResult(userPermissions);
        }

        public async Task<UserPermission> GetAllUserPermission(UserPermissionEmployeeIdDetails employeeIdDetails)
        {
            var userPermission = FindByCondition(x => x.EmployeeId == employeeIdDetails.EmployeeId)
                .SingleOrDefault();

            return await Task.FromResult(userPermission);
        }

        public async Task<List<int>> GetAllUserPermissionsByEmployeeId(UserPermissionEmployeeIdDetails userPermissionEmployeeIdDetails)
        {
            var userPermissions = FindByCondition(x => string.Equals(x.EmployeeId.Trim(), userPermissionEmployeeIdDetails.EmployeeId.Trim(), StringComparison.OrdinalIgnoreCase))
                .Select(x => x.Id)
                .ToList();

            return await Task.FromResult(userPermissions);
        }

        public async Task<List<int>> GetAllUserPermissionsBySso(UserPermissionSsoDetails userPermissionSsoDetails)
        {
            var userPermissions = FindByCondition(x => string.Equals(x.Sso.Trim(), userPermissionSsoDetails.Sso.Trim(), StringComparison.OrdinalIgnoreCase))
                .Select(x => x.Id)
                .ToList();

            return await Task.FromResult(userPermissions);
        }

        public void UpdateUserPermission(UserPermission userPermission)
        {
            Update(userPermission);
        }

        private IQueryable<UserPermission> FilterUserPermissions(IQueryable<UserPermission> userPermissions, UserPermissionQueryParameters userPermissionQueryParameters)
        {
            if (!userPermissions.Any() || string.IsNullOrWhiteSpace(userPermissionQueryParameters.SearchKeyword))
            {
                return userPermissions;
            }

            if (!string.IsNullOrWhiteSpace(userPermissionQueryParameters.SearchKeyword))
            {
                userPermissions = userPermissions.Where(o =>
                    (o.EmployeeId.ToLower().Contains(userPermissionQueryParameters.SearchKeyword.Trim().ToLower()))
                || (o.Firstname.ToLower().Contains(userPermissionQueryParameters.SearchKeyword.Trim().ToLower()))
                || (o.Lastname.ToLower().Contains(userPermissionQueryParameters.SearchKeyword.Trim().ToLower()))
                );
            }

            return userPermissions;

        }
    }
}
