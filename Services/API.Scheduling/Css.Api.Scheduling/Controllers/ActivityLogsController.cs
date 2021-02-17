using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Models.DTO.Request.ActivityLog;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Controllers
{
    /// <summary>
    /// Controller for handling the Time zones resource
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ActivityLogsController : ControllerBase
    {
        /// <summary>
        /// The activity log service
        /// </summary>
        private readonly IActivityLogService _activityLogService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityLogsController"/> class.
        /// </summary>
        /// <param name="activityLogService">The activity log service.</param>
        public ActivityLogsController(IActivityLogService activityLogService)
        {
            _activityLogService = activityLogService;
        }

        /// <summary>
        /// Gets the activity logs.
        /// </summary>
        /// <param name="activityLogQueryParameter">The activity log query parameter.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetActivityLogs([FromQuery] ActivityLogQueryParameter activityLogQueryParameter)
        {
            var result = await _activityLogService.GetActivityLogs(activityLogQueryParameter);
            return StatusCode((int)result.Code, result.Value);
        }
    }
}

