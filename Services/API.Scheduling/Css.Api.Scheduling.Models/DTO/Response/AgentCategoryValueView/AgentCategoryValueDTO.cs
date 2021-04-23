using Css.Api.Core.Models.Domain.NoSQL;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.DTO.Response.AgentCategoryValueView
{
    public class AgentCategoryValueDTO
    {
        ///// <summary>
        ///// Gets or sets the identifier.
        ///// </summary>
        //[BsonRepresentation(BsonType.ObjectId)]
        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        //public string Id { get; set; }

        /// <summary>Gets or sets the employee identifier.</summary>
        /// <value>The employee identifier.</value>
        public string EmployeeId { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the agent data.
        /// </summary>
        public List<AgentCategoryValue> AgentCategoryValues { get; set; }
    }
}
