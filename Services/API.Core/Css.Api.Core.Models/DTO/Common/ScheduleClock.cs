using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Core.Models.DTO.Common
{
    public class ScheduleClock
    {
        public int EmployeeId { get; set; }
        public int CurrentAgentSchedulingGroupId { get; set; }
        public DateTime Date { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public int ActivityId { get; set; }
        public string ActivityName { get; set; }
        public string Timezone { get; set; }
    }

}
