using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Job.Models.DTO.Common
{
    public class IntraDayDetails
    {
        public int TimezoneId { get; set; }
        public DateTime CurrentDate { get; set; }
        public DateTime CurrentDateTime { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? RecentlyUpdatedInDays { get; set; }
    }
}
