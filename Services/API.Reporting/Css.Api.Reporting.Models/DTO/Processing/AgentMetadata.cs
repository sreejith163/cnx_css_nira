using Css.Api.Core.Models.Domain.NoSQL;
using Css.Api.Reporting.Models.DTO.Request.UDW;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Models.DTO.Processing
{
    public class AgentMetadata
    {
        public List<Agent> Agents { get; set; }
        public List<ActivityLog> ActivityLogs { get; set; }
        public UDWAgentList Unprocessed { get; set; }
    }
}
