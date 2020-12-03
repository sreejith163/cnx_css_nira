using Css.Api.Admin.Models.Domain;
using Css.Api.Core.Models.DTO.Response;
using System.Collections.Generic;
using System.Net;

namespace Css.Api.Admin.UnitTest.Mock
{
    public class MockCssLanguageData
    {
        /// <summary>
        /// The CSS languages database
        /// </summary>
        public readonly List<CssLanguage> cssLanguagesDB = new List<CssLanguage>()
        {
            new CssLanguage{ Id = 1, Name = "lang1", Description = "lang1"},
            new CssLanguage{ Id = 2, Name = "lang2", Description = "lang2"},
            new CssLanguage{ Id = 3, Name = "lang3", Description = "lang3"}
        };

        /// <summary>
        /// Gets the languages.
        /// </summary>
        /// <returns></returns>
        public CSSResponse GetLanguages()
        {
            return new CSSResponse(cssLanguagesDB, HttpStatusCode.OK);
        }
    }
}
