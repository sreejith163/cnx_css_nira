using Css.Api.Scheduling.Models.DTO.Request.Client;
using System;

namespace Css.Api.Scheduling.Models.Profiles.Client
{
    public class UpdateClientProfile : AutoMapper.Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateClientProfile"/> class.
        /// </summary>
        public UpdateClientProfile()
        {
            CreateMap<UpdateClient, Domain.Client>()
                .ForMember(x => x.ModifiedDate, opt => opt.MapFrom(o => DateTime.UtcNow)).ReverseMap().ReverseMap();
        }
    }
}
