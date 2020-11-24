using System;
using System.Collections.Generic;

namespace Css.Api.Admin.Models.Domain
{
    public partial class AgentCategoryDataType
    {
        public AgentCategoryDataType()
        {
            AgentCategory = new HashSet<AgentCategory>();
        }

        public int Id { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }

        public virtual ICollection<AgentCategory> AgentCategory { get; set; }
    }
}
