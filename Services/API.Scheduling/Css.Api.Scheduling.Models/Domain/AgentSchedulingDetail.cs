using System;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.Domain
{
    public partial class AgentSchedulingDetail
    {
        public AgentSchedulingDetail()
        {
            AgentSchedulingChart = new HashSet<AgentSchedulingChart>();
        }

        public int Id { get; set; }
        public int AgentId { get; set; }
        public string AgentName { get; set; }
        public int SchedulingGroupId { get; set; }
        public string Name { get; set; }
        public DateTimeOffset DateFrom { get; set; }
        public DateTimeOffset DateTo { get; set; }
        public int SchedulingStatusId { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }

        public virtual AgentSchedulingGroup SchedulingGroup { get; set; }
        public virtual SchedulingStatus SchedulingStatus { get; set; }
        public virtual ICollection<AgentSchedulingChart> AgentSchedulingChart { get; set; }
    }
}
