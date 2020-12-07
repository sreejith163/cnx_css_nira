using Css.Api.Admin.Business.Interfaces;
using Css.Api.Admin.Repository.Interfaces;
using Css.Api.Core.Models.DTO.Response;
using System.Net;
using System.Threading.Tasks;

namespace Css.Api.Admin.Business
{
    public class CssLanguageService : ICssLanguageService
    {
        /// <summary>
        /// The repository
        /// </summary>
        private readonly IRepositoryWrapper _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CssLanguageService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public CssLanguageService(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets the languages.
        /// </summary>
        /// <returns></returns>
        public async Task<CSSResponse> GetCssLanguages()
        {
            var menus = await _repository.CssLanguage.GetCssLanguages();
            return new CSSResponse(menus, HttpStatusCode.OK);
        }
    }
}
