using Css.Api.Core.Models.Domain.NoSQL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Models.DTO.Processing
{
    public class ScheduleManagerData
    {
        public List<string> Messages { get; set; }
        public List<AgentScheduleManagerChart> ManagerCharts { get; set; }
    }
}
