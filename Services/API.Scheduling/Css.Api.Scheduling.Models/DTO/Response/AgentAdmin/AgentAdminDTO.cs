﻿using Css.Api.Core.Models.Domain.NoSQL;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Css.Api.Scheduling.Models.DTO.Response.AgentAdmin
{
    public class AgentAdminDTO
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        /// <summary>Gets or sets the employee identifier.</summary>
        /// <value>The employee identifier.</value>
        public string EmployeeId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Sso { get; set; }       

        /// <summary>Gets or sets the skill tag identifier.</summary>
        /// <value>The skill tag identifier.</value>
        public int SkillTagId { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the agent scheduling group identifier.
        /// </summary>
        public int? AgentSchedulingGroupId { get; set; }

        /// <summary>
        /// Gets or sets the hire date.
        /// </summary>
        public DateTime? HireDate { get; set; }

        /// <summary>
        /// Gets or sets the agent category values.
        /// </summary>
        public List<AgentCategoryValue> AgentCategoryValues { get; set; }

        /// <summary>
        /// Gets or sets the super visor identifier.
        /// </summary>
        public string SupervisorId { get; set; }

        /// <summary>
        /// Gets or sets the name of the super visor.
        /// </summary>
        public string SupervisorName { get; set; }

        /// <summary>
        /// Gets or sets the super visor sso.
        /// </summary>
        public string SupervisorSso { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        public DateTimeOffset CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the modified by.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the modified date.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? ModifiedDate { get; set; }
    }
}


