using Css.Api.Core.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using Css.Api.Scheduling.Models.DTO.Response.AgentSchedule;
using Css.Api.Scheduling.Models.Enums;
using System;

namespace Css.Api.Scheduling.Models.Profiles.AgentSchedule
{
    public class AgentScheduleProfile : AutoMapper.Profile
    {
        /// <summary>Initializes a new instance of the <see cref="AgentScheduleProfile" /> class.</summary>
        public AgentScheduleProfile()
        {
            CreateMap<CreateAgentAdmin, Domain.AgentSchedule>()
                .ForMember(x => x.EmployeeId, opt => opt.MapFrom(o => o.EmployeeId))
                .ForMember(x => x.Status, opt => opt.MapFrom(o => SchedulingStatus.Approved))
                .ForMember(x => x.CreatedDate, opt => opt.MapFrom(o => DateTime.Now))
                .ReverseMap();

            CreateMap<Domain.AgentSchedule, AgentScheduleDTO>()
                .ReverseMap();

            CreateMap<Domain.AgentSchedule, AgentScheduleDetailsDTO>()
                .ReverseMap();

            CreateMap<PagedList<Entity>, AgentScheduleDTO>()
                .ReverseMap();
        }
    }
}