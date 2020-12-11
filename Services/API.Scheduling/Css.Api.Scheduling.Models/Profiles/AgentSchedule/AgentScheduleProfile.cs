using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using Css.Api.Scheduling.Models.Enums;

namespace Css.Api.Scheduling.Models.Profiles.AgentAdmin
{
    public class AgentScheduleProfile : AutoMapper.Profile
    {
        /// <summary>Initializes a new instance of the <see cref="AgentScheduleProfile" /> class.</summary>
        public AgentScheduleProfile()
        {
            CreateMap<CreateAgentAdmin, Domain.AgentSchedule>()
                .ForMember(x => x.EmployeeId, opt => opt.MapFrom(o => o.EmployeeId.Trim()))
                .ForMember(x => x.Status, opt => opt.MapFrom(o => SchedulingStatus.Approved))
                .ReverseMap();

        }
    }
}