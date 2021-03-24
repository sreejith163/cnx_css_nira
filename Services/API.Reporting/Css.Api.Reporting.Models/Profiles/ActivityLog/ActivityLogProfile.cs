using Css.Api.Reporting.Models.Profiles.ValueResolvers;
using Css.Api.Reporting.Models.DTO.Processing;
using Domain = Css.Api.Core.Models.Domain.NoSQL;

namespace Css.Api.Reporting.Models.Profiles.ActivityLog
{
    public class ActivityLogProfile : AutoMapper.Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityLogProfile"/> class.
        /// </summary>
        public ActivityLogProfile()
        {
            CreateMap<ScheduleManagerDetails, Domain.ActivityLog>()
                .ForMember(x => x.EmployeeId, opt => opt.MapFrom(o => o.EmployeeId))
                .ForMember(x => x.CreatedAt, opt => opt.MapFrom(o => o.ModifiedDate ?? o.CreatedDate))
                .ForMember(x => x.ActivityOrigin, opt => opt.MapFrom(o => o.Origin))
                .ForMember(x => x.ActivityStatus, opt => opt.MapFrom(o => o.Status))
                .ForMember(x => x.ActivityType, opt => opt.MapFrom(o => o.Type))
                .ForMember(x => x.ExecutedBy, opt => opt.MapFrom(o => o.ModifiedBy ?? o.CreatedBy))
                .ForMember(x => x.ExecutedUser, opt => opt.Ignore())
                .ForMember(x => x.TimeStamp, opt => opt.MapFrom(o => o.ModifiedDate ?? o.CreatedDate))
                .ForMember(x => x.SchedulingFieldDetails, opt => opt.MapFrom<SchedulingFieldDetailsResolverSMD>());

            CreateMap<ScheduleData, Domain.ActivityLog>()
                .ForMember(x => x.EmployeeId, opt => opt.MapFrom(o => o.Schedule.EmployeeId))
                .ForMember(x => x.CreatedAt, opt => opt.MapFrom(o => o.Schedule.ModifiedDate.HasValue ? o.Schedule.ModifiedDate.Value.Date : o.Schedule.CreatedDate.Date))
                .ForMember(x => x.ActivityOrigin, opt => opt.MapFrom(o => o.Origin))
                .ForMember(x => x.ActivityStatus, opt => opt.MapFrom(o => o.Status))
                .ForMember(x => x.ActivityType, opt => opt.MapFrom(o => o.Type))
                .ForMember(x => x.ExecutedBy, opt => opt.MapFrom(o => o.Schedule.ModifiedBy ?? o.Schedule.CreatedBy))
                .ForMember(x => x.ExecutedUser, opt => opt.Ignore())
                .ForMember(x => x.TimeStamp, opt => opt.MapFrom(o => o.Schedule.ModifiedDate ?? o.Schedule.CreatedDate))
                .ForMember(x => x.SchedulingFieldDetails, opt => opt.MapFrom<SchedulingFieldDetailsResolverSD>());

        }
    }
}
