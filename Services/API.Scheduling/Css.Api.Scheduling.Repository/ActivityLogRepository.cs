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
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityLogRepository" /> class.
        /// </summary>
        /// <param name="mongoContext">The mongo context.</param>
        /// <param name="mapper">The mapper.</param>
        public ActivityLogRepository(
            IMongoContext mongoContext,
            IMapper mapper) : base(mongoContext)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the activity logs.
        /// </summary>
        /// <param name="activityLogQueryParameter">The activity log query parameter.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<PagedList<Entity>> GetActivityLogs(ActivityLogQueryParameter activityLogQueryParameter)
        {
            var activityLogs = FilterBy(x => true);

            var filteredActivityLogs = FilterActivityLogs(activityLogs, activityLogQueryParameter);

            var sortedActivityLogs = SortHelper.ApplySort(filteredActivityLogs, activityLogQueryParameter.OrderBy);

            var pagedActivityLogs = sortedActivityLogs;

            if (!activityLogQueryParameter.SkipPageSize)
            {
                pagedActivityLogs = sortedActivityLogs
                   .Skip((activityLogQueryParameter.PageNumber - 1) * activityLogQueryParameter.PageSize)
                   .Take(activityLogQueryParameter.PageSize);
            }

            var mappedActivityLogs = pagedActivityLogs
            .ProjectTo<ActivityLogDTO>(_mapper.ConfigurationProvider);

            var shapedActivityLogs = DataShaper.ShapeData(mappedActivityLogs, activityLogQueryParameter.Fields);

            return await PagedList<Entity>
                .ToPagedList(shapedActivityLogs, filteredActivityLogs.Count(), activityLogQueryParameter.PageNumber, activityLogQueryParameter.PageSize);
        }

        /// <summary>
        /// Creates the activity log.
        /// </summary>
        /// <param name="activityLogRequest">The activity log request.</param>
        public void CreateActivityLog(ActivityLog activityLogRequest)
        {
            InsertOneAsync(activityLogRequest);
        }

        /// <summary>
        /// Creates the activity logs.
        /// </summary>
        /// <param name="activityLogRequest">The activity log request.</param>
        public void CreateActivityLogs(List<ActivityLog> activityLogRequest)
        {
            if (activityLogRequest.Any())
            {
                InsertManyAsync(activityLogRequest);
            }
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

            if (!string.IsNullOrWhiteSpace(activityLogQueryParameter.SearchKeyword))
            {
                activityLogs = activityLogs.Where(o => o.ExecutedBy.ToLower().Contains(activityLogQueryParameter.SearchKeyword.Trim().ToLower()) ||
                                                       o.FieldDetails
                                                        .Any(field => field.Name.ToLower().Contains(activityLogQueryParameter.SearchKeyword.Trim().ToLower()) ||
                                                             field.NewValue.ToLower().Contains(activityLogQueryParameter.SearchKeyword.Trim().ToLower()) ||
                                                             field.OldValue.ToLower().Contains(activityLogQueryParameter.SearchKeyword.Trim().ToLower())
                                                             ));
            }

            if (activityLogQueryParameter.EmployeeId.HasValue && activityLogQueryParameter.EmployeeId != default(int))
            {
                activityLogs = activityLogs.Where(x => x.EmployeeId == activityLogQueryParameter.EmployeeId);
            }

            if (activityLogQueryParameter.ExecutedUser.HasValue && activityLogQueryParameter.ExecutedUser != default(int))
            {
                activityLogs = activityLogs.Where(x => x.ExecutedUser == activityLogQueryParameter.ExecutedUser);
            }

            if (activityLogQueryParameter.ActivityType != null)
            {
                activityLogs = activityLogs.Where(o => o.ActivityType == activityLogQueryParameter.ActivityType);
            }

            if (!string.IsNullOrWhiteSpace(activityLogQueryParameter.NewValue))
            {
                activityLogs = activityLogs.Where(o => o.FieldDetails.Find(x => x.NewValue == activityLogQueryParameter.NewValue).NewValue == activityLogQueryParameter.NewValue);
            }

            if (!string.IsNullOrWhiteSpace(activityLogQueryParameter.OldValue))
            {
                activityLogs = activityLogs.Where(o => o.FieldDetails.Find(x => x.OldValue == activityLogQueryParameter.OldValue).OldValue == activityLogQueryParameter.OldValue);
            }

            if (!string.IsNullOrWhiteSpace(activityLogQueryParameter.Field))
            {
                activityLogs = activityLogs.Where(o => o.FieldDetails.Find(x => x.Name == activityLogQueryParameter.Field).Name == activityLogQueryParameter.Field);
            }

            if (!string.IsNullOrWhiteSpace(activityLogQueryParameter.Id))
            {
                activityLogs = activityLogs.Where(o => o.Id == new ObjectId(activityLogQueryParameter.Id));
            }

            return activityLogs;
        }
    }
}