using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Admin.Models.Domain
{
   public class Log
    {
        public int Id { get; set; }

        public string SSO { get; set; }

        public DateTimeOffset? TimeStamp { get; set; }

        public string UserAgent { get; set; }
    }
}
