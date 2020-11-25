using Css.Api.Setup.Models.DTO.Request.SkillTag;
using Css.Api.Setup.Models.DTO.Response.SkillTag;
using System;

namespace Css.Api.Setup.Models.Profiles.SkillTag
{
    public class SkillTagProfile : AutoMapper.Profile
    {
        /// <summary>Initializes a new instance of the <see cref="SkillTagProfile" /> class.</summary>
        public SkillTagProfile()
        {
            CreateMap<CreateSkillTag, Domain.SkillTag>()
                .ForMember(x => x.Name, opt => opt.MapFrom(o => o.Name.Trim()))
                .ForMember(x => x.CreatedBy, opt => opt.MapFrom(o => o.CreatedBy.Trim()))
                .ForMember(x => x.CreatedDate, opt => opt.MapFrom(o => DateTime.UtcNow))
                .ReverseMap();

            CreateMap<UpdateSkillTag, Domain.SkillTag>()
                .ForMember(x => x.Name, opt => opt.MapFrom(o => o.Name.Trim()))
                .ForMember(x => x.ModifiedBy, opt => opt.MapFrom(o => o.ModifiedBy.Trim()))
                .ForMember(x => x.ModifiedDate, opt => opt.MapFrom(o => DateTime.UtcNow))
                .ReverseMap();

            CreateMap<Domain.SkillTag, SkillTagDTO>()
                .ForMember(x => x.ClientName, opt => opt.MapFrom(o => o.Client != null ? o.Client.Name : ""))
                .ForMember(x => x.ClientLobGroupName, opt => opt.MapFrom(o => o.ClientLobGroup != null ? o.ClientLobGroup.Name : ""))
                .ForMember(x => x.SkillGroupName, opt => opt.MapFrom(o => o.SkillGroup != null ? o.SkillGroup.Name : ""))
                .ReverseMap();

            CreateMap<Domain.SkillTag, SkillTagDetailsDTO>()
               .ForMember(x => x.ClientName, opt => opt.MapFrom(o => o.Client != null ? o.Client.Name : ""))
               .ForMember(x => x.ClientLobGroupName, opt => opt.MapFrom(o => o.ClientLobGroup != null ? o.ClientLobGroup.Name : ""))
               .ForMember(x => x.SkillGroupName, opt => opt.MapFrom(o => o.SkillGroup != null ? o.SkillGroup.Name : ""))
               .ReverseMap();
        }
    }
}
