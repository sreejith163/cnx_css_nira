using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Job.Models.DTO.EStart
{
    public class EStartExportRequest
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public List<int> TimezoneIds { get; set; }
        public int? UpdatedInPastDays { get; set; }
    }
}
