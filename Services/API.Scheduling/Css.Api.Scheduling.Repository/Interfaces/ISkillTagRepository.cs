using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.SkillTag;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Repository.Interfaces
{
    /// <summary>
    /// Interface for hte Skill Tag repository
    /// </summary>
    public interface ISkillTagRepository
    {
        /// <summary>
        /// Gets the skill tag.
        /// </summary>
        /// <param name="skillTagIdDetails">The skill tag identifier details.</param>
        /// <returns></returns>
        Task<SkillTag> GetSkillTag(SkillTagIdDetails skillTagIdDetails);

        /// <summary>
        /// Creates the skill tags.
        /// </summary>
        /// <param name="skillTagRequestCollection">The skill tag request collection.</param>
        void CreateSkillTags(ICollection<SkillTag> skillTagRequestCollection);

        /// <summary>
        /// Gets the skill tags count.
        /// </summary>
        /// <returns></returns>
        Task<int> GetSkillTagsCount();
    }
}