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
using System.Net.Http;
using System.Text.Json;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

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

        private readonly IConfiguration _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="userPermissionService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="mapper">The mapper.</param>
        public UserPermissionService(
            IConfiguration config,
            IRepositoryWrapper repository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            IBusService bus)
        {
            _config = config;
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

                // call the agent admins api to check if the user exists on agent admins
                HttpClient client = new HttpClient();
                var apiUrl = _config.GetValue<string>("GlobalConfiguration:GatewayUrl");
                client.BaseAddress = new Uri(apiUrl);
                var urlParameters = $"agentadmins/employees/{userPermissionEmployeeIdDetails.EmployeeId}";

                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync(urlParameters).Result;

                if (response.IsSuccessStatusCode)
                {   
                    string responseStream = await response.Content.ReadAsStringAsync();
                    UserPermissionAgent agentPermission = JsonConvert.DeserializeObject<UserPermissionAgent>(responseStream);
                    // map the response to new userPermission intended for agents
                    userPermission = new UserPermission { EmployeeId = agentPermission.EmployeeId, Firstname = agentPermission.Firstname, Lastname = agentPermission.Lastname, Sso = agentPermission.Sso };
                }
                else
                {
                    return new CSSResponse(HttpStatusCode.NotFound);
                }
            }

            var mappedPermission = _mapper.Map<UserPermissionDTO>(userPermission);
            return new CSSResponse(mappedPermission, HttpStatusCode.OK);
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
                return new CSSResponse($"User Permission with name '{userPermissionDetails.Sso}' already exists.", HttpStatusCode.Conflict);
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
                    return new CSSResponse($"User Permission with sso '{userPermissionDetails.Sso}' already exists.", HttpStatusCode.Conflict);
                }
            }

            if (userPermission.EmployeeId != userPermissionDetails.EmployeeId)
            {
                var userPermissions = await _repository.UserPermissions.GetUserPermissionsByEmployeeId(new UserPermissionEmployeeIdDetails { EmployeeId = userPermissionDetails.EmployeeId });
                if (userPermissions?.Count > 0 && userPermissions.IndexOf(userPermission.Id) == -1)
                {
                    return new CSSResponse($"User Permission with employeeId '{userPermissionDetails.EmployeeId}' already exists.", HttpStatusCode.Conflict);
                }
            }


            var userPermissionRequest = _mapper.Map(userPermissionDetails, userPermission);

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
                return new CSSResponse($"User Permission with sso '{userPermissionDetails.Sso}' already exists.", HttpStatusCode.Conflict);
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

            userPermission.IsDeleted = true;

            _repository.UserPermissions.UpdateUserPermission(userPermission);
            await _repository.SaveAsync();

            return new CSSResponse(HttpStatusCode.NoContent);
        }
    }
}
