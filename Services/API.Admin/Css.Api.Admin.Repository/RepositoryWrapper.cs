using AutoMapper;
using Css.Api.Admin.Repository.DatabaseContext;
using Css.Api.Admin.Repository.Interfaces;
using System.Threading.Tasks;

namespace Css.Api.Admin.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        /// <summary>
        /// Gets or sets the repository context.
        /// </summary>
        private AdminContext _repositoryContext { get; set; }

        /// <summary>
        /// Gets or sets the mapper.
        /// </summary>
        private IMapper _mapper { get; set; }

        /// <summary>
        /// Gets or sets the scheduling codes repository.
        /// </summary>
        private ISchedulingCodeRepository _schedulingCodesRepository { get; set; }

        /// <summary>
        /// Gets or sets the agent category repository.
        /// </summary>
        private IAgentCategoryRepository _agentCategoryRepository { get; set; }

        /// <summary>
        /// Gets or sets the scheduling code icons repository.
        /// </summary>
        private ISchedulingCodeIconRepository _schedulingCodeIconsRepository { get; set; }

        /// <summary>
        /// Gets or sets the scheduling code types repository.
        /// </summary>
        private ISchedulingCodeTypeRepository _schedulingCodeTypesRepository { get; set; }

        /// <summary>
        /// Gets or sets the scheduling type codes repository.
        /// </summary>
        private ISchedulingTypeCodeRepository _schedulingTypeCodesRepository { get; set; }

        /// <summary>
        /// Gets or sets the language repository.
        /// </summary>
        private ICssLanguageRepository _languageRepository { get; set; }

        /// <summary>
        /// Gets or sets the CSS menu repository.
        /// </summary>
        private ICssMenuRepository _cssMenuRepository { get; set; }

        /// <summary>
        /// Gets or sets the CSS variable repository.
        /// </summary>
        private ICssVariableRepository _cssVariableRepository { get; set; }

        /// <summary>
        /// Gets or sets the language translation repository.
        /// </summary>
        private ILanguageTranslationRepository _languageTranslationRepository { get; set; }

        /// <summary>
        /// Gets the scheduling codes.
        /// </summary>
        public ISchedulingCodeRepository SchedulingCodes
        {
            get
            {
                if (_schedulingCodesRepository == null)
                {
                    _schedulingCodesRepository = new SchedulingCodeRepository(_repositoryContext, _mapper);
                }
                return _schedulingCodesRepository;
            }
        }

        /// <summary>Gets the agent categories.</summary>
        /// <value>The agent categories.</value>
        public IAgentCategoryRepository AgentCategories
        {
            get
            {
                if (_agentCategoryRepository == null)
                {
                    _agentCategoryRepository = new AgentCategoryRepository(_repositoryContext, _mapper);
                }
                return _agentCategoryRepository;
            }
        }

        /// <summary>
        /// Gets the scheduling code icons.
        /// </summary>
        public ISchedulingCodeIconRepository SchedulingCodeIcons
        {
            get
            {
                if (_schedulingCodeIconsRepository == null)
                {
                    _schedulingCodeIconsRepository = new SchedulingCodeIconRepository(_repositoryContext, _mapper);
                }
                return _schedulingCodeIconsRepository;
            }
        }

        /// <summary>
        /// Gets the scheduling code types.
        /// </summary>
        public ISchedulingCodeTypeRepository SchedulingCodeTypes
        {
            get
            {
                if (_schedulingCodeTypesRepository == null)
                {
                    _schedulingCodeTypesRepository = new SchedulingCodeTypeRepository(_repositoryContext, _mapper);
                }
                return _schedulingCodeTypesRepository;
            }
        }

        /// <summary>
        /// Gets the scheduling type codes.
        /// </summary>
        public ISchedulingTypeCodeRepository SchedulingTypeCodes
        {
            get
            {
                if (_schedulingTypeCodesRepository == null)
                {
                    _schedulingTypeCodesRepository = new SchedulingTypeCodeRepository(_repositoryContext);
                }
                return _schedulingTypeCodesRepository;
            }
        }

        /// <summary>
        /// Gets the language.
        /// </summary>
        public ICssLanguageRepository CssLanguage
        {
            get
            {
                if (_languageRepository == null)
                {
                    _languageRepository = new CssLanguageRepository(_repositoryContext,_mapper);
                }
                return _languageRepository;
            }
        }

        /// <summary>
        /// Gets the language.
        /// </summary>
        public ICssMenuRepository CssMenu
        {
            get
            {
                if (_cssMenuRepository == null)
                {
                    _cssMenuRepository = new CssMenuRepository(_repositoryContext, _mapper);
                }
                return _cssMenuRepository;
            }
        }

        /// <summary>
        /// Gets the CSS variable.
        /// </summary>
        public ICssVariableRepository CssVariable
        {
            get
            {
                if (_cssVariableRepository == null)
                {
                    _cssVariableRepository = new CssVariableRepository(_repositoryContext, _mapper);
                }
                return _cssVariableRepository;
            }
        }

        /// <summary>
        /// Gets the language translation.
        /// </summary>
        public ILanguageTranslationRepository LanguageTranslation
        {
            get
            {
                if (_languageTranslationRepository == null)
                {
                    _languageTranslationRepository = new LanguageTranslationRepository(_repositoryContext, _mapper);
                }
                return _languageTranslationRepository;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryWrapper" /> class.
        /// </summary>
        /// <param name="repositoryContext">The repository context.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="">The .</param>
        public RepositoryWrapper(
            AdminContext repositoryContext,
            IMapper mapper)
        {
            _repositoryContext = repositoryContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Saves the asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SaveAsync()
        {
            return await _repositoryContext.SaveChangesAsync() >= 0;
        }
    }
}
