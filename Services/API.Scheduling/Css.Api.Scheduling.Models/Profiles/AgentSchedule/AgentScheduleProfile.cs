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
                .ForMember(x => x.AgentScheduleCharts, opt => opt.Condition(o => o.AgentScheduleType == AgentScheduleType.SchedulingTab))
                .ForMember(x => x.AgentScheduleManager, opt => opt.Condition(o => o.AgentScheduleType == AgentScheduleType.SchedulingMangerTab))
                .ForMember(x => x.ModifiedDate, opt => opt.MapFrom(o => DateTime.UtcNow))
                .ReverseMap();

            CreateMap<ImportAgentSchedule, Domain.AgentSchedule>()
                .ForMember(x => x.AgentScheduleCharts, opt => opt.Condition(o => o.AgentScheduleType == AgentScheduleType.SchedulingTab))
                .ForMember(x => x.AgentScheduleManager, opt => opt.Condition(o => o.AgentScheduleType == AgentScheduleType.SchedulingMangerTab))
                .ForMember(x => x.ModifiedDate, opt => opt.MapFrom(o => DateTime.UtcNow))
                .ReverseMap();
        }
    }
}