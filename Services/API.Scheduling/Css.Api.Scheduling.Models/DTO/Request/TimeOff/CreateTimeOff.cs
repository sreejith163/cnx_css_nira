using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Css.Api.Scheduling.Models.DTO.Request.TimeOff
{
    public class CreateTimeOff : TimeOffAttributes
    {
        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        [BsonRepresentation(BsonType.String)]
        public DateTimeOffset? CreatedDate { get; set; }
    }
}