using Css.Api.Core.Models.DTO.Response;

namespace Css.Api.Scheduling.Models.Profiles.Timezone
{
    public class TimezoneProfile : AutoMapper.Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimezoneProfile" /> class.
        /// </summary>
        public TimezoneProfile()
        {
            CreateMap<Domain.Timezone, KeyValue>()
               .ForMember(x => x.Id, opt => opt.MapFrom(o => o.Id))
               .ForMember(x => x.Value, opt => opt.MapFrom(o => o.DisplayName))
               .ReverseMap();
        }
    }
}
