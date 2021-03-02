using Css.Api.Scheduling.Models.DTO.Response.AgentSchedule;
using NoSQL = Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Scheduling.Models.Domain;

namespace Css.Api.Scheduling.Models.Profiles.ActivityLog
{
    public class AgentLogProfile : AutoMapper.Profile
    {
        /// <summary>Initializes a new instance of the <see cref="AgentLogProfile" /> class.</summary>
        public AgentLogProfile()
        {
            CreateMap<NoSQL.AgentScheduleRange, ActivityLogScheduleRange>()
                .ReverseMap();

            CreateMap<NoSQL.AgentScheduleManager, ActivityLogScheduleManager>()
                .ReverseMap();
        }
    }
}