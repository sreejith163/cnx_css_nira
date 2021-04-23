using Css.Api.Core.Models.Domain.NoSQL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentCategoryValueView
{
    public class ImportUpdateAgentSchedule
    {
        public string EmployeeId { get; set; }

       
        public int AgentSchedulingGroupId { get; set; }

        /// <summary>Gets or sets the ranges.</summary>
        /// <value>The ranges.</value>
        public List<AgentScheduleRange> Ranges { get; set; }

        /// <summary>
        /// Gets or sets the modified by.
        /// </summary>
       public string ModifiedBy { get; set; }
    }
}
