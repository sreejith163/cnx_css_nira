
using Css.Api.Scheduling.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Controllers
{
    /// <summary>
    /// Controller for handling the scheduling code types resource
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SchedulingCodeTypesController : ControllerBase
    {
        /// <summary>
        /// The scheduling code icon service
        /// </summary>
        private readonly ISchedulingCodeIconService _schedulingCodeIconService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulingCodeIconsController" /> class.
        /// </summary>
        /// <param name="schedulingCodeIconService">The scheduling code icon service.</param>
        public SchedulingCodeTypesController(ISchedulingCodeIconService schedulingCodeIconService)
        {
            _schedulingCodeIconService = schedulingCodeIconService;
        }

        /// <summary>
        /// Gets the scheduling codes.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetSchedulingCodes()
        {
            var result = await _schedulingCodeIconService.GetSchedulingCodeIcons();
            return StatusCode((int)result.Code, result.Value);
        }
    }
}
