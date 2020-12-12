using Css.Api.Core.Models.Domain;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.SkillTag;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Repository.Interfaces
{
    /// <summary>
    /// Interface for the Skill Tag repository
    /// </summary>
    public interface ISkillTagRepository
    {
        /// <summary>
        /// Gets the skill tags.
        /// </summary>
        /// <param name="skillTagQueryparameter">The skill tag queryparameter.</param>
        /// <returns></returns>
        Task<PagedList<Entity>> GetSkillTags(SkillTagQueryparameter skillTagQueryparameter);

        /// <summary>
        /// Gets the skill tag.
        /// </summary>
        /// <param name="skillTagIdDetails">The skill tag identifier details.</param>g
        /// <returns></returns>
        Task<SkillTag> GetSkillTag(SkillTagIdDetails skillTagIdDetails);

        /// <summary>
        /// Gets the skill tags count.
        /// </summary>
        /// <returns></returns>
        Task<int> GetSkillTagsCount();

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
    }
}