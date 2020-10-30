using System;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.Domain
{
    public partial class SchedulingStatus
    {
        public SchedulingStatus()
        {
            AgentSchedulingDetail = new HashSet<AgentSchedulingDetail>();
        }

        public int Id { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }

        public virtual ICollection<AgentSchedulingDetail> AgentSchedulingDetail { get; set; }
    }
}
