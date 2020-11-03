using Css.Api.Scheduling.Models.DTO.Response.SchedulingCodeIcon;

namespace Css.Api.Scheduling.Models.Profiles.SchedulingCodeIcon
{
    public class SchedulingCodeIconProfile : AutoMapper.Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulingCodeIconProfile" /> class.
        /// </summary>
        public SchedulingCodeIconProfile()
        {
            CreateMap<Domain.SchedulingCodeIcon, SchedulingCodeIconDTO>()
                .ForMember(x => x.Id, opt => opt.MapFrom(o => o.Id))
                .ForMember(x => x.Value, opt => opt.MapFrom(o => o.Value))
                .ReverseMap();
        }
    }
}
