using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Models.DTO.Processing
{
    public class CalendarChart
    {
        public int EmployeeId { get; set; }
        public int AgentSchedulingGroupId { get; set; }
        public DateTime ScheduledDate { get; set; }
        public int ActivityId { get; set; }
        public string ActivityName { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string TimezoneOffset { get; set; }
    }
}
