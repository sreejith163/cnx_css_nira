using Newtonsoft.Json;

namespace Css.Api.SetupMenu.Models.DTO.Request.OperationHour
{
    public class OperationHourAttribute
    {
        /// <summary>Gets or sets the day.</summary>
        /// <value>The day.</value>
        public int Day { get; set; }

        /// <summary>Gets or sets the operation hour open type identifier.</summary>
        /// <value>The operation hour open type identifier.</value>
        public int OperationHourOpenTypeId { get; set; }

        /// <summary>Gets or sets from.</summary>
        /// <value>From.</value>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string From { get; set; }

        /// <summary>Gets or sets to.</summary>
        /// <value>To.</value>

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string To { get; set; }
    }
}
