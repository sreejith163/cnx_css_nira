using AutoMapper;
using Css.Api.Admin.Business.Interfaces;
using Css.Api.Admin.Models.Domain;
using Css.Api.Admin.Models.DTO.Request.LanguageTranslation;
using Css.Api.Admin.Models.DTO.Request.Menu;
using Css.Api.Admin.Models.DTO.Response.LanguageTranslation;
using Css.Api.Admin.Repository.Interfaces;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Models.DTO.Response;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;

namespace Css.Api.Admin.Business
{
    public class LanguageTranslationService : ILanguageTranslationService
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
        /// Initializes a new instance of the <see cref="LanguageTranslationService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="mapper">The mapper.</param>
        public LanguageTranslationService(IRepositoryWrapper repository, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the language translations.
        /// </summary>
        /// <param name="translationQueryParameters">The translation query parameters.</param>
        /// <returns></returns>
        public async Task<CSSResponse> GetLanguageTranslations(TranslationQueryParameters translationQueryParameters)
        {
            var languageTranslations = await _repository.LanguageTranslation.GetLanguageTranslations(translationQueryParameters);
            _httpContextAccessor.HttpContext.Response.Headers.Add("X-Pagination", PagedList<Entity>.ToJson(languageTranslations));

            return new CSSResponse(languageTranslations, HttpStatusCode.OK);
        }

        /// <summary>
        /// Gets the language translation.
        /// </summary>
        /// <param name="translationIdDetails">The translation identifier details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> GetLanguageTranslation(TranslationIdDetails translationIdDetails)
        {
            var languageTranslation = await _repository.LanguageTranslation.GetLanguageTranslation(translationIdDetails);
            if (languageTranslation == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var mappedLanguageTranslation = _mapper.Map<LanguageTranslationDTO>(languageTranslation);
            return new CSSResponse(mappedLanguageTranslation, HttpStatusCode.OK);
        }

        /// <summary>
        /// Creates the language translation.
        /// </summary>
        /// <param name="translationDetails">The translation details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> CreateLanguageTranslation(CreateLanguageTranslation translationDetails)
        {
            var languageIdDetails = new LanguageIdDetails { LanguageId = translationDetails.LanguageId };
            var menuIdDetails = new MenuIdDetails { MenuId = translationDetails.MenuId };
            var variableIdDetails = new VariableIdDetails { VariableId = translationDetails.VariableId };

            var languageTranslations = await _repository.LanguageTranslation.GetLanguageTranslationByOtherIds(languageIdDetails, menuIdDetails, variableIdDetails);
            if (languageTranslations?.Count > 0)
            {
                return new CSSResponse($"Translation with language id '{translationDetails.LanguageId}' and " +
                    $"menu id '{translationDetails.MenuId}' and variable id '{translationDetails.VariableId}' already exists.", HttpStatusCode.Conflict);
            }

            var languageTranslationRequest = _mapper.Map<LanguageTranslation>(translationDetails);

            _repository.LanguageTranslation.CreateLanguageTranslation(languageTranslationRequest);

            await _repository.SaveAsync();

            return new CSSResponse(new TranslationIdDetails { TranslationId = languageTranslationRequest.Id }, HttpStatusCode.Created);
        }

        /// <summary>
        /// Updates the language translation.
        /// </summary>
        /// <param name="translationIdDetails">The translation identifier details.</param>
        /// <param name="translationDetails">The translation details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> UpdateLanguageTranslation(TranslationIdDetails translationIdDetails, UpdateLanguageTranslation translationDetails)
        {
            var language = await _repository.LanguageTranslation.GetLanguageTranslation(translationIdDetails);
            if (language == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var languageIdDetails = new LanguageIdDetails { LanguageId = translationDetails.LanguageId };
            var menuIdDetails = new MenuIdDetails { MenuId = translationDetails.MenuId };
            var variableIdDetails = new VariableIdDetails { VariableId = translationDetails.VariableId };
            var languages = await _repository.LanguageTranslation.GetLanguageTranslationByOtherIds(languageIdDetails, menuIdDetails, variableIdDetails);

            if (languages?.Count > 0 && languages.IndexOf(translationIdDetails.TranslationId) == -1)
            {
                return new CSSResponse($"Translation with language id '{translationDetails.LanguageId}' and " +
                    $"menu id '{translationDetails.MenuId}' and variable id '{translationDetails.VariableId}' already exists.", HttpStatusCode.Conflict);
            }

            var languageTranslationRequest = _mapper.Map(translationDetails, language);

            _repository.LanguageTranslation.UpdateLanguageTranslation(languageTranslationRequest);

            await _repository.SaveAsync();

            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Deletes the language translation.
        /// </summary>
        /// <param name="translationIdDetails">The translation identifier details.</param>
        /// <returns></returns>
        public async Task<CSSResponse> DeleteLanguageTranslation(TranslationIdDetails translationIdDetails)
        {
            var language = await _repository.LanguageTranslation.GetLanguageTranslation(translationIdDetails);
            if (language == null)
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            language.IsDeleted = true;

            _repository.LanguageTranslation.UpdateLanguageTranslation(language);
            await _repository.SaveAsync();

            return new CSSResponse(HttpStatusCode.NoContent);
        }
    }
}
