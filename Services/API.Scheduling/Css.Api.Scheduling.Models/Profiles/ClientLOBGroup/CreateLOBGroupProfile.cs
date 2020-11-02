using Css.Api.Scheduling.Models.DTO.Request.ClientLOBGroup;
using System;

namespace Css.Api.Scheduling.Models.Profiles.ClientLOBGroup
{
    public class CreateLOBGroupProfile : AutoMapper.Profile
    {
        /// <summary>Initializes a new instance of the <see cref="CreateLOBGroupProfile" /> class.</summary>
        public CreateLOBGroupProfile()
        {
            CreateMap<CreateClientLOBGroup, Domain.ClientLobGroup>()
                .ForMember(x => x.CreatedDate, opt => opt.MapFrom(o => DateTime.UtcNow)).ReverseMap();
        }
    }
}
