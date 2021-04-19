using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Scheduling.Models.DTO.Request.AgentCategoryValueView;
using Css.Api.Scheduling.Models.DTO.Response.AgentCategoryValueView;

namespace Css.Api.Scheduling.Models.Profiles.AgentCategoryValue
{
    public class AgentCategoryValueProfile : AutoMapper.Profile
    {
        /// <summary>Initializes a new instance of the <see cref="AgentCategoryValueProfile" /> class.</summary>
        public AgentCategoryValueProfile()
        {
            CreateMap<Agent, AgentCategoryValueDTO>()
                .ForMember(x => x.EmployeeId, opt => opt.MapFrom(o => o.Ssn))
                .ReverseMap();


            CreateMap<AgentCategory, AgentCategoryGenericValidator>()
              .ReverseMap();

               CreateMap<AgentSchedulingGroup, AgentSchedulingGroupList>()
              .ReverseMap();
        }
    }
}
