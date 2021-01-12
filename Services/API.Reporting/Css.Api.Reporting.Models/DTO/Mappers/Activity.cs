using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Models.DTO.Mappers
{
    public class Activity
    {
        public string Key { get; set; }
        public string Source { get; set; }
        public string Target { get; set; }
        public string SourceDataOption { get; set; }
        public string TargetDataOption { get; set; }
    }
}
