using AutoMapper;
using Css.Api.Core.EventBus;
using Css.Api.Core.EventBus.Services;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Models.DTO.Response;
using Css.Api.Admin.Business.Interfaces;
using Css.Api.Admin.Models.Domain;
using Css.Api.Admin.Models.DTO.Request.UserPermission;
using Css.Api.Admin.Models.DTO.Response.UserPermission;
using Css.Api.Admin.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Admin.Business
{
    public class UserPermissionService : IUserPermissionService
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
        /// Initializes a new instance of the <see cref="userPermissionService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="mapper">The mapper.</param>
        public UserPermissionService(
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
        /// Gets the userPermissions.
        /// </summary>
        /// <param name="userPermissionParameters">The userPermission parameters.</param>
        /// <returns></returns>
        public async Task<CSSResponse> GetUserPermissions(UserPermissionQueryParameters userPermissionParameters)
        {
            var userPermissions = await _repository.UserPermissions.GetUserPermissions(userPermissionParameters);
            _httpContextAccessor.HttpContext.Response.Headers.Add("X-Pagination", PagedList<Entity>.ToJson(userPermissions));

            return new CSSResponse(userPermissions, HttpStatusCode.OK);
        }

        /// <summary>
        /// Gets the userPermission.
        /// </summary>
        /// <param name="userPermissionEmployeeIdDetails">The userPermission identifier details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> GetUserPermission(UserPermissionEmployeeIdDetails userPermissionEmployeeIdDetails)
        {
            var userPermission = await _repository.UserPermissions.GetUserPermission(userPermissionEmployeeIdDetails);
            if (userPermission == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var mappedAgent = _mapper.Map<UserPermissionDTO>(userPermission);
            return new CSSResponse(mappedAgent, HttpStatusCode.OK);
        }

        /// <summary>
        /// Creates the userPermission.
        /// </summary>
        /// <param name="userPermissionDetails">The userPermission details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> CreateUserPermission(CreateUserPermissionDTO userPermissionDetails)
        {
            var userPermissions = await _repository.UserPermissions.GetUserPermissionsBySso(new UserPermissionSsoDetails { Sso = userPermissionDetails.Sso });

            if (userPermissions?.Count > 0)
            {
                return new CSSResponse($"userPermission with name '{userPermissionDetails.Sso}' already exists.", HttpStatusCode.Conflict);
            }

            var userPermissionRequest = _mapper.Map<UserPermission>(userPermissionDetails);

            _repository.UserPermissions.CreateUserPermission(userPermissionRequest);

            await _repository.SaveAsync();


            return new CSSResponse(new UserPermissionSsoDetails { Sso = userPermissionRequest.Sso }, HttpStatusCode.Created);
            //return new CSSResponse(userPermissions, HttpStatusCode.Created);
        }

        /// <summary>
        /// Updates the userPermission.
        /// </summary>
        /// <param name="userPermissionEmployeeIdDetails">The userPermission identifier details.</param>
        /// <param name="userPermissionDetails">The userPermission details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> UpdateUserPermission(UserPermissionEmployeeIdDetails userPermissionEmployeeIdDetails, UpdateUserPermissionDTO userPermissionDetails)
        {
            UserPermission userPermission = await _repository.UserPermissions.GetUserPermission(userPermissionEmployeeIdDetails);

            if (userPermission == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            if (userPermission.Sso != userPermissionDetails.Sso)
            {
                var userPermissions = await _repository.UserPermissions.GetUserPermissionsBySso(new UserPermissionSsoDetails { Sso = userPermissionDetails.Sso });
                if (userPermissions?.Count > 0 && userPermissions.IndexOf(userPermission.Id) == -1)
                {
                    return new CSSResponse($"Agent with sso '{userPermissionDetails.Sso}' already exists.", HttpStatusCode.Conflict);
                }
            }

            if (userPermission.EmployeeId != userPermissionDetails.EmployeeId)
            {
                var userPermissions = await _repository.UserPermissions.GetUserPermissionsByEmployeeId(new UserPermissionEmployeeIdDetails { EmployeeId = userPermissionDetails.EmployeeId });
                if (userPermissions?.Count > 0 && userPermissions.IndexOf(userPermission.Id) == -1)
                {
                    return new CSSResponse($"Agent with employeeId '{userPermissionDetails.EmployeeId}' already exists.", HttpStatusCode.Conflict);
                }
            }

            var userPermissionDetailsPreUpdate = new UserPermission
            {
                Sso = userPermission.Sso,
                UserRoleId = userPermission.UserRoleId,
                LanguagePreference = userPermission.LanguagePreference,
                EmployeeId = userPermission.EmployeeId,
                Firstname = userPermission.Firstname,
                Lastname = userPermission.Lastname,
                IsDeleted = userPermission.IsDeleted,
                ModifiedDate = userPermission.ModifiedDate
            };

            var userPermissionRequest = _mapper.Map(userPermissionDetails, userPermission);

            _repository.UserPermissions.UpdateUserPermission(userPermissionRequest);

            await _repository.SaveAsync();

            return new CSSResponse(HttpStatusCode.NoContent);
        }


        /// <summary>
        /// Updates the userPermission.
        /// </summary>
        /// <param name="userPermissionEmployeeIdDetails">The userPermission identifier details.</param>
        /// <param name="userPermissionDetails">The userPermission details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> UpdateUserLanguagePreference(UserPermissionEmployeeIdDetails userPermissionEmployeeIdDetails, UserLanguagePreference userPermissionLanguagePreference)
        {
            UserPermission userPermission = await _repository.UserPermissions.GetUserPermission(userPermissionEmployeeIdDetails);

            if (userPermission == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }


            var updateLanguagePreferencePreRequest = new UserPermission
            {
                Sso = userPermission.Sso,
                UserRoleId = userPermission.UserRoleId,
                LanguagePreference = userPermission.LanguagePreference,
                EmployeeId = userPermission.EmployeeId,
                Firstname = userPermission.Firstname,
                Lastname = userPermission.Lastname,
                IsDeleted = userPermission.IsDeleted,
                ModifiedDate = userPermission.ModifiedDate
            };

            var userPermissionRequest = _mapper.Map(userPermissionLanguagePreference, userPermission);

            _repository.UserPermissions.UpdateUserPermission(userPermissionRequest);

            await _repository.SaveAsync();


            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>Reverts the userPermission.</summary>
        /// <param name="userPermissionEmployeeIdDetails">The userPermission identifier details.</param>
        /// <param name="userPermissionDetails">The userPermission details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<CSSResponse> RevertUserPermission(UserPermissionEmployeeIdDetails userPermissionEmployeeIdDetails, UpdateUserPermissionDTO userPermissionDetails)
        {
            UserPermission userPermission = await _repository.UserPermissions.GetAllUserPermission(userPermissionEmployeeIdDetails);

            if (userPermission == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var userPermissions = await _repository.UserPermissions.GetAllUserPermissionsBySso(new UserPermissionSsoDetails { Sso = userPermissionDetails.Sso });
            if (userPermissions?.Count > 0 && userPermissions.IndexOf(userPermission.Id) == -1)
            {
                return new CSSResponse($"userPermission with sso '{userPermissionDetails.Sso}' already exists.", HttpStatusCode.Conflict);
            }

            var userPermissionRequest = _mapper.Map(userPermissionDetails, userPermission);
            _repository.UserPermissions.UpdateUserPermission(userPermissionRequest);

            await _repository.SaveAsync();

            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Deletes the userPermission.
        /// </summary>
        /// <param name="userPermissionEmployeeIdDetails">The userPermission identifier details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> DeleteUserPermission(UserPermissionEmployeeIdDetails userPermissionEmployeeIdDetails)
        {
            var userPermission = await _repository.UserPermissions.GetUserPermission(userPermissionEmployeeIdDetails);
            if (userPermission == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var userPermissionDetailsPreUpdate = new UserPermission
            {
                Sso = userPermission.Sso,
                EmployeeId = userPermission.EmployeeId,
                LanguagePreference = userPermission.LanguagePreference,
                Firstname = userPermission.Firstname,
                Lastname = userPermission.Lastname,
                IsDeleted = userPermission.IsDeleted,
                ModifiedDate = userPermission.ModifiedDate

            };

            userPermission.IsDeleted = true;

            _repository.UserPermissions.UpdateUserPermission(userPermission);
            await _repository.SaveAsync();

            return new CSSResponse(HttpStatusCode.NoContent);
        }
    }
}
