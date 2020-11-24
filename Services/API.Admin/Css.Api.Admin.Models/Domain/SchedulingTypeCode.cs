using System;
using System.Collections.Generic;

namespace Css.Api.Admin.Models.Domain
{
    public partial class SchedulingTypeCode
    {
        public int Id { get; set; }
        public int SchedulingCodeId { get; set; }
        public int SchedulingCodeTypeId { get; set; }

        public virtual SchedulingCode SchedulingCode { get; set; }
        public virtual SchedulingCodeType SchedulingCodeType { get; set; }
    }
}
