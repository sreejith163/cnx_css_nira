using System;
using System.Collections.Generic;
using System.Text;
using Domain = Css.Api.Core.Models.Domain.NoSQL;

namespace Css.Api.Reporting.Models.Profiles.AgentSchedulingGroupHistory
{
    public class AgentSchedulingGroupHistoryProfile : AutoMapper.Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AgentSchedulingGroupHistoryProfile"/> class.
        /// </summary>
        public AgentSchedulingGroupHistoryProfile()
        {
            CreateMap<Domain.Agent, Domain.AgentSchedulingGroupHistory>()
                .ForMember(x => x.EmployeeId, opt => opt.MapFrom(o => o.Ssn))
                .ForMember(x => x.AgentSchedulingGroupId, opt => opt.MapFrom(o => o.AgentSchedulingGroupId))
                .ForMember(x => x.CreatedAt, opt => opt.MapFrom(o => o.CreatedAt))
                .ForMember(x => x.CreatedBy, opt => opt.MapFrom(o => o.CreatedBy))
                .ForMember(x => x.CreatedDate, opt => opt.MapFrom(o => o.CreatedDate))
                .ForMember(x => x.ModifiedBy, opt => opt.MapFrom(o => o.ModifiedBy))
                .ForMember(x => x.ModifiedDate, opt => opt.MapFrom(o => o.ModifiedDate))
                .ForMember(x => x.StartDate, opt => opt.MapFrom(o => o.ModifiedDate.HasValue ? o.ModifiedDate.Value.DateTime : o.CreatedDate.DateTime))
                .ForMember(x => x.ActivityOrigin, opt => opt.MapFrom(o => o.Origin));
        }
    }
}
