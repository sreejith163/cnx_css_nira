using Css.Api.Admin.Models.DTO.Response.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Admin.Models.Profiles.Auth
{
    public class AuthProfile : AutoMapper.Profile
    {
        public AuthProfile()
        {
            CreateMap<Domain.NonSsoModel, NonSsoDTO>()
             .ReverseMap();
        }
        }
}
