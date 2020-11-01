using Css.Api.Scheduling.Models.DTO.Request.SchedulingCode;
using System;

namespace Css.Api.Scheduling.Models.Profiles.SchedulingCode
{
    public class CreateSchedulingCodeProfile : AutoMapper.Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateSchedulingCodeProfile" /> class.
        /// </summary>
        public CreateSchedulingCodeProfile()
        {
            CreateMap<CreateSchedulingCode, Domain.SchedulingCode>()
                .ForMember(x => x.CreatedDate, opt => opt.MapFrom(o => DateTime.UtcNow)).ReverseMap();
        }
    }
}
