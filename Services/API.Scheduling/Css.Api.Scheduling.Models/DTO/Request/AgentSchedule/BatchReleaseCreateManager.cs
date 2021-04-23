using Css.Api.Core.Models.Domain.NoSQL;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentSchedule
{
   public class BatchReleaseCreateManager
    {
        public int EmployeeId { get; set; }

        /// <summary>
        /// Gets or sets the agent scheduling group identifier.
        /// </summary>
        public int AgentSchedulingGroupId { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        [BsonDateTimeOptions(Representation = BsonType.Document, Kind = DateTimeKind.Utc)]
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the charts.
        /// </summary>
        public List<AgentScheduleManagerChart> Charts { get; set; }

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
    }
}
