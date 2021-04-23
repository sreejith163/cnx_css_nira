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
using Css.Api.Admin.Models.DTO.Request.Log;
using AutoMapper.Configuration;
using Css.Api.Admin.Models.DTO.Request.UserPermission;

namespace Css.Api.Admin.Business
{
    public class LogService : ILogService
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

     

   

        /// <summary>
        /// Initializes a new instance of the <see cref="logService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="mapper">The mapper.</param>
        public LogService(
          
            IRepositoryWrapper repository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
      
        {
         
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
          
        }

        public async Task<CSSResponse> CreateLog(CreateLogDTO logDetails)
        {
            var userPermissions = await _repository.UserPermissions.GetUserPermissionsBySso(new UserPermissionSsoDetails { Sso = logDetails.SSO });
            if (userPermissions?.Count > 0)
            {
                return new CSSResponse($"SSO '{logDetails.SSO}' not exists.", HttpStatusCode.Conflict);
            }
            var logRequest = _mapper.Map<Log>(logDetails);

            _repository.Log.CreateLog(logRequest);

            await _repository.SaveAsync();

           

            return new CSSResponse("",HttpStatusCode.Created);

        }
    }
}
