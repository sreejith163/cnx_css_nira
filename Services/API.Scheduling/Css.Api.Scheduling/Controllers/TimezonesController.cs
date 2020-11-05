using Css.Api.Scheduling.Business.Interfaces;
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

        /// <summary>Gets the time zones.</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> GetTimeZones()
        {
            var result = await _timezoneService.GetTimezones();
            return StatusCode((int)result.Code, result.Value);
        }
    }
}

