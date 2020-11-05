using System;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.Domain
{
    public partial class SkillTag
    {
        public SkillTag()
        {
            AgentDetail = new HashSet<AgentDetail>();
            AgentSchedulingGroup = new HashSet<AgentSchedulingGroup>();
            OperationHour = new HashSet<OperationHour>();
        }

        public int Id { get; set; }
        public int? RefId { get; set; }
        public string Name { get; set; }
        public int ClientId { get; set; }
        public int ClientLobGroupId { get; set; }
        public int SkillGroupId { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Client Client { get; set; }
        public virtual ClientLobGroup ClientLobGroup { get; set; }
        public virtual SkillGroup SkillGroup { get; set; }
        public virtual ICollection<AgentDetail> AgentDetail { get; set; }
        public virtual ICollection<AgentSchedulingGroup> AgentSchedulingGroup { get; set; }
        public virtual ICollection<OperationHour> OperationHour { get; set; }
    }
}
