using Css.Api.Core.Models.Domain;
using Css.Api.Core.Models.DTO.Response;
using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;
using Css.Api.Scheduling.Models.DTO.Request.ActivityLog;

namespace Css.Api.Scheduling.Business
{
    public class ActivityLogService : IActivityLogService
    {
        /// <summary>
        /// The HTTP context accessor
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// The repository
        /// </summary>
        private readonly IActivityLogRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityLogService" /> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="repository">The repository.</param>
        public ActivityLogService(
            IHttpContextAccessor httpContextAccessor,
            IActivityLogRepository repository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = repository;
        }

        /// <summary>
        /// Gets the activity logs.
        /// </summary>
        /// <param name="activityLogQueryParameter">The activity log query parameter.</param>
        /// <returns></returns>
        public async Task<CSSResponse> GetActivityLogs(ActivityLogQueryParameter activityLogQueryParameter)
        {
            var timeZones = await _repository.GetActivityLogs(activityLogQueryParameter);
            _httpContextAccessor.HttpContext.Response.Headers.Add("X-Pagination", PagedList<Entity>.ToJson(timeZones));

            return new CSSResponse(timeZones, HttpStatusCode.OK);
        }
    }
}
