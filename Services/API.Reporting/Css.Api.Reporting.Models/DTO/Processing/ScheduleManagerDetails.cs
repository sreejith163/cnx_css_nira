using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Models.DTO.Processing
{
    public class ScheduleManagerDetails
    {
        public int EmployeeId { get; set; }
        public int AgentSchedulingGroupId { get; set; }
        public DateTime ScheduledDate { get; set; }
        public List<AgentScheduleManagerChart> Schedules { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public ActivityOrigin Origin { get; set; }
        public ActivityStatus Status { get; set; }
        public ActivityType Type { get; set; }
        public bool ExistingSchedule { get; set; }
    }
}
