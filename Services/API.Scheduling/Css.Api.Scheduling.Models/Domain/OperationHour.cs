using System;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.Domain
{
    public partial class OperationHour
    {
        public int Id { get; set; }
        public int? SkillGroupId { get; set; }
        public int? SkillTagId { get; set; }
        public int? SchedulingGroupId { get; set; }
        public int Day { get; set; }
        public int OperationHourOpenTypeId { get; set; }
        public string From { get; set; }
        public string To { get; set; }

        public virtual AgentSchedulingGroup SchedulingGroup { get; set; }
        public virtual SkillGroup SkillGroup { get; set; }
        public virtual SkillTag SkillTag { get; set; }
    }
}
