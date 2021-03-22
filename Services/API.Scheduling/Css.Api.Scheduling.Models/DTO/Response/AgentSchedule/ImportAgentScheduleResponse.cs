using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Scheduling.Models.DTO.Response.AgentSchedule
{
    public class ImportAgentScheduleResponse
    {
        public List<string> Errors;
        public List<string> Success;
        public string ImportStatus;
    }
}
