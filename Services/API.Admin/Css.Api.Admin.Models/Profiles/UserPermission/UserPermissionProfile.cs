using Css.Api.Core.Models.DTO.Response;
using Css.Api.Admin.Models.DTO.Request.UserPermission;
using Css.Api.Admin.Models.DTO.Response.UserPermission;
using System;

namespace Css.Api.Admin.Models.Profiles.Agent
{
    public class UserPermissionProfile : AutoMapper.Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserPermissionProfile"/> class.
        /// </summary>
        public UserPermissionProfile()
        {
            CreateMap<CreateUserPermissionDTO, Domain.UserPermission>()
                .ForMember(x => x.Sso, opt => opt.MapFrom(o => o.Sso.Trim()))
                .ForMember(x => x.EmployeeId, opt => opt.MapFrom(o => o.EmployeeId.Trim()))
                .ForMember(x => x.Firstname, opt => opt.MapFrom(o => o.Firstname))
                .ForMember(x => x.Lastname, opt => opt.MapFrom(o => o.Lastname))
                .ForMember(x => x.CreatedBy, opt => opt.MapFrom(o => o.CreatedBy))
                .ForMember(x => x.CreatedDate, opt => opt.MapFrom(o => DateTime.UtcNow))
                .ReverseMap();

            CreateMap<UpdateUserPermissionDTO, Domain.UserPermission>()
                .ForMember(x => x.Sso, opt => opt.MapFrom(o => o.Sso.Trim()))
                .ForMember(x => x.EmployeeId, opt => opt.MapFrom(o => o.EmployeeId.Trim()))
                .ForMember(x => x.Firstname, opt => opt.MapFrom(o => o.Firstname))
                .ForMember(x => x.Lastname, opt => opt.MapFrom(o => o.Lastname))
                .ForMember(x => x.ModifiedBy, opt => opt.MapFrom(o => o.ModifiedBy))
                .ForMember(x => x.ModifiedDate, opt => opt.MapFrom(o => o.ModifiedDate.HasValue ? o.ModifiedDate : DateTime.UtcNow))
                .ReverseMap();

            CreateMap<Domain.UserPermission, UserLanguagePreference>()
                .ReverseMap();
             
            CreateMap<Domain.UserPermission, UserPermissionDTO>()
                .ForMember(x => x.UserRoleId, opt => opt.MapFrom(o => o.Role.RoleId))
                .ForMember(x => x.RoleIndex, opt => opt.MapFrom(o => o.Role.Id))
                .ForMember(x => x.RoleName, opt => opt.MapFrom(o => o.Role != null ? o.Role.Name : ""))
                .ReverseMap();

        }
    }
}
