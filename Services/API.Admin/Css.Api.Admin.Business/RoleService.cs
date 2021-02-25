using AutoMapper;
using Css.Api.Core.EventBus.Services;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Models.DTO.Response;
using Css.Api.Admin.Business.Interfaces;
using Css.Api.Admin.Models.Domain;
using Css.Api.Admin.Models.DTO.Request.Role;
using Css.Api.Admin.Models.DTO.Response.Role;
using Css.Api.Admin.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Css.Api.Admin.Business
{
        public class RoleService : IRoleService
        {
            /// <summary>
            /// The repository
            /// </summary>
            private readonly IRepositoryWrapper _repository;

            /// <summary>
            /// The HTTP context accessor
            /// </summary>
            private readonly IHttpContextAccessor _httpContextAccessor;

            /// <summary>
            /// The mapper
            /// </summary>
            private readonly IMapper _mapper;

            private readonly IBusService _bus;

            /// <summary>
            /// Initializes a new instance of the <see cref="roleService" /> class.
            /// </summary>
            /// <param name="repository">The repository.</param>
            /// <param name="httpContextAccessor">The HTTP context accessor.</param>
            /// <param name="mapper">The mapper.</param>
            public RoleService(
                IRepositoryWrapper repository,
                IHttpContextAccessor httpContextAccessor,
                IMapper mapper,
                IBusService bus)
            {
                _repository = repository;
                _httpContextAccessor = httpContextAccessor;
                _mapper = mapper;
                _bus = bus;
            }

            /// <summary>
            /// Gets the roles.
            /// </summary>
            /// <param name="roleParameters">The role parameters.</param>
            /// <returns></returns>
            public async Task<CSSResponse> GetRoles(RoleQueryParameters roleParameters)
            {
                var roles = await _repository.Roles.GetRoles(roleParameters);
                _httpContextAccessor.HttpContext.Response.Headers.Add("X-Pagination", PagedList<Entity>.ToJson(roles));

                return new CSSResponse(roles, HttpStatusCode.OK);
            }

            /// <summary>
            /// Gets the role.
            /// </summary>
            /// <param name="roleIdDetails">The role identifier details.</param>
            /// <returns></returns>
            public async Task<CSSResponse> GetRole(RoleIdDetails roleIdDetails)
            {
                Role roleDetails;
                if (roleIdDetails.RoleId == 0)
                {
                    roleDetails = new Role
                    {
                        RoleId = 0,
                        Name = "Agent"
                    };
                }
                else
                {
                    roleDetails = await _repository.Roles.GetRole(roleIdDetails);

                    if (roleDetails == null)
                    {
                        return new CSSResponse(HttpStatusCode.NotFound);
                    }

                }

                     

                var mappedRole = _mapper.Map<RoleDTO>(roleDetails);
                return new CSSResponse(mappedRole, HttpStatusCode.OK);
            }

            /// <summary>
            /// Creates the role.
            /// </summary>
            /// <param name="roleDetails">The role details.</param>
            /// <returns></returns>
            public async Task<CSSResponse> CreateRole(CreateRoleDTO roleDetails)
            {
                var roles = await _repository.Roles.GetRolesByName(new RoleNameDetails { Name = roleDetails.Name });

                if (roles?.Count > 0)
                {
                    return new CSSResponse($"role with Role Name '{roleDetails.Name}' already exists.", HttpStatusCode.Conflict);
                }

                var roleRequest = _mapper.Map<Role>(roleDetails);
                _repository.Roles.CreateRole(roleRequest);

                await _repository.SaveAsync();

                //_bus.SendCommand<CreateroleCommand>(MassTransitConstants.roleCreateCommandRouteKey,
                //    new
                //    {
                //        Id = roleRequest.Id,
                //        Sso = roleRequest.Sso,
                //        EmployeeId = roleRequest.EmployeeId,
                //        Firstname = roleRequest.Firstname,
                //        Lastname = roleRequest.Lastname,
                //        ModifiedDate = roleRequest.ModifiedDate
                //    });

                return new CSSResponse(new RoleNameDetails { Name = roleRequest.Name }, HttpStatusCode.Created);
                //return new CSSResponse(roles, HttpStatusCode.Created);
            }

            /// <summary>
            /// Updates the role.
            /// </summary>
            /// <param name="roleIdDetails">The role identifier details.</param>
            /// <param name="roleDetails">The role details.</param>
            /// <returns></returns>
            public async Task<CSSResponse> UpdateRole(RoleIdDetails roleIdDetails, UpdateRoleDTO roleDetails)
            {
                Role role = await _repository.Roles.GetRole(roleIdDetails);

                if (role == null)
                {
                    return new CSSResponse(HttpStatusCode.NotFound);
                }

                var roles = await _repository.Roles.GetRolesByName(new RoleNameDetails { Name = roleDetails.Name });
                if (roles?.Count > 0 && roles.IndexOf(roleIdDetails.RoleId) == -1)
                {
                    return new CSSResponse($"role with Role Name '{roleDetails.Name}' already exists.", HttpStatusCode.Conflict);
                }

                var roleDetailsPreUpdate = new Role
                {
                    RoleId = role.RoleId,
                    Name = role.Name,
                    Description = role.Description,
                    IsDeleted = role.IsDeleted,
                    ModifiedDate = role.ModifiedDate
                };

                var roleRequest = _mapper.Map(roleDetails, role);

                _repository.Roles.UpdateRole(roleRequest);

                await _repository.SaveAsync();


                //_bus.SendCommand<UpdateroleCommand>(
                //    MassTransitConstants.roleUpdateCommandRouteKey,
                //    new
                //    {
                //        Id = roleRequest.Id,
                //        Sso = roleDetailsPreUpdate.Sso,
                //        EmployeeId = roleDetailsPreUpdate.EmployeeId,
                //        Firstname = roleDetailsPreUpdate.Firstname,
                //        Lastname = roleDetailsPreUpdate.Lastname,
                //        ModifiedDate = roleRequest.ModifiedDate
                //    });


                return new CSSResponse(HttpStatusCode.NoContent);
            }


            /// <summary>Reverts the role.</summary>
            /// <param name="roleIdDetails">The role identifier details.</param>
            /// <param name="roleDetails">The role details.</param>
            /// <returns>
            ///   <br />
            /// </returns>
            public async Task<CSSResponse> RevertRole(RoleIdDetails roleIdDetails, UpdateRoleDTO roleDetails)
            {
                Role role = await _repository.Roles.GetAllRole(roleIdDetails);

                if (role == null)
                {
                    return new CSSResponse(HttpStatusCode.NotFound);
                }

                var roles = await _repository.Roles.GetRolesByName(new RoleNameDetails { Name = roleDetails.Name });
                if (roles?.Count > 0 && roles.IndexOf(roleIdDetails.RoleId) == -1)
                {
                    return new CSSResponse($"role with Role Name '{roleDetails.Name}' already exists.", HttpStatusCode.Conflict);
                }

                var roleRequest = _mapper.Map(roleDetails, role);
                _repository.Roles.UpdateRole(roleRequest);

                await _repository.SaveAsync();

                return new CSSResponse(HttpStatusCode.NoContent);
            }

            /// <summary>
            /// Deletes the role.
            /// </summary>
            /// <param name="roleIdDetails">The role identifier details.</param>
            /// <returns></returns>
            public async Task<CSSResponse> DeleteRole(RoleIdDetails roleIdDetails)
            {
                var role = await _repository.Roles.GetRole(roleIdDetails);
                if (role == null)
                {
                    return new CSSResponse(HttpStatusCode.NotFound);
                }

                var roleDetailsPreUpdate = new Role
                {
                    RoleId = role.RoleId,
                    Name = role.Name,
                    Description = role.Description,
                    IsDeleted = role.IsDeleted,
                    ModifiedDate = role.ModifiedDate

                };

                role.IsDeleted = true;

                _repository.Roles.UpdateRole(role);
                await _repository.SaveAsync();

                //_bus.SendCommand<DeleteroleCommand>(
                //   MassTransitConstants.roleDeleteCommandRouteKey,
                //   new
                //   {
                //       Sso = role.Sso,
                //       EmployeeId = role.EmployeeId,
                //       Firstname = role.Firstname,
                //       Lastname = role.Lastname,
                //       IsDeletedOldValue = roleDetailsPreUpdate.IsDeleted,
                //       ModifiedDateOldValue = roleDetailsPreUpdate.ModifiedDate,
                //       IsDeletedNewValue = role.IsDeleted
                //   });

                return new CSSResponse(HttpStatusCode.NoContent);
            }

        }
    
}
