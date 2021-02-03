using Css.Api.Core.Models.DTO.Response;
using Css.Api.Scheduling.Models.DTO.Request.ActivityLog;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Business.Interfaces
{
    public interface IActivityLogService
    {
        /// <summary>
        /// Gets the activity logs.
        /// </summary>
        /// <param name="activityLogQueryParameter">The activity log query parameter.</param>
        /// <returns></returns>
        Task<CSSResponse> GetActivityLogs(ActivityLogQueryParameter activityLogQueryParameter);
    }
}