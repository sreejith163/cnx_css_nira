using Css.Api.Core.Models.DTO.Response;
using Css.Api.Setup.Models.DTO.Request.SkillGroup;
using System.Threading.Tasks;

namespace Css.Api.Setup.Business.Interfaces
{
    /// <summary>Service for handling skill group related business logics</summary>
    public interface ISkillGroupService
    {
        /// <summary>Gets the skill groups.</summary>
        /// <param name="skillGroupQueryParameter">The skill group query parameter.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<CSSResponse> GetSkillGroups(SkillGroupQueryParameter skillGroupQueryParameter);

        /// <summary>
        /// Gets the skill group.
        /// </summary>
        /// <param name="skillIdDetails">The skill identifier details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<CSSResponse> GetSkillGroup(SkillGroupIdDetails skillIdDetails);

        /// <summary>
        /// Creates the skill group.
        /// </summary>
        /// <param name="skillDetails">The skill details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<CSSResponse> CreateSkillGroup(CreateSkillGroup skillDetails);

        /// <summary>
        /// Updates the skill group.
        /// </summary>
        /// <param name="skillIdDetails">The skill identifier details.</param>
        /// <param name="skillDetails">The skill details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<CSSResponse> UpdateSkillGroup(SkillGroupIdDetails skillIdDetails, UpdateSkillGroup skillDetails);

        /// <summary>Reverts the skill group.</summary>
        /// <param name="skillGroupIdDetails">The skill group identifier details.</param>
        /// <param name="skillGroupDetails">The skill group details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<CSSResponse> RevertSkillGroup(SkillGroupIdDetails skillGroupIdDetails, UpdateSkillGroup skillGroupDetails);

        /// <summary>
        /// Deletes the skill group.
        /// </summary>
        /// <param name="skillIdDetails">The skill identifier details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<CSSResponse> DeleteSkillGroup(SkillGroupIdDetails skillIdDetails);
    }
}