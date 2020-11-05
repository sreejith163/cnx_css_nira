using System;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.Domain
{
    public partial class ClientLobGroup
    {
        public ClientLobGroup()
        {
            AgentSchedulingGroup = new HashSet<AgentSchedulingGroup>();
            SkillGroup = new HashSet<SkillGroup>();
            SkillTag = new HashSet<SkillTag>();
        }

        public int Id { get; set; }
        public int? RefId { get; set; }
        public string Name { get; set; }
        public int ClientId { get; set; }
        public int FirstDayOfWeek { get; set; }
        public int TimezoneId { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Client Client { get; set; }
        public virtual Timezone Timezone { get; set; }
        public virtual ICollection<AgentSchedulingGroup> AgentSchedulingGroup { get; set; }
        public virtual ICollection<SkillGroup> SkillGroup { get; set; }
        public virtual ICollection<SkillTag> SkillTag { get; set; }
    }
}
