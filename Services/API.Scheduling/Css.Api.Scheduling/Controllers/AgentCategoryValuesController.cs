using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Models.DTO.Request.AgentCategoryValueView;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
    public class AgentCategoryValuesController : ControllerBase
    {
        /// <summary>The agentCategoryValue service</summary>
        private readonly IAgentCategoryValueService _agentCategoryValueService;

        /// <summary>Initializes a new instance of the <see cref="AgentCategoryValuesController" /> class.</summary>
        /// <param name="agentCategoryValueService">The agentCategoryValue service.</param>
        public AgentCategoryValuesController(IAgentCategoryValueService agentCategoryValueService)
        {
            _agentCategoryValueService = agentCategoryValueService;
        }

        /// <summary>
        /// Gets the agent category values.
        /// </summary>
        /// <param name="agentCategoryValueQueryParameters">The agentCategoryValue query parameters.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> GetAgentCategoryValues([FromQuery] AgentCategoryValueQueryParameter agentCategoryValueQueryParameters)
        {
            var result = await _agentCategoryValueService.GetAgentCategoryValues(agentCategoryValueQueryParameters);
            return StatusCode((int)result.Code, result.Value);
        }

        [HttpPut("import")]
        public async Task<IActionResult> ImportAgentCategoryValue([FromBody] List<ImportAgentCategoryValue> importAgentCategoryValue)
        {
            var result = await _agentCategoryValueService.ImportAgentCategoryValue(importAgentCategoryValue);
            return StatusCode((int)result.Code, result.Value);
        }

    }
}