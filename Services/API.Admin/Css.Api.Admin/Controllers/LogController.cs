using Css.Api.Admin.Business.Interfaces;
using Css.Api.Admin.Models.DTO.Request.Log;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Css.Api.Admin.Controllers
{
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    //[ApiExplorerSettings(IgnoreApi = true)] 
    public class LogController : ControllerBase
    {
        private readonly ILogService _iLogService;


        /// <summary>
        /// Initializes a new instance of the <see cref="LogController"/> class.
        /// </summary>
        /// <param name="logService">The role service.</param>
        public LogController(ILogService logService)
        {
            _iLogService = logService;
        }
        /// <summary>
        /// Creates login log
        /// </summary>
        /// <param name="createLog">The user log details.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateLogger([FromBody] CreateLogDTO createLog)
        {
            var result = await _iLogService.CreateLog(createLog);
            return StatusCode((int)result.Code, result.Value);
        }

    }
}
