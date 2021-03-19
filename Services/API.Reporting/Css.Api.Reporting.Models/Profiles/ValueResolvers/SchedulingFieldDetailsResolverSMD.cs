using AutoMapper;
using Css.Api.Reporting.Models.DTO.Processing;
using Css.Api.Core.Models.Domain.NoSQL;
using Domain = Css.Api.Core.Models.Domain.NoSQL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Models.Profiles.ValueResolvers
{
    public class SchedulingFieldDetailsResolverSMD : IValueResolver<ScheduleManagerDetails, Domain.ActivityLog, SchedulingFieldDetails>
    {
        public SchedulingFieldDetails Resolve(ScheduleManagerDetails source, Domain.ActivityLog destination, SchedulingFieldDetails destMember, ResolutionContext context)
        {
            return context.Mapper.Map<SchedulingFieldDetails>(new SchedulingFieldDetails()
            {
                
                ActivityLogManager = new ActivityLogScheduleManager
                {
                    AgentSchedulingGroupId  = source.AgentSchedulingGroupId,
                    Charts = source.Schedules,
                    Date = source.ScheduledDate
                }
            });
        }
    }
}
