using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Models.DTO.Processing
{
    public class AgentActivityScheduleUpdate
    {
        public List<ActivityScheduleUpdate> AgentSchedule { get; set; }

        public AgentActivityScheduleUpdate()
        {
            AgentSchedule = new List<ActivityScheduleUpdate>();
        }
    }
}
