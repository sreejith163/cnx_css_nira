using Css.Api.Scheduling.Models.DTO.Request.SchedulingCode;
using System;

namespace Css.Api.Scheduling.Models.Profiles.SchedulingCode
{
    public class UpdateSchedulingCodeProfile : AutoMapper.Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateSchedulingCodeProfile"/> class.
        /// </summary>
        public UpdateSchedulingCodeProfile()
        {
            CreateMap<UpdateSchedulingCode, Domain.SchedulingCode>()
                .ForMember(x => x.ModifiedDate, opt => opt.MapFrom(o => DateTime.UtcNow)).ReverseMap().ReverseMap();
        }
    }
}
