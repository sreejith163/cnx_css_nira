using System;
using Css.Api.Scheduling.Models.DTO.Request.TimeOff;

namespace Css.Api.Scheduling.Models.Profiles.AgentAdmin
{
    public class TimeOffProfile : AutoMapper.Profile
    {
        /// <summary>Initializes a new instance of the <see cref="TimeOffProfile" /> class.</summary>
        public TimeOffProfile()
        {
            CreateMap<CreateTimeOff, Domain.TimeOff>()
                .ForMember(x => x.Description, opt => opt.MapFrom(o => o.Description.Trim()))
                .ForMember(x => x.FTEDayLength, opt => opt.MapFrom(o => o.FTEDayLength.Trim()))
                .ForMember(x => x.CreatedBy, opt => opt.MapFrom(o => o.CreatedBy.Trim()))
                .ForMember(x => x.CreatedDate, opt => opt.MapFrom(o => DateTime.UtcNow))
                .ReverseMap();

            CreateMap<UpdateTimeOff, Domain.TimeOff>()
                .ForMember(x => x.Description, opt => opt.MapFrom(o => o.Description.Trim()))
                .ForMember(x => x.FTEDayLength, opt => opt.MapFrom(o => o.FTEDayLength.Trim()))
                .ForMember(x => x.ModifiedBy, opt => opt.MapFrom(o => o.ModifiedBy.Trim()))
                .ForMember(x => x.ModifiedDate, opt => opt.MapFrom(o => DateTime.UtcNow))
                .ReverseMap();
        }
    }
}