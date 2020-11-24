using System;
using System.Collections.Generic;

namespace Css.Api.Admin.Models.Domain
{
    public partial class SchedulingCodeIcon
    {
        public SchedulingCodeIcon()
        {
            SchedulingCode = new HashSet<SchedulingCode>();
        }

        public int Id { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }

        public virtual ICollection<SchedulingCode> SchedulingCode { get; set; }
    }
}
