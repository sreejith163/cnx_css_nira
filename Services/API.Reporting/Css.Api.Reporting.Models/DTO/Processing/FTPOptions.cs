using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Models.DTO.Processing
{
    public class FTPOptions
    {
        public string Server { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Inbox { get; set; }
        public string Outbox { get; set; }
    }
}
