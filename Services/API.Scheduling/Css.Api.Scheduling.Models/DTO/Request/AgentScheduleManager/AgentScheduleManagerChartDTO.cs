using Css.Api.Core.Models.Domain.NoSQL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentScheduleManager
{
    public class AgentScheduleManagerChartDTO
    {
        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the charts.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<AgentScheduleManagerChart> Charts { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Models.Domain.NoSQL.AgentScheduleManager"/> class.
        /// </summary>
        public AgentScheduleManagerChartDTO()
        {
            Charts = new List<AgentScheduleManagerChart>();
        }
    }
}
