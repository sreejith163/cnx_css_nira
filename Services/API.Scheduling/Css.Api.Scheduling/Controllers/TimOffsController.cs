using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Models.DTO.Request.TimeOff;
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
    public class TimOffsController : ControllerBase
    {
        /// <summary>
        /// The time off service
        /// </summary>
        private readonly ITimeOffService _timeOffService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeZonesController" /> class.
        /// </summary>
        /// <param name="timeOffService">The time off service.</param>
        public TimOffsController(ITimeOffService timeOffService)
        {
            _timeOffService = timeOffService;
        }

        /// <summary>
        /// Gets the time offs.
        /// </summary>
        /// <param name="timeOffQueryparameter">The time off queryparameter.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetTimeOffs([FromQuery] TimeOffQueryparameter timeOffQueryparameter)
        {
            var result = await _timeOffService.GetTimeOffs(timeOffQueryparameter);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Gets the time off.
        /// </summary>
        /// <param name="timeOffId">The time off identifier.</param>
        /// <returns></returns>
        [HttpGet("{timeOffId}")]
        public async Task<IActionResult> GetTimeOff(string timeOffId)
        {
            var result = await _timeOffService.GetTimeOff(new TimeOffIdDetails { TimeOffId = timeOffId });
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Creates the time off.
        /// </summary>
        /// <param name="timeOffDetails">The time off details.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateTimeOff([FromBody] CreateTimeOff timeOffDetails)
        {
            var result = await _timeOffService.CreateTimeOff(timeOffDetails);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Updates the time off.
        /// </summary>
        /// <param name="timeOffId">The time off identifier.</param>
        /// <param name="timeOffDetails">The time off details.</param>
        /// <returns></returns>
        [HttpPut("{timeOffId}")]
        public async Task<IActionResult> UpdateTimeOff(string timeOffId, [FromBody] UpdateTimeOff timeOffDetails)
        {
            var result = await _timeOffService.UpdateTimeOff(new TimeOffIdDetails { TimeOffId = timeOffId }, timeOffDetails);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Deletes the time off.
        /// </summary>
        /// <param name="timeOffId">The time off identifier.</param>
        /// <returns></returns>
        [HttpDelete("{timeOffId}")]
        public async Task<IActionResult> DeleteTimeOff(string timeOffId)
        {
            var result = await _timeOffService.DeleteTimeOff(new TimeOffIdDetails { TimeOffId = timeOffId });
            return StatusCode((int)result.Code, result.Value);
        }
    }
}

