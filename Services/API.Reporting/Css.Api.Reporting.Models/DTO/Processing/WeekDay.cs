using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Models.DTO.Processing
{
    public class WeekDay
    {
        public DateTime Date { get; set; }
        public DayOfWeek Day { get => Date.DayOfWeek; }
    }
}
