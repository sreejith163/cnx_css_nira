﻿using System;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.Domain
{
    public partial class SchedulingCode
    {
        public int Id { get; set; }
        public int RefId { get; set; }
        public string Description { get; set; }
        public int PriorityNumber { get; set; }
        public int IconId { get; set; }
        public int? EmployeeId { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }

        public virtual SchedulingCodeIcon Icon { get; set; }
    }
}
