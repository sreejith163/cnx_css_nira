using Css.Api.Core.Models.Domain.NoSQL;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.DTO.Request.AgentScheduleManager
{
    public class AgentScheduleManagerChartDTO
    {
        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        [BsonRepresentation(BsonType.String)]
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the charts.
        /// </summary>
        public List<ScheduleChart> Charts { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentScheduleManagerChart"/> class.
        /// </summary>
        public AgentScheduleManagerChartDTO()
        {
            Charts = new List<ScheduleChart>();
        }
    }
}
