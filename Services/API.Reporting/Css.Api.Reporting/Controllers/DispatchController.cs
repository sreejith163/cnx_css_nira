using Css.Api.Reporting.Business.Interfaces;
using Css.Api.Reporting.Models.DTO.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Css.Api.Reporting.Controllers
{
    /// <summary>
    /// The Dispatch controller
    /// </summary>
    [Authorize]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DispatchController : ControllerBase
    {
        /// <summary>
        /// The dispatch strategy
        /// </summary>
        private readonly IDispatchStrategy _dispatchStrategy;

        /// <summary>
        /// The constructor to initialize properties
        /// </summary>
        /// <param name="dispatchStrategy"></param>
        public DispatchController(IDispatchStrategy dispatchStrategy)
        {
            _dispatchStrategy = dispatchStrategy;
        }

        /// <summary>
        /// The method to export the agent info to the target
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{target}")]
        public async Task<DispatchResponse> Put(string target)
        {
            var response = await _dispatchStrategy.ExportAgent();
            Response.StatusCode =  (int) response.StatusCode;
            return response;
        }

    }
}
