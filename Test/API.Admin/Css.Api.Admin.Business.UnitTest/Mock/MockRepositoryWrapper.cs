using AutoMapper;
using Css.Api.Admin.Business.UnitTest.Mock;
using Css.Api.Admin.Models.Domain;
using Css.Api.Admin.Repository.DatabaseContext;
using Css.Api.Admin.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq;
using System.Threading.Tasks;

namespace Css.Api.Admin.Repository
{
    public class MockRepositoryWrapper : IRepositoryWrapper
    {
        /// <summary>
        /// Gets or sets the repository context.
        /// </summary>
        private Mock<AdminContext> _repositoryContext { get; set; }

        /// <summary>
        /// Gets or sets the mapper.
        /// </summary>
        private IMapper _mapper { get; set; }

        /// <summary>
        /// Gets or sets the scheduling codes repository.
        /// </summary>
        private ISchedulingCodeRepository _schedulingCodesRepository { get; set; }

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
        /// Gets or sets the agent category repository.
        /// </summary>
        private IAgentCategoryRepository _agentCategoryRepository { get; set; }

        /// <summary>
        /// Gets or sets the CSS language repository.
        /// </summary>
        public ICssLanguageRepository _cssLanguageRepository { get; set; }

        /// <summary>
        /// Gets or sets the CSS menu repository.
        /// </summary>
        public ICssMenuRepository _cssMenuRepository { get; set; }

        /// <summary>
        /// Gets or sets the CSS variable repository.
        /// </summary>
        public ICssVariableRepository _cssVariableRepository { get; set; }

        /// <summary>
        /// Gets or sets the language translation repository.
        /// </summary>
        public ILanguageTranslationRepository _languageTranslationRepository { get; set; }

        /// <summary>
        /// Gets the scheduling codes.
        /// </summary>
        public ISchedulingCodeRepository SchedulingCodes
        {
            get
            {
                if (_schedulingCodesRepository == null)
                {
                    var mockSchedulingCode = new Mock<DbSet<SchedulingCode>>();
                    mockSchedulingCode.As<IQueryable<SchedulingCode>>().Setup(m => m.Provider).Returns(new MockDataContext().schedulingCodesDB.Provider);
                    mockSchedulingCode.As<IQueryable<SchedulingCode>>().Setup(m => m.Expression).Returns(new MockDataContext().schedulingCodesDB.Expression);
                    mockSchedulingCode.As<IQueryable<SchedulingCode>>().Setup(m => m.ElementType).Returns(new MockDataContext().schedulingCodesDB.ElementType);
                    mockSchedulingCode.As<IQueryable<SchedulingCode>>().Setup(m => m.GetEnumerator()).Returns(new MockDataContext().schedulingCodesDB.GetEnumerator());

                    _repositoryContext.Setup(x => x.Set<SchedulingCode>()).Returns(mockSchedulingCode.Object);

                    _schedulingCodesRepository = new SchedulingCodeRepository(_repositoryContext.Object, _mapper);
                }
                return _schedulingCodesRepository;
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
                    var mockSchedulingCodeIcon = new Mock<DbSet<SchedulingCodeIcon>>();
                    mockSchedulingCodeIcon.As<IQueryable<SchedulingCodeIcon>>().Setup(m => m.Provider).Returns(new MockDataContext().schedulingCodeIconsDB.Provider);
                    mockSchedulingCodeIcon.As<IQueryable<SchedulingCodeIcon>>().Setup(m => m.Expression).Returns(new MockDataContext().schedulingCodeIconsDB.Expression);
                    mockSchedulingCodeIcon.As<IQueryable<SchedulingCodeIcon>>().Setup(m => m.ElementType).Returns(new MockDataContext().schedulingCodeIconsDB.ElementType);
                    mockSchedulingCodeIcon.As<IQueryable<SchedulingCodeIcon>>().Setup(m => m.GetEnumerator()).Returns(new MockDataContext().schedulingCodeIconsDB.GetEnumerator());

                    _repositoryContext.Setup(x => x.Set<SchedulingCodeIcon>()).Returns(mockSchedulingCodeIcon.Object);

                    _schedulingCodeIconsRepository = new SchedulingCodeIconRepository(_repositoryContext.Object, _mapper);
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
                    var mockSchedulingCodeTypes = new Mock<DbSet<SchedulingCodeType>>();
                    mockSchedulingCodeTypes.As<IQueryable<SchedulingCodeType>>().Setup(m => m.Provider).Returns(new MockDataContext().schedulingCodeTypesDB.Provider);
                    mockSchedulingCodeTypes.As<IQueryable<SchedulingCodeType>>().Setup(m => m.Expression).Returns(new MockDataContext().schedulingCodeTypesDB.Expression);
                    mockSchedulingCodeTypes.As<IQueryable<SchedulingCodeType>>().Setup(m => m.ElementType).Returns(new MockDataContext().schedulingCodeTypesDB.ElementType);
                    mockSchedulingCodeTypes.As<IQueryable<SchedulingCodeType>>().Setup(m => m.GetEnumerator()).Returns(new MockDataContext().schedulingCodeTypesDB.GetEnumerator());

                    _repositoryContext.Setup(x => x.Set<SchedulingCodeType>()).Returns(mockSchedulingCodeTypes.Object);

                    _schedulingCodeTypesRepository = new SchedulingCodeTypeRepository(_repositoryContext.Object, _mapper);
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
                    var mockSchedulingTypeCode = new Mock<DbSet<SchedulingTypeCode>>();
                    mockSchedulingTypeCode.As<IQueryable<SchedulingTypeCode>>().Setup(m => m.Provider).Returns(new MockDataContext().schedulingTypeCodes.Provider);
                    mockSchedulingTypeCode.As<IQueryable<SchedulingTypeCode>>().Setup(m => m.Expression).Returns(new MockDataContext().schedulingTypeCodes.Expression);
                    mockSchedulingTypeCode.As<IQueryable<SchedulingTypeCode>>().Setup(m => m.ElementType).Returns(new MockDataContext().schedulingTypeCodes.ElementType);
                    mockSchedulingTypeCode.As<IQueryable<SchedulingTypeCode>>().Setup(m => m.GetEnumerator()).Returns(new MockDataContext().schedulingTypeCodes.GetEnumerator());

                    _repositoryContext.Setup(x => x.Set<SchedulingTypeCode>()).Returns(mockSchedulingTypeCode.Object);

                    _schedulingTypeCodesRepository = new SchedulingTypeCodeRepository(_repositoryContext.Object);
                }
                return _schedulingTypeCodesRepository;
            }
        }

        /// <summary>
        /// Gets the agent categories.
        /// </summary>
        public IAgentCategoryRepository AgentCategories
        {
            get
            {
                if (_agentCategoryRepository == null)
                {
                    var mockAgentCategory = new Mock<DbSet<AgentCategory>>();
                    mockAgentCategory.As<IQueryable<AgentCategory>>().Setup(m => m.Provider).Returns(new MockDataContext().agentCategorysDB.Provider);
                    mockAgentCategory.As<IQueryable<AgentCategory>>().Setup(m => m.Expression).Returns(new MockDataContext().agentCategorysDB.Expression);
                    mockAgentCategory.As<IQueryable<AgentCategory>>().Setup(m => m.ElementType).Returns(new MockDataContext().agentCategorysDB.ElementType);
                    mockAgentCategory.As<IQueryable<AgentCategory>>().Setup(m => m.GetEnumerator()).Returns(new MockDataContext().agentCategorysDB.GetEnumerator());

                    _repositoryContext.Setup(x => x.Set<AgentCategory>()).Returns(mockAgentCategory.Object);

                    _agentCategoryRepository = new AgentCategoryRepository(_repositoryContext.Object, _mapper);
                }
                return _agentCategoryRepository;
            }
        }

        /// <summary>
        /// Gets the language.
        /// </summary>
        public ICssLanguageRepository CssLanguage
        {
            get
            {
                if (_cssLanguageRepository == null)
                {
                    var mockCssLanguage = new Mock<DbSet<CssLanguage>>();
                    mockCssLanguage.As<IQueryable<CssLanguage>>().Setup(m => m.Provider).Returns(new MockDataContext().cssLanguagesDB.Provider);
                    mockCssLanguage.As<IQueryable<CssLanguage>>().Setup(m => m.Expression).Returns(new MockDataContext().cssLanguagesDB.Expression);
                    mockCssLanguage.As<IQueryable<CssLanguage>>().Setup(m => m.ElementType).Returns(new MockDataContext().cssLanguagesDB.ElementType);
                    mockCssLanguage.As<IQueryable<CssLanguage>>().Setup(m => m.GetEnumerator()).Returns(new MockDataContext().cssLanguagesDB.GetEnumerator());

                    _repositoryContext.Setup(x => x.Set<CssLanguage>()).Returns(mockCssLanguage.Object);

                    _cssLanguageRepository = new CssLanguageRepository(_repositoryContext.Object, _mapper);
                }
                return _cssLanguageRepository;
            }
        }

        /// <summary>
        /// Gets the CSS menu.
        /// </summary>
        public ICssMenuRepository CssMenu
        {
            get
            {
                if (_cssMenuRepository == null)
                {
                    var mockCssMenu = new Mock<DbSet<CssMenu>>();
                    mockCssMenu.As<IQueryable<CssMenu>>().Setup(m => m.Provider).Returns(new MockDataContext().cssMenusDB.Provider);
                    mockCssMenu.As<IQueryable<CssMenu>>().Setup(m => m.Expression).Returns(new MockDataContext().cssMenusDB.Expression);
                    mockCssMenu.As<IQueryable<CssMenu>>().Setup(m => m.ElementType).Returns(new MockDataContext().cssMenusDB.ElementType);
                    mockCssMenu.As<IQueryable<CssMenu>>().Setup(m => m.GetEnumerator()).Returns(new MockDataContext().cssMenusDB.GetEnumerator());

                    _repositoryContext.Setup(x => x.Set<CssMenu>()).Returns(mockCssMenu.Object);

                    _cssMenuRepository = new CssMenuRepository(_repositoryContext.Object, _mapper);
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
                    var mockCssVariable = new Mock<DbSet<CssVariable>>();
                    mockCssVariable.As<IQueryable<CssVariable>>().Setup(m => m.Provider).Returns(new MockDataContext().cssVariablesDB.Provider);
                    mockCssVariable.As<IQueryable<CssVariable>>().Setup(m => m.Expression).Returns(new MockDataContext().cssVariablesDB.Expression);
                    mockCssVariable.As<IQueryable<CssVariable>>().Setup(m => m.ElementType).Returns(new MockDataContext().cssVariablesDB.ElementType);
                    mockCssVariable.As<IQueryable<CssVariable>>().Setup(m => m.GetEnumerator()).Returns(new MockDataContext().cssVariablesDB.GetEnumerator());

                    _repositoryContext.Setup(x => x.Set<CssVariable>()).Returns(mockCssVariable.Object);

                    _cssVariableRepository = new CssVariableRepository(_repositoryContext.Object, _mapper);
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
                    var mockLanguageTranslation = new Mock<DbSet<LanguageTranslation>>();
                    mockLanguageTranslation.As<IQueryable<LanguageTranslation>>().Setup(m => m.Provider).Returns(new MockDataContext().languageTranslationsDB.Provider);
                    mockLanguageTranslation.As<IQueryable<LanguageTranslation>>().Setup(m => m.Expression).Returns(new MockDataContext().languageTranslationsDB.Expression);
                    mockLanguageTranslation.As<IQueryable<LanguageTranslation>>().Setup(m => m.ElementType).Returns(new MockDataContext().languageTranslationsDB.ElementType);
                    mockLanguageTranslation.As<IQueryable<LanguageTranslation>>().Setup(m => m.GetEnumerator()).Returns(new MockDataContext().languageTranslationsDB.GetEnumerator());

                    _repositoryContext.Setup(x => x.Set<LanguageTranslation>()).Returns(mockLanguageTranslation.Object);

                    _languageTranslationRepository = new LanguageTranslationRepository(_repositoryContext.Object, _mapper);
                }
                return _languageTranslationRepository;
            }
        }

        public IRoleRepository Roles => throw new System.NotImplementedException();

        public IUserPermissionRepository UserPermissions => throw new System.NotImplementedException();

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryWrapper" /> class.
        /// </summary>
        /// <param name="repositoryContext">The repository context.</param>
        /// <param name="mapper">The mapper.</param>
        public MockRepositoryWrapper(
            Mock<AdminContext> repositoryContext,
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
            return await _repositoryContext.Object.SaveChangesAsync() >= 0;
        }
    }
}
