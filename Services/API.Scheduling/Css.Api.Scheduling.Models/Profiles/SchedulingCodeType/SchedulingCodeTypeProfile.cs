using Css.Api.Scheduling.Models.DTO.Response.SchedulingCodeIcon;

namespace Css.Api.Scheduling.Models.Profiles.SchedulingCodeIcon
{
    public class SchedulingCodeType : AutoMapper.Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulingCodeType" /> class.
        /// </summary>
        public SchedulingCodeType()
        {
            CreateMap<Domain.SchedulingCodeType, SchedulingCodeTypeDTO>()
                .ForMember(x => x.Id, opt => opt.MapFrom(o => o.Id))
                .ForMember(x => x.Value, opt => opt.MapFrom(o => o.Value))
                .ReverseMap();
        }
    }
}
