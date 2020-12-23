using AutoMapper;
using AutoMapper.QueryableExtensions;
using Css.Api.Core.DataAccess.Repository.NoSQL;
using Css.Api.Core.DataAccess.Repository.NoSQL.Interfaces;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Utilities.Extensions;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.ActivityLog;
using Css.Api.Scheduling.Models.DTO.Response.ActivityLog;
using Css.Api.Scheduling.Repository.Interfaces;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;


namespace Css.Api.Scheduling.Repository
{
    public class ActivityLogRepository : GenericRepository<ActivityLog>, IActivityLogRepository
    {
        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>Initializes a new instance of the <see cref="ActivityLogRepository" /> class.</summary>
        /// <param name="mongoContext">The mongo context.</param>
        /// <param name="mapper">The mapper.</param>
        public ActivityLogRepository(IMongoContext mongoContext,
            IMapper mapper) : base(mongoContext)
        {
            _mapper = mapper;
        }

        /// <summary>Gets the activity logs.</summary>
        /// <param name="activityLogQueryParameter">The activity log query parameter.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<PagedList<Entity>> GetActivityLogs(ActivityLogQueryParameter activityLogQueryParameter)
        {
            var activityLogs = FilterBy(x => true);

            var filteredActivityLogs = FilterActivityLogs(activityLogs, activityLogQueryParameter);

            var sortedActivityLogs = SortHelper.ApplySort(filteredActivityLogs, activityLogQueryParameter.OrderBy);

            var pagedActivityLogs = sortedActivityLogs
                .Skip((activityLogQueryParameter.PageNumber - 1) * activityLogQueryParameter.PageSize)
                .Take(activityLogQueryParameter.PageSize);

            var mappedActivityLogs = pagedActivityLogs
                .ProjectTo<ActivityLogDTO>(_mapper.ConfigurationProvider);

            var shapedActivityLogs = DataShaper.ShapeData(mappedActivityLogs, activityLogQueryParameter.Fields);

            return await PagedList<Entity>
                .ToPagedList(shapedActivityLogs, filteredActivityLogs.Count(), activityLogQueryParameter.PageNumber, activityLogQueryParameter.PageSize);
        }


        /// <summary>Gets the activity logs count.</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<int> GetActivityLogsCount()
        {
            var count = FilterBy(x => true)
                .Count();

            return await Task.FromResult(count);
        }

        /// <summary>Creates the activity logs.</summary>
        /// <param name="activityLogRequest">The activity log request.</param>
        public void CreateActivityLogs(ActivityLog activityLogRequest)
        {
            InsertOneAsync(activityLogRequest);
        }

        /// <summary>Filters the activity logs.</summary>
        /// <param name="activityLogs">The activity logs.</param>
        /// <param name="activityLogQueryParameter">The activity log query parameter.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        private IQueryable<ActivityLog> FilterActivityLogs(IQueryable<ActivityLog> activityLogs, ActivityLogQueryParameter activityLogQueryParameter)
        {
            if (!activityLogs.Any())
            {
                return activityLogs;
            }

            string searchKeyword = activityLogQueryParameter.SearchKeyword;

            if (!string.IsNullOrWhiteSpace(searchKeyword))
            {
                activityLogs = activityLogs.Where(o => o.ExecutedBy.ToLower().Contains(searchKeyword.Trim().ToLower()) ||
                                                           o.FieldDetails.Any(
                                                               field => field.Name.ToLower().Contains(searchKeyword.Trim().ToLower()) ||
                                                               field.NewValue.ToLower().Contains(searchKeyword.Trim().ToLower()) ||
                                                               field.OldValue.ToLower().Contains(searchKeyword.Trim().ToLower())
                                                               ));
            }

            if (activityLogQueryParameter.ActivityType != null)
            {
                activityLogs = activityLogs.Where(o => o.ActivityType == activityLogQueryParameter.ActivityType);
            }

            return activityLogs;
        }
    }
}