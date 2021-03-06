﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Css.Api.Admin.Models.Domain;
using Css.Api.Admin.Models.DTO.Request.Language;
using Css.Api.Admin.Models.DTO.Request.LanguageTranslation;
using Css.Api.Admin.Models.DTO.Request.Menu;
using Css.Api.Admin.Models.DTO.Request.Variable;
using Css.Api.Admin.Models.DTO.Response.LanguageTranslation;
using Css.Api.Admin.Repository.DatabaseContext;
using Css.Api.Admin.Repository.Interfaces;
using Css.Api.Core.DataAccess.Repository.SQL;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
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
        /// <param name="languageTranslationQueryParameters">The language translation query parameters.</param>
        /// <returns></returns>
        public async Task<PagedList<Entity>> GetLanguageTranslations(LanguageTranslationQueryParameters languageTranslationQueryParameters)
        {
            var languages = FindByCondition(x => x.IsDeleted == false);

            var filteredLanguages = FilterLanguages(languages, languageTranslationQueryParameters);

            var sortedSchedulingCodes = SortHelper.ApplySort(filteredLanguages, languageTranslationQueryParameters.OrderBy);

            var pagedLanguages = sortedSchedulingCodes
                .Skip((languageTranslationQueryParameters.PageNumber - 1) * languageTranslationQueryParameters.PageSize)
                .Take(languageTranslationQueryParameters.PageSize)
                .Include(x => x.Language)
                .Include(x => x.Menu)
                .Include(x => x.Variable);

            var mappedLanguages = pagedLanguages
                .ProjectTo<LanguageTranslationDTO>(_mapper.ConfigurationProvider);

            var shapedSchedulingCodes = DataShaper.ShapeData(mappedLanguages, languageTranslationQueryParameters.Fields);

            return await PagedList<Entity>
                .ToPagedList(shapedSchedulingCodes, filteredLanguages.Count(), languageTranslationQueryParameters.PageNumber, languageTranslationQueryParameters.PageSize);

        }

        /// <summary>
        /// Gets the language translation.
        /// </summary>
        /// <param name="languageTranslationIdDetails">The translation identifier details.</param>
        /// <returns></returns>
        public async Task<LanguageTranslation> GetLanguageTranslation(LanguageTranslationIdDetails languageTranslationIdDetails)
        {
            var language = FindByCondition(x => x.Id == languageTranslationIdDetails.TranslationId && x.IsDeleted == false)
                .Include(x => x.Language)
                .Include(x => x.Menu)
                .Include(x => x.Variable)
                .SingleOrDefault();

            return await Task.FromResult(language);
        }

        /// <summary>
        /// Gets the language translations by menu and language.
        /// </summary>
        /// <param name="languageIdDetails">The language identifier details.</param>
        /// <param name="menuIdDetails">The menu identifier details.</param>
        /// <returns></returns>
        public async Task<List<LanguageTranslation>> GetLanguageTranslationsByMenuAndLanguage(LanguageIdDetails languageIdDetails, MenuIdDetails menuIdDetails)
        {
            var languages = FindByCondition(x => x.IsDeleted == false && x.LanguageId == languageIdDetails.LanguageId && x.MenuId == menuIdDetails.MenuId)
                .Include(x => x.Language)
                .Include(x => x.Variable)
                .ToList();

            return await Task.FromResult(languages);
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
        /// <param name="languageTranslationQueryParameters">The language translation query parameters.</param>
        /// <returns></returns>
        private IQueryable<LanguageTranslation> FilterLanguages(IQueryable<LanguageTranslation> languages, LanguageTranslationQueryParameters languageTranslationQueryParameters)
        {
            if (!languages.Any())
            {
                return languages;
            }

            if (languageTranslationQueryParameters.LanguageId.HasValue && languageTranslationQueryParameters.LanguageId != default(int))
            {
                languages = languages.Where(x => x.LanguageId == languageTranslationQueryParameters.LanguageId);
            }

            if (languageTranslationQueryParameters.MenuId.HasValue && languageTranslationQueryParameters.MenuId != default(int))
            {
                languages = languages.Where(x => x.MenuId == languageTranslationQueryParameters.MenuId);
            }

            if (languageTranslationQueryParameters.VariableId.HasValue && languageTranslationQueryParameters.VariableId != default(int))
            {
                languages = languages.Where(x => x.VariableId == languageTranslationQueryParameters.VariableId);
            }

            if (!string.IsNullOrWhiteSpace(languageTranslationQueryParameters.SearchKeyword))
            {
                languages = languages.Where(o => o.Translation.ToLower().Contains(languageTranslationQueryParameters.SearchKeyword.Trim().ToLower()) ||
                                                 o.CreatedBy.ToLower().Contains(languageTranslationQueryParameters.SearchKeyword.Trim().ToLower()) ||
                                                 o.ModifiedBy.ToLower().Contains(languageTranslationQueryParameters.SearchKeyword.Trim().ToLower()));
            }

            return languages;
        }
    }
}
