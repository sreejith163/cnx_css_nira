using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Models.DTO.Processing
{
    public class ScheduleClock
    {
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public int ActivityId { get; set; }
        public string ActivityName { get; set; }
    }
}
