using AutoMapper;
using AutoMapper.QueryableExtensions;
using Css.Api.Admin.Models.Domain;
using Css.Api.Admin.Repository.DatabaseContext;
using Css.Api.Admin.Repository.Interfaces;
using Css.Api.Core.DataAccess.Repository.SQL;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Models.DTO.Response;
using Css.Api.Core.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Css.Api.Admin.Repository
{
    public class CssLanguageRepository : GenericRepository<CssLanguage>, ICssLanguageRepository
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
        public CssLanguageRepository(
            AdminContext repositoryContext,
            IMapper mapper)
            : base(repositoryContext)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the languages.
        /// </summary>
        /// <param name="languageQueryParameters">The language query parameters.</param>
        /// <returns></returns>
        public async Task<List<KeyValue>> GetCssLanguages()
        {
            var languages = FindAll().ProjectTo<KeyValue>(_mapper.ConfigurationProvider).ToList();
            return await Task.FromResult(languages);
        }
    }
}
