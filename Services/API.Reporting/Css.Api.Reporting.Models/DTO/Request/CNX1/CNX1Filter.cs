using Css.Api.Reporting.Models.DTO.Request.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Models.DTO.Request.CNX1
{
    public class CNX1Filter
    {
        public List<int> AgentIds { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public CNX1Filter()
        {
            AgentIds = new List<int>();
        }
    }
}
