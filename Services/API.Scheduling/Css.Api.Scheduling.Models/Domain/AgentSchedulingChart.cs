using System;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.Domain
{
    public partial class AgentSchedulingChart
    {
        public int Id { get; set; }
        public int AgentSchedulingDetailId { get; set; }
        public int Day { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int IconId { get; set; }
        public string IconValue { get; set; }
        public string Meridian { get; set; }

        public virtual AgentSchedulingDetail AgentSchedulingDetail { get; set; }
        public virtual SchedulingCodeIcon IdNavigation { get; set; }
    }
}
