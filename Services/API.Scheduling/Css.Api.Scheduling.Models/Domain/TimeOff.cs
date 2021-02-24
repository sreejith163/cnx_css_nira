using Css.Api.Core.Models.Domain;
using Css.Api.Scheduling.Models.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.Domain
{
    [BsonCollection("time_off")]
    public class TimeOff : BaseDocument
    {
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
        public DateTimeOffset StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        [BsonRepresentation(BsonType.String)]
        public DateTimeOffset EndDate { get; set; }

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
        public int ForceOffDaysBeforeWeek { get; set; }

        /// <summary>
        /// Gets or sets the force off days after week.
        /// </summary>
        public int ForceOffDaysAfterWeek { get; set; }

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
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the modified date.
        /// </summary>
        [BsonRepresentation(BsonType.String)]
        public DateTimeOffset? ModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeOff"/> class.
        /// </summary>
        public TimeOff()
        {
            AllowDayRequest = new List<DayOfWeek>();
        }
    }
}
