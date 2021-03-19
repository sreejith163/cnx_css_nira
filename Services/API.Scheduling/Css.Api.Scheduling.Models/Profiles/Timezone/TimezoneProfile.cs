using Css.Api.Scheduling.Models.DTO.Request.Timezone;
using NoSQL = Css.Api.Core.Models.Domain.NoSQL;
using System;

namespace Css.Api.Scheduling.Models.Profiles.Timezone
{
    public class TimezoneProfile : AutoMapper.Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimezoneProfile" /> class.
        /// </summary>
        public TimezoneProfile()
        {
            CreateMap<CreateTimezone, NoSQL.Timezone>()
               .ForMember(x => x.Name, opt => opt.MapFrom(o => o.Name.Trim()))
               //.ForMember(x => x.CreatedBy, opt => opt.MapFrom(o => o.CreatedBy.Trim()))
               //.ForMember(x => x.CreatedDate, opt => opt.MapFrom(o => DateTime.UtcNow))
               .ReverseMap();

            CreateMap<UpdateTimezone, NoSQL.Timezone>()
                .ForMember(x => x.Name, opt => opt.MapFrom(o => o.Name.Trim()))
                //.ForMember(x => x.ModifiedBy, opt => opt.MapFrom(o => o.ModifiedBy.Trim()))
                //.ForMember(x => x.ModifiedDate, opt => opt.MapFrom(o => DateTime.UtcNow))
                .ReverseMap();
        }
    }
}
