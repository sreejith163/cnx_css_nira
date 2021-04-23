using Css.Api.Admin.Models.DTO.Request.Log;
using Css.Api.Admin.Models.DTO.Response.Log;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Admin.Models.Profiles.Log
{
   public class LogProfile :  AutoMapper.Profile
    {
        public LogProfile()
        {
            

            CreateMap<CreateLogDTO, Domain.Log>()
                .ReverseMap();

            CreateMap<Domain.Role, LogDTO>()
    .ReverseMap();

        }
    }
}
