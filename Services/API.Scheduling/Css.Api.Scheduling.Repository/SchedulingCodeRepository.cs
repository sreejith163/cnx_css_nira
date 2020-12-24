using Css.Api.Core.DataAccess.Repository.NoSQL;
using Css.Api.Core.DataAccess.Repository.NoSQL.Interfaces;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Utilities.Extensions;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.SchedulingCode;
using Css.Api.Scheduling.Repository.Interfaces;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Repository
{
    public class SchedulingCodeRepository : GenericRepository<SchedulingCode>, ISchedulingCodeRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulingCodeRepository" /> class.
        /// </summary>
        /// <param name="mongoContext">The mongo context.</param>
        public SchedulingCodeRepository(IMongoContext mongoContext) : base(mongoContext)
        {
        }

        /// <summary>
        /// Gets the schedulingCodes.
        /// </summary>
        /// <param name="schedulingCodeQueryparameter">The schedulingCode queryparameter.</param>
        /// <returns></returns>
        public async Task<PagedList<Entity>> GetSchedulingCodes(SchedulingCodeQueryparameter schedulingCodeQueryparameter)
        {
            var schedulingCodes = FilterBy(x => true);

            var filteredSchedulingCodes = FilterSchedulingCodes(schedulingCodes, schedulingCodeQueryparameter);

            var sortedSchedulingCodes = SortHelper.ApplySort(filteredSchedulingCodes, schedulingCodeQueryparameter.OrderBy);

            var pagedSchedulingCodes = sortedSchedulingCodes
                .Skip((schedulingCodeQueryparameter.PageNumber - 1) * schedulingCodeQueryparameter.PageSize)
                .Take(schedulingCodeQueryparameter.PageSize);

            var shapedSchedulingCodes = DataShaper.ShapeData(pagedSchedulingCodes, schedulingCodeQueryparameter.Fields);

            return await PagedList<Entity>
                .ToPagedList(shapedSchedulingCodes, filteredSchedulingCodes.Count(), schedulingCodeQueryparameter.PageNumber, schedulingCodeQueryparameter.PageSize);
        }

        /// <summary>
        /// Gets the schedulingCode.
        /// </summary>
        /// <param name="schedulingCodeIdDetails">The schedulingCode identifier details.</param>
        /// <returns></returns>
        public async Task<SchedulingCode> GetSchedulingCode(SchedulingCodeIdDetails schedulingCodeIdDetails)
        {
            var query = Builders<SchedulingCode>.Filter.Eq(i => i.IsDeleted, false) &
                        Builders<SchedulingCode>.Filter.Eq(i => i.SchedulingCodeId, schedulingCodeIdDetails.SchedulingCodeId);

            return await FindByIdAsync(query);
        }

        /// <summary>
        /// Gets the scheduling codes by ids.
        /// </summary>
        /// <param name="codes">The codes.</param>
        /// <returns></returns>
        public async Task<long> GetSchedulingCodesCountByIds(List<int> codes)
        {
            var query = Builders<SchedulingCode>.Filter.Eq(i => i.IsDeleted, false) & 
                        Builders<SchedulingCode>.Filter.In(i => i.SchedulingCodeId, codes);

            return await FindCountByIdAsync(query);
        }

        /// <summary>
        /// Gets the schedulingCodes count.
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetSchedulingCodesCount()
        {
            var count = FilterBy(x => true)
                .Count();

            return await Task.FromResult(count);
        }

        /// <summary>
        /// Creates the schedulingCode.
        /// </summary>
        /// <param name="schedulingCodeRequest">The schedulingCode request.</param>
        public void CreateSchedulingCode(SchedulingCode schedulingCodeRequest)
        {
            InsertOneAsync(schedulingCodeRequest);
        }

        /// <summary>
        /// Updates the schedulingCode.
        /// </summary>
        /// <param name="schedulingCodeRequest">The schedulingCode request.</param>
        public void UpdateSchedulingCode(SchedulingCode schedulingCodeRequest)
        {
            ReplaceOneAsync(schedulingCodeRequest);
        }

        /// <summary>
        /// Filters the schedulingCodes.
        /// </summary>
        /// <param name="schedulingCodes">The schedulingCodes.</param>
        /// <param name="schedulingCodeQueryparameter">The schedulingCode queryparameter.</param>
        /// <returns></returns>
        private IQueryable<SchedulingCode> FilterSchedulingCodes(IQueryable<SchedulingCode> schedulingCodes, SchedulingCodeQueryparameter schedulingCodeQueryparameter)
        {
            if (!schedulingCodes.Any())
            {
                return schedulingCodes;
            }

            if (!string.IsNullOrWhiteSpace(schedulingCodeQueryparameter.SearchKeyword))
            {
                schedulingCodes = schedulingCodes.Where(o => o.Name.ToLower().Contains(schedulingCodeQueryparameter.SearchKeyword.Trim().ToLower()));
            }

            return schedulingCodes;
        }
    }
}


