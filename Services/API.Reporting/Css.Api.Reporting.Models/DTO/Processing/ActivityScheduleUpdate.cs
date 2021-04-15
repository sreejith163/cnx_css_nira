using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Models.DTO.Processing
{
    public class ActivityScheduleUpdate
    {
        public string AgentId { get; set; }
        public string Timezone { get; set; }
        public string ScheduleDate { get; set; }
        public List<ActivityScheduleUpdateDetail> ScheduleActivities { get; set; }

        public ActivityScheduleUpdate()
        {
            ScheduleActivities = new List<ActivityScheduleUpdateDetail>();
        }
    }
}
