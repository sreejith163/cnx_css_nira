using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Models.DTO.Mappers
{
    public class MapperIndividualSettings
    {
        public string Key { get; set; }
        public string DBConnection { get; set; }
        public string Service { get; set; }
        public string FTPServer { get; set; }
        public string FTPFolder { get; set; }
    }
}
