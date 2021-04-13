using AutoMapper;
using Css.Api.Admin.Business.Interfaces;
using Css.Api.Admin.Models.Domain;
using Css.Api.Admin.Models.DTO.Response.Auth;
using Css.Api.Admin.Repository.Interfaces;
using Css.Api.Core.Models.DTO.Response;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Admin.Business
{
  public class AuthService : IAuthService
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
        /// Initializes a new instance of the <see cref="AuthService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="mapper">The mapper.</param>
        public AuthService(

            IRepositoryWrapper repository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)

        {

            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;

        }

        public async Task<CSSResponse> Login(CredentialsViewModel credentials)
        {
            if (credentials.UserName == "undefined" || String.IsNullOrEmpty(credentials.UserName) || String.IsNullOrWhiteSpace(credentials.UserName))
            {
                return new CSSResponse($"Username is required", HttpStatusCode.BadRequest);
            }
            else if (credentials.Password == "undefined" || String.IsNullOrEmpty(credentials.Password) || String.IsNullOrWhiteSpace(credentials.Password))
            {
                return new CSSResponse($"Password is required", HttpStatusCode.BadRequest);
            }
            var authLogin = await _repository.Auth.Login(credentials);
            if (authLogin == null)
            {
                return new CSSResponse($"Invalid username or password.", HttpStatusCode.NotFound);
            }
            else
            {
                var mappedAuth = _mapper.Map<NonSsoDTO>(authLogin);
                return new CSSResponse(mappedAuth, HttpStatusCode.OK);
            }
        }

    }
}
