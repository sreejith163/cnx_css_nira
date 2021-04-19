using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentCategoryValueView
{
    public class ImportAgentCategoryValue
    {
        public string EmployeeId { get; set; }

        public string AgentCategory { get; set; }

        public string StartDate { get; set; }

        public string Value { get; set; }

    }
}
