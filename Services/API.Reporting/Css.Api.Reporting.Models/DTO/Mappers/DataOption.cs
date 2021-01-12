using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Models.DTO.Mappers
{
    public class DataOption
    {
        public string Key { get; set; }
        public string Type { get; set; }
        public Dictionary<string,string> Options { get; set; }
    }
}
