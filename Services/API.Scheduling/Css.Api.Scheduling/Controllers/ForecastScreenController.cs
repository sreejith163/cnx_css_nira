using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Models.DTO.Request.ForecastScreen;
using Css.Api.Scheduling.Models.DTO.Request.SkillGroup;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Controllers
{
    /// <summary>
    /// Controller for handling the agent admins resource
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]


    public class ForecastScreenController : ControllerBase
    {
        /// <summary>
        /// The agentAdmin service
        /// </summary>
        private readonly IForecastScreenService _forecastScreenService;


        /// <summary>Initializes a new instance of the <see cref="ForecastScreenController" /> class.</summary>

        /// <param name="forecastScreenService">The aagent schedule service.</param>
        public ForecastScreenController(IForecastScreenService forecastScreenService)
        {
            _forecastScreenService = forecastScreenService;
        }

        /// <summary>
        /// Gets the Forecast Screen
        /// </summary>
        /// <param name="forecastScreenQueryparameter">The agent schedule queryparameter.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetForecastScreen([FromQuery] ForeCastScreenQueryParameter forecastScreenQueryparameter)
        {
            var result = await _forecastScreenService.GetForecastScreen(forecastScreenQueryparameter);
            return StatusCode((int)result.Code, result.Value);
        }


        /// <summary>
        /// Gets the Forecast Screen by skill group id
        /// </summary>
        /// <param name="skillGroupId">The forecast screen query parameter</param>
        /// <param name="date">The forecast screen query parameter</param>
        /// <returns></returns>
        [HttpGet("{skillGroupId}")]
        public async Task<IActionResult> GetForecastScreenBySkillGroupId(int skillGroupId, string date)
        {
            var result = await _forecastScreenService.GetForecastScreenBySkillGroupId(
                new CreateForecastData { SkillGroupId = Convert.ToInt32(skillGroupId), Date = date.ToString() }


                );
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Creates Forecast Data
        /// </summary>
        /// <param name="forecastData">forecastData query parameter.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateForecastData([FromBody] CreateForecastData forecastData)
        {
            var result = await _forecastScreenService.CreateForecastData(forecastData);
            return StatusCode((int)result.Code, result.Value);
        }
        /// <summary>
        /// Updates the agent admin.
        /// </summary>

        /// <param name="forecastDetails">The client details.</param>
        /// <param name="forecastId">Forecast ID</param>
        /// <returns></returns>
        [HttpPut("{forecastId}")]

        public async Task<IActionResult> UpdateForecastData(long forecastId, [FromBody] UpdateForecastData forecastDetails)
        { 
           
            var result = await _forecastScreenService.UpdateForecastData(forecastDetails, forecastId);
            return StatusCode((int)result.Code, result.Value);
        }
        ///// <summary>
        ///// import forecast data
        ///// </summary>

        ///// <param name="importForecast">Forecast ID</param>
        ///// <returns></returns>

        //[HttpPut("import")]
        //public async Task<IActionResult> ImportForecast([FromBody] ImportForecast importForecast)
        //{
        //    var result = await _forecastScreenService.ImportForecast(importForecast);
        //    return StatusCode((int)result.Code, result.Value);
        //}

    }
}
