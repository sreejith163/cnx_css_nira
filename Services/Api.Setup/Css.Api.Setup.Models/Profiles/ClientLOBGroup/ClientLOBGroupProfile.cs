using Css.Api.Setup.Models.DTO.Request.ClientLOBGroup;
using Css.Api.Setup.Models.DTO.Response.ClientLOBGroup;
using System;

namespace Css.Api.Setup.Models.Profiles.ClientLOBGroup
{
    public class ClientLOBGroupProfile : AutoMapper.Profile
    {
        /// <summary>Initializes a new instance of the <see cref="ClientLOBGroupProfile" /> class.</summary>
        public ClientLOBGroupProfile()
        {
            CreateMap<CreateClientLOBGroup, Domain.ClientLobGroup>()
                .ForMember(x => x.Name, opt => opt.MapFrom(o => o.Name.Trim()))
                .ForMember(x => x.CreatedBy, opt => opt.MapFrom(o => o.CreatedBy.Trim()))
                .ForMember(x => x.CreatedDate, opt => opt.MapFrom(o => DateTime.UtcNow))
                .ReverseMap();

            CreateMap<UpdateClientLOBGroup, Domain.ClientLobGroup>()
                .ForMember(x => x.Name, opt => opt.MapFrom(o => o.Name.Trim()))
                .ForMember(x => x.ModifiedBy, opt => opt.MapFrom(o => o.ModifiedBy.Trim()))
                .ForMember(x => x.ModifiedDate, opt => opt.MapFrom(o => o.ModifiedDate.HasValue? o.ModifiedDate: DateTime.UtcNow))
                .ReverseMap();

            CreateMap<Domain.ClientLobGroup, ClientLOBGroupDTO>()
                .ForMember(x => x.ClientName, opt => opt.MapFrom(o => o.Client != null ? o.Client.Name : ""))
                .ForMember(x => x.TimezoneLabel, opt => opt.MapFrom(o => o.Timezone != null ? o.Timezone.Name : ""))
                .ReverseMap();
        }
    }
}
