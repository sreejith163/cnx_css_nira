using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Models.DTO.Mappers
{
    public class MapperSettings
    {
        public List<Dispatch> Dispatchers { get; set; }
        public List<Activity> Activities { get; set; }
        public List<DataOption> DataOptions { get; set; }
    }
}
