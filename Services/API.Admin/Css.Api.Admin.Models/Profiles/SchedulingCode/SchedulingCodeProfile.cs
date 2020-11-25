using Css.Api.Core.Models.DTO.Response;
using Css.Api.Admin.Models.Domain;
using Css.Api.Admin.Models.DTO.Request.SchedulingCode;
using Css.Api.Admin.Models.DTO.Response.SchedulingCode;
using System;

namespace Css.Api.Admin.Models.Profiles.SchedulingCode
{
    public class SchedulingCodeProfile : AutoMapper.Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulingCodeProfile" /> class.
        /// </summary>
        public SchedulingCodeProfile()
        {
            CreateMap<CreateSchedulingCode, Domain.SchedulingCode>()
                .ForMember(x => x.Description, opt => opt.MapFrom(o => o.Description.Trim()))
                .ForMember(x => x.CreatedBy, opt => opt.MapFrom(o => o.CreatedBy.Trim()))
                .ForMember(x => x.CreatedDate, opt => opt.MapFrom(o => DateTime.UtcNow))
                .ReverseMap();

            CreateMap<UpdateSchedulingCode, Domain.SchedulingCode>()
                .ForMember(x => x.Description, opt => opt.MapFrom(o => o.Description.Trim()))
                .ForMember(x => x.ModifiedBy, opt => opt.MapFrom(o => o.ModifiedBy.Trim()))
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
