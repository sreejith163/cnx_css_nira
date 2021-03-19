using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Models.DTO.Processing
{
    public class TimezoneDetails
    {
        public int TimezoneId { get; set; }
        public string Name { get; set; }
        public string TimezoneValue { get; set; }
        public TimeSpan TimezoneOffset { get; set; }
    }
}
