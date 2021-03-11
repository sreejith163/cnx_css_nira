using NoSQL = Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Scheduling.Models.DTO.Request.AgentScheduleManager;
using Css.Api.Scheduling.Models.DTO.Response.AgentScheduleManager;
using Css.Api.Scheduling.Models.DTO.Request.AgentAdmin;
using Css.Api.Core.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.AgentSchedulingGroup;

namespace Css.Api.Scheduling.Models.Profiles.AgentSchedule
{
    public class AgentScheduleManagerProfile : AutoMapper.Profile
    {
        /// <summary>Initializes a new instance of the <see cref="AgentScheduleManagerProfile" /> class.</summary>
        public AgentScheduleManagerProfile()
        {
            CreateMap<NoSQL.AgentScheduleManager, AgentScheduleManagerChartDTO>()
                .ReverseMap();

            CreateMap<NoSQL.AgentScheduleManager, AgentScheduleManagerChartDetailsDTO>()
                .ReverseMap();

            CreateMap<NoSQL.AgentScheduleManager, AgentSchedulingGroupIdDetails>()
             .ReverseMap();
            CreateMap<AgentScheduleManagerChartQueryparameter, AgentAdminQueryParameter>()
                .ReverseMap();
        }
    }
}