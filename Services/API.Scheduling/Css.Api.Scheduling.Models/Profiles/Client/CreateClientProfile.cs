using Css.Api.Scheduling.Models.DTO.Requests.Client;
using System;

namespace Css.Api.Scheduling.Models.Profiles.Client
{
    public class CreateClientProfile : AutoMapper.Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateClientProfile"/> class.
        /// </summary>
        public CreateClientProfile()
        {
            CreateMap<CreateClient, Domain.Client>()
                .ForMember(x => x.CreatedDate, opt => opt.MapFrom(o => DateTime.UtcNow)).ReverseMap();
        }
    }
}
