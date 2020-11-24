using System;
using System.Collections.Generic;

namespace Css.Api.Setup.Models.Domain
{
    public partial class AgentDetail
    {
        public AgentDetail()
        {
            AgentGroupDetail = new HashSet<AgentGroupDetail>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? AcdId { get; set; }
        public int? AgentCategoryId { get; set; }
        public string MuValue { get; set; }
        public int SkillTagId { get; set; }
        public string AgentLogon { get; set; }
        public string Ssn { get; set; }
        public int? StartId { get; set; }
        public string SenExt { get; set; }
        public string SenDateDay { get; set; }
        public string SenDateMonth { get; set; }
        public string SenDateYear { get; set; }

        public virtual SkillTag SkillTag { get; set; }
        public virtual ICollection<AgentGroupDetail> AgentGroupDetail { get; set; }
    }
}
