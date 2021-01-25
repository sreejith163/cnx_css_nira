using Css.Api.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Domain = Css.Api.Core.Models.Domain.NoSQL;

namespace Css.Api.Reporting.Models.Profiles.AgentSchedule
{
    public class AgentScheduleProfile : AutoMapper.Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AgentScheduleProfile"/> class.
        /// </summary>
        public AgentScheduleProfile()
        {
            CreateMap<Domain.Agent, Domain.AgentSchedule>()
                .ForMember(x => x.EmployeeId, opt => opt.MapFrom(o => o.Ssn))
                .ForMember(x => x.CreatedBy, opt => opt.MapFrom(o => "UDW Import"))
                .ForMember(x => x.CreatedDate, opt => opt.MapFrom(o => DateTime.UtcNow))
                .ForMember(x => x.ModifiedBy, opt => opt.Ignore())
                .ForMember(x => x.ModifiedDate, opt => opt.Ignore())
                .ForMember(x => x.IsDeleted, opt => opt.MapFrom(o => false))
                .ForMember(x => x.AgentSchedulingGroupId, opt => opt.MapFrom(o => o.AgentSchedulingGroupId))
                .ForMember(x => x.Status, opt => opt.MapFrom(o => SchedulingStatus.Pending_Schedule));
        }
    }
}
