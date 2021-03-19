using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Models.DTO.Request
{
    public class MappingContext
    {
        public string Key { get; set; }
        public string Source { get; set; }
        public string SourceType { get; set; }
        public Dictionary<string,string> SourceOptions { get; set; }
        public string Target { get; set; }
        public string TargetType { get; set; }
        public Dictionary<string, string> TargetOptions { get; set; }
        public string RequestBody { get; set; }
        public string RequestQueryParams { get; set; }
    }
}
