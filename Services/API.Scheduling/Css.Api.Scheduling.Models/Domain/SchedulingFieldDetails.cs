using Newtonsoft.Json;

namespace Css.Api.Scheduling.Models.Domain
{
    public class SchedulingFieldDetails
    {
        /// <summary>
        /// Gets or sets the activity log range.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ActivityLogScheduleRange ActivityLogRange { get; set; }

        /// <summary>
        /// Gets or sets the activity log manager.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ActivityLogScheduleManager ActivityLogManager { get; set; }
    }
}
