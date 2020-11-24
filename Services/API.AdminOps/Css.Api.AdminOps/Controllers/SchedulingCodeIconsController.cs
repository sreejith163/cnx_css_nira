using Css.Api.AdminOps.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Css.Api.AdminOps.Controllers
{
    /// <summary>
    /// Controller for handling the scheduling code icons resource
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SchedulingCodeIconsController : ControllerBase
    {
        /// <summary>
        /// The scheduling code type service
        /// </summary>
        private readonly ISchedulingCodeIconService _schedulingCodeIconsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulingCodeIconsController"/> class.
        /// </summary>
        /// <param name="schedulingCodeIconsService">The scheduling code icons service.</param>
        public SchedulingCodeIconsController(ISchedulingCodeIconService schedulingCodeIconsService)
        {
            _schedulingCodeIconsService = schedulingCodeIconsService;
        }

        /// <summary>
        /// Gets the scheduling code types.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetSchedulingCodeTypes()
        {
            var result = await _schedulingCodeIconsService.GetSchedulingCodeIcons();
            return StatusCode((int)result.Code, result.Value);
        }
    }
}
