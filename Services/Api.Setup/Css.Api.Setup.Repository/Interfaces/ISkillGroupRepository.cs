using Css.Api.Core.Models.Domain;
using Css.Api.Setup.Models.Domain;
using Css.Api.Setup.Models.DTO.Request.ClientLOBGroup;
using Css.Api.Setup.Models.DTO.Request.SkillGroup;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Css.Api.Setup.Repository.Interfaces
{
    public interface ISkillGroupRepository
    {
        /// <summary>
        /// Gets the skill groups.
        /// </summary>
        /// <param name="skillGroupQueryParameter">The skill group query parameter.</param>
        /// <returns></returns>
        Task<PagedList<Entity>> GetSkillGroups(SkillGroupQueryParameter skillGroupQueryParameter);

        /// <summary>
        /// Gets the skill group.
        /// </summary>
        /// <param name="skillGroupIdDetails">The skill group identifier details.</param>
        /// <returns></returns>
        Task<SkillGroup> GetSkillGroup(SkillGroupIdDetails skillGroupIdDetails);

        /// <summary>Gets all skill group.</summary>
        /// <param name="skillGroupIdDetails">The skill group identifier details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<SkillGroup> GetAllSkillGroup(SkillGroupIdDetails skillGroupIdDetails);

        /// <summary>
        /// Gets the skill groups count by client lob identifier.
        /// </summary>
        /// <param name="clientLOBGroupIdDetails">The client lob group identifier details.</param>
        /// <returns></returns>
        Task<int> GetSkillGroupsCountByClientLobId(ClientLOBGroupIdDetails clientLOBGroupIdDetails);

        /// <summary>
        /// Gets the name of the skill group ids by client lob identifier and skill group.
        /// </summary>
        /// <param name="clientLOBGroupIdDetails">The client lob group identifier details.</param>
        /// <param name="skillGroupNameDetails">The skill group name details.</param>
        /// <returns></returns>
        Task<List<int>> GetSkillGroupIdsByClientLobIdAndSkillGroupName(ClientLOBGroupIdDetails clientLOBGroupIdDetails,
                    SkillGroupNameDetails skillGroupNameDetails);


        /// <summary>Gets the name of all skill group ids by client lob identifier and skill group.</summary>
        /// <param name="clientLOBGroupIdDetails">The client lob group identifier details.</param>
        /// <param name="skillGroupNameDetails">The skill group name details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<List<int>> GetAllSkillGroupIdsByClientLobIdAndSkillGroupName(ClientLOBGroupIdDetails clientLOBGroupIdDetails,
                   SkillGroupNameDetails skillGroupNameDetails);

        /// <summary>
        /// Creates the skill group.
        /// </summary>
        /// <param name="skillGroup">The skill group.</param>
        void CreateSkillGroup(SkillGroup skillGroup);

        /// <summary>
        /// Updates the skill group.
        /// </summary>
        /// <param name="skillGroup">The skill group.</param>
        void UpdateSkillGroup(SkillGroup skillGroup);

        /// <summary>
        /// Deletes the skill group.
        /// </summary>
        /// <param name="skillGroup">The skill group.</param>
        void DeleteSkillGroup(SkillGroup skillGroup);
    }
}
