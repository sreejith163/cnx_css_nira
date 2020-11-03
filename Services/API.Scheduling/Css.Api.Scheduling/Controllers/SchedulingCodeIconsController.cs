using Css.Api.Scheduling.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Controllers
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
        private readonly ISchedulingCodeTypeService _schedulingCodeTypeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulingCodeIconsController"/> class.
        /// </summary>
        /// <param name="schedulingCodeTypeService">The scheduling code type service.</param>
        public SchedulingCodeIconsController(ISchedulingCodeTypeService schedulingCodeTypeService)
        {
            _schedulingCodeTypeService = schedulingCodeTypeService;
        }

        /// <summary>
        /// Gets the scheduling code types.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetSchedulingCodeTypes()
        {
            var result = await _schedulingCodeTypeService.GetSchedulingCodeTypes();
            return StatusCode((int)result.Code, result.Value);
        }
    }
}
