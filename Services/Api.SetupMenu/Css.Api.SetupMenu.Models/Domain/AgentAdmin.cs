using System;
using System.Collections.Generic;

namespace Css.Api.SetupMenu.Models.Domain
{
    public partial class AgentAdmin
    {
        public int Id { get; set; }
        public string AgentSso { get; set; }
        public int? EmployeeId { get; set; }
        public int AgnetSchedulingGroupId { get; set; }
        public DateTimeOffset HireDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }

        public virtual AgentSchedulingGroup AgnetSchedulingGroup { get; set; }
    }
}
