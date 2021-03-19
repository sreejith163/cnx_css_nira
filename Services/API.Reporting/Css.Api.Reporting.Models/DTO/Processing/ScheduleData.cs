using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Models.DTO.Processing
{
    public class ScheduleData
    {
        public ActivityOrigin Origin { get; set; }
        public ActivityStatus Status { get; set; }
        public ActivityType Type { get; set; }
        public List<string> Messages { get; set; }
        public AgentScheduleManager Schedule { get; set; }
    }
}
