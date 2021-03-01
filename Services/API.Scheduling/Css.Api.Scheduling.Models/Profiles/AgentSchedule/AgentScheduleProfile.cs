using Css.Api.Scheduling.Models.DTO.Response.AgentSchedule;
using NoSQL = Css.Api.Core.Models.Domain.NoSQL;
using System;

namespace Css.Api.Scheduling.Models.Profiles.AgentSchedule
{
    public class AgentScheduleProfile : AutoMapper.Profile
    {
        /// <summary>Initializes a new instance of the <see cref="AgentScheduleProfile" /> class.</summary>
        public AgentScheduleProfile()
        {
            CreateMap<NoSQL.Agent, NoSQL.AgentSchedule>()
                .ForMember(x => x.EmployeeId, opt => opt.MapFrom(o => o.Ssn))
                .ForMember(x => x.FirstName, opt => opt.MapFrom(o => o.FirstName.Trim()))
                .ForMember(x => x.LastName, opt => opt.MapFrom(o => o.LastName.Trim()))
                .ForMember(x => x.CurrentAgentShedulingGroupId, opt => opt.MapFrom(o => o.AgentSchedulingGroupId))
                .ForMember(x => x.CreatedDate, opt => opt.MapFrom(o => DateTimeOffset.UtcNow))
                .ReverseMap();

            CreateMap<NoSQL.AgentSchedule, AgentScheduleDTO>()
                .ReverseMap();

            CreateMap<NoSQL.AgentSchedule, AgentScheduleDetailsDTO>()
                .ReverseMap();

            CreateMap<NoSQL.AgentSchedule, AgentScheduleChartDetailsDTO>()
                .ReverseMap();
        }
    }
}