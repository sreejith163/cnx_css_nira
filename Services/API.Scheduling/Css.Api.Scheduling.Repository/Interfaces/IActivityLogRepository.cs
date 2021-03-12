using Css.Api.Core.Models.Domain;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.ActivityLog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Repository.Interfaces
{
    public interface IActivityLogRepository
    {
        /// <summary>Gets the activity logs.</summary>
        /// <param name="activityLogQueryParameter">The activity log query parameter.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<PagedList<Entity>> GetActivityLogs(ActivityLogQueryParameter activityLogQueryParameter);

        /// <summary>
        /// Creates the activity log.
        /// </summary>
        /// <param name="activityLogRequest">The activity log request.</param>
        void CreateActivityLog(ActivityLog activityLogRequest);

        /// <summary>
        /// Creates the activity logs.
        /// </summary>
        /// <param name="activityLogRequest">The activity log request.</param>
        void CreateActivityLogs(List<ActivityLog> activityLogRequest);
    }
}