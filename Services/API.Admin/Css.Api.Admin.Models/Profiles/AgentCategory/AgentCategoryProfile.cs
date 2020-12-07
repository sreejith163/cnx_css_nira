using Css.Api.Admin.Models.DTO.Request.AgentCategory;
using Css.Api.Admin.Models.DTO.Response.AgentCategory;
using System;

namespace Css.Api.Admin.Models.Profiles.AgentCategory
{
    public class AgentCategoryProfile : AutoMapper.Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AgentCategoryProfile"/> class.
        /// </summary>
        public AgentCategoryProfile()
        {
            CreateMap<CreateAgentCategory, Domain.AgentCategory>()
                .ForMember(x => x.Name, opt => opt.MapFrom(o => o.Name.Trim()))
                .ForMember(x => x.CreatedBy, opt => opt.MapFrom(o => o.CreatedBy.Trim()))
                .ForMember(x => x.CreatedDate, opt => opt.MapFrom(o => DateTime.UtcNow))
                .ReverseMap();

            CreateMap<UpdateAgentCategory, Domain.AgentCategory>()
                .ForMember(x => x.Name, opt => opt.MapFrom(o => o.Name.Trim()))
                .ForMember(x => x.ModifiedBy, opt => opt.MapFrom(o => o.ModifiedBy.Trim()))
                .ForMember(x => x.ModifiedDate, opt => opt.MapFrom(o => DateTime.UtcNow))
                .ReverseMap();

            CreateMap<Domain.AgentCategory, AgentCategoryDTO>()
                .ForMember(x => x.DataTypeLabel, opt => opt.MapFrom(o => o.DataType != null ? o.DataType.Value : ""))
                .ReverseMap();
        }
    }
}

