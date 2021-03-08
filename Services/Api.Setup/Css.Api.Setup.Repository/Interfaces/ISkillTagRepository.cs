using Css.Api.Core.Models.Domain;
using Css.Api.Setup.Models.Domain;
using Css.Api.Setup.Models.DTO.Request.SkillGroup;
using Css.Api.Setup.Models.DTO.Request.SkillTag;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Css.Api.Setup.Repository.Interfaces
{
    public interface ISkillTagRepository
    {
        /// <summary>
        /// Gets the skill tags.
        /// </summary>
        /// <param name="skillTagQueryParameter">The skill tag query parameter.</param>
        /// <returns></returns>
        Task<PagedList<Entity>> GetSkillTags(SkillTagQueryParameter skillTagQueryParameter);

        /// <summary>
        /// Gets the skill tag.
        /// </summary>
        /// <param name="skillTagIdDetails">The skill tag identifier details.</param>
        /// <returns></returns>
        Task<SkillTag> GetSkillTag(SkillTagIdDetails skillTagIdDetails);

        /// <summary>Gets all skill tag.</summary>
        /// <param name="skillTagIdDetails">The skill tag identifier details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<SkillTag> GetAllSkillTag(SkillTagIdDetails skillTagIdDetails);

        /// <summary>
        /// Gets the skill tags count by skill group identifier.
        /// </summary>
        /// <param name="skillGroupIdDetails">The skill group identifier details.</param>
        /// <returns></returns>
        Task<int> GetSkillTagsCountBySkillGroupId(SkillGroupIdDetails skillGroupIdDetails);

        /// <summary>
        /// Gets the name of the skill tag identifier by skill group identifier and group.
        /// </summary>
        /// <param name="skillGroupIdDetails">The skill group identifier details.</param>
        /// <param name="skillTagNameDetails">The skill tag name details.</param>
        /// <returns></returns>
        Task<List<int>> GetSkillTagIdBySkillGroupIdAndGroupName(SkillGroupIdDetails skillGroupIdDetails, SkillTagNameDetails skillTagNameDetails);

        /// <summary>Gets the name of all skill tag identifier by skill group identifier and group.</summary>
        /// <param name="skillGroupIdDetails">The skill group identifier details.</param>
        /// <param name="skillTagNameDetails">The skill tag name details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<List<int>> GetAllSkillTagIdBySkillGroupIdAndGroupName(SkillGroupIdDetails skillGroupIdDetails, SkillTagNameDetails skillTagNameDetails);

        /// <summary>
        /// Creates the skill tag.
        /// </summary>
        /// <param name="skillTagRequest">The skill tag request.</param>
        void CreateSkillTag(SkillTag skillTagRequest);

        /// <summary>
        /// Updates the skill tag.
        /// </summary>
        /// <param name="skillTagRequest">The skill tag request.</param>
        void UpdateSkillTag(SkillTag skillTagRequest);

        /// <summary>
        /// Deletes the skill tag.
        /// </summary>
        /// <param name="skillTagRequest">The skill tag request.</param>
        void DeleteSkillTag(SkillTag skillTagRequest);
    }
}
