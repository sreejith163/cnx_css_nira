using Css.Api.Scheduling.Models.DTO.Response.AgentSchedule;
using NoSQL = Css.Api.Core.Models.Domain.NoSQL;

namespace Css.Api.Scheduling.Models.Profiles.ActivityLog
{
    public class AgentLogProfile : AutoMapper.Profile
    {
        /// <summary>Initializes a new instance of the <see cref="AgentLogProfile" /> class.</summary>
        public AgentLogProfile()
        {
            CreateMap<NoSQL.AgentScheduleRange, NoSQL.ActivityLogScheduleRange>()
                .ReverseMap();

            CreateMap<NoSQL.AgentScheduleManager, NoSQL.ActivityLogScheduleManager>()
                .ReverseMap();
        }
    }
}