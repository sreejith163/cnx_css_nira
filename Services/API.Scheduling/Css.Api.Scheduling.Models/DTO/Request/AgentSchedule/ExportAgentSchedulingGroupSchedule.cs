using Css.Api.Core.Models.Domain.NoSQL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentSchedule
{
   public class ExportAgentSchedulingGroupSchedule
    {
        public int EmployeeId { get; set; }


        public string StartDate { get; set; }

        public string EndDate { get; set; }


        public string ActivityCode { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }
       
    }
}
