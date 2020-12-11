using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.SkillTag;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Css.Api.Scheduling.Repository.Interfaces
{
    public interface ISkillTagRepository
    {
        Task<SkillTagCollection> GetSkillTag(SkillTagIdDetails skillTagIdDetails);
        void CreateSkillTags(ICollection<SkillTagCollection> skillTagRequestCollection);
        Task<int> GetSkillTagsCount();
    }
}