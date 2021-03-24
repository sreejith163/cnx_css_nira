using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Models.DTO.Processing
{
    public class ActivitySchedule
    {
        public string ScheduleDate { get; set; }
        public List<ActivityScheduleDetail> ScheduleDetail { get; set; }

        public ActivitySchedule()
        {
            ScheduleDetail = new List<ActivityScheduleDetail>();
        }
    }
}
