
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentSchedule
{
    public class AgentScheduleImportData
    {
        /// <summary>
        /// Gets or sets the employee identifier.
        /// </summary>
        public int EmployeeId { get; set; }

        /// Gets or sets the date from.
        /// </summary>
        [BsonDateTimeOptions(Representation = BsonType.Document, Kind = DateTimeKind.Utc)]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the date to.
        /// </summary>
        [BsonDateTimeOptions(Representation = BsonType.Document, Kind = DateTimeKind.Utc)]
        public DateTime EndDate { get; set; }


        /// <summary>
        /// Gets or sets the start time.
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time.
        /// </summary>
        public string EndTime { get; set; }

        /// <summary>
        /// Gets or sets the scheduling code identifier.
        /// </summary>
        public int SchedulingCodeId { get; set; }


    }
}
