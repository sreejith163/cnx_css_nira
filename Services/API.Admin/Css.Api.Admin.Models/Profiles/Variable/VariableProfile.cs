﻿using Css.Api.Admin.Models.Domain;
using Css.Api.Admin.Models.DTO.Response.SchedulingCode;
using Css.Api.Core.Models.DTO.Response;

namespace Css.Api.Admin.Models.Profiles.Variable
{
    public class VariableProfile : AutoMapper.Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VariableProfile"/> class.
        /// </summary>
        public VariableProfile()
        {
            CreateMap<CssVariable, VariableDTO>()
                .ForMember(x => x.Value, opt => opt.MapFrom(o => o.Name))
                .ForMember(x => x.Description, opt => opt.MapFrom(o => o.Description))
                .ForMember(x => x.MenuName, opt => opt.MapFrom(o => o.Menu != null ? o.Menu.Name : ""))
                .ReverseMap();

            CreateMap<CssVariable, KeyValue>()
               .ForMember(x => x.Value, opt => opt.MapFrom(o => o.Name))
               .ReverseMap();
        }
    }
}
