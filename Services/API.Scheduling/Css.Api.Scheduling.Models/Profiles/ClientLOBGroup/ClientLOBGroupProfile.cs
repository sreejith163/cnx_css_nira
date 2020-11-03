﻿using Css.Api.Scheduling.Models.DTO.Request.ClientLOBGroup;
using Css.Api.Scheduling.Models.DTO.Response.ClientLOBGroup;
using System;

namespace Css.Api.Scheduling.Models.Profiles.ClientLOBGroup
{
    public class ClientLOBGroupProfile : AutoMapper.Profile
    {
        /// <summary>Initializes a new instance of the <see cref="ClientLOBGroupProfile" /> class.</summary>
        public ClientLOBGroupProfile()
        {
            CreateMap<CreateClientLOBGroup, Domain.ClientLobGroup>()
                .ForMember(x => x.CreatedDate, opt => opt.MapFrom(o => DateTime.UtcNow))
                .ReverseMap();

            CreateMap<UpdateClientLOBGroup, Domain.ClientLobGroup>()
                .ForMember(x => x.ModifiedDate, opt => opt.MapFrom(o => DateTime.UtcNow))
                .ReverseMap();

            CreateMap<Domain.ClientLobGroup, ClientLOBGroupDTO>()
                .ReverseMap();
        }
    }
}