using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Reporting.Business.Interfaces;
using Css.Api.Reporting.Models.DTO.Response;
using Css.Api.Reporting.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Css.Api.Reporting.Controllers
{
    /// <summary>
    /// Controller for handling the all imports
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        /// <summary>
        /// The strategy for import
        /// </summary>
        private readonly IActivityStrategy _activityStrategy;

        /// <summary>
        /// Constructor to initialize all properties
        /// </summary>
        /// <param name="activityStrategy"></param>
        public ActivityController(IActivityStrategy activityStrategy)
        {
            _activityStrategy = activityStrategy;
        }

        /// <summary>
        /// The post method to process the requests
        /// </summary>
        /// <returns>An instance of ActivityResponse</returns>
        [HttpPost]
        public async Task<ActivityResponse> Post()
        {
            return await _activityStrategy.Process();
        }

    }
}
