using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Models.DTO.Mappers
{
    public class MapperSettings
    {
        public MapperGlobalSettings GlobalSettings { get; set; }
        public List<MapperIndividualSettings> Imports { get; set; }
        public List<MapperIndividualSettings> Exports { get; set; }
    }
}
