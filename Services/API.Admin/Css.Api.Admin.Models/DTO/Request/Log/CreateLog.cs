using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Admin.Models.DTO.Request.Log
{
    public class CreateLogDTO
    {
     

        public string SSO { get; set; }

        public  DateTimeOffset? TimeStamp { get; set; }

        public string UserAgent { get; set; }
    }
}
