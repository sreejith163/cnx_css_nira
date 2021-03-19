using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Core.Models.Enums;
using Css.Api.Reporting.Models.Profiles.ValueResolvers;
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
                .ForMember(x => x.FirstName, opt => opt.MapFrom(o => o.FirstName))
                .ForMember(x => x.LastName, opt => opt.MapFrom(o => o.LastName))
                .ForMember(x => x.ActiveAgentSchedulingGroupId, opt => opt.MapFrom(o => o.AgentSchedulingGroupId))
                .ForMember(x => x.CreatedBy, opt => opt.MapFrom(o => o.CreatedBy))
                .ForMember(x => x.CreatedDate, opt => opt.MapFrom(o => o.CreatedDate))
                .ForMember(x => x.CreatedAt, opt => opt.MapFrom(o => o.CreatedDate.Date))
                .ForMember(x => x.ModifiedBy, opt => opt.Ignore())
                .ForMember(x => x.ModifiedDate, opt => opt.Ignore())
                .ForMember(x => x.IsDeleted, opt => opt.MapFrom(o => false))
                .ForMember(x => x.Ranges, opt => opt.MapFrom(o => new List<AgentScheduleRange>()))
                ;
        }
    }
}
