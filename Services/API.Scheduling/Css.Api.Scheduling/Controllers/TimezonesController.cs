using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Models.DTO.Request.Timezone;
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
    public class TimeZonesController : ControllerBase
    {
        /// <summary>The timezone service</summary>
        private readonly ITimezoneService _timezoneService;

        /// <summary>Initializes a new instance of the <see cref="TimeZonesController" /> class.</summary>
        /// <param name="timezoneService">The timezone service.</param>
        public TimeZonesController(ITimezoneService timezoneService)
        {
            _timezoneService = timezoneService;
        }

        /// <summary>
        /// Gets the time zones.
        /// </summary>
        /// <param name="timezoneQueryParameters">The timezone query parameters.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> GetTimeZones([FromQuery] TimezoneQueryParameters timezoneQueryParameters)
        {
            var result = await _timezoneService.GetTimeZones(timezoneQueryParameters);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Gets the time zone.
        /// </summary>
        /// <param name="timezoneId">The timezone identifier.</param>
        /// <returns></returns>
        [HttpGet("{timezoneId}")]
        public async Task<IActionResult> GetTimeZone(int timezoneId)
        {
            var result = await _timezoneService.GetTimeZone(new TimezoneIdDetails { TimezoneId = timezoneId });
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Creates the time zone.
        /// </summary>
        /// <param name="clientDetails">The client details.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateTimeZone([FromBody] CreateTimezone clientDetails)
        {
            var result = await _timezoneService.CreateTimeZone(clientDetails);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Updates the time zone.
        /// </summary>
        /// <param name="timezoneId">The timezone identifier.</param>
        /// <param name="clientDetails">The client details.</param>
        /// <returns></returns>
        [HttpPut("{timezoneId}")]
        public async Task<IActionResult> UpdateTimeZone(int timezoneId, [FromBody] UpdateTimezone clientDetails)
        {
            var result = await _timezoneService.UpdateTimeZone(new TimezoneIdDetails { TimezoneId = timezoneId }, clientDetails);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Deletes the time zone.
        /// </summary>
        /// <param name="timezoneId">The timezone identifier.</param>
        /// <returns></returns>
        [HttpDelete("{timezoneId}")]
        public async Task<IActionResult> DeleteTimeZone(int timezoneId)
        {
            var result = await _timezoneService.DeleteTimeZone(new TimezoneIdDetails { TimezoneId = timezoneId });
            return StatusCode((int)result.Code, result.Value);
        }
    }
}

