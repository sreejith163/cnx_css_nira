using Css.Api.Core.Models.Domain;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.SkillGroup;
using System.Threading.Tasks;

namespace Css.Api.Scheduling.Repository.Interfaces
{
    /// <summary>
    /// Repository interface for Skill group
    /// </summary>
    public interface ISkillGroupRepository
    {
        /// <summary>
        /// Gets the skill groups.
        /// </summary>
        /// <param name="skillGroupQueryparameter">The skill group queryparameter.</param>
        /// <returns></returns>
        Task<PagedList<Entity>> GetSkillGroups(SkillGroupQueryparameter skillGroupQueryparameter);

        /// <summary>
        /// Gets the skill group.
        /// </summary>
        /// <param name="skillGroupIdDetails">The skill group identifier details.</param>
        /// <returns></returns>
        Task<SkillGroup> GetSkillGroup(SkillGroupIdDetails skillGroupIdDetails);

        /// <summary>
        /// Gets the skill groups count.
        /// </summary>
        /// <returns></returns>
        Task<int> GetSkillGroupsCount();

        /// <summary>
        /// Creates the skill group.
        /// </summary>
        /// <param name="skillGroupRequest">The skill group request.</param>
        void CreateSkillGroup(SkillGroup skillGroupRequest);

        /// <summary>
        /// Updates the skill group.
        /// </summary>
        /// <param name="skillGroupRequest">The skill group request.</param>
        void UpdateSkillGroup(SkillGroup skillGroupRequest);
    }
}
