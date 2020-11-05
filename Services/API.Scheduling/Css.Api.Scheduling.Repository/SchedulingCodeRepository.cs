using Css.Api.Core.Models.Domain;
using Css.Api.Core.Utilities.Interfaces;
using Css.Api.Core.DataAccess.Repository;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Repository.DatabaseContext;
using Css.Api.Scheduling.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Css.Api.Scheduling.Models.DTO.Request.SchedulingCode;
using AutoMapper;
using Css.Api.Scheduling.Models.DTO.Response.SchedulingCode;
using AutoMapper.QueryableExtensions;

namespace Css.Api.Scheduling.Repository
{
    public class SchedulingCodeRepository : GenericRepository<SchedulingCode>, ISchedulingCodeRepository
    {
        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// The sort helper
        /// </summary>
        private readonly ISortHelper<SchedulingCodeDTO> _sortHelper;

        /// <summary>
        /// The data shaper
        /// </summary>
        private readonly IDataShaper<SchedulingCodeDTO> _dataShaper;

        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulingCodeRepository" /> class.
        /// </summary>
        /// <param name="repositoryContext">The repository context.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="sortHelper">The sort helper.</param>
        /// <param name="dataShaper">The data shaper.</param>
        public SchedulingCodeRepository(
            SchedulingContext repositoryContext,
            IMapper mapper,
            ISortHelper<SchedulingCodeDTO> sortHelper,
            IDataShaper<SchedulingCodeDTO> dataShaper)
            : base(repositoryContext)
        {
            _mapper = mapper;
            _sortHelper = sortHelper;
            _dataShaper = dataShaper;
        }

        /// <summary>
        /// Gets the scheduling codes.
        /// </summary>
        /// <param name="schedulingCodeParameters">The scheduling code parameters.</param>
        /// <returns></returns>
        public async Task<PagedList<Entity>> GetSchedulingCodes(SchedulingCodeQueryParameters schedulingCodeParameters)
        {
            var schedulingCodes = FindByCondition(x => x.IsDeleted == false);

            var filteredSchedulingCodes = FilterSchedulingCodes(schedulingCodes, schedulingCodeParameters.SearchKeyword);

            var pagedSchedulingCodes = filteredSchedulingCodes
                .Skip((schedulingCodeParameters.PageNumber - 1) * schedulingCodeParameters.PageSize)
                .Take(schedulingCodeParameters.PageSize)
                .Include(x => x.Icon).Include(x => x.SchedulingTypeCode)
                .ThenInclude(x => x.SchedulingCodeType)
                .ToList();

            var mappedSchedulingCodes = pagedSchedulingCodes
                .AsQueryable()
                .ProjectTo<SchedulingCodeDTO>(_mapper.ConfigurationProvider);

            var sortedSchedulingCodes = _sortHelper.ApplySort(mappedSchedulingCodes, schedulingCodeParameters.OrderBy);
            var shapedSchedulingCodes = _dataShaper.ShapeData(sortedSchedulingCodes, schedulingCodeParameters.Fields);

            return await PagedList<Entity>
                .ToPagedList(shapedSchedulingCodes, schedulingCodes.Count(), schedulingCodeParameters.PageNumber, schedulingCodeParameters.PageSize);
        }

        /// <summary>
        /// Gets the scheduling code.
        /// </summary>
        /// <param name="schedulingCodeIdDetails">The scheduling code identifier details.</param>
        /// <returns></returns>
        public async Task<SchedulingCode> GetSchedulingCode(SchedulingCodeIdDetails schedulingCodeIdDetails)
        {
            var schedulingCode = FindByCondition(x => x.Id == schedulingCodeIdDetails.SchedulingCodeId && x.IsDeleted == false)
                .Include(x => x.Icon)
                .Include(x => x.SchedulingTypeCode)
                .ThenInclude(x => x.SchedulingCodeType)
                .SingleOrDefault();

            return await Task.FromResult(schedulingCode);
        }

        /// <summary>
        /// Creates the scheduling code.
        /// </summary>
        /// <param name="schedulingCode">The scheduling code.</param>
        public void CreateSchedulingCode(SchedulingCode schedulingCode)
        {
            Create(schedulingCode);
        }

        /// <summary>
        /// Updates the scheduling code.
        /// </summary>
        /// <param name="schedulingCode">The scheduling code.</param>
        public void UpdateSchedulingCode(SchedulingCode schedulingCode)
        {
            Update(schedulingCode);
        }

        /// <summary>
        /// Deletes the scheduling code.
        /// </summary>
        /// <param name="schedulingCode">The scheduling code.</param>
        public void DeleteSchedulingCode(SchedulingCode schedulingCode)
        {
            Delete(schedulingCode);
        }

        /// <summary>
        /// Searches the name of the by.
        /// </summary>
        /// <param name="schedulingCodes">The scheduling codes.</param>
        /// <param name="schedulingCodeName">Name of the scheduling code.</param>
        /// <returns></returns>
        private IQueryable<SchedulingCode> FilterSchedulingCodes(IQueryable<SchedulingCode> schedulingCodes, string schedulingCodeName)
        {
            if (!schedulingCodes.Any() || string.IsNullOrWhiteSpace(schedulingCodeName))
            {
                return schedulingCodes;
            }

            return schedulingCodes.Where(o => o.Description.ToLower().Contains(schedulingCodeName.Trim().ToLower()));
        }
    }
}
