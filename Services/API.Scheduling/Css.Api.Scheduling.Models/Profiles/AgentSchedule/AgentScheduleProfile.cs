using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using Css.Api.Scheduling.Models.Enums;
using System;

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
                .ForMember(x => x.CreatedDate, opt => opt.MapFrom(o => DateTime.UtcNow))
                .ReverseMap();

            CreateMap<UpdateAgentSchedule, Domain.AgentSchedule>()
                .ForMember(x => x.AgentScheduleCharts, opt => opt.MapFrom(o => o.AgentScheduleType == AgentScheduleType.SchedulingTab ? o.AgentScheduleCharts : null))
                .ForMember(x => x.AgentScheduleManager, opt => opt.MapFrom(o => o.AgentScheduleType == AgentScheduleType.SchedulingMangerTab ? o.AgentScheduleManager : null))
                .ForMember(x => x.ModifiedDate, opt => opt.MapFrom(o => DateTime.UtcNow))
                .ReverseMap();
        }
    }
}