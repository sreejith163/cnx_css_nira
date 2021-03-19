using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Models.DTO.Processing
{
    public class ActivitySchedule
    {
        public string ScheduleDate { get; set; }
        public string StartDateTime { get; set; }
        public string EndDateTime { get; set; }
        public List<ActivityScheduleDetail> ScheduleDetail { get; set; }

        public ActivitySchedule()
        {
            ScheduleDetail = new List<ActivityScheduleDetail>();
        }
    }
}
