using AutoMapper;
using Css.Api.Reporting.Models.DTO.Processing;
using Css.Api.Core.Models.Domain.NoSQL;
using Domain = Css.Api.Core.Models.Domain.NoSQL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Models.Profiles.ValueResolvers
{
    public class SchedulingFieldDetailsResolverSD : IValueResolver<ScheduleData, Domain.ActivityLog, SchedulingFieldDetails>
    {
        public SchedulingFieldDetails Resolve(ScheduleData source, Domain.ActivityLog destination, SchedulingFieldDetails destMember, ResolutionContext context)
        {
            return context.Mapper.Map<SchedulingFieldDetails>(new SchedulingFieldDetails()
            {

                ActivityLogManager = new ActivityLogScheduleManager
                {
                    AgentSchedulingGroupId = source.Schedule.AgentSchedulingGroupId,
                    Charts = source.Schedule.Charts,
                    Date = source.Schedule.Date
                }
            });
        }
    }
}
