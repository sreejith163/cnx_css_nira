using Css.Api.Core.Models.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Css.Api.Core.Models.Domain.NoSQL
{
    [BsonCollection("agent")]
    public class Agent : BaseDocument
    {
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the mu.
        /// </summary>
        public string Mu { get; set; }

        /// <summary>
        /// Gets or sets the SSN.
        /// </summary>
        public int Ssn { get; set; }

        /// <summary>
        /// Gets or sets the sso.
        /// </summary>
        public string Sso { get; set; }

        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        public int ClientId { get; set; }

        /// <summary>
        /// Gets or sets the client lob group identifier.
        /// </summary>
        public int ClientLobGroupId { get; set; }

        /// <summary>
        /// Gets or sets the skill group identifier.
        /// </summary>
        public int SkillGroupId { get; set; }

        /// <summary>
        /// Gets or sets the skill tag identifier.
        /// </summary>
        public int SkillTagId { get; set; }

        /// <summary>
        /// Gets or sets the agent scheduling group identifier.
        /// </summary>
        public int AgentSchedulingGroupId { get; set; }

        /// <summary>
        /// Gets or sets the sen date.
        /// </summary>
        public DateTime? SenDate { get; set; }

        /// <summary>
        /// Gets or sets the sen ext.
        /// </summary>
        public DateTime? SenExt { get; set; }

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
        /// Gets or sets the agent role.
        /// </summary>
        public string AgentRole { get; set; }

        /// <summary>
        /// Gets or sets the pto.
        /// </summary>
        public AgentPto Pto { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the agent data.
        /// </summary>
        public List<AgentData> AgentData { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the created date.
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
        /// Gets or sets the origin.
        /// </summary>
        public ActivityOrigin Origin { get; set; }
    }
}
