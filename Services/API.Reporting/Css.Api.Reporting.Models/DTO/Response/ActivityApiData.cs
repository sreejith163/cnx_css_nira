using Css.Api.Reporting.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Models.DTO.Response
{
    public class ActivityApiData
    {
        public string AgentId { get; set; }
        public string ScheduleDate { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
