using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Job.Models.DTO.Common
{
    public class SpanFilter
    {
        public TimeSpan TimeOfDay { get; set; }
        public int DaysOfData { get; set; }
        public TimeSpan Start => TimeOfDay;
        public virtual TimeSpan End => TimeOfDay.Add(new TimeSpan(0, 5, 0));
        public List<int> Include { get; set; }
        public List<int> Exclude { get; set; }

        public SpanFilter()
        {
            Include = new List<int>();
            Exclude = new List<int>();
        }
    }
}
