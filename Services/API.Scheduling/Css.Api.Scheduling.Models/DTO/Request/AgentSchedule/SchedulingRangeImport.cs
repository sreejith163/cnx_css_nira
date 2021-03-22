using System;
using System.Collections.Generic;
using System.Text;
using Css.Api.Core.Models.Domain.NoSQL;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentSchedule
{
    public class SchedulingRangeImport: AgentScheduleRange
    {
        /// <summary>
        /// Gets or sets the employee identifier.
        /// </summary>
        public int EmployeeId { get; set; }
    }
}
