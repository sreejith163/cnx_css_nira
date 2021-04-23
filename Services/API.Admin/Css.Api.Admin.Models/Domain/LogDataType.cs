using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Admin.Models.Domain
{
    public class LogDataType
    {
        public LogDataType()
        {
            Log = new HashSet<Log>();
        }

        public string SSO { get; set; }

        public DateTimeOffset? TimeStamp { get; set; }

        public virtual ICollection<Log> Log { get; set; }
    }
}
