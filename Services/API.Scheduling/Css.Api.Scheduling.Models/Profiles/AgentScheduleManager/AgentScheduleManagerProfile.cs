using NoSQL = Css.Api.Core.Models.Domain.NoSQL;
using System;
using Css.Api.Scheduling.Models.DTO.Request.AgentScheduleManager;
using Css.Api.Scheduling.Models.DTO.Response.AgentScheduleManager;
using Css.Api.Core.Models.Domain;

namespace Css.Api.Scheduling.Models.Profiles.AgentSchedule
{
    public class AgentScheduleManagerProfile : AutoMapper.Profile
    {
        /// <summary>Initializes a new instance of the <see cref="AgentScheduleManagerProfile" /> class.</summary>
        public AgentScheduleManagerProfile()
        {
            CreateMap<NoSQL.Agent, NoSQL.AgentScheduleManager>()
                .ForMember(x => x.EmployeeId, opt => opt.MapFrom(o => o.Ssn))
                .ForMember(x => x.FirstName, opt => opt.MapFrom(o => o.FirstName.Trim()))
                .ForMember(x => x.LastName, opt => opt.MapFrom(o => o.LastName.Trim()))
                .ForMember(x => x.CurrentAgentShedulingGroupId, opt => opt.MapFrom(o => o.AgentSchedulingGroupId))
                .ForMember(x => x.CreatedDate, opt => opt.MapFrom(o => DateTimeOffset.UtcNow))
                .ReverseMap();

            CreateMap<NoSQL.AgentScheduleManager, AgentScheduleManagerDTO>()
                .ReverseMap();

            CreateMap<NoSQL.AgentScheduleManager, AgentScheduleManagerChartDetailsDTO>()
                .ReverseMap();

            CreateMap<PagedList<Entity>, AgentScheduleManagerChartDetailsDTO>()
                .ReverseMap();
        }
    }
}