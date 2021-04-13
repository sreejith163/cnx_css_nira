using Css.Api.Core.Models.Domain.NoSQL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Models.DTO.Processing
{
    public class AgentMetadata
    {
        public List<Agent> Agents { get; set; }
        public List<ActivityLog> ActivityLogs { get; set; }
        public string Metadata { get; set; }
    }
}
