using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Models.DTO.Processing
{
    public class CalendarChart
    {
        public string EmployeeId { get; set; }
        public int AgentSchedulingGroupId { get; set; }
        public DateTime ScheduledDate { get; set; }
        public int ActivityId { get; set; }
        public string ActivityName { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string Timezone { get; set; }
        public string TimezoneOffset { get; set; }
    }
}
