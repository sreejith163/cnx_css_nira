using Css.Api.Admin.Models.Domain;
using Css.Api.Admin.Models.DTO.Request.LanguageTranslation;
using Css.Api.Core.Models.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Css.Api.Admin.UnitTest.Mock
{
    public class MockLanguageTranslationData
    {
        /// <summary>
        /// The SchedulingCodes
        /// </summary>
        private readonly List<LanguageTranslation> languageTranslationsDB = new List<LanguageTranslation>()
        {
            new LanguageTranslation{ Id = 1, LanguageId = 1, MenuId = 1, VariableId = 1, Description = "test1",
                Translation = "test1", IsDeleted = false, CreatedBy = "admin", CreatedDate = DateTime.UtcNow },
            new LanguageTranslation{ Id = 2, LanguageId = 2, MenuId = 2, VariableId = 2, Description = "test2",
                Translation = "test2", IsDeleted = false, CreatedBy = "admin", CreatedDate = DateTime.UtcNow },
            new LanguageTranslation{ Id = 3, LanguageId = 3, MenuId = 3, VariableId = 3, Description = "test3",
                Translation = "test3", IsDeleted = false, CreatedBy = "admin", CreatedDate = DateTime.UtcNow }

        };

        /// <summary>
        /// Gets the translation languages.
        /// </summary>
        /// <param name="languageTranslationQueryParameters">The query parameters.</param>
        /// <returns></returns>
        public CSSResponse GetLanguageTranslations(LanguageTranslationQueryParameters languageTranslationQueryParameters)
        {
            var translationLanguages = languageTranslationsDB.Skip((languageTranslationQueryParameters.PageNumber - 1) * languageTranslationQueryParameters.PageSize).Take(languageTranslationQueryParameters.PageSize);
            return new CSSResponse(translationLanguages, HttpStatusCode.OK);
        }

        /// <summary>
        /// Gets the language translation.
        /// </summary>
        /// <param name="languageTranslationIdDetails">The translation details.</param>
        /// <returns></returns>
        public CSSResponse GetLanguageTranslation(LanguageTranslationIdDetails languageTranslationIdDetails)
        {
            var translationLanguage = languageTranslationsDB.Where(x => x.Id == languageTranslationIdDetails.TranslationId && x.IsDeleted == false).FirstOrDefault();
            return translationLanguage != null ? new CSSResponse(translationLanguage, HttpStatusCode.OK) : new CSSResponse(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Creates the language translation.
        /// </summary>
        /// <param name="languageTranslation">The language translation.</param>
        /// <returns></returns>
        public CSSResponse CreateLanguageTranslation(CreateLanguageTranslation languageTranslation)
        {
            if (languageTranslationsDB.Exists(x => x.IsDeleted == false && x.MenuId == languageTranslation.MenuId &&
                x.VariableId == languageTranslation.VariableId && x.LanguageId == languageTranslation.LanguageId))
            {
                return new CSSResponse($"Translation with language id '{languageTranslation.LanguageId}' and " +
                    $"menu id '{languageTranslation.MenuId}' and variable id '{languageTranslation.VariableId}' already exists.", HttpStatusCode.Conflict);
            }

            LanguageTranslation language = new LanguageTranslation()
            {
                Id = 99,
                LanguageId = languageTranslation.LanguageId,
                MenuId = languageTranslation.MenuId,
                VariableId = languageTranslation.VariableId,
                Description = languageTranslation.Description,
                Translation = languageTranslation.Translation,
                CreatedBy = languageTranslation.CreatedBy
            };

            languageTranslationsDB.Add(language);

            return new CSSResponse(new LanguageTranslationIdDetails { TranslationId = language.Id }, HttpStatusCode.Created);
        }

        /// <summary>
        /// Updates the scheduling code.
        /// </summary>
        /// <param name="schedulingCodeIdDetails">The scheduling code identifier details.</param>
        /// <param name="updateSchedulingCode">The update scheduling code.</param>
        /// <returns></returns>
        public CSSResponse UpdateLanguageTranslatione(LanguageTranslationIdDetails languageTranslationIdDetails, UpdateLanguageTranslation languageTranslation)
        {
            if (!languageTranslationsDB.Exists(x => x.IsDeleted == false && x.Id == languageTranslationIdDetails.TranslationId))
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            if (languageTranslationsDB.Exists(x => x.IsDeleted == false && x.MenuId == languageTranslation.MenuId &&
                x.VariableId == languageTranslation.VariableId && x.LanguageId == languageTranslation.LanguageId && x.Id != languageTranslationIdDetails.TranslationId))
            {
                return new CSSResponse($"Translation with language id '{languageTranslation.LanguageId}' and " +
                    $"menu id '{languageTranslation.MenuId}' and variable id '{languageTranslation.VariableId}' already exists.", HttpStatusCode.Conflict);
            }

            var language = languageTranslationsDB.Where(x => x.IsDeleted == false && x.Id == languageTranslationIdDetails.TranslationId).FirstOrDefault();

            language.LanguageId = languageTranslation.LanguageId;
            language.MenuId = languageTranslation.MenuId;
            language.VariableId = languageTranslation.VariableId;
            language.Description = languageTranslation.Description;
            language.Translation = languageTranslation.Translation;
            language.ModifiedDate = DateTime.UtcNow;

            return new CSSResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Deletes the language translation.
        /// </summary>
        /// <param name="languageTranslationIdDetails">The translation identifier details.</param>
        /// <returns></returns>
        public CSSResponse DeleteLanguageTranslation(LanguageTranslationIdDetails languageTranslationIdDetails)
        {
            if (!languageTranslationsDB.Exists(x => x.IsDeleted == false && x.Id == languageTranslationIdDetails.TranslationId))
            {
                return new CSSResponse(HttpStatusCode.NotFound);
            }

            var language = languageTranslationsDB.Where(x => x.IsDeleted == false && x.Id == languageTranslationIdDetails.TranslationId).FirstOrDefault();
            languageTranslationsDB.Remove(language);
            return new CSSResponse(HttpStatusCode.NoContent);
        }
    }
}
