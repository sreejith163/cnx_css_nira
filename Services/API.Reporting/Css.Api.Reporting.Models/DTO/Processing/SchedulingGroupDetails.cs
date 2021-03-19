using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Models.DTO.Processing
{
    public class SchedulingGroupDetails
    {
        public int AgentSchedulingGroupId { get; set; }
        public int TimezoneId { get; set; }
        public string TimezoneValue { get; set; }
    }
}
