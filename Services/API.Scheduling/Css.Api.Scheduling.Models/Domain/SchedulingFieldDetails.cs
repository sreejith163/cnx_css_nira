using Css.Api.Core.Models.Domain.NoSQL;
using Newtonsoft.Json;

namespace Css.Api.Scheduling.Models.Domain
{
    public class SchedulingFieldDetails
    {
        /// <summary>
        /// Gets or sets the charts.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ActivityLogScheduleRange ActivityLogScheduleRange { get; set; }

        /// <summary>
        /// Gets or sets the agent schedule manager chart.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ActivityLogScheduleManager ActivityLogScheduleManager { get; set; }
    }
}
