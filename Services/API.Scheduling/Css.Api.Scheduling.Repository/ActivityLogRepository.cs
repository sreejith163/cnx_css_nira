using AutoMapper;
using AutoMapper.QueryableExtensions;
using Css.Api.Core.DataAccess.Repository.NoSQL;
using Css.Api.Core.DataAccess.Repository.NoSQL.Interfaces;
using Css.Api.Core.Models.Domain;
using Css.Api.Core.Utilities.Extensions;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.ActivityLog;
using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using Css.Api.Scheduling.Models.DTO.Response.ActivityLog;
using Css.Api.Scheduling.Models.Enums;
using Css.Api.Scheduling.Repository.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
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

        /// <summary>
        /// Updates the activity logs employee identifier.
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <param name="newEmployeeIdDetails">The new employee identifier details.</param>
        public void UpdateActivityLogsEmployeeId(EmployeeIdDetails employeeIdDetails, EmployeeIdDetails newEmployeeIdDetails)
        {
            var query =
                Builders<ActivityLog>.Filter.Eq(i => i.EmployeeId, employeeIdDetails.Id) &
                (Builders<ActivityLog>.Filter.Eq(i => i.ActivityType, ActivityType.SchedulingGrid) |
                Builders<ActivityLog>.Filter.Eq(i => i.ActivityType, ActivityType.SchedulingmanagerGrid));

            var update = Builders<ActivityLog>.Update
                .Set(x => x.EmployeeId, newEmployeeIdDetails.Id);

            UpdateManyAsync(query, update);
        }

        /// <summary>
        /// Updates the activity logs scheduling range.
        /// </summary>
        /// <param name="employeeIdDetails">The employee identifier details.</param>
        /// <param name="activityLogRange">The activity log range.</param>
        public void UpdateActivityLogsSchedulingRange(EmployeeIdDetails employeeIdDetails, UpdateActivityLogRange activityLogRange)
        {
            activityLogRange.DateFrom = new DateTime(activityLogRange.DateFrom.Year, activityLogRange.DateFrom.Month,
                                                     activityLogRange.DateFrom.Day, 0, 0, 0, DateTimeKind.Utc);
            activityLogRange.DateTo = new DateTime(activityLogRange.DateTo.Year, activityLogRange.DateTo.Month,
                                                   activityLogRange.DateTo.Day, 0, 0, 0, DateTimeKind.Utc);
            activityLogRange.NewDateFrom = new DateTime(activityLogRange.NewDateFrom.Year, activityLogRange.NewDateFrom.Month,
                                                        activityLogRange.NewDateFrom.Day, 0, 0, 0, DateTimeKind.Utc);
            activityLogRange.NewDateTo = new DateTime(activityLogRange.NewDateTo.Year, activityLogRange.NewDateTo.Month,
                                                      activityLogRange.NewDateTo.Day, 0, 0, 0, DateTimeKind.Utc);

            var query =
                Builders<ActivityLog>.Filter.Eq(i => i.EmployeeId, employeeIdDetails.Id) &
                Builders<ActivityLog>.Filter.Eq(i => i.ActivityType, ActivityType.SchedulingGrid) &
                Builders<ActivityLog>.Filter.Eq(i => i.SchedulingFieldDetails.ActivityLogRange.DateFrom, activityLogRange.DateFrom) &
                Builders<ActivityLog>.Filter.Eq(i => i.SchedulingFieldDetails.ActivityLogRange.DateTo, activityLogRange.DateTo);

            var update = Builders<ActivityLog>.Update
                .Set(x => x.SchedulingFieldDetails.ActivityLogRange.DateFrom, activityLogRange.NewDateFrom)
                .Set(x => x.SchedulingFieldDetails.ActivityLogRange.DateTo, activityLogRange.NewDateTo);

            UpdateManyAsync(query, update);
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

            if (activityLogQueryParameter.Date.HasValue && activityLogQueryParameter.Date != default(DateTime) &&
                activityLogQueryParameter.ActivityType != null && activityLogQueryParameter.ActivityType == ActivityType.SchedulingmanagerGrid)
            {
                var date = activityLogQueryParameter.Date.Value;
                var dateTimeWithZeroTimeSpan = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Utc);

                activityLogs = activityLogs.Where(x => x.SchedulingFieldDetails.ActivityLogManager.Date == dateTimeWithZeroTimeSpan);
            }

            if (activityLogQueryParameter.DateFrom.HasValue && activityLogQueryParameter.DateFrom != default(DateTime) &&
                activityLogQueryParameter.DateTo.HasValue && activityLogQueryParameter.DateTo != default(DateTime) &&
                activityLogQueryParameter.ActivityType != null && activityLogQueryParameter.ActivityType == ActivityType.SchedulingGrid)
            {
                var dateFrom = activityLogQueryParameter.DateFrom.Value;
                var dateTo = activityLogQueryParameter.DateTo.Value;
                var dateTimeFromWithZeroTimeSpan = new DateTime(dateFrom.Year, dateFrom.Month, dateFrom.Day, 0, 0, 0, DateTimeKind.Utc);
                var dateTimeToWithZeroTimeSpan = new DateTime(dateTo.Year, dateTo.Month, dateTo.Day, 0, 0, 0, DateTimeKind.Utc);

                activityLogs = activityLogs.Where(x => x.SchedulingFieldDetails.ActivityLogRange.DateFrom == dateTimeFromWithZeroTimeSpan &&
                                                       x.SchedulingFieldDetails.ActivityLogRange.DateTo == dateTimeToWithZeroTimeSpan);
            }

            return activityLogs;
        }
    }
}