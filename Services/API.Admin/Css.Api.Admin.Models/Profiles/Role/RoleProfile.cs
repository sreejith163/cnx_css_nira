using Css.Api.Admin.Models.DTO.Request.Role;
using Css.Api.Admin.Models.DTO.Response.Role;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Admin.Models.Profiles.Role
{
    class RoleProfile : AutoMapper.Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleProfile"/> class.
        /// </summary>
        public RoleProfile()
        {
            CreateMap<CreateRoleDTO, Domain.Role>()
                .ForMember(x => x.Name, opt => opt.MapFrom(o => o.Name.Trim()))
                .ForMember(x => x.RoleId, opt => opt.MapFrom(o => o.RoleId))
                .ForMember(x => x.Description, opt => opt.MapFrom(o => o.Description))
                .ForMember(x => x.CreatedDate, opt => opt.MapFrom(o => DateTime.UtcNow))
                .ReverseMap();

            CreateMap<UpdateRoleDTO, Domain.Role>()
                .ForMember(x => x.Name, opt => opt.MapFrom(o => o.Name.Trim()))
                .ForMember(x => x.RoleId, opt => opt.MapFrom(o => o.RoleId))
                .ForMember(x => x.Description, opt => opt.MapFrom(o => o.Description))
                .ForMember(x => x.ModifiedDate, opt => opt.MapFrom(o => o.ModifiedDate.HasValue ? o.ModifiedDate : DateTime.UtcNow))
                .ReverseMap();

            CreateMap<Domain.Role, RoleDTO>()
                .ReverseMap();

        }
    }
}
