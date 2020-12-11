using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.SkillGroup;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Css.Api.Scheduling.Repository.Interfaces
{
    /// <summary>
    /// Repository interface for Skill group
    /// </summary>
    public interface ISkillGroupRepository
    {
        /// <summary>
        /// Gets the skill group.
        /// </summary>
        /// <param name="skillGroupIdDetails">The skill group identifier details.</param>
        /// <returns></returns>
        Task<SkillGroup> GetSkillGroup(SkillGroupIdDetails skillGroupIdDetails);

        /// <summary>
        /// Creates the skill groups.
        /// </summary>
        /// <param name="skillGroupRequestCollection">The skill group request collection.</param>
        void CreateSkillGroups(ICollection<SkillGroup> skillGroupRequestCollection);

        /// <summary>
        /// Gets the skill groups count.
        /// </summary>
        /// <returns></returns>
        Task<int> GetSkillGroupsCount();
    }
}
