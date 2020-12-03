using AutoMapper;
using AutoMapper.QueryableExtensions;
using Css.Api.Admin.Models.Domain;
using Css.Api.Admin.Models.DTO.Request.LanguageTranslation;
using Css.Api.Admin.Models.DTO.Request.Menu;
using Css.Api.Admin.Models.DTO.Response.LanguageTranslation;
using Css.Api.Admin.Repository.DatabaseContext;
using Css.Api.Admin.Repository.Interfaces;
using Css.Api.Core.DataAccess.Repository.SQL;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Utilities.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Css.Api.Admin.Repository
{
    public class LanguageTranslationRepository : GenericRepository<LanguageTranslation>, ILanguageTranslationRepository
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
        public LanguageTranslationRepository(
            AdminContext repositoryContext,
            IMapper mapper)
            : base(repositoryContext)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the language translations.
        /// </summary>
        /// <param name="translationQueryParameters">The translation query parameters.</param>
        /// <returns></returns>
        public async Task<PagedList<Entity>> GetLanguageTranslations(TranslationQueryParameters translationQueryParameters)
        {
            var languages = FindByCondition(x => x.IsDeleted == false);

            var filteredLanguages = FilterLanguages(languages, translationQueryParameters.SearchKeyword, translationQueryParameters.LanguageId, 
                                                    translationQueryParameters.MenuId, translationQueryParameters.VariableId);

            var sortedSchedulingCodes = SortHelper.ApplySort(filteredLanguages, translationQueryParameters.OrderBy);

            var pagedLanguages = sortedSchedulingCodes
                .Skip((translationQueryParameters.PageNumber - 1) * translationQueryParameters.PageSize)
                .Take(translationQueryParameters.PageSize);

            var mappedLanguages = pagedLanguages
                .ProjectTo<LanguageTranslationDTO>(_mapper.ConfigurationProvider);

            var shapedSchedulingCodes = DataShaper.ShapeData(mappedLanguages, translationQueryParameters.Fields);

            return await PagedList<Entity>
                .ToPagedList(shapedSchedulingCodes, filteredLanguages.Count(), translationQueryParameters.PageNumber, translationQueryParameters.PageSize);

        }

        /// <summary>
        /// Gets the language translation.
        /// </summary>
        /// <param name="translationIdDetails">The translation identifier details.</param>
        /// <returns></returns>
        public async Task<LanguageTranslation> GetLanguageTranslation(TranslationIdDetails translationIdDetails)
        {
            var language = FindByCondition(x => x.Id == translationIdDetails.TranslationId && x.IsDeleted == false)
                                 .SingleOrDefault();

            return await Task.FromResult(language);
        }

        /// <summary>
        /// Gets the language translation by other ids.
        /// </summary>
        /// <param name="languageIdDetails">The language identifier details.</param>
        /// <param name="menuIdDetails">The menu identifier details.</param>
        /// <param name="variableIdDetails">The variable identifier details.</param>
        /// <returns></returns>
        public async Task<List<int>> GetLanguageTranslationByOtherIds(LanguageIdDetails languageIdDetails, MenuIdDetails menuIdDetails, VariableIdDetails variableIdDetails)
        {
            var languages = FindByCondition(x => x.LanguageId == languageIdDetails.LanguageId && x.MenuId == menuIdDetails.MenuId &&
                            x.VariableId == variableIdDetails.VariableId && x.IsDeleted == false)
                                  .Select(x => x.Id)
                                  .ToList();

            return await Task.FromResult(languages);
        }

        /// <summary>
        /// Creates the language translation.
        /// </summary>
        /// <param name="languageTranslation">The language translation.</param>
        public void CreateLanguageTranslation(LanguageTranslation languageTranslation)
        {
            Create(languageTranslation);
        }

        /// <summary>
        /// Updates the language translation.
        /// </summary>
        /// <param name="languageTranslation">The language translation.</param>
        public void UpdateLanguageTranslation(LanguageTranslation languageTranslation)
        {
            Update(languageTranslation);
        }

        /// <summary>
        /// Deletes the language translation.
        /// </summary>
        /// <param name="languageTranslation">The language translation.</param>
        public void DeleteLanguageTranslation(LanguageTranslation languageTranslation)
        {
            Delete(languageTranslation);
        }

        /// <summary>
        /// Filters the languages.
        /// </summary>
        /// <param name="languages">The languages.</param>
        /// <param name="keyword">The keyword.</param>
        /// <param name="languageId">The language identifier.</param>
        /// <param name="menuId">The menu identifier.</param>
        /// <param name="variableId">The variable identifier.</param>
        /// <returns></returns>
        private IQueryable<LanguageTranslation> FilterLanguages(IQueryable<LanguageTranslation> languages, string keyword, int? languageId,
            int? menuId, int? variableId)
        {
            if (!languages.Any())
            {
                return languages;
            }

            if (languageId.HasValue && languageId != default(int))
            {
                languages = languages.Where(x => x.LanguageId == languageId);
            }

            if (menuId.HasValue && menuId != default(int))
            {
                languages = languages.Where(x => x.MenuId == menuId);
            }

            if (variableId.HasValue && variableId != default(int))
            {
                languages = languages.Where(x => x.VariableId == variableId);
            }

            if (string.IsNullOrWhiteSpace(keyword))
            {
                languages = languages.Where(o => o.Description.ToLower().Contains(keyword.Trim().ToLower()) || o.Translation.ToLower().Contains(keyword.Trim().ToLower()));

            }
            return languages;
        }
    }
}
