using Css.Api.Core.Models.DTO.Response;
using Css.Api.Scheduling.Models.Domain;
using Css.Api.Scheduling.Models.DTO.Request.SchedulingCode;
using Css.Api.Scheduling.Models.DTO.Response.SchedulingCode;
using System;

namespace Css.Api.Scheduling.Models.Profiles.SchedulingCode
{
    public class SchedulingCodeProfile : AutoMapper.Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulingCodeProfile" /> class.
        /// </summary>
        public SchedulingCodeProfile()
        {
            CreateMap<CreateSchedulingCode, Domain.SchedulingCode>()
                .ForMember(x => x.CreatedDate, opt => opt.MapFrom(o => DateTime.UtcNow))
                .ReverseMap();

            CreateMap<UpdateSchedulingCode, Domain.SchedulingCode>()
                .ForMember(x => x.ModifiedDate, opt => opt.MapFrom(o => DateTime.UtcNow))
                .ReverseMap();

            CreateMap<Domain.SchedulingCode, SchedulingCodeDTO>()
                .ReverseMap();

            CreateMap<SchedulingCodeTypes, SchedulingTypeCode>()
                .ReverseMap();

            CreateMap<SchedulingCodeIcon, KeyValue>()
                .ForMember(x => x.Id, opt => opt.MapFrom(o => o.Id))
                .ForMember(x => x.Value, opt => opt.MapFrom(o => o.Value))
                .ReverseMap();

            CreateMap<SchedulingTypeCode, KeyValue>()
               .ForMember(x => x.Id, opt => opt.MapFrom(o => o.SchedulingCodeType.Id))
               .ForMember(x => x.Value, opt => opt.MapFrom(o => o.SchedulingCodeType.Value))
               .ReverseMap();

            CreateMap<SchedulingCodeType, KeyValue>()
                .ForMember(x => x.Id, opt => opt.MapFrom(o => o.Id))
                .ForMember(x => x.Value, opt => opt.MapFrom(o => o.Value))
                .ReverseMap();
        }
    }
}
