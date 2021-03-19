using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Reporting.Models.DTO.Processing;
using System;
using System.Collections.Generic;
using System.Text;
using Domain = Css.Api.Core.Models.Domain.NoSQL;

namespace Css.Api.Reporting.Models.Profiles.AgentScheduleManager
{
    public class AgentScheduleManagerProfile : AutoMapper.Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AgentScheduleManagerProfile"/> class.
        /// </summary>
        public AgentScheduleManagerProfile()
        {
            CreateMap<ScheduleManagerDetails, Domain.AgentScheduleManager>()
                .ForMember(x => x.EmployeeId, opt => opt.MapFrom(o => o.EmployeeId))
                .ForMember(x => x.AgentSchedulingGroupId, opt => opt.MapFrom(o => o.AgentSchedulingGroupId))
                .ForMember(x => x.CreatedAt, opt => opt.MapFrom(o => o.CreatedDate))
                .ForMember(x => x.CreatedBy, opt => opt.MapFrom(o => o.CreatedBy))
                .ForMember(x => x.CreatedDate, opt => opt.MapFrom(o => o.CreatedDate))
                .ForMember(x => x.Charts, opt => opt.MapFrom(o => o.Schedules))
                .ForMember(x => x.Date, opt => opt.MapFrom(o => o.ScheduledDate))
                .ForMember(x => x.ModifiedBy, opt => opt.MapFrom(o => o.ModifiedBy))
                .ForMember(x => x.ModifiedDate, opt => opt.MapFrom(o => o.ModifiedDate));
        }
    }
}
