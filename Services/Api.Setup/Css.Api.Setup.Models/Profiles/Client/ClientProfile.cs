using Css.Api.Core.Models.DTO.Response;
using Css.Api.Setup.Models.DTO.Request.Client;
using Css.Api.Setup.Models.DTO.Response.Client;
using System;

namespace Css.Api.Setup.Models.Profiles.Client
{
    public class ClientProfile : AutoMapper.Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientProfile"/> class.
        /// </summary>
        public ClientProfile()
        {
            CreateMap<CreateClient, Domain.Client>()
                .ForMember(x => x.Name, opt => opt.MapFrom(o => o.Name.Trim()))
                .ForMember(x => x.CreatedBy, opt => opt.MapFrom(o => o.CreatedBy.Trim()))
                .ForMember(x => x.CreatedDate, opt => opt.MapFrom(o => DateTime.UtcNow))
                .ReverseMap();

            CreateMap<UpdateClient, Domain.Client>()
                .ForMember(x => x.Name, opt => opt.MapFrom(o => o.Name.Trim()))
                .ForMember(x => x.ModifiedBy, opt => opt.MapFrom(o => o.ModifiedBy.Trim()))
                .ForMember(x => x.ModifiedDate, opt => opt.MapFrom(o => o.ModifiedDate.HasValue? o.ModifiedDate: DateTime.UtcNow))
                .ReverseMap();

            CreateMap<Domain.Client, ClientDTO>()
                .ReverseMap();

            CreateMap<Domain.Client, KeyValue>()
               .ForMember(x => x.Value, opt => opt.MapFrom(o => o.Name))
               .ReverseMap();
        }
    }
}
