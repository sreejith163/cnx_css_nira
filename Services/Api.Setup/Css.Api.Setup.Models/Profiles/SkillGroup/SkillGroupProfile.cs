using Css.Api.Setup.Models.DTO.Request.SkillGroup;
using Css.Api.Setup.Models.DTO.Response.SkillGroup;
using System;

namespace Css.Api.Setup.Models.Profiles.SkillGroup
{
    public class SkillGroupProfile : AutoMapper.Profile
    {
        /// <summary>Initializes a new instance of the <see cref="SkillGroupProfile" /> class.</summary>
        public SkillGroupProfile()
        {
            CreateMap<CreateSkillGroup, Domain.SkillGroup>()
                .ForMember(x => x.Name, opt => opt.MapFrom(o => o.Name.Trim()))
                .ForMember(x => x.CreatedBy, opt => opt.MapFrom(o => o.CreatedBy.Trim()))
                .ForMember(x => x.CreatedDate, opt => opt.MapFrom(o => DateTime.UtcNow))
                .ReverseMap();

            CreateMap<UpdateSkillGroup, Domain.SkillGroup>()
                .ForMember(x => x.Name, opt => opt.MapFrom(o => o.Name.Trim()))
                .ForMember(x => x.ModifiedBy, opt => opt.MapFrom(o => o.ModifiedBy.Trim()))
                .ForMember(x => x.ModifiedDate, opt => opt.MapFrom(o => DateTime.UtcNow))
                .ReverseMap();

            CreateMap<Domain.SkillGroup, SkillGroupDTO>()
                .ForMember(x => x.ClientName, opt => opt.MapFrom(o => o.Client.Name ?? ""))
                .ForMember(x => x.ClientLobGroupName, opt => opt.MapFrom(o => o.ClientLobGroup.Name ?? ""))
                .ForMember(x => x.TimezoneLabel, opt => opt.MapFrom(o => o.Timezone.Name ?? ""))
                .ReverseMap();

            CreateMap<Domain.SkillGroup, SkillGroupDetailsDTO>()
               .ForMember(x => x.ClientName, opt => opt.MapFrom(o => o.Client.Name ?? ""))
               .ForMember(x => x.ClientLobGroupName, opt => opt.MapFrom(o => o.ClientLobGroup.Name ?? ""))
               .ForMember(x => x.TimezoneLabel, opt => opt.MapFrom(o => o.Timezone.Name ?? ""))
               .ReverseMap();
        }
    }
}
