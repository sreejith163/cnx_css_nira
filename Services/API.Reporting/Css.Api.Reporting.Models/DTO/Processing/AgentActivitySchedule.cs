using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Reporting.Models.DTO.Processing
{
    public class AgentActivitySchedule
    {
        public int AgentId { get; set; }
        
        public List<ActivitySchedule> BaseSchedule { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }
        
        public AgentActivitySchedule()
        {
            BaseSchedule = new List<ActivitySchedule>();
        }
    }
}
