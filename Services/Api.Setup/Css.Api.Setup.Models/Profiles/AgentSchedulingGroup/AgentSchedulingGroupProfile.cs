using Css.Api.Setup.Models.DTO.Request.AgentSchedulingGroup;
using Css.Api.Setup.Models.DTO.Response.AgentSchedulingGroup;
using System;

namespace Css.Api.Setup.Models.Profiles.AgentSchedulingGroup
{
    public class AgentSchedulingGroupProfile : AutoMapper.Profile
    {
        /// <summary>Initializes a new instance of the <see cref="AgentSchedulingGroupProfile" /> class.</summary>
        public AgentSchedulingGroupProfile()
        {
            CreateMap<CreateAgentSchedulingGroup, Domain.AgentSchedulingGroup>()
                .ForMember(x => x.Name, opt => opt.MapFrom(o => o.Name.Trim()))
                .ForMember(x => x.CreatedBy, opt => opt.MapFrom(o => o.CreatedBy.Trim()))
                .ForMember(x => x.CreatedDate, opt => opt.MapFrom(o => DateTime.UtcNow))
                .ReverseMap();

            CreateMap<UpdateAgentSchedulingGroup, Domain.AgentSchedulingGroup>()
                .ForMember(x => x.Name, opt => opt.MapFrom(o => o.Name.Trim()))
                .ForMember(x => x.ModifiedBy, opt => opt.MapFrom(o => o.ModifiedBy.Trim()))
                .ForMember(x => x.ModifiedDate, opt => opt.MapFrom(o => DateTime.UtcNow))
                .ReverseMap();

            CreateMap<Domain.AgentSchedulingGroup, AgentSchedulingGroupDTO>()
                .ForMember(x => x.ClientName, opt => opt.MapFrom(o => o.Client != null ? o.Client.Name : ""))
                .ForMember(x => x.ClientLobGroupName, opt => opt.MapFrom(o => o.ClientLobGroup != null ? o.ClientLobGroup.Name : ""))
                .ForMember(x => x.SkillGroupName, opt => opt.MapFrom(o => o.SkillGroup != null ? o.SkillGroup.Name : ""))
                .ForMember(x => x.SkillTagName, opt => opt.MapFrom(o => o.SkillTag != null ? o.SkillTag.Name : ""))
                .ForMember(x => x.TimezoneLabel, opt => opt.MapFrom(o => o.Timezone != null ? o.Timezone.Name : ""))
                .ReverseMap();


            CreateMap<Domain.AgentSchedulingGroup, AgentSchedulingGroupDetailsDTO>()
               .ForMember(x => x.ClientName, opt => opt.MapFrom(o => o.Client != null ? o.Client.Name : ""))
               .ForMember(x => x.ClientLobGroupName, opt => opt.MapFrom(o => o.ClientLobGroup != null ? o.ClientLobGroup.Name : ""))
               .ForMember(x => x.SkillGroupName, opt => opt.MapFrom(o => o.SkillGroup != null ? o.SkillGroup.Name : ""))
               .ForMember(x => x.SkillTagName, opt => opt.MapFrom(o => o.SkillTag != null ? o.SkillTag.Name : ""))
               .ForMember(x => x.TimezoneLabel, opt => opt.MapFrom(o => o.Timezone != null ? o.Timezone.Name : ""))
               .ReverseMap();
        }
    }
}

