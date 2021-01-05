using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Models.DTO.Processing
{
    public class DataFeed
    {
        public string Accessor { get; set; }
        public string Path { get; set; }
        public byte[] Content { get; set; }
        public string Metadata { get; set; }
    }
}
