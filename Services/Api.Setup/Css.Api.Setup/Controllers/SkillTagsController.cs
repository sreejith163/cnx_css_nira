using Css.Api.Setup.Business.Interfaces;
using Css.Api.Setup.Models.DTO.Request.SkillTag;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Css.Api.Setup.Controllers
{
    /// <summary>Controller for handling Skill Tags resource</summary>
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SkillTagsController : ControllerBase
    {
        /// <summary>The skill tag service</summary>
        private readonly ISkillTagService _skillTagService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SkillTagsController"/> class.
        /// </summary>
        /// <param name="skillTagService">The skill tag service.</param>
        public SkillTagsController(ISkillTagService skillTagService)
        {
            _skillTagService = skillTagService;
        }

        /// <summary>
        /// Gets the skill tags.
        /// </summary>
        /// <param name="skillTagQueryParameter">The skill tag query parameter.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetSkillTags([FromQuery] SkillTagQueryParameter skillTagQueryParameter)
        {
            var result = await _skillTagService.GetSkillTags(skillTagQueryParameter);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Gets the skill tag.
        /// </summary>
        /// <param name="skillTagId">The skill tag identifier.</param>
        /// <returns></returns>
        [HttpGet("{skillTagId}")]
        public async Task<IActionResult> GetSkillTag(int skillTagId)
        {
            var result = await _skillTagService.GetSkillTag(new SkillTagIdDetails { SkillTagId = skillTagId });
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Creates the skill tag.
        /// </summary>
        /// <param name="skillTagDetails">The skill tag details.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateSkillTag([FromBody] CreateSkillTag skillTagDetails)
        {
            var result = await _skillTagService.CreateSkillTag(skillTagDetails);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Updates the skill tag.
        /// </summary>
        /// <param name="skillTagId">The skill tag identifier.</param>
        /// <param name="skillTagDetails">The skill tag details.</param>
        /// <returns></returns>
        [HttpPut("{skillTagId}")]
        public async Task<IActionResult> UpdateSkillTag(int skillTagId, [FromBody] UpdateSkillTag skillTagDetails)
        {
            var result = await _skillTagService.UpdateSkillTag(new SkillTagIdDetails { SkillTagId = skillTagId }, skillTagDetails);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>
        /// Deletes the skill tag.
        /// </summary>
        /// <param name="skillTagId">The skill tag identifier.</param>
        /// <returns></returns>
        [HttpDelete("{skillTagId}")]
        public async Task<IActionResult> DeleteSkillTag(int skillTagId)
        {
            var result = await _skillTagService.DeleteSkillTag(new SkillTagIdDetails { SkillTagId = skillTagId });
            return StatusCode((int)result.Code, result.Value);
        }
    }
}

