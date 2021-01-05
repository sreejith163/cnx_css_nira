using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Models.DTO.Mappers
{
    public class MapperGlobalSettings
    {
        public string DBConnection { get; set; }
        public string FTPServer { get; set; }
        public string FTPInbox { get; set; }
        public string FTPOutbox { get; set; }
    }
}
