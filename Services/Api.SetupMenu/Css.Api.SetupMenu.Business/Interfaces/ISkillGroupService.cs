using Css.Api.Core.Models.DTO.Response;
using Css.Api.SetupMenu.Models.DTO.Request.SkillGroup;
using System.Threading.Tasks;

namespace Css.Api.SetupMenu.Business.Interfaces
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
        ///   <para>
        /// Gets the skill group.
        /// </para>
        /// </summary>
        /// <param name="skillIdDetails">The skill identifier details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<CSSResponse> GetSkillGroup(SkillGroupIdDetails skillIdDetails);

        /// <summary>Creates the skill group.</summary>
        /// <param name="skillDetails">The skill details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<CSSResponse> CreateSkillGroup(CreateSkillGroup skillDetails);

        /// <summary>Updates the skill group.</summary>
        /// <param name="skillIdDetails">The skill identifier details.</param>
        /// <param name="skillDetails">The skill details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<CSSResponse> UpdateSkillGroup(SkillGroupIdDetails skillIdDetails, UpdateSkillGroup skillDetails);

        /// <summary>Deletes the skill group.</summary>
        /// <param name="skillIdDetails">The skill identifier details.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        Task<CSSResponse> DeleteSkillGroup(SkillGroupIdDetails skillIdDetails);
    }
}