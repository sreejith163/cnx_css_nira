using System;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.Domain
{
    public partial class AgentGroupDetail
    {
        public int Id { get; set; }
        public int? AgentGroupId { get; set; }
        public string AgentGroupValue { get; set; }
        public string AgentGroupDescription { get; set; }
        public int AgentId { get; set; }

        public virtual AgentDetail Agent { get; set; }
    }
}
