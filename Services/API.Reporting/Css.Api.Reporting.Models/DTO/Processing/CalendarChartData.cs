using Css.Api.Core.Models.DTO.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Models.DTO.Processing
{
    public class CalendarChartData
    {
        public int LineNo { get; set; }
        public string Data { get; set; }
        public string EmpNo { get; set; }
        public string ActDate { get; set; }
        public string SchDate { get; set; }
        public string ActText { get; set; }
        public string ActStartTime { get; set; }
        public string ActEndTime { get; set; }
        public bool PatternParseStatus { get; set; }
        public bool EStartProvisioningEnabled { get; set; }
        public CalendarChart Chart { get; set; }
    }
}
