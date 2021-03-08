using Css.Api.Core.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Response.AgentSchedule;
using Css.Api.Core.Models.Enums;
using NoSQL = Css.Api.Core.Models.Domain.NoSQL;
using System;
using System.Linq;

namespace Css.Api.Scheduling.Models.Profiles.AgentSchedule
{
    public class AgentScheduleProfile : AutoMapper.Profile
    {
        /// <summary>Initializes a new instance of the <see cref="AgentScheduleProfile" /> class.</summary>
        public AgentScheduleProfile()
        {
            CreateMap<NoSQL.Agent, NoSQL.AgentSchedule>()
                .ForMember(x => x.EmployeeId, opt => opt.MapFrom(o => o.Ssn))
                .ForMember(x => x.Status, opt => opt.MapFrom(o => SchedulingStatus.Approved))
                .ForMember(x => x.CreatedDate, opt => opt.MapFrom(o => DateTimeOffset.UtcNow))
                .ReverseMap();

            CreateMap<NoSQL.AgentSchedule, AgentScheduleDTO>()
                .ReverseMap();

            CreateMap<NoSQL.AgentSchedule, AgentScheduleDetailsDTO>()
                .ReverseMap();

            CreateMap<NoSQL.AgentSchedule, AgentScheduleChartDetailsDTO>()
                .ReverseMap();

            CreateMap<NoSQL.AgentSchedule, AgentScheduleManagerChartDetailsDTO>()
                .ForMember(x => x.AgentScheduleChart, opt => opt.MapFrom(o => o.AgentScheduleCharts.ToList().FirstOrDefault()))
                .ReverseMap();

            CreateMap<PagedList<Entity>, AgentScheduleDTO>()
                .ReverseMap();
        }
    }
}