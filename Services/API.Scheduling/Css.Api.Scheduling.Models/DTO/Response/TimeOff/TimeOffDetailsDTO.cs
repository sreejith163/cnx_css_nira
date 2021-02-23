using Css.Api.Scheduling.Models.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.DTO.Response.AgentSchedule
{
    public class TimeOffDetailsDTO
    {
        /// <summary>
        /// Gets or sets the agent schedule identifier.
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the scheduling code identifier.
        /// </summary>
        public int SchedulingCodeId { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        [BsonRepresentation(BsonType.String)]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        [BsonRepresentation(BsonType.String)]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? EndDate { get; set; }

        /// <summary>
        /// Gets or sets the allow day request.
        /// </summary>
        public List<DayOfWeek> AllowDayRequest { get; set; }

        /// <summary>
        /// Gets or sets the length of the fte day.
        /// </summary>
        public string FTEDayLength { get; set; }

        /// <summary>
        /// Gets or sets the first day of week.
        /// </summary>
        public int FirstDayOfWeek { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [view allotments].
        /// </summary>
        public bool ViewAllotments { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [view wait lists].
        /// </summary>
        public bool ViewWaitLists { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [time offs].
        /// </summary>
        public bool TimeOffs { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [add notes].
        /// </summary>
        public bool AddNotes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [show past days].
        /// </summary>
        public bool ShowPastDays { get; set; }

        /// <summary>
        /// Gets or sets the force off days before week.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? ForceOffDaysBeforeWeek { get; set; }

        /// <summary>
        /// Gets or sets the force off days after week.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? ForceOffDaysAfterWeek { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [allow full week request].
        /// </summary>
        public bool AllowFullWeekRequest { get; set; }

        /// <summary>
        /// Gets or sets the de selected time.
        /// </summary>
        public DeSelectedTime DeSelectedTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [deselect saved days].
        /// </summary>
        public bool DeselectSavedDays { get; set; }

        /// <summary>
        /// Gets or sets the modified by.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the modified date.
        /// </summary>
        [BsonRepresentation(BsonType.String)]
        public DateTimeOffset CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the modified by.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the modified date.
        /// </summary>
        [BsonRepresentation(BsonType.String)]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? ModifiedDate { get; set; }
    }
}


