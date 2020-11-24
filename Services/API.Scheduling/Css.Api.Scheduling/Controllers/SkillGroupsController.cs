using Css.Api.Scheduling.Business.Interfaces;
using Css.Api.Scheduling.Models.DTO.Request.SkillGroup;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Controllers
{
    /// <summary>Controller for handling skill group related routings</summary>
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SkillGroupsController : ControllerBase
    {
        /// <summary>The skill group service</summary>
        private readonly ISkillGroupService _skillGroupService;

        /// <summary>Initializes a new instance of the <see cref="SkillGroupsController" /> class.</summary>
        /// <param name="skillGroupService">The skill group service.</param>
        public SkillGroupsController(ISkillGroupService skillGroupService)
        {
            _skillGroupService = skillGroupService;
        }

        /// <summary>Gets the skill groups.</summary>
        /// <param name="skillQueryParameters">The skill query parameters.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> GetSkillGroups([FromQuery] SkillGroupQueryParameter skillQueryParameters)
        {
            var result = await _skillGroupService.GetSkillGroups(skillQueryParameters);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>Gets the skill group.</summary>
        /// <param name="skillGroupId">The skill group identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpGet("{skillGroupId}")]
        public async Task<IActionResult> GetSkillGroup(int skillGroupId)
        {
            var result = await _skillGroupService.GetSkillGroup(new SkillGroupIdDetails { SkillGroupId = skillGroupId });
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>Creates the skill group.</summary>
        /// <param name="skillGroupDetails">The skill group details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> CreateSkillGroup([FromBody] CreateSkillGroup skillGroupDetails)
        {
            var result = await _skillGroupService.CreateSkillGroup(skillGroupDetails);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>Updates the skill group.</summary>
        /// <param name="skillGroupId">The skill group identifier.</param>
        /// <param name="skillGroupDetails">The skill group details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpPut("{skillGroupId}")]
        public async Task<IActionResult> UpdateSkillGroup(int skillGroupId, [FromBody] UpdateSkillGroup skillGroupDetails)
        {
            var result = await _skillGroupService.UpdateSkillGroup(new SkillGroupIdDetails { SkillGroupId = skillGroupId }, skillGroupDetails);
            return StatusCode((int)result.Code, result.Value);
        }

        /// <summary>Deletes the skill group.</summary>
        /// <param name="skillGroupId">The skill group identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpDelete("{skillGroupId}")]
        public async Task<IActionResult> DeleteSkillGroup(int skillGroupId)
        {
            var result = await _skillGroupService.DeleteSkillGroup(new SkillGroupIdDetails { SkillGroupId = skillGroupId });
            return StatusCode((int)result.Code, result.Value);
        }
    }
}
