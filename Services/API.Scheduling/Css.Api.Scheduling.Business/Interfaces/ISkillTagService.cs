using Css.Api.Core.Models.DTO.Response;
using Css.Api.Scheduling.Models.DTO.Request.SkillTag;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Business.Interfaces
{
    /// <summary>Interface for skill tag service</summary>
    public interface ISkillTagService
    {
        /// <summary>
        /// Gets the skill tags.
        /// </summary>
        /// <param name="skillTagQueryParameter">The skill tag query parameter.</param>
        /// <returns></returns>
        Task<CSSResponse> GetSkillTags(SkillTagQueryParameter skillTagQueryParameter);

        /// <summary>
        /// Gets the skill tag.
        /// </summary>
        /// <param name="skillTagIdDetails">The skill tag identifier details.</param>
        /// <returns></returns>
        Task<CSSResponse> GetSkillTag(SkillTagIdDetails skillTagIdDetails);

        /// <summary>
        /// Creates the skill tag.
        /// </summary>
        /// <param name="skillTagDetails">The skill tag details.</param>
        /// <returns></returns>
        Task<CSSResponse> CreateSkillTag(CreateSkillTag skillTagDetails);

        /// <summary>
        /// Updates the skill tag.
        /// </summary>
        /// <param name="skillTagIdDetails">The skill tag identifier details.</param>
        /// <param name="skillTagDetails">The skill tag details.</param>
        /// <returns></returns>
        Task<CSSResponse> UpdateSkillTag(SkillTagIdDetails skillTagIdDetails, UpdateSkillTag skillTagDetails);

        /// <summary>
        /// Deletes the skill tag.
        /// </summary>
        /// <param name="skillTagIdDetails">The skill tag identifier details.</param>
        /// <returns></returns>
        Task<CSSResponse> DeleteSkillTag(SkillTagIdDetails skillTagIdDetails);
    }
}
