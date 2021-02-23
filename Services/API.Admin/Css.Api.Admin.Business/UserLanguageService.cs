using Css.Api.Admin.Models.DTO.Request.UserLanguage;
using Css.Api.Admin.Models.DTO.Response.UserLanguage;
using Css.Api.Core.Models.DTO.Response;
using System.Threading.Tasks;
using AutoMapper;
using Css.Api.Core.EventBus.Services;
using Css.Api.Admin.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Css.Api.Admin.Models.Domain;
using System.Net;

namespace Css.Api.Admin.Business.Interfaces
{
    public class UserLanguageService: IUserLanguageService
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


        /// <param name="repository">The repository.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="mapper">The mapper.</param>
        public UserLanguageService(
            IRepositoryWrapper repository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }


        public async Task<CSSResponse> UpdateUserLanguagePreference(UserLanguageEmployeeIdDetails userLanguageEmployeeIdDetails, UpdateUserLanguagePreferenceDTO userLanguagePreference)
        {
            UserLanguagePreference userLanguage = await _repository.UserLanguage.GetUserLanguagePreference(userLanguageEmployeeIdDetails);

            if (userLanguage == null)
            {
                //return new CSSResponse(HttpStatusCode.NotFound);

                var languagePreferenceRequest = _mapper.Map<UserLanguagePreference>(new UserLanguagePreference { EmployeeId = userLanguageEmployeeIdDetails.EmployeeId, LanguagePreference = userLanguagePreference.LanguagePreference });

                _repository.UserLanguage.CreateUserLanguagePreference(languagePreferenceRequest);

                await _repository.SaveAsync();

                return new CSSResponse(userLanguagePreference, HttpStatusCode.Created);

            }


            var updateLanguagePreferenceRequest = _mapper.Map(userLanguagePreference, userLanguage);

            _repository.UserLanguage.UpdateUserLanguagePreference(updateLanguagePreferenceRequest);

            await _repository.SaveAsync();

            return new CSSResponse(HttpStatusCode.NoContent);
        }

        public async Task<CSSResponse> GetUserLanguagePreference(string employeeId)
        {
            var userLanguage = await _repository.UserLanguage.GetUserLanguagePreference(new UserLanguageEmployeeIdDetails { EmployeeId = employeeId});
            if (userLanguage == null)
            {
                userLanguage = new UserLanguagePreference { EmployeeId = employeeId, LanguagePreference = "en" };
            }

            var mappedUserLanguage = _mapper.Map<UserLanguagePreference>(userLanguage);
            return new CSSResponse(mappedUserLanguage, HttpStatusCode.OK);
        }

    }
}
