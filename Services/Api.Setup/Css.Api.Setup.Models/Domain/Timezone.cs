using System;
using System.Collections.Generic;

namespace Css.Api.Setup.Models.Domain
{
    public partial class Timezone
    {
        public Timezone()
        {
            AgentSchedulingGroup = new HashSet<AgentSchedulingGroup>();
            ClientLobGroup = new HashSet<ClientLobGroup>();
            SkillGroup = new HashSet<SkillGroup>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Abbreviation { get; set; }
        
        public virtual ICollection<AgentSchedulingGroup> AgentSchedulingGroup { get; set; }
        public virtual ICollection<ClientLobGroup> ClientLobGroup { get; set; }
        public virtual ICollection<SkillGroup> SkillGroup { get; set; }
    }
}
