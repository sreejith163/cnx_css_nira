using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Reporting.Models.DTO.Processing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Models.DTO.Request.Common
{
    public class AgentWeekDayChart
    {
        public int EmployeeId { get; set; }
        public int AgentSchedulingGroupId { get; set; }
        public int TimezoneId { get; set; }
        public string TimezoneValue { get; set; }
        public WeekDay ChartWeekDay { get; set; }
        public List<ScheduleChart> Charts { get; set; }
    }
}
