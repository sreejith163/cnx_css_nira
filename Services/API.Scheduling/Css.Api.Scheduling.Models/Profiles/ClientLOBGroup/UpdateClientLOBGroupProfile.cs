using Css.Api.Scheduling.Models.DTO.Request.ClientLOBGroup;
using System;

namespace Css.Api.Scheduling.Models.Profiles.ClientLOBGroup
{
    public class UpdateClientLOBGroupProfile : AutoMapper.Profile
    {

        /// <summary>Initializes a new instance of the <see cref="UpdateClientLOBGroupProfile" /> class.</summary>
        public UpdateClientLOBGroupProfile()
        {
            CreateMap<UpdateClientLOBGroup, Domain.ClientLobGroup>()
                .ForMember(x => x.ModifiedDate, opt => opt.MapFrom(o => DateTime.UtcNow)).ReverseMap().ReverseMap();
        }
    }
}

