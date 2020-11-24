using Css.Api.Admin.Business.Interfaces;
using Css.Api.Admin.Models.DTO.Request.SchedulingCode;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Css.Api.Admin.Controllers
{
    /// <summary>
    /// Controller for handling the schedulingCode resource
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SchedulingCodesController : ControllerBase
    {
        /// <summary>
        /// The scheduling code service
        /// </summary>
        private readonly ISchedulingCodeService _schedulingCodeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulingCodesController" /> class.
        /// </summary>
        /// <param name="schedulingCodeService">The scheduling code service.</param>
        public SchedulingCodesController(ISchedulingCodeService schedulingCodeService)
        {
            _schedulingCodeService = schedulingCodeService;
        }

        /// <summary>
        /// Gets the scheduling codes.
        /// </summary>
        /// <param name="schedulingCodeParameters">The scheduling code parameters.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetSchedulingCodes([FromQuery] SchedulingCodeQueryParameters schedulingCodeParameters)
        {
            var result = await _schedulingCodeService.GetSchedulingCodes(schedulingCodeParameters);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Gets the scheduling code.
        /// </summary>
        /// <param name="schedulingCodeId">The scheduling code identifier.</param>
        /// <returns></returns>
        [HttpGet("{schedulingCodeId}")]
        public async Task<IActionResult> GetSchedulingCode(int schedulingCodeId)
        {
            var result = await _schedulingCodeService.GetSchedulingCode(new SchedulingCodeIdDetails { SchedulingCodeId = schedulingCodeId });
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Creates the scheduling code.
        /// </summary>
        /// <param name="schedulingCodeDetails">The scheduling code details.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateSchedulingCode([FromBody] CreateSchedulingCode schedulingCodeDetails)
        {
            var result = await _schedulingCodeService.CreateSchedulingCode(schedulingCodeDetails);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Updates the scheduling code.
        /// </summary>
        /// <param name="schedulingCodeId">The scheduling code identifier.</param>
        /// <param name="schedulingCodeDetails">The scheduling code details.</param>
        /// <returns></returns>
        [HttpPut("{schedulingCodeId}")]
        public async Task<IActionResult> UpdateSchedulingCode(int schedulingCodeId, [FromBody] UpdateSchedulingCode schedulingCodeDetails)
        {
            var result = await _schedulingCodeService.UpdateSchedulingCode(new SchedulingCodeIdDetails { SchedulingCodeId = schedulingCodeId }, schedulingCodeDetails);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Deletes the scheduling code.
        /// </summary>
        /// <param name="schedulingCodeId">The scheduling code identifier.</param>
        /// <returns></returns>
        [HttpDelete("{schedulingCodeId}")]
        public async Task<IActionResult> DeleteSchedulingCode(int schedulingCodeId)
        {
            var result = await _schedulingCodeService.DeleteSchedulingCode(new SchedulingCodeIdDetails { SchedulingCodeId = schedulingCodeId });
            return StatusCode((int)result.Code, result.Value);
        }
    }
}
