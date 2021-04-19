using Css.Api.Admin.Business.Interfaces;
using Css.Api.Admin.Models.DTO.Request.AgentCategory;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Css.Api.Admin.Controllers
{
    /// <summary>Controller for handling Agent Categories resource</summary>
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AgentCategoriesController : ControllerBase
    {
        /// <summary>The agent category service</summary>
        private readonly IAgentCategoryService _agentCategoryService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentCategoriesController"/> class.
        /// </summary>
        /// <param name="agentCategoryService">The agent category service.</param>
        public AgentCategoriesController(IAgentCategoryService agentCategoryService)
        {
            _agentCategoryService = agentCategoryService;
        }

        /// <summary>
        /// Gets the agent categories.
        /// </summary>
        /// <param name="agentCategoryQueryParameter">The agent category query parameter.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAgentCategories([FromQuery] AgentCategoryQueryParameter agentCategoryQueryParameter)
        {
            var result = await _agentCategoryService.GetAgentCategories(agentCategoryQueryParameter);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Gets the agent category.
        /// </summary>
        /// <param name="agentCategoryId">The agent category identifier.</param>
        /// <returns></returns>
        [HttpGet("{agentCategoryId}")]
        public async Task<IActionResult> GetAgentCategory(int agentCategoryId)
        {
            var result = await _agentCategoryService.GetAgentCategory(new AgentCategoryIdDetails { AgentCategoryId = agentCategoryId });
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Creates the agent category.
        /// </summary>
        /// <param name="agentCategoryDetails">The agent category details.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateAgentCategory([FromBody] CreateAgentCategory agentCategoryDetails)
        {
            var result = await _agentCategoryService.CreateAgentCategory(agentCategoryDetails);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Updates the agent category.
        /// </summary>
        /// <param name="agentCategoryId">The agent category identifier.</param>
        /// <param name="agentCategoryDetails">The agent category details.</param>
        /// <returns></returns>
        [HttpPut("{agentCategoryId}")]
        public async Task<IActionResult> UpdateAgentCategory(int agentCategoryId, [FromBody] UpdateAgentCategory agentCategoryDetails)
        {
            var result = await _agentCategoryService.UpdateAgentCategory(new AgentCategoryIdDetails { AgentCategoryId = agentCategoryId }, agentCategoryDetails);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Deletes the agent category.
        /// </summary>
        /// <param name="agentCategoryId">The agent category identifier.</param>
        /// <returns></returns>
        [HttpDelete("{agentCategoryId}")]
        public async Task<IActionResult> DeleteAgentCategory(int agentCategoryId)
        {
            var result = await _agentCategoryService.DeleteAgentCategory(new AgentCategoryIdDetails { AgentCategoryId = agentCategoryId });
            return StatusCode((int)result.Code, result.Value);
        }

      
    }
}