using Css.Api.Scheduling.Models.DTO.Request.Client;
using System;

namespace Css.Api.Scheduling.Models.Profiles.Client
{
    public class ClientProfile : AutoMapper.Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientProfile"/> class.
        /// </summary>
        public ClientProfile()
        {
            CreateMap<CreateClient, Domain.Client>()
                .ForMember(x => x.CreatedDate, opt => opt.MapFrom(o => DateTime.UtcNow)).ReverseMap();
            CreateMap<UpdateClient, Domain.Client>()
                .ForMember(x => x.ModifiedDate, opt => opt.MapFrom(o => DateTime.UtcNow)).ReverseMap().ReverseMap();
        }
    }
}
