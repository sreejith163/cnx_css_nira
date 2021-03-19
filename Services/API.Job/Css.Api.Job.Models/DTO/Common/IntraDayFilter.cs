using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Job.Models.DTO.Common
{
    public class IntraDayFilter
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int RelativeStartDay { get; set; }
        public int RelativeEndDay { get; set; }
        public int? PickRecentlyUpdatedInDays { get; set; }
        public List<int> Include { get; set; }
        public List<int> Exclude { get; set; }

        public IntraDayFilter()
        {
            Include = new List<int>();
            Exclude = new List<int>();
        }
    }
}
