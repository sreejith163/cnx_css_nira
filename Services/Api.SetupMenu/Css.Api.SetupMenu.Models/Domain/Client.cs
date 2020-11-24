using System;
using System.Collections.Generic;

namespace Css.Api.SetupMenu.Models.Domain
{
    public partial class Client
    {
        public Client()
        {
            AgentSchedulingGroup = new HashSet<AgentSchedulingGroup>();
            ClientLobGroup = new HashSet<ClientLobGroup>();
            SkillGroup = new HashSet<SkillGroup>();
            SkillTag = new HashSet<SkillTag>();
        }

        public int Id { get; set; }
        public int? RefId { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<AgentSchedulingGroup> AgentSchedulingGroup { get; set; }
        public virtual ICollection<ClientLobGroup> ClientLobGroup { get; set; }
        public virtual ICollection<SkillGroup> SkillGroup { get; set; }
        public virtual ICollection<SkillTag> SkillTag { get; set; }
    }
}
