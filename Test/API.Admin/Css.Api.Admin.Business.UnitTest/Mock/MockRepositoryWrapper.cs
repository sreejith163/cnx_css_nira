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
    public class MockRepositoryWrapper: IRepositoryWrapper
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
        /// <value>
        /// The agent category repository.
        /// </value>
        private IAgentCategoryRepository _agentCategoryRepository { get; set; }

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

        /// <summary>Gets the agent categories.</summary>
        /// <value>The agent categories.</value>
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
