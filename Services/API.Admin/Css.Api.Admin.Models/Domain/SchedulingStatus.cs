using System;
using System.Collections.Generic;

namespace Css.Api.Admin.Models.Domain
{
    public partial class SchedulingStatus
    {
        public SchedulingStatus()
        {
        }

        public int Id { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }
}
