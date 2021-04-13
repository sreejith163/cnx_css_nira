using Css.Api.Scheduling.Models.DTO.Response.AgentSchedule;
using NoSQL = Css.Api.Core.Models.Domain.NoSQL;
using System;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedule;

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
                .ForMember(x => x.ActiveAgentSchedulingGroupId, opt => opt.MapFrom(o => o.AgentSchedulingGroupId))
                .ForMember(x => x.CreatedDate, opt => opt.MapFrom(o => DateTimeOffset.UtcNow))
                .ReverseMap();

            CreateMap<NoSQL.AgentSchedule, AgentScheduleDTO>()
                .ReverseMap();

            CreateMap<NoSQL.AgentSchedule, AgentScheduleDetailsDTO>()
                .ReverseMap();

            CreateMap<NoSQL.AgentSchedule, AgentScheduleChartDetailsDTO>()
                .ReverseMap();

            CreateMap<NoSQL.AgentScheduleRange, AgentScheduleRangeDTO>()
                .ReverseMap();
            CreateMap<NoSQL.AgentSchedule, ExportAgentSchedulingGroupSchedule>()
               .ReverseMap();
           


        }
    }
}