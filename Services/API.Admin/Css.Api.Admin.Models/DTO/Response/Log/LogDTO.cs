using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Admin.Models.DTO.Response.Log
{
   public class LogDTO
    {
        public int Id { get; set; }

        public string SSO { get; set; }

        public DateTimeOffset? TimeStamp { get; set; }

        public string UserAgent { get; set; }
    }
}
