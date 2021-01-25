using Css.Api.Scheduling.Models.DTO.Response.EntityHierarchy;
using NoSQL = Css.Api.Core.Models.Domain.NoSQL;

namespace Css.Api.Scheduling.Models.Profiles.EntityHierarchy
{
    public class EntityHierarchyProfile : AutoMapper.Profile
    {
        /// <summary>Initializes a new instance of the <see cref="EntityHierarchyProfile" /> class.</summary>
        public EntityHierarchyProfile()
        {
            CreateMap<ClientDTO, Domain.Client>()
                .ForMember(x => x.ClientId, opt => opt.MapFrom(o => o.Id))
                .ReverseMap();

            CreateMap<ClientLobDTO, Domain.ClientLobGroup>()
                .ForMember(x => x.ClientLobGroupId, opt => opt.MapFrom(o => o.Id))
                .ReverseMap();

            CreateMap<SkillGroupDTO, Domain.SkillGroup>()
               .ForMember(x => x.SkillGroupId, opt => opt.MapFrom(o => o.Id))
               .ReverseMap();

            CreateMap<SkillTagDTO, Domain.SkillTag>()
              .ForMember(x => x.SkillTagId, opt => opt.MapFrom(o => o.Id))
              .ReverseMap();

            CreateMap<AgentSchedulingGroupDTO, NoSQL.AgentSchedulingGroup>()
             .ForMember(x => x.AgentSchedulingGroupId, opt => opt.MapFrom(o => o.Id))
             .ReverseMap();
        }
    }
}
