using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Models.DTO.Request.Common
{
    public class ScheduleFilter
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<string> AgentIds { get; set; }
        public List<int> TimezoneIds { get; set; }
        public List<int> AgentSchedulingGroupIds { get; set; }
        public bool? EstartProvision { get; set; }
        public int? UpdatedInPastDays { get; set; }

        public ScheduleFilter()
        {
            TimezoneIds = new List<int>();
            AgentSchedulingGroupIds = new List<int>();
            AgentIds = new List<string>();
        }
    }
}
