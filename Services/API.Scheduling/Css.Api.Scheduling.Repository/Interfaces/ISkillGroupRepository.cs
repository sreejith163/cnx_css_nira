using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.SkillGroup;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Css.Api.Scheduling.Repository.Interfaces
{
    public interface ISkillGroupRepository
    {
        Task<SkillGroupCollection> GetSkillGroup(SkillGroupIdDetails skillGroupIdDetails);
        void CreateSkillGroups(ICollection<SkillGroupCollection> skillGroupRequestCollection);
        Task<int> GetSkillGroupsCount();
    }
}
