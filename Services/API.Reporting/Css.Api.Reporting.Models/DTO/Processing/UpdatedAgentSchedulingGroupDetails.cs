using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Models.DTO.Processing
{
    public class UpdatedAgentSchedulingGroupDetails
    {
        public string EmployeeId { get; set; }
        public int CurrentAgentSchedulingGroupId { get; set; }
        public DateTime StartDate { get; set; }
    }
}
