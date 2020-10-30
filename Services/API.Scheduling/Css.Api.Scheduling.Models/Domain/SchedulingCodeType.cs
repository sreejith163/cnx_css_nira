using System;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.Domain
{
    public partial class SchedulingCodeType
    {
        public SchedulingCodeType()
        {
            SchedulingTypeCode = new HashSet<SchedulingTypeCode>();
        }

        public int Id { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }

        public virtual ICollection<SchedulingTypeCode> SchedulingTypeCode { get; set; }
    }
}
