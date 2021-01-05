using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Job.Models.DTO.Configurations
{
    public class CronJob
    {
        public string Key { get; set; }
        public string Url { get; set; }
        public string Method { get; set; }
        public string Content { get; set; }
        public Dictionary<string,string> Headers { get; set; }
        public string CronExpression { get; set; }
        public string TimeZone { get; set; }
    }
}
