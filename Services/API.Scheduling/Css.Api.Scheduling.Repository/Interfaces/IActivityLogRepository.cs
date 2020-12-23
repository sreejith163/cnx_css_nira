using Css.Api.Core.Models.Domain;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.ActivityLog;
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

        /// <summary>Gets the activity logs count.</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<int> GetActivityLogsCount();

        /// <summary>Creates the activity logs.</summary>
        /// <param name="activityLogRequest">The activity log request.</param>
        void CreateActivityLogs(ActivityLog activityLogRequest);
    }
}